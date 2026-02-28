using BoulderBackport.Common.ID;
using BoulderBackport.Content.Configs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BoulderBackport.Content.Scenes.MusicScenes;

public class EaterOfWorldsMusicScene : ProximityMusicSceneEffect
{
    public override SceneEffectPriority Priority => SceneEffectPriority.BossLow;

    protected override int PrimaryNPCID => NPCID.EaterofWorldsHead;
    protected override int[] ExtraNPCIDs => [NPCID.EaterofWorldsBody, NPCID.EaterofWorldsTail];
    public override int Music => NewMusicID.EaterOfWorlds;

    protected override bool CanActivate(Player player) => BackportConfig.Instance.EaterOfWorldsBossMusic;
}