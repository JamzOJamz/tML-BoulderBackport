using BoulderBackport.Common.ID;
using BoulderBackport.Content.Configs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BoulderBackport.Content.Scenes.MusicScenes;

public class SkeletronMusicScene : ProximityMusicSceneEffect
{
    public override SceneEffectPriority Priority => SceneEffectPriority.BossLow;

    protected override int PrimaryNPCID => NPCID.SkeletronHead;
    protected override int[] ExtraNPCIDs => [NPCID.SkeletronHand];
    public override int Music => NewMusicID.Skeletron;

    protected override bool CanActivate(Player player) => BackportConfig.Instance.SkeletronBossMusic;
}