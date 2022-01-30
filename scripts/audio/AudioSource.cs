using Godot;
using System;
using System.Linq;

public interface AudioSource
{
	void FadeInTrack(string trackName);
	void FadeInTrack(string trackName, int fadeTimeMilliseconds);
	void FadeOutTrack(string trackName);
	void FadeOutTrack(string trackName, int fadeTimeMilliseconds);
	void PlaySound(string soundName);
}
