using System;
using System.Collections.Generic;
using BoulderBackport.Content.Configs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace BoulderBackport.Core.Systems;

public class NPCPortraitSystem : BackportingSystemBase
{
    private static readonly Asset<Texture2D> PortraitBackground;
    private static readonly Dictionary<int, NPCPortraitProvider> Portraits = new();
    private static float _animationTimer;
    private static string _lastNPCChatText;

    static NPCPortraitSystem()
    {
        /*On_Player.SetTalkNPC += (orig, player, index, net) =>
        {
            orig(player, index, net);

            if (player.whoAmI != Main.myPlayer)
                return;

            DoNPCPortraitHop();
        };*/

        On_Main.GUIChatDrawInner += (orig, self) =>
        {
            orig(self);

            var player = Main.LocalPlayer;
            if (player.talkNPC < 0 && player.sign == -1)
                return;

            DrawNPCPortrait();
        };

        PortraitBackground = ModContent.Request<Texture2D>("Terraria/Images/TownNPCs/Portraits/Portrait_Window");
        RegisterDefaultPortraits();
    }

    public override void PostUpdateEverything()
    {
        if (Main.npcChatText != _lastNPCChatText)
            DoNPCPortraitHop();
        _lastNPCChatText = Main.npcChatText;
    }

    /// <summary>
    ///     Registers a portrait provider for an NPC type. Replaces any existing registration.
    /// </summary>
    public static void Register(int npcID, NPCPortraitProvider provider)
    {
        Portraits[npcID] = provider;
    }

    // Shorthand for the most common case: one portrait with an optional shimmer variant.
    private static void RegisterSimple(int npcID, string basePath, bool hasShimmerVariant = true)
    {
        var provider = NPCPortraitProvider.Prioritized();

        if (hasShimmerVariant)
            provider.With(NPCPortraitConditions.IsShimmered, basePath + "_shimmer");

        provider.Default(basePath);
        Register(npcID, provider);
    }

