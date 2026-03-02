using System;
using BoulderBackport.Common.ID;
using BoulderBackport.Content.Configs;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace BoulderBackport.Core.Systems;

public class MusicBackportingSystem : BackportingSystemBase
{
    internal static void EarlyInitialize() => MusicLoader.MusicCount = NewMusicID.Count;

    public override void Load()
    {
        if (Main.audioSystem is not LegacyAudioSystem legacyAudioSystem ||
            legacyAudioSystem.AudioTracks.Length >= NewMusicID.Count || !AnyMusicConfigEnabled())
            return;

        Array.Resize(ref legacyAudioSystem.AudioTracks, NewMusicID.Count);

        for (var i = NewMusicID.Destroyer; i < NewMusicID.Count; i++)
        {
            BoulderBackport.Instance.DebugLog($"Loading music ID {i}");
            legacyAudioSystem.LoadCue(i, $"Music_{i}");
        }
    }

    private static bool AnyMusicConfigEnabled()
    {
        var config = BackportConfig.Instance;
        return config.KingSlimeBossMusic
               || config.EaterOfWorldsBossMusic
               || config.QueenBeeBossMusic
               || config.SkeletronBossMusic
               || config.TheTwinsBossMusic
               || config.TheDestroyerBossMusic
               || config.SkeletronPrimeBossMusic
               || config.LunaticCultistBossMusic
               || config.TorchGodBossMusic;
    }
}