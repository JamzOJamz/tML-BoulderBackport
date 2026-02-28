using BoulderBackport.Common.ID;
using BoulderBackport.Content.Configs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BoulderBackport.Content.Scenes.MusicScenes;

public class TheTwinsMusicScene : ProximityMusicSceneEffect
{
    public override SceneEffectPriority Priority => SceneEffectPriority.BossLow;

    protected override int PrimaryNPCID => NPCID.Retinazer;
    protected override int[] ExtraNPCIDs => [NPCID.Spazmatism];
    public override int Music => NewMusicID.Twins;

    protected override bool CanActivate(Player player) => BackportConfig.Instance.TheTwinsBossMusic;
}