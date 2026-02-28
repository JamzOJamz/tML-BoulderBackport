using BoulderBackport.Common.ID;
using BoulderBackport.Content.Configs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BoulderBackport.Content.Scenes.MusicScenes;

public class QueenBeeMusicScene : ProximityMusicSceneEffect
{
    public override SceneEffectPriority Priority => SceneEffectPriority.BossLow;

    protected override int PrimaryNPCID => NPCID.QueenBee;
    public override int Music => NewMusicID.QueenBee;

    protected override bool CanActivate(Player player) => BackportConfig.Instance.QueenBeeBossMusic;
}