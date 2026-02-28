using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;

namespace BoulderBackport.Core.Systems;

/// <summary>
///     A delegate that evaluates an NPC and returns true if a given portrait condition is met.
/// </summary>
public delegate bool NPCPortraitCondition(NPC npc);

/// <summary>
///     Represents a single conditional portrait entry: if the condition is met, use this texture.
/// </summary>
public readonly record struct ConditionalPortrait(
    NPCPortraitCondition Condition,
    Asset<Texture2D> Texture
);

/// <summary>
///     Evaluates a prioritized list of conditions and returns the first matching portrait texture.
///     Falls back to a default if no condition matches.
/// </summary>
public sealed class NPCPortraitProvider
{
    private readonly List<ConditionalPortrait> _conditionals = [];
    private Asset<Texture2D> _default;

    private NPCPortraitProvider()
    {
    }

    /// <summary>Creates a new prioritized portrait provider.</summary>
    public static NPCPortraitProvider Prioritized() => new();

    /// <summary>Adds a conditional portrait. Conditions are evaluated in the order they are added.</summary>
    public NPCPortraitProvider With(NPCPortraitCondition condition, string assetPath)
    {
        _conditionals.Add(new ConditionalPortrait(condition, ModContent.Request<Texture2D>(assetPath)));
        return this;
    }

    /// <summary>Adds a conditional portrait. Conditions are evaluated in the order they are added.</summary>
    public NPCPortraitProvider With(NPCPortraitCondition condition, Asset<Texture2D> texture)
    {
        _conditionals.Add(new ConditionalPortrait(condition, texture));
        return this;
    }

    /// <summary>Sets the fallback portrait used when no condition matches.</summary>
    public NPCPortraitProvider Default(string assetPath)
    {
        _default = ModContent.Request<Texture2D>(assetPath);
        return this;
    }
    
    public NPCPortraitProvider Default(Asset<Texture2D> texture)
    {
        _default = texture;
        return this;
    }

    /// <summary>Resolves the correct portrait for the given NPC, or null if none matches.</summary>
    public Asset<Texture2D> Resolve(NPC npc)
    {
        foreach (var (condition, texture) in _conditionals)
            if (condition(npc))
                return texture;

        return _default;
    }
}