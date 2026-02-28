using System.ComponentModel;
using Terraria.ModLoader.Config;

// ReSharper disable UnassignedField.Global

namespace BoulderBackport.Content.Configs;

public class BackportConfig : ModConfig
{
#pragma warning disable CA2211
    public static BackportConfig Instance;
#pragma warning restore CA2211
    public override ConfigScope Mode => ConfigScope.ClientSide;

    [Header("Portraits")]
    [DefaultValue(true)]
    public bool VanillaTownNPCPortraits;
    
    [DefaultValue(true)]
    public bool ModdedTownNPCPortraits;

    [Header("Music")]
    [DefaultValue(true)]
    public bool KingSlimeBossMusic;
    
    [DefaultValue(true)]
    public bool EaterOfWorldsBossMusic;
    
    [DefaultValue(true)]
    public bool QueenBeeBossMusic;
    
    [DefaultValue(true)]
    public bool SkeletronBossMusic;
    
    [DefaultValue(true)]
    public bool TheTwinsBossMusic;
    
    [DefaultValue(true)]
    public bool TheDestroyerBossMusic;
    
    [DefaultValue(true)]
    public bool SkeletronPrimeBossMusic;
    
    [DefaultValue(true)]
    public bool LunaticCultistBossMusic;
    
    [DefaultValue(true)]
    public bool TorchGodBossMusic;

    [Header("Sounds")]
    [DefaultValue(true)]
    public bool TrashItemSound;
    
    [DefaultValue(true)]
    public bool SonarPotionSound;

    [DefaultValue(true)]
    [ReloadRequired]
    public bool ThunderSounds;

    [DefaultValue(true)]
    [ReloadRequired]
    public bool VolumeBalancingChanges;

    [Header("Textures")]
    [DefaultValue(true)]
    [ReloadRequired]
    public bool GameplayTextures;
    
    [DefaultValue(true)]
    [ReloadRequired]
    public bool UITextures;
    
    [DefaultValue(true)]
    [ReloadRequired]
    public bool ExtraTextures;
}