using BoulderBackport.Common.ID;
using BoulderBackport.Content.Configs;
using Terraria;
using Terraria.ModLoader;

namespace BoulderBackport.Content.Scenes.MusicScenes;

public class TorchGodMusicScene : ModSceneEffect
{
    public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;
    public override int Music => NewMusicID.TorchGodInstrumental;

    public override bool IsSceneEffectActive(Player player) =>
        BackportConfig.Instance.TorchGodBossMusic && player.happyFunTorchTime;
}