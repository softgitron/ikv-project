using System;
using Godot;
using System.Collections.Generic;

public class AudioSourceImplementation : Node, AudioSource
{
    private class TrackData
    {
        public TrackData(AudioStreamPlayer player, int volume)
        {
            this.player = player;
            this.volume = volume;
        }
        public AudioStreamPlayer player { get; }
        public int volume { get; }
    }
    private static AudioSource instance;
    public const string lightPlayerTheme = "LightPlayerTheme";
    public const string darkPlayerTheme = "DarkPlayerTheme";
    public const string lightWorldTheme = "LightWorldTheme";
    public const string darkWorldTheme = "DarkWorldTheme";

    private const int lightPlayerThemeVolume = -4;
    private const int darkPlayerThemeVolume = 0;
    private const int lightWorldThemeVolume = -5;
    private const int darkWorldThemeVolume = -3;

    private const int fadeTimeMilliseconds = 1000;

    private Dictionary<string, TrackData> musicLayers;
    private Tween tween;

    private void AddLayers(Dictionary<string, int> layers)
    {
        this.musicLayers = new Dictionary<string, TrackData>();
        foreach (KeyValuePair<string, int> layer in layers)
        {
            TrackData trackData = new TrackData((AudioStreamPlayer)this.GetNode(layer.Key), layer.Value);
            this.musicLayers.Add(layer.Key, trackData);
        }
    }

    private void StartAll()
    {
        foreach (TrackData track in this.musicLayers.Values)
        {
            track.player.Play();
            track.player.VolumeDb = -80;
        }
    }

    public override void _Ready()
    {
        this.tween = (Tween)this.GetNode("Tween");
        this.AddLayers(new Dictionary<string, int>
        {
            {lightPlayerTheme, lightPlayerThemeVolume},
            {darkPlayerTheme, darkPlayerThemeVolume},
            {lightWorldTheme, lightWorldThemeVolume},
            {darkWorldTheme, darkWorldThemeVolume}
        });
        this.StartAll();
    }

    public void FadeInTrack(string trackName)
    {
        TrackData track = this.musicLayers[trackName];
        this.PerformFade(track.player, track.volume);
    }

    public void FadeOutTrack(string trackName)
    {
        TrackData track = this.musicLayers[trackName];
        this.PerformFade(track.player, -80);
    }

    private void PerformFade(AudioStreamPlayer player, int targetVolume)
    {
        this.tween.InterpolateProperty(player, "volume_db", player.VolumeDb, targetVolume, fadeTimeMilliseconds / 1000);
        this.tween.Start();
    }

    public void PlaySound(string soundName)
    {
        throw new NotImplementedException();
    }
}
