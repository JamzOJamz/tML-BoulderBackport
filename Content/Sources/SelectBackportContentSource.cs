using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using ReLogic.Content.Sources;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.ModLoader.Engine;

namespace BoulderBackport.Content.Sources;

internal sealed class SelectBackportContentSource : ContentSource
{
    private static readonly Dictionary<string, string> BackportedAssets = [];

    internal static readonly string VanillaContentDirectory =
        ((TMLContentManager)Main.instance.Content).RootDirectories.Last();

    private SelectBackportContentSource(IEnumerable<KeyValuePair<string, string>> assetMappings)
    {
        SetAssetNames(assetMappings.Select(kvp => kvp.Key));
    }

    public override Stream OpenStream(string fullAssetName)
    {
        // Use a mapped path if available, otherwise fall back to the asset name itself
        var diskPath = BackportedAssets.GetValueOrDefault(fullAssetName, fullAssetName);
        var fullyResolvedPath = Path.Combine(VanillaContentDirectory, diskPath);
        return File.OpenRead(fullyResolvedPath);
    }

    internal static void RegisterAutoLifecycle()
    {
        MonoModHooks.Add(
            typeof(ModContent).GetMethod(nameof(ModContent.Load), BindingFlags.NonPublic | BindingFlags.Static),
            (Action<
                Action<CancellationToken>,
                CancellationToken
            >)((orig, token) =>
            {
                orig(token);
                Main.QueueMainThreadAction(() =>
                {
                    var customSource = new SelectBackportContentSource(BackportedAssets);
                    var assetSourceController = Main.AssetSourceController;
                    assetSourceController._staticSources.Insert(0, customSource);
                    assetSourceController.Refresh();
                });
            }));

        MonoModHooks.Add(
            typeof(ModLoader).GetMethod(nameof(ModLoader.Unload), BindingFlags.NonPublic | BindingFlags.Static),
            (Func<
                Func<bool>,
                bool
            >)(orig =>
            {
                var success = orig();
                Main.QueueMainThreadAction(() =>
                {
                    var assetSourceController = Main.AssetSourceController;
                    var customSource = assetSourceController._staticSources.OfType<SelectBackportContentSource>()
                        .FirstOrDefault();
                    if (customSource != null)
                    {
                        assetSourceController._staticSources.Remove(customSource);
                        assetSourceController.Refresh();
                    }
                });
                return success;
            }));

        // This'll throw trying to take .Single() so we patch it to take .Last() instead
        MonoModHooks.Add(
            typeof(AssetSourceController).GetMethod("get_StaticSource", BindingFlags.Instance | BindingFlags.NonPublic),
            (Func<
                Func<AssetSourceController, IContentSource>,
                AssetSourceController, IContentSource
            >)((_, instance) => instance._staticSources.Last()));
    }

    /// <summary>
    ///     Registers an asset using the same path for both lookup and disk location.
    /// </summary>
    public static void Add(string path)
    {
        BackportedAssets.TryAdd(path, path);
    }

    /// <summary>
    ///     Registers an asset under <paramref name="oldPath" /> but reads it from <paramref name="newPath" />.
    /// </summary>
    public static void Add(string oldPath, string newPath)
    {
        BackportedAssets.TryAdd(oldPath, newPath);
    }
}