using Terraria.ModLoader;

namespace BoulderBackport.Core.Systems;

public class BackportingSystemBase : ModSystem
{
    public override bool IsLoadingEnabled(Mod mod) => BoulderBackport.Status == BackportingStatus.Backporting;
}