using Godot;
using System;
using System.Linq;

public interface AudioSource
{
    void FadeInTrack(string trackName);
    void FadeOutTrack(string trackName);
    void PlaySound(string soundName);
}
