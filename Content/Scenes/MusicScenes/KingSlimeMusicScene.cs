using BoulderBackport.Common.ID;
using BoulderBackport.Content.Configs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BoulderBackport.Content.Scenes.MusicScenes;

public class KingSlimeMusicScene : ProximityMusicSceneEffect
{
    public override SceneEffectPriority Priority => SceneEffectPriority.BossLow;

    protected override int PrimaryNPCID => NPCID.KingSlime;
    public override int Music => NewMusicID.KingSlime;

    protected override bool CanActivate(Player player) => BackportConfig.Instance.KingSlimeBossMusic;
}