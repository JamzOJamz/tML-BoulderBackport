using BoulderBackport.Content.Configs;
using BoulderBackport.Content.Sources;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;

namespace BoulderBackport.Core.Systems;

public class TextureBackportingSystem : BackportingSystemBase
{
    private static readonly int[] NewBuffTexturesToLoad =
    [
        BuffID.ShadowOrb,
        BuffID.BabySlime,
        BuffID.ImpMinion,
        // BuffID.MiniMinotaur, // TODO: Don't change this buff's texture for now since the pet projectile texture hasn't been updated yet and the buff icon would look weird without it
        BuffID.DrillMount,
        BuffID.CrimsonHeart
    ];

    private static readonly int[] NewGlowMaskTexturesToLoad =
    [
        GlowMaskID.MartianConduitWallItem,
        GlowMaskID.MartianArmorDye,
        GlowMaskID.ChlorophyteDye,
        GlowMaskID.PixieDye,
        GlowMaskID.WispDye,
        GlowMaskID.InfernalWispDye,
        GlowMaskID.UnicornWispDye,
        GlowMaskID.ShadowflameApparation,
        GlowMaskID.LunarCraftingStation
        // GlowMaskID.JimsHead // Jim's Helmet is weird in 1.4.5, skipping it for now
    ];

    private static readonly int[] NewGoreTexturesToLoad =
    [
        // Fire Imp
        45,
        46,
        47,
        // Hornet
        70,
        71,
        // Lihzahrd
        258,
        259,
        260,
        261,
        // Flying Snake
        317,
        318,
        319,
        // Witch Doctor (Shimmered)
        1369
    ];

