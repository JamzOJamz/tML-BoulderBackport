using System;
using BoulderBackport.Common.ID;
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
            legacyAudioSystem.AudioTracks.Length >= NewMusicID.Count) return;

        Array.Resize(ref legacyAudioSystem.AudioTracks, NewMusicID.Count);
        for (var i = NewMusicID.Destroyer; i < NewMusicID.Count; i++)
            legacyAudioSystem.LoadCue(i, $"Music_{i}");
    }
}