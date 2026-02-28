using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace BoulderBackport.Content.Scenes.MusicScenes;

public abstract class ProximityMusicSceneEffect : ModSceneEffect
{
    #region Hooks

    protected virtual bool CanActivate(Player player) => true;

    #endregion

    #region Scene Effect

    public override bool IsSceneEffectActive(Player player)
    {
        if (!CanActivate(player))
            return false;

        if (!HasMusic())
            return false;

        return NPCInRange() || ProjectileInRange();
    }

    #endregion

    #region Music Resolution

    private bool HasMusic()
    {
        return Music > 0;
    }

    #endregion

    #region Range Checks

    private bool NPCInRange()
    {
        var screen = ScreenBounds;

        foreach (var npc in Main.ActiveNPCs)
        {
            if (!IsRelevantNpc(npc.type))
                continue;

            if (screen.Intersects(EffectBounds(npc.Center)))
                return true;
        }

        return false;
    }

    private bool ProjectileInRange()
    {
        if (TrackedProjectileID is null)
            return false;

        var screen = ScreenBounds;

        foreach (var proj in Main.ActiveProjectiles)
        {
            if (proj.type != TrackedProjectileID)
                continue;

            if (screen.Intersects(EffectBounds(proj.Center)))
                return true;
        }

        return false;
    }

    private bool IsRelevantNpc(int npcType)
    {
        if (npcType == PrimaryNPCID)
            return true;

        foreach (var extra in ExtraNPCIDs)
        {
            if (npcType == extra)
                return true;
        }

        return false;
    }

    #endregion

    #region Geometry

    private static Rectangle ScreenBounds =>
        new(
            (int)Main.screenPosition.X,
            (int)Main.screenPosition.Y,
            Main.screenWidth,
            Main.screenHeight
        );

    private Rectangle EffectBounds(Vector2 center)
    {
        var diameter = EffectRadius * 2;
        return new Rectangle(
            (int)center.X - EffectRadius,
            (int)center.Y - EffectRadius,
            diameter,
            diameter
        );
    }

    #endregion

    #region Configuration

    protected abstract int PrimaryNPCID { get; }
    protected virtual int[] ExtraNPCIDs => [];

    protected virtual int? TrackedProjectileID => null;

    protected virtual int EffectRadius => 5000;

    #endregion
}