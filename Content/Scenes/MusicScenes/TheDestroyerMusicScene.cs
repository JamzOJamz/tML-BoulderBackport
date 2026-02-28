using BoulderBackport.Common.ID;
using BoulderBackport.Content.Configs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BoulderBackport.Content.Scenes.MusicScenes;

public class TheDestroyerMusicScene : ProximityMusicSceneEffect
{
    public override SceneEffectPriority Priority => SceneEffectPriority.BossLow;

    protected override int PrimaryNPCID => NPCID.TheDestroyer;
    protected override int[] ExtraNPCIDs => [NPCID.TheDestroyerBody, NPCID.TheDestroyerTail];
    public override int Music => NewMusicID.Destroyer;

    protected override bool CanActivate(Player player) => BackportConfig.Instance.TheDestroyerBossMusic;
}