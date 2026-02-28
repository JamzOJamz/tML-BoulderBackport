namespace BoulderBackport.Core.Systems;

/// <summary>
///     Built-in reusable portrait conditions.
/// </summary>
public static class NPCPortraitConditions
{
    /// <summary>Matches NPCs currently in their shimmer variant.</summary>
    public static readonly NPCPortraitCondition IsShimmered =
        npc => npc.IsShimmerVariant;

    /// <summary>Used for the Zoologist.</summary>
    public static readonly NPCPortraitCondition IsLycanthrope =
        npc => npc.ShouldBestiaryGirlBeLycantrope();

    /// <summary>Matches NPCs with a specific variant index (e.g. cat breeds, dog breeds).</summary>
    public static NPCPortraitCondition HasVariant(int variantID) =>
        npc => npc.townNpcVariationIndex == variantID;
}