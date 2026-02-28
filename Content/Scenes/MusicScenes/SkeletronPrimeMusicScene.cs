using BoulderBackport.Common.ID;
using BoulderBackport.Content.Configs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BoulderBackport.Content.Scenes.MusicScenes;

public class SkeletronPrimeMusicScene : ProximityMusicSceneEffect
{
    public override SceneEffectPriority Priority => SceneEffectPriority.BossLow;

    protected override int PrimaryNPCID => NPCID.SkeletronPrime;
    protected override int[] ExtraNPCIDs => [NPCID.PrimeCannon, NPCID.PrimeSaw, NPCID.PrimeVice, NPCID.PrimeLaser];
    public override int Music => NewMusicID.SkeletronPrime;

    protected override bool CanActivate(Player player) => BackportConfig.Instance.SkeletronPrimeBossMusic;
}