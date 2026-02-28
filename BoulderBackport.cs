using System;
using System.Diagnostics;
using System.IO;
using BoulderBackport.Content.Configs;
using BoulderBackport.Content.Sources;
using BoulderBackport.Core.Systems;
using ReLogic.OS;
using Terraria;
using Terraria.ModLoader;

namespace BoulderBackport;

public enum BackportingStatus
{
    Backporting,
    NotBackporting,
    VanillaOutdated
}

public partial class BoulderBackport : Mod
{
    static BoulderBackport()
    {
        var gameVersion = new Version(Main.versionNumber.TrimStart('v'));
        if (gameVersion >= new Version(1, 4, 5))
        {
            Status = BackportingStatus.NotBackporting;
            return;
        }

        if (!IsVanillaUpdatedTo145())
        {
            Status = BackportingStatus.VanillaOutdated;
            return;
        }

        MusicBackportingSystem.EarlyInitialize();
        SelectBackportContentSource.RegisterAutoLifecycle();
    }

    public BoulderBackport() => Instance = this;

    public static BoulderBackport Instance { get; private set; }

    internal static BackportingStatus Status { get; private set; }

    private static bool IsVanillaUpdatedTo145()
    {
        var vanillaContentDirectory = SelectBackportContentSource.VanillaContentDirectory;

        if (Platform.IsWindows)
        {
            var vanillaExePath = Path.Combine(vanillaContentDirectory, "..", "Terraria.exe");
            var info = FileVersionInfo.GetVersionInfo(vanillaExePath);
            if (info.ProductVersion != null)
            {
                var productVersion = new Version(info.ProductVersion);
                return productVersion >= new Version(1, 4, 5);
            }
        }

        // Fallback for non-Windows platforms (or if Windows version check fails):
        // Check for an item introduced in 1.4.5
        var itemPath = Path.Combine(vanillaContentDirectory, "Item_6144.xnb");
        return System.IO.File.Exists(itemPath);
    }

    public override void Load()
    {
        switch (Status)
        {
            case BackportingStatus.VanillaOutdated:
                Logger.Warn("Vanilla installation is not updated to 1.4.5; backporting features are disabled.");
                break;
            case BackportingStatus.NotBackporting:
                Logger.Info("Vanilla installation is up to date; backporting features are disabled.");
                break;
            case BackportingStatus.Backporting:
            default:
                break;
        }
    }

    public void DebugLog(object message)
    {
        if (BackportConfig.Instance.VerboseDebugLogging)
            Logger.Debug(message);
    }
}