    private static void RegisterDefaultPortraits()
    {
        const string root = "Terraria/Images/TownNPCs/Portraits";

        // Pre-Hardmode NPCs
        RegisterSimple(NPCID.Guide, $"{root}/Portrait_Guide");
        RegisterSimple(NPCID.Merchant, $"{root}/Portrait_Merchant");
        RegisterSimple(NPCID.Nurse, $"{root}/Portrait_Nurse");
        RegisterSimple(NPCID.Demolitionist, $"{root}/Portrait_Demolitionist");
        RegisterSimple(NPCID.DyeTrader, $"{root}/Portrait_DyeTrader");
        RegisterSimple(NPCID.Angler, $"{root}/Portrait_Angler");
        Register(NPCID.BestiaryGirl, NPCPortraitProvider.Prioritized()
            .With(npc => NPCPortraitConditions.IsLycanthrope(npc) && NPCPortraitConditions.IsShimmered(npc),
                $"{root}/Portrait_Zoologistb_shimmer")
            .With(NPCPortraitConditions.IsLycanthrope, $"{root}/Portrait_Zoologistb")
            .With(NPCPortraitConditions.IsShimmered, $"{root}/Portrait_Zoologista_shimmer")
            .Default($"{root}/Portrait_Zoologista"));
        RegisterSimple(NPCID.Dryad, $"{root}/Portrait_Dryad");
        RegisterSimple(NPCID.Painter, $"{root}/Portrait_Painter");
        RegisterSimple(NPCID.Golfer, $"{root}/Portrait_Golfer");
        RegisterSimple(NPCID.ArmsDealer, $"{root}/Portrait_ArmsDealer");
        RegisterSimple(NPCID.DD2Bartender, $"{root}/Portrait_Tavernkeep");
        RegisterSimple(NPCID.Stylist, $"{root}/Portrait_Stylist");
        RegisterSimple(NPCID.GoblinTinkerer, $"{root}/Portrait_GoblinTinkerer");
        RegisterSimple(NPCID.WitchDoctor, $"{root}/Portrait_WitchDoctor");
        RegisterSimple(NPCID.Clothier, $"{root}/Portrait_Clothier");
        RegisterSimple(NPCID.Mechanic, $"{root}/Portrait_Mechanic");
        RegisterSimple(NPCID.PartyGirl, $"{root}/Portrait_PartyGirl");

        // Hardmode NPCs
        RegisterSimple(NPCID.Wizard, $"{root}/Portrait_Wizard");
        RegisterSimple(NPCID.TaxCollector, $"{root}/Portrait_TaxCollector");
        RegisterSimple(NPCID.Truffle, $"{root}/Portrait_Truffle");
        RegisterSimple(NPCID.Pirate, $"{root}/Portrait_Pirate");
        RegisterSimple(NPCID.Steampunker, $"{root}/Portrait_Steampunker");
        RegisterSimple(NPCID.Cyborg, $"{root}/Portrait_Cyborg");
        RegisterSimple(NPCID.SantaClaus, $"{root}/Portrait_Santa");
        RegisterSimple(NPCID.Princess, $"{root}/Portrait_Princess");

        // Other NPCs
        RegisterSimple(NPCID.TravellingMerchant, $"{root}/Portrait_TravellingMerchant");
        RegisterSimple(NPCID.OldMan, $"{root}/Portrait_OldMan");
        RegisterSimple(NPCID.SkeletonMerchant, $"{root}/Portrait_SkeletonMerchant");

        // Zoologist town pets (variant-driven, no shimmer)
        Register(NPCID.TownDog, NPCPortraitProvider.Prioritized()
            .With(NPCPortraitConditions.HasVariant(0), $"{root}/Portrait_Dog_Labrador")
            .With(NPCPortraitConditions.HasVariant(1), $"{root}/Portrait_Dog_PitBull")
            .With(NPCPortraitConditions.HasVariant(2), $"{root}/Portrait_Dog_Beagle")
            .With(NPCPortraitConditions.HasVariant(3), $"{root}/Portrait_Dog_Corgi")
            .With(NPCPortraitConditions.HasVariant(4), $"{root}/Portrait_Dog_Dalmatian")
            .With(NPCPortraitConditions.HasVariant(5), $"{root}/Portrait_Dog_Husky")
            .Default($"{root}/Portrait_Dog_Labrador"));

        Register(NPCID.TownCat, NPCPortraitProvider.Prioritized()
            .With(NPCPortraitConditions.HasVariant(0), $"{root}/Portrait_Cat_Siamese")
            .With(NPCPortraitConditions.HasVariant(1), $"{root}/Portrait_Cat_Black")
            .With(NPCPortraitConditions.HasVariant(2), $"{root}/Portrait_Cat_Orangetabby")
            .With(NPCPortraitConditions.HasVariant(3), $"{root}/Portrait_Cat_RussianBlue")
            .With(NPCPortraitConditions.HasVariant(4), $"{root}/Portrait_Cat_Silver")
            .With(NPCPortraitConditions.HasVariant(5), $"{root}/Portrait_Cat_White")
            .Default($"{root}/Portrait_Cat_Black"));

        Register(NPCID.TownBunny, NPCPortraitProvider.Prioritized()
            .With(NPCPortraitConditions.HasVariant(0), $"{root}/Portrait_Bunny_White")
            .With(NPCPortraitConditions.HasVariant(1), $"{root}/Portrait_Bunny_Angora")
            .With(NPCPortraitConditions.HasVariant(2), $"{root}/Portrait_Bunny_Dutch")
            .With(NPCPortraitConditions.HasVariant(3), $"{root}/Portrait_Bunny_Flemish")
            .With(NPCPortraitConditions.HasVariant(4), $"{root}/Portrait_Bunny_Lop")
            .With(NPCPortraitConditions.HasVariant(5), $"{root}/Portrait_Bunny_Silver")
            .Default($"{root}/Portrait_Bunny_Angora"));

        // Town Slimes (no shimmer, no variants)
        RegisterSimple(NPCID.TownSlimeCopper, $"{root}/Portrait_SlimeSquire", hasShimmerVariant: false);
        RegisterSimple(NPCID.TownSlimePurple, $"{root}/Portrait_SlimeClumsy", hasShimmerVariant: false);
        RegisterSimple(NPCID.TownSlimeBlue, $"{root}/Portrait_SlimeNerdy", hasShimmerVariant: false);
        RegisterSimple(NPCID.TownSlimeRed, $"{root}/Portrait_SlimeSurly", hasShimmerVariant: false);
        RegisterSimple(NPCID.TownSlimeYellow, $"{root}/Portrait_SlimeMystic", hasShimmerVariant: false);
        RegisterSimple(NPCID.TownSlimeOld, $"{root}/Portrait_SlimeElder", hasShimmerVariant: false);
        RegisterSimple(NPCID.TownSlimeGreen, $"{root}/Portrait_SlimeCool", hasShimmerVariant: false);
        RegisterSimple(NPCID.TownSlimeRainbow, $"{root}/Portrait_SlimeDiva", hasShimmerVariant: false);
    }

    internal static void ExperimentalAutoDetectAndRegisterModdedPortraits()
    {
        foreach (var mod in ModLoader.Mods)
        {
            if (mod.Side != ModSide.Both) continue;

            var npcs = mod.Content.GetContent<ModNPC>();
            foreach (var npc in npcs)
            {
                if (!npc.NPC.townNPC) continue;

                var baseTexturePath = npc.Texture + "_Portrait";
                var shimmerTexturePath = npc.Texture + "_Shimmer_Portrait";

                var hasPortrait = ModContent.HasAsset(baseTexturePath);
                var hasShimmerPortrait = ModContent.HasAsset(shimmerTexturePath);

                if (!hasPortrait) continue;

                var provider = NPCPortraitProvider.Prioritized();

                if (hasShimmerPortrait)
                    provider.With(NPCPortraitConditions.IsShimmered, shimmerTexturePath);

                provider.Default(baseTexturePath);
                Register(npc.Type, provider);
            }
        }
    }

    private static void DoNPCPortraitHop() => _animationTimer = 0;

    private static void DrawNPCPortrait()
    {
        var talkNPC = Main.npc[Main.LocalPlayer.talkNPC];
        var isModded = talkNPC.type >= NPCID.Count;

        if ((isModded && !BackportConfig.Instance.ModdedTownNPCPortraits) || (!isModded && BackportConfig.Instance.VanillaTownNPCPortraits))
            return;

        if (!Portraits.TryGetValue(talkNPC.type, out var provider))
            return;

        var portrait = provider.Resolve(talkNPC);
        if (portrait == null)
            return;

        // ReSharper disable once PossibleLossOfFraction
        var npcChatTopLeft = new Vector2(Main.screenWidth / 2 - 250, 100);
        var drawPos = npcChatTopLeft + new Vector2(-62, 62);

        _animationTimer += (float)Main._drawInterfaceGameTime.ElapsedGameTime.TotalSeconds;

        if (_animationTimer <= 2f)
        {
            const double animFrames = 80.0;
            const float hopHeight = 0.25f;
            var progress = (float)EaseOutBounce(
                Utils.Clamp((int)(_animationTimer * 60f), 0.0, animFrames) / animFrames);
            drawPos.Y -= 56 * hopHeight * (1f - progress);
        }

        var chatBack = new Color(200, 200, 200, 200);
        var spriteBatch = Main.spriteBatch;
        var bg = PortraitBackground.Value;

        spriteBatch.Draw(bg, drawPos, bg.Frame(), chatBack,
            0f, bg.Size() / 2, Vector2.One, SpriteEffects.None, 0f);
        spriteBatch.Draw(portrait.Value, drawPos, portrait.Frame(), Color.White,
            0f, portrait.Size() / 2, Vector2.One, SpriteEffects.None, 0f);

        var textFont = FontAssets.ItemStack.Value;
        var givenName = talkNPC.GivenName;
        var textOrigin = textFont.MeasureString(givenName) / 2f;
        ChatManager.DrawColorCodedStringWithShadow(spriteBatch, textFont, givenName,
            drawPos + new Vector2(0f, 57), Color.White, 0f, textOrigin, Vector2.One);
    }

    private static double EaseOutBounce(double x) =>
        BounceEaseOut(x, bounces: 4, elasticity: 2.0);

    private static double BounceEaseOut(double t, int bounces, double elasticity)
    {
        var decay = Math.Pow(1.0 - t, elasticity);
        var oscillation = Math.Abs(Math.Sin(t * bounces * Math.PI));
        return 1.0 - decay * oscillation;
    }
}

internal sealed class ModdedNPCPortraitLoadBootstrapperPlayer : ModPlayer
{
    private bool _activatedOnce;

    public override void OnEnterWorld()
    {
        if (_activatedOnce)
            return;

        _activatedOnce = true;
        NPCPortraitSystem.ExperimentalAutoDetectAndRegisterModdedPortraits();
    }
}