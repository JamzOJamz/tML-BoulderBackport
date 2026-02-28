using System.Runtime.CompilerServices;
using BoulderBackport.Content.Configs;
using BoulderBackport.Content.Sources;
using MonoMod.Cil;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace BoulderBackport.Core.Systems;

public class SoundBackportingSystem : BackportingSystemBase
{
    private static readonly string[] VolumeBalancedSounds =
    [
        "Sounds\\Item_36.xnb",
        "Sounds\\Item_38.xnb"
    ];

    private static readonly string[] ThunderSounds =
    [
        "Sounds\\Thunder_0.xnb",
        "Sounds\\Thunder_1.xnb",
        "Sounds\\Thunder_2.xnb",
        "Sounds\\Thunder_3.xnb",
        "Sounds\\Thunder_4.xnb",
        "Sounds\\Thunder_5.xnb"
    ];

    private bool _hasChangedThunderSoundStyle;

    public static SoundStyle SonarPotion { get; private set; }
    public static SoundStyle TrashItem { get; private set; }

    public override void Load()
    {
        if (BackportConfig.Instance.VolumeBalancingChanges)
        {
            foreach (var path in VolumeBalancedSounds)
            {
                BoulderBackport.Instance.DebugLog($"Loading new sound effect at {path}");
                SelectBackportContentSource.Add(path);
            }
        }

        if (BackportConfig.Instance.ThunderSounds)
        {
            BoulderBackport.Instance.DebugLog("Loading new thunder sound effects");

            // Resize thunder to 6 variants to match the new backported thunder sounds
            ref var soundRef = ref Unsafe.AsRef(in SoundID.Thunder);
            soundRef = new SoundStyle("Terraria/Sounds/Thunder_", 0, 6, SoundType.Ambient)
                { PitchVariance = 0.2f, RerollAttempts = 5, LimitsArePerVariant = true };

            _hasChangedThunderSoundStyle = true;
        }

        if (BackportConfig.Instance.TrashItemSound)
        {
            BoulderBackport.Instance.DebugLog("Loading new trash item sound effect");

            // Load new TrashItem sound...
            TrashItem = new SoundStyle("Terraria/Sounds/Custom/trash_item_", 0, 2)
            {
                Volume = 0.55f,
                PitchVariance = 0.25f
            };

            // ...and apply IL edits to make it play when trashing items in the inventory
            IL_ItemSlot.SellOrTrash += il =>
            {
                try
                {
                    // Start the Cursor at the start
                    var c = new ILCursor(il);

                    // Locate where we are going to perform our edit
                    c.GotoNext(i => i.MatchLdfld(typeof(Item).GetField(nameof(Item.favorited))!),
                        i => i.MatchBrtrue(out _),
                        i => i.MatchLdcI4(7));

                    // Advance the cursor a bit
                    c.Index += 2;

                    // Inject a delegate to play the new sound effect
                    c.EmitDelegate(() =>
                    {
                        if (BackportConfig.Instance.TrashItemSound)
                            SoundEngine.PlaySound(TrashItem);
                    });
                }
                catch
                {
                    MonoModHooks.DumpIL(Mod, il);
                }
            };

            IL_ItemSlot.LeftClick_ItemArray_int_int += il =>
            {
                try
                {
                    // Start the Cursor at the start
                    var c = new ILCursor(il);

                    // Locate where we are going to perform our edit
                    c.GotoNext(i => i.MatchLdcI4(7));

                    // Load the context argument onto the stack so we can use it in our delegate
                    c.EmitLdarg1();

                    // Inject a delegate to play the new sound effect
                    c.EmitDelegate((int context) =>
                    {
                        if (BackportConfig.Instance.TrashItemSound && context == 6 &&
                            Main.mouseItem.type == ItemID.None)
                            SoundEngine.PlaySound(TrashItem);
                    });
                }
                catch
                {
                    MonoModHooks.DumpIL(Mod, il);
                }
            };
        }

        if (BackportConfig.Instance.SonarPotionSound)
        {
            BoulderBackport.Instance.DebugLog("Loading new sonar potion sound effect");

            // Load new SonarPotion sound...
            SonarPotion = new SoundStyle("Terraria/Sounds/Custom/sonar_potion")
            {
                Volume = 0.65f,
                PitchVariance = 0.03f
            };

            // ...and apply an IL edit to make it play when catching a fish while using a Sonar Potion
            IL_Projectile.FishingCheck += il =>
            {
                try
                {
                    // Start the Cursor at the start
                    var c = new ILCursor(il);

                    // Locate where we are going to perform our edit
                    c.GotoNext(i => i.MatchNewobj(typeof(Item).GetConstructor([])!));

                    // Load the Projectile instance onto the stack so we can use it in our delegate
                    c.EmitLdarg0();

                    // Inject a delegate to play the new sound effect
                    c.EmitDelegate((Projectile projectile) =>
                    {
                        if (BackportConfig.Instance.SonarPotionSound)
                            SoundEngine.PlaySound(SonarPotion, projectile.position);
                    });

                    // Locate the second place we need to perform an edit (the first is for catching an item, the second is for catching an enemy)
                    c.GotoNext(i => i.MatchLdfld(typeof(Player).GetField(nameof(Player.sonarPotion))!));

                    // Advance the cursor a bit
                    c.Index += 2;

                    // Load the Projectile instance onto the stack so we can use it in our delegate
                    c.EmitLdarg0();

                    // Inject a delegate to play the new sound effect
                    c.EmitDelegate((Projectile projectile) =>
                    {
                        if (BackportConfig.Instance.SonarPotionSound)
                            SoundEngine.PlaySound(SonarPotion, projectile.position);
                    });
                }
                catch
                {
                    MonoModHooks.DumpIL(Mod, il);
                }
            };
        }
    }

    public override void Unload()
    {
        if (!_hasChangedThunderSoundStyle) return;

        // Restore thunder to 7 variants to match the original vanilla thunder sounds
        ref var soundRef = ref Unsafe.AsRef(in SoundID.Thunder);
        soundRef = new SoundStyle("Terraria/Sounds/Thunder_", 0, 7, SoundType.Ambient)
            { PitchVariance = 0.2f, RerollAttempts = 6, LimitsArePerVariant = true };
    }
}