    private static readonly int[] NewItemTexturesToLoad =
    [
        ItemID.ShadowOrb,
        ItemID.RedBanner,
        ItemID.GreenBanner,
        ItemID.BlueBanner,
        ItemID.YellowBanner,
        ItemID.Cloud,
        ItemID.CloudWall,
        ItemID.RainCloud,
        ItemID.FleshGrinder,
        ItemID.LihzahrdBrick,
        ItemID.LihzahrdBrickWall,
        ItemID.LihzahrdStatue,
        ItemID.LihzahrdWatcherStatue,
        ItemID.LihzahrdGuardianStatue,
        ItemID.WaterChest,
        ItemID.SlimeStaff,
        ItemID.EyeofCthulhuTrophy,
        ItemID.EaterofWorldsTrophy,
        ItemID.BrainofCthulhuTrophy,
        ItemID.SkeletronTrophy,
        ItemID.QueenBeeTrophy,
        ItemID.WallofFleshTrophy,
        ItemID.DestroyerTrophy,
        ItemID.SkeletronPrimeTrophy,
        ItemID.RetinazerTrophy,
        ItemID.SpazmatismTrophy,
        ItemID.PlanteraTrophy,
        ItemID.GolemTrophy,
        ItemID.WallSkeleton,
        ItemID.HangingSkeleton,
        ItemID.Catacomb,
        ItemID.ImbuingStation,
        ItemID.GothicChair,
        ItemID.GothicTable,
        ItemID.GothicWorkBench,
        ItemID.GothicBookcase,
        ItemID.JimsHelmet,
        ItemID.JimsBreastplate,
        ItemID.JimsLeggings,
        ItemID.AnglerFishBanner,
        ItemID.AngryNimbusBanner,
        ItemID.AnomuraFungusBanner,
        ItemID.AntlionBanner,
        ItemID.ArapaimaBanner,
        ItemID.ArmoredSkeletonBanner,
        ItemID.BatBanner,
        ItemID.BirdBanner,
        ItemID.BlackRecluseBanner,
        ItemID.BloodFeederBanner,
        ItemID.BloodJellyBanner,
        ItemID.BloodCrawlerBanner,
        ItemID.BoneSerpentBanner,
        ItemID.BunnyBanner,
        ItemID.ChaosElementalBanner,
        ItemID.MimicBanner,
        ItemID.ClownBanner,
        ItemID.CorruptBunnyBanner,
        ItemID.CorruptGoldfishBanner,
        ItemID.CrabBanner,
        ItemID.CrimeraBanner,
        ItemID.CrimsonAxeBanner,
        ItemID.CursedHammerBanner,
        ItemID.DemonBanner,
        ItemID.DemonEyeBanner,
        ItemID.DerplingBanner,
        ItemID.EaterofSoulsBanner,
        ItemID.EnchantedSwordBanner,
        ItemID.ZombieEskimoBanner,
        ItemID.FaceMonsterBanner,
        ItemID.FloatyGrossBanner,
        ItemID.FlyingFishBanner,
        ItemID.FlyingSnakeBanner,
        ItemID.FrankensteinBanner,
        ItemID.FungiBulbBanner,
        ItemID.FungoFishBanner,
        ItemID.GastropodBanner,
        ItemID.GoblinThiefBanner,
        ItemID.GoblinSorcererBanner,
        ItemID.GoblinPeonBanner,
        ItemID.GoblinScoutBanner,
        ItemID.GoblinWarriorBanner,
        ItemID.GoldfishBanner,
        ItemID.HarpyBanner,
        ItemID.HellbatBanner,
        ItemID.HerplingBanner,
        ItemID.HornetBanner,
        ItemID.IceElementalBanner,
        ItemID.IcyMermanBanner,
        ItemID.FireImpBanner,
        ItemID.JellyfishBanner,
        ItemID.JungleCreeperBanner,
        ItemID.LihzahrdBanner,
        ItemID.ManEaterBanner,
        ItemID.MeteorHeadBanner,
        ItemID.MothBanner,
        ItemID.MummyBanner,
        ItemID.MushiLadybugBanner,
        ItemID.ParrotBanner,
        ItemID.PigronBanner,
        ItemID.PiranhaBanner,
        ItemID.PirateBanner,
        ItemID.PixieBanner,
        ItemID.RaincoatZombieBanner,
        ItemID.ReaperBanner,
        ItemID.SharkBanner,
        ItemID.SkeletonBanner,
        ItemID.SkeletonMageBanner,
        ItemID.SlimeBanner,
        ItemID.SnowFlinxBanner,
        ItemID.SpiderBanner,
        ItemID.SporeZombieBanner,
        ItemID.SwampThingBanner,
        ItemID.TortoiseBanner,
        ItemID.ToxicSludgeBanner,
        ItemID.UmbrellaSlimeBanner,
        ItemID.UnicornBanner,
        ItemID.VampireBanner,
        ItemID.VultureBanner,
        ItemID.NypmhBanner,
        ItemID.WerewolfBanner,
        ItemID.WolfBanner,
        ItemID.WorldFeederBanner,
        ItemID.WormBanner,
        ItemID.WraithBanner,
        ItemID.WyvernBanner,
        ItemID.ZombieBanner,
        ItemID.BarStool,
        ItemID.BanquetTable,
        ItemID.Bar,
        ItemID.MourningWoodTrophy,
        ItemID.PumpkingTrophy,
        ItemID.PineTreeBlock,
        ItemID.PineDoor,
        ItemID.PineChair,
        ItemID.PineTable,
        ItemID.IceQueenTrophy,
        ItemID.SantaNK1Trophy,
        ItemID.EverscreamTrophy,
        ItemID.BlacksmithRack,
        ItemID.CarpentryRack,
        ItemID.HelmetRack,
        ItemID.SpearRack,
        ItemID.SwordRack,
        ItemID.WaterGun,
        ItemID.Minecart,
        ItemID.ImpStaff,
        ItemID.GoldfishTrophy,
        ItemID.BunnyfishTrophy,
        ItemID.SwordfishTrophy,
        ItemID.SharkteethTrophy,
        ItemID.KingSlimeTrophy,
        ItemID.SlimeHook,
        //ItemID.TartarSauce, // TODO: This item changed to something completely different in 1.4.5 (Beguiling Lyre), skipping it for now
        ItemID.DukeFishronTrophy,
        ItemID.SlimeGun,
        ItemID.Flairon,
        ItemID.WeaponRack,
        ItemID.LunarTabletFragment,
        ItemID.SolarTablet,
        ItemID.MartianBed, // Flipped horizontally in 1.4.5 to match all other bed item sprites
        ItemID.MartianHoverChair, // Flipped horizontally in 1.4.5 to match all other chair item sprites
        ItemID.MartianConduitWall, // Additional padding added to the sprite (?)
        ItemID.AngryTrapperBanner,
        ItemID.ArmoredVikingBanner,
        ItemID.BlackSlimeBanner,
        ItemID.BlueArmoredBonesBanner,
        ItemID.BlueCultistArcherBanner,
        ItemID.BlueCultistCasterBanner,
        ItemID.BoneLeeBanner,
        ItemID.ClingerBanner,
        ItemID.CochinealBeetleBanner,
        ItemID.CorruptPenguinBanner,
        ItemID.CorruptSlimeBanner,
        ItemID.CorruptorBanner,
        ItemID.CrimslimeBanner,
        ItemID.CursedSkullBanner,
        ItemID.CyanBeetleBanner,
        ItemID.DevourerBanner,
        ItemID.DiablolistBanner,
        ItemID.DungeonSlimeBanner,
        ItemID.DungeonSpiritBanner,
        ItemID.ElfArcherBanner,
        ItemID.ElfCopterBanner,
        ItemID.EyezorBanner,
        ItemID.FlockoBanner,
        ItemID.GhostBanner,
        ItemID.GiantBatBanner,
        ItemID.GiantCursedSkullBanner,
        ItemID.GiantFlyingFoxBanner,
        ItemID.GingerbreadManBanner,
        ItemID.GoblinArcherBanner,
        ItemID.GreenSlimeBanner,
        ItemID.HeadlessHorsemanBanner,
        ItemID.HellArmoredBonesBanner,
        ItemID.HellhoundBanner,
        ItemID.HoppinJackBanner,
        ItemID.IceBatBanner,
        ItemID.IceGolemBanner,
        ItemID.IceSlimeBanner,
        ItemID.IchorStickerBanner,
        ItemID.IlluminantBatBanner,
        ItemID.IlluminantSlimeBanner,
        ItemID.JungleBatBanner,
        ItemID.JungleSlimeBanner,
        ItemID.KrampusBanner,
        ItemID.LacBeetleBanner,
        ItemID.LavaBatBanner,
        ItemID.LavaSlimeBanner,
        ItemID.MartianBrainscramblerBanner,
        ItemID.MartianDroneBanner,
        ItemID.MartianEngineerBanner,
        ItemID.MartianGigazapperBanner,
        ItemID.MartianGreyGruntBanner,
        ItemID.MartianOfficerBanner,
        ItemID.MartianRaygunnerBanner,
        ItemID.MartianScutlixGunnerBanner,
        ItemID.MartianTeslaTurretBanner,
        ItemID.MotherSlimeBanner,
        ItemID.NecromancerBanner,
        ItemID.NutcrackerBanner,
        ItemID.PaladinBanner,
        ItemID.PenguinBanner,
        ItemID.PinkyBanner,
        ItemID.PoltergeistBanner,
        ItemID.PossessedArmorBanner,
        ItemID.PresentMimicBanner,
        ItemID.PurpleSlimeBanner,
        ItemID.RaggedCasterBanner,
        ItemID.RainbowSlimeBanner,
        ItemID.RavenBanner,
        ItemID.RedSlimeBanner,
        ItemID.RuneWizardBanner,
        ItemID.RustyArmoredBonesBanner,
        ItemID.ScarecrowBanner,
        ItemID.ScutlixBanner,
        ItemID.SkeletonArcherBanner,
        ItemID.SkeletonCommandoBanner,
        ItemID.SkeletonSniperBanner,
        ItemID.SlimerBanner,
        ItemID.SnatcherBanner,
        ItemID.SpikedIceSlimeBanner,
        ItemID.SpikedJungleSlimeBanner,
        ItemID.SplinterlingBanner,
        ItemID.SquidBanner,
        ItemID.TacticalSkeletonBanner,
        ItemID.TheGroomBanner,
        ItemID.TimBanner,
        ItemID.UndeadMinerBanner,
        ItemID.UndeadVikingBanner,
        ItemID.YellowSlimeBanner,
        ItemID.YetiBanner,
        ItemID.ZombieElfBanner,
        ItemID.BewitchingTable,
        ItemID.AlchemyTable,
        ItemID.CrimsonHeart, // Remade to be anatomically correct in 1.4.5
        ItemID.MinecartMech, // Flipped horizontally in 1.4.5 to match all other minecart item sprites
        ItemID.AncientCultistTrophy,
        ItemID.MartianSaucerTrophy,
        ItemID.FlyingDutchmanTrophy,
        ItemID.GoblinSummonerBanner,
        ItemID.SalamanderBanner,
        ItemID.GiantShellyBanner,
        ItemID.CrawdadBanner,
        ItemID.FritzBanner,
        ItemID.CreatureFromTheDeepBanner,
        ItemID.DrManFlyBanner,
        ItemID.MothronBanner,
        ItemID.ThePossessedBanner,
        ItemID.PsychoBanner,
        ItemID.DeadlySphereBanner,
        ItemID.NailheadBanner,
        ItemID.MedusaBanner,
        ItemID.GreekSkeletonBanner,
        ItemID.GraniteFlyerBanner,
        ItemID.GraniteGolemBanner,
        ItemID.BloodZombieBanner,
        ItemID.DripplerBanner,
        ItemID.TombCrawlerBanner,
        ItemID.DuneSplicerBanner,
        ItemID.FlyingAntlionBanner,
        ItemID.WalkingAntlionBanner,
        ItemID.DesertGhoulBanner,
        ItemID.DesertLamiaBanner,
        ItemID.DesertDjinnBanner,
        ItemID.DesertBasiliskBanner,
        ItemID.RavagerScorpionBanner,
        ItemID.PirateCaptainBanner,
        ItemID.PirateDeadeyeBanner,
        ItemID.PirateCorsairBanner,
        ItemID.PirateCrossbowerBanner,
        ItemID.MartianWalkerBanner,
        ItemID.RedDevilBanner,
        ItemID.PinkJellyfishBanner,
        ItemID.GreenJellyfishBanner,
        ItemID.DarkMummyBanner,
        ItemID.LightMummyBanner,
        ItemID.AngryBonesBanner,
        ItemID.IceTortoiseBanner,
        ItemID.LunarCraftingStation,
        ItemID.SandSlimeBanner,
        ItemID.SeaSnailBanner,
        ItemID.SnowCloudBlock,
        ItemID.SandElementalBanner,
        ItemID.SandsharkBanner,
        ItemID.SandsharkCorruptBanner,
        ItemID.SandsharkCrimsonBanner,
        ItemID.SandsharkHallowedBanner,
        ItemID.TumbleweedBanner,
        ItemID.CrystalChair, // Flipped horizontally in 1.4.5 to match all other chair item sprites
        ItemID.SandstoneChair, // Flipped horizontally in 1.4.5 to match all other chair item sprites
        ItemID.TheBrideBanner,
        ItemID.EyeballFlyingFishBanner,
        ItemID.BloodSquidBanner,
        ItemID.BloodEelBanner,
        ItemID.GoblinSharkBanner,
        ItemID.BloodNautilusBanner,
        ItemID.DandelionBanner,
        ItemID.BloodMummyBanner,
        ItemID.SporeSkeletonBanner,
        ItemID.SporeBatBanner,
        ItemID.LarvaeAntlionBanner,
        ItemID.CrimsonBunnyBanner,
        ItemID.CrimsonGoldfishBanner,
        ItemID.CrimsonPenguinBanner,
        ItemID.BigMimicCorruptionBanner,
        ItemID.BigMimicCrimsonBanner,
        ItemID.BigMimicHallowBanner,
        ItemID.MossHornetBanner,
        ItemID.MusicBoxQueenSlime,
        ItemID.RemnantsofDevotion,
        ItemID.BlessingfromTheHeavens,
        ItemID.ShimmerSlimeBanner
    ];

