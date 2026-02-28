using BoulderBackport.Common.ID;
using BoulderBackport.Content.Configs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BoulderBackport.Content.Scenes.MusicScenes;

public class LunaticCultistMusicScene : ProximityMusicSceneEffect
{
    public override SceneEffectPriority Priority => SceneEffectPriority.BossLow;

    protected override int PrimaryNPCID => NPCID.CultistBoss;
    public override int Music => NewMusicID.LunaticCultist;

    protected override bool CanActivate(Player player) => BackportConfig.Instance.LunaticCultistBossMusic;
}