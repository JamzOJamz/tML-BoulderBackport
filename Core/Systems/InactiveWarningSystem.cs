using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace BoulderBackport.Core.Systems;

public class InactiveWarningSystem : ModPlayer
{
    private static readonly LocalizedText InactiveMessage =
        Language.GetText("Mods.BoulderBackport.Misc.InactiveMessage");

    private static readonly LocalizedText TMLTerrariaVersionIs145Message =
        Language.GetText("Mods.BoulderBackport.Misc.TMLTerrariaVersionIs145Message");

    public override void OnEnterWorld()
    {
        switch (BoulderBackport.Status)
        {
            case BackportingStatus.VanillaOutdated:
                Main.NewText(InactiveMessage.Value, 255, 160, 96);
                break;
            case BackportingStatus.NotBackporting:
                Main.NewText(TMLTerrariaVersionIs145Message.Value, 255, 160, 96);
                break;
            case BackportingStatus.Backporting:
            default:
                break;
        }
    }
}