    private static readonly int[] NewNPCTexturesToLoad =
    [
        NPCID.FireImp,
        NPCID.Demolitionist,
        NPCID.Corruptor,
        NPCID.PigronCorruption,
        NPCID.PigronHallow,
        NPCID.PigronCrimson,
        NPCID.Lihzahrd,
        NPCID.LihzahrdCrawler,
        NPCID.FlyingSnake,
        NPCID.DesertDjinn,
        NPCID.DD2Bartender
    ];

    private static readonly int[] NewNPCHeadTexturesToLoad =
    [
        NPCHeadID.SlimeRainbow,
        NPCHeadID.WitchDoctorShimmered
    ];

    private static readonly int[] NewProjectileTexturesToLoad =
    [
        ProjectileID.ShadowOrb,
        ProjectileID.SporeCloud,
        // ProjectileID.BabySlime, // TODO: Frame count increased in 1.4.5, skipping it for now
        ProjectileID.FlyingImp,
        // ProjectileID.MiniMinotaur, // TODO: Frame count increased in 1.4.5, skipping it for now
        // ProjectileID.MoonLeech, // TODO: Should this be loaded even though the other Moon Lord resprites aren't?
        ProjectileID.CrimsonHeart
    ];

    private static readonly int[] NewTileTexturesToLoad =
    [
        TileID.ClosedDoor,
        TileID.OpenDoor,
        TileID.Tables,
        // TileID.Chairs, // TODO: Has a second column in 1.4.5 for new chair styles, skipping it for now
        // TileID.WorkBenches, // TODO: Has a second row in 1.4.5, skipping it for now
        // TileID.Platforms, // Only new platform types added to the sheet; old sprites didn't change
        TileID.Containers,
        TileID.DemonAltar,
        TileID.Pots,
        // TileID.ShadowOrbs, // TODO: Had frame count increased in 1.4.5, skipping it for now
        // TileID.Candles, // Only new candle types added to the sheet; old sprites didn't change
        // TileID.Chandeliers, // Only new chandelier types added to the sheet; old sprites didn't change
        // TileID.HangingLanterns, // Only new hanging lantern types added to the sheet; old sprites didn't change
        // TileID.Beds, // TODO: Only new bed styles added to the sheet (adding a new column) but some bed sprites were slightly edited in 1.4.5, skipping it for now
        TileID.Pianos,
        // TileID.Dressers, // Only new dresser styles added to the sheet; old sprites didn't change
        // TileID.Benches, // Only new bench styles added to the sheet; old sprites didn't change
        // TileID.Bathtubs, // Only new bathtub styles added to the sheet; old sprites didn't change
        TileID.Banners,
        // TileID.Lampposts, // Only new lamppost styles added to the sheet; old sprites didn't change
        // TileID.Candelabras, // Only new candelabra styles added to the sheet; old sprites didn't change
        TileID.Bookcases,
        // TileID.GrandfatherClocks, // Only new grandfather clock styles added to the sheet; old sprites didn't change
        TileID.Statues, // Lihzahrd statues were refreshed in 1.4.5
        TileID.MusicBoxes,
        TileID.Stalactite,
        // TileID.PineTree, // TODO: Sheet was vastly expanded in 1.4.5, skipping it for now
        // TileID.Sinks, // Only new sink styles added to the sheet; old sprites didn't change
        TileID.SmallPiles,
        TileID.Cloud,
        TileID.RainCloud,
        TileID.LihzahrdBrick,
        TileID.Painting3X3,
        TileID.Painting4X3,
        // TileID.Painting6X4, // Only new painting styles added to the sheet; old sprites didn't change
        TileID.ImbuingStation,
        // TileID.Painting2X3, // Only new painting styles added to the sheet; old sprites didn't change
        TileID.BewitchingTable,
        TileID.AlchemyTable,
        TileID.LunarCraftingStation,
        // Dull variants were added as new items for the following in 1.4.5, but the original bright variants didn't change
        // TileID.TeamBlockRed,
        // TileID.TeamBlockGreen,
        // TileID.TeamBlockBlue,
        // TileID.TeamBlockYellow,
        // TileID.TeamBlockPink,
        // TileID.TeamBlockWhite,
        TileID.FakeContainers,
        TileID.SnowCloud,
        // TileID.Containers2, // Only new container styles added to the sheet; old sprites didn't change
        // TileID.FakeContainers2, // Only new fake container styles added to the sheet; old sprites didn't change
        // TileID.Tables2, // Only new table styles added to the sheet; old sprites didn't change
        TileID.WeaponsRack2,
        TileID.FallenLog,
        // TileID.Toilets, // Only new toilet styles added to the sheet; old sprites didn't change
        TileID.SeahorseCage,
        TileID.GoldSeahorseCage,
        // TileID.TeleportationPylon, // Only new pylon styles added to the sheet; old sprites didn't change
        TileID.SmallPiles2x1Echo,
        TileID.SmallPiles1x1Echo,
        TileID.PotsEcho
    ];

