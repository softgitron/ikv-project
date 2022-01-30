using Godot;
using System;

public class GameManager : Node
{
	private Player player;
	private MainCamera camera;
	private DebugOverlay debug;
	public AudioSource audioSource;
	public override void _Ready()
	{
		player = (Player)GetNode("Player");
		camera = (MainCamera)GetNode("MainCamera");
		debug = (DebugOverlay)GetNode("UILayer/DebugOverlay");
		audioSource = (AudioSourceImplementation)GetNode("AudioManager");

		debug.AddStat("Fps: ", this, "GetFps", true);
		debug.AddStat("Nearby Items: ", player, "GetSurroundingItems", true);
		debug.AddStat("Static Memory Usage: ", this, "GetStaticMemory", true);
		debug.AddStat("Dynamic Memory usage: ", this, "GetDynamicMemory", true);

		camera.Initialize(player);
		player.Initialize();
		if (player.type == "light")
		{
			audioSource.FadeInTrack(AudioSourceImplementation.lightPlayerTheme, 0);
		}
		else
		{
			audioSource.FadeInTrack(AudioSourceImplementation.darkPlayerTheme, 0);
		}
	}

	private string GetFps()
	{
		return Engine.GetFramesPerSecond().ToString();
	}
	private string GetStaticMemory()
	{
		return string.Format("{0} Mb", OS.GetStaticMemoryUsage() / 1000000);
	}
	private string GetDynamicMemory()
	{
		return string.Format("{0} Mb", OS.GetDynamicMemoryUsage() / 1000000);
	}
}
