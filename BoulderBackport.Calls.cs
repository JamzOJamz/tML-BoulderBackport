using System;
using BoulderBackport.Core.Systems;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace BoulderBackport;

public partial class BoulderBackport
{
    // Example call: ModLoader.GetMod("BoulderBackport").Call("AddPortrait", ModContent.NPCType<YourModNPC>(), "YourMod/Content/NPCs/YourTownNPC_Portrait", "YourMod/Content/NPCs/YourTownNPC_Shimmer_Portrait");
    public override object Call(params object[] args)
    {
        ArgumentNullException.ThrowIfNull(args);

        if (args.Length == 0)
            throw new ArgumentException("Arguments cannot be empty!");

        if (args[0] is not string content)
            throw new ArgumentException("Argument 1 must be a string command");

        if (!content.Equals("AddPortrait", StringComparison.InvariantCultureIgnoreCase)) return false;
        
        if (args.Length < 3)
            throw new ArgumentException(
                "AddPortrait requires at least 3 arguments: (string command, int npcType, string texturePath OR Asset<Texture2D> texture, [optional] string shimmerTexturePath OR Asset<Texture2D> shimmerTexture)");

        if (args[1] is not int npcType)
            throw new ArgumentException("Argument 2 (npcType) must be an integer");

        if (args[2] == null)
            throw new ArgumentException("Argument 3 (texture) cannot be null");

        var provider = NPCPortraitProvider.Prioritized();

        if (args.Length >= 4 && args[3] != null)
        {
            switch (args[3])
            {
                case string shimmerTexturePath:
                    provider.With(NPCPortraitConditions.IsShimmered, shimmerTexturePath);
                    break;
                case Asset<Texture2D> shimmerTextureAsset:
                    provider.With(NPCPortraitConditions.IsShimmered, shimmerTextureAsset);
                    break;
                default:
                    throw new ArgumentException(
                        "Argument 4 (shimmer texture) must be either a string path or Asset<Texture2D>");
            }
        }

        switch (args[2])
        {
            case string texturePath:
                provider.Default(texturePath);
                break;
            case Asset<Texture2D> textureAsset:
                provider.Default(textureAsset);
                break;
            default:
                throw new ArgumentException(
                    "Argument 3 (texture) must be either a string path or Asset<Texture2D>");
        }

        NPCPortraitSystem.Register(npcType, provider);
        return true;

    }
}