    private static readonly int[] NewWallTexturesToLoad =
    [
        WallID.Cloud,
        WallID.LihzahrdBrickUnsafe
    ];

    private static readonly int[] NewCursorTexturesToLoad =
    [
        CursorOverrideID.BackInventory,
        CursorOverrideID.ChestToInventory,
        CursorOverrideID.InventoryToChest
    ];

    public override void Load()
    {
        if (BackportConfig.Instance.GameplayTextures)
        {
            BoulderBackport.Instance.DebugLog("Loading new accessory face textures");
            SelectBackportContentSource.Add("Images\\Acc_Face_16.xnb");
            SelectBackportContentSource.Add("Images\\Acc_Face_18.xnb");

            // BoulderBackport.Instance.DebugLog("Loading new armor head textures");
            // SelectBackportContentSource.Add("Images\\Armor_Head_109.xnb"); // Jim's Helmet is weird in 1.4.5

            BoulderBackport.Instance.DebugLog("Loading new armor leg textures");
            // SelectBackportContentSource.Add("Images\\Armor_Legs_60.xnb"); // Jim's Leggings are weird in 1.4.5
            SelectBackportContentSource.Add("Images\\Armor_Legs_83.xnb");
            SelectBackportContentSource.Add("Images\\Armor_Legs_117.xnb");
            SelectBackportContentSource.Add("Images\\Armor_Legs_128.xnb");
            SelectBackportContentSource.Add("Images\\Armor_Legs_214.xnb");
            SelectBackportContentSource.Add("Images\\Armor_Legs_215.xnb");
            SelectBackportContentSource.Add("Images\\Armor_Legs_216.xnb");

            Mod.Logger.Info($"Total of {NewBuffTexturesToLoad.Length} new buff textures to load");

            foreach (var id in NewBuffTexturesToLoad)
            {
                BoulderBackport.Instance.DebugLog(
                    $"Loading new buff texture for buff ID {id} ({Lang.GetBuffName(id)})");
                SelectBackportContentSource.Add($"Images\\Buff_{id}.xnb");
            }

            Mod.Logger.Info($"Total of {NewGlowMaskTexturesToLoad.Length} new glow mask textures to load");

            foreach (var id in NewGlowMaskTexturesToLoad)
            {
                BoulderBackport.Instance.DebugLog($"Loading new glow mask texture for glow mask ID {id}");
                SelectBackportContentSource.Add($"Images\\Glow_{id}.xnb");
            }

            Mod.Logger.Info($"Total of {NewGoreTexturesToLoad.Length} new gore textures to load");

            foreach (var id in NewGoreTexturesToLoad)
            {
                BoulderBackport.Instance.DebugLog($"Loading new gore texture for gore ID {id}");
                SelectBackportContentSource.Add($"Images\\Gore_{id}.xnb");
            }

            Mod.Logger.Info($"Total of {NewItemTexturesToLoad.Length} new item textures to load");

            foreach (var id in NewItemTexturesToLoad)
            {
                BoulderBackport.Instance.DebugLog(
                    $"Loading new item texture for item ID {id} ({Lang.GetItemNameValue(id)})");
                SelectBackportContentSource.Add($"Images\\Item_{id}.xnb");
            }

            BoulderBackport.Instance.DebugLog("Loading new mount textures");
            SelectBackportContentSource.Add("Images\\Mount_Minecart.xnb");
            SelectBackportContentSource.Add("Images\\Mount_MinecartWood.xnb");
            SelectBackportContentSource.Add("Images\\Mount_Pigron.xnb");

            Mod.Logger.Info($"Total of {NewNPCTexturesToLoad.Length} new NPC textures to load");

            foreach (var id in NewNPCTexturesToLoad)
            {
                BoulderBackport.Instance.DebugLog(
                    $"Loading new NPC texture for NPC ID {id} ({Lang.GetNPCNameValue(id)})");
                SelectBackportContentSource.Add($"Images\\NPC_{id}.xnb");
            }

            Mod.Logger.Info($"Total of {NewProjectileTexturesToLoad.Length} new projectile textures to load");

            foreach (var id in NewProjectileTexturesToLoad)
            {
                BoulderBackport.Instance.DebugLog(
                    $"Loading new projectile texture for projectile ID {id} ({Lang.GetProjectileName(id).Value})");
                SelectBackportContentSource.Add($"Images\\Projectile_{id}.xnb");
            }

            Mod.Logger.Info($"Total of {NewTileTexturesToLoad.Length} new tile textures to load");

            foreach (var id in NewTileTexturesToLoad)
            {
                BoulderBackport.Instance.DebugLog($"Loading new tile texture for tile ID {id}");
                SelectBackportContentSource.Add($"Images\\Tiles_{id}.xnb");
                SelectBackportContentSource.Add(
                    $@"Images\Misc\TileOutlines\Tiles_{id}.xnb"); // For smart cursor highlight
            }

            Mod.Logger.Info($"Total of {NewWallTexturesToLoad.Length} new wall textures to load");

            foreach (var id in NewWallTexturesToLoad)
            {
                BoulderBackport.Instance.DebugLog($"Loading new wall texture for wall ID {id}");
                SelectBackportContentSource.Add($"Images\\Wall_{id}.xnb");
            }

            BoulderBackport.Instance.DebugLog("Loading new Christmas tree texture");
            SelectBackportContentSource.Add("Images\\Xmas_0.xnb");

            BoulderBackport.Instance.DebugLog("Loading new armor textures");
            // SelectBackportContentSource.Add(@"Images\Armor\Armor_71.xnb"); // Part of Jim's Breastplate
            SelectBackportContentSource.Add(@"Images\Armor\Armor_99.xnb");
            SelectBackportContentSource.Add(@"Images\Armor\Armor_237.xnb");

            BoulderBackport.Instance.DebugLog("Loading new town NPC textures");
            SelectBackportContentSource.Add(@"Images\TownNPCs\Demolitionist_Default.xnb");
            SelectBackportContentSource.Add(@"Images\TownNPCs\SlimeOld_Default_Party.xnb");
            SelectBackportContentSource.Add(@"Images\TownNPCs\SlimeRed_Default_Party.xnb");
            SelectBackportContentSource.Add(@"Images\TownNPCs\SlimeYellow_Default_Party.xnb");
            SelectBackportContentSource.Add(@"Images\TownNPCs\Tavernkeep_Default.xnb");
            SelectBackportContentSource.Add(@"Images\TownNPCs\Shimmered\Demolitionist_Default.xnb");
            SelectBackportContentSource.Add(@"Images\TownNPCs\Shimmered\Demolitionist_Default_Party.xnb");
            SelectBackportContentSource.Add(@"Images\TownNPCs\Shimmered\Tavernkeep_Default.xnb");
            SelectBackportContentSource.Add(@"Images\TownNPCs\Shimmered\WitchDoctor_Default.xnb");
        }

        if (BackportConfig.Instance.UITextures)
        {
            Mod.Logger.Info($"Total of {NewNPCHeadTexturesToLoad.Length} new NPC head textures to load");

            foreach (var id in NewNPCHeadTexturesToLoad)
            {
                BoulderBackport.Instance.DebugLog($"Loading new NPC head texture for head ID {id}");
                SelectBackportContentSource.Add($"Images\\NPC_Head_{id}.xnb");
            }

            BoulderBackport.Instance.DebugLog("Loading new quick stack to chest button textures");
            SelectBackportContentSource.Add("Images\\ChestStack_0.xnb", @"Images\UI\ChestStack_0.xnb");
            SelectBackportContentSource.Add("Images\\ChestStack_1.xnb", @"Images\UI\ChestStack_1.xnb");

            BoulderBackport.Instance.DebugLog("Loading new crafting menu toggle button textures");
            SelectBackportContentSource.Add(@"Images\UI\Craft_Toggle_0.xnb");
            SelectBackportContentSource.Add(@"Images\UI\Craft_Toggle_1.xnb");
            SelectBackportContentSource.Add(@"Images\UI\Craft_Toggle_2.xnb");
            SelectBackportContentSource.Add(@"Images\UI\Craft_Toggle_3.xnb");

            BoulderBackport.Instance.DebugLog($"Total of {NewCursorTexturesToLoad.Length} new cursor textures to load");

            foreach (var id in NewCursorTexturesToLoad)
            {
                BoulderBackport.Instance.DebugLog($"Loading new cursor texture for cursor ID {id}");
                SelectBackportContentSource.Add($@"Images\UI\Cursor_{id}.xnb");
            }

            // TODO: Images\UI\Glyphs_0.xnb, now has 3 rows of glyphs instead of just 1, for XBox, PlayStation, and Switch, respectively
            // Should this be backported? If so, need to check decompiled 1.4.5 to see how the new rows are used and adjust drawing code accordingly if necessary
            // SelectBackportContentSource.Add(@"Images\UI\Glyphs_0.xnb");

            BoulderBackport.Instance.DebugLog("Loading new sort inventory button textures");
            SelectBackportContentSource.Add(@"Images\UI\Sort_0.xnb");
            SelectBackportContentSource.Add(@"Images\UI\Sort_1.xnb");

            BoulderBackport.Instance.DebugLog("Loading new spawn point map indicator texture");
            SelectBackportContentSource.Add(@"Images\UI\SpawnPoint.xnb");
        }

        if (BackportConfig.Instance.ExtraTextures)
        {
            BoulderBackport.Instance.DebugLog("Loading new background textures");
            SelectBackportContentSource.Add("Images\\Background_53.xnb");
            SelectBackportContentSource.Add("Images\\Background_54.xnb");
            SelectBackportContentSource.Add("Images\\Background_55.xnb");
            // TODO: The following backgrounds only remove a baked-in gradient effect drawn primitively in 1.4.5, do not backport (for now at least)
            // SelectBackportContentSource.Add("Images\\Background_59.xnb");
            // SelectBackportContentSource.Add("Images\\Background_176.xnb");
            // SelectBackportContentSource.Add("Images\\Background_179.xnb");
            // SelectBackportContentSource.Add("Images\\Background_248.xnb");
            // SelectBackportContentSource.Add("Images\\Background_258.xnb");
            // SelectBackportContentSource.Add("Images\\Background_263.xnb");
            // SelectBackportContentSource.Add("Images\\Background_269.xnb");
            // SelectBackportContentSource.Add("Images\\Background_283.xnb");
            // SelectBackportContentSource.Add("Images\\Background_284.xnb");

            BoulderBackport.Instance.DebugLog("Loading new extra textures");

            // Credit scenes
            SelectBackportContentSource.Add("Images\\Extra_235.xnb");
            SelectBackportContentSource.Add("Images\\Extra_237.xnb");

            for (var i = 0; i < TextureAssets.Moon.Length; i++)
            {
                BoulderBackport.Instance.DebugLog($"Loading new moon texture for style {i}");
                SelectBackportContentSource.Add($"Images\\Moon_{i}.xnb");
            }

            BoulderBackport.Instance.DebugLog("Loading new special moon textures");
            SelectBackportContentSource.Add("Images\\Moon_Pumpkin.xnb");
            SelectBackportContentSource.Add("Images\\Moon_Smiley.xnb");
            SelectBackportContentSource.Add("Images\\Moon_Snow.xnb");
        }

        // TODO: Extra_153.xnb seems to be Jim's armor set's cloak texture

        // Extra_262.xnb is some particle that had more frames added to it, but the logic also vastly changed. Likely never backporting this one, but may want to look into it at some point just out of curiosity

        // TODO: Look into what the Flame_X.xnb textures are used for
    }
}