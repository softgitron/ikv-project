using Godot;
using System;

public class GameManager : Node
{
	private Player player;
	private MainCamera camera;
	private DebugOverlay debug;
	private SpriteFrames lightFrames, darkFrames;
	public AudioSource audioSource;
	public override void _Ready()
	{
		lightFrames = (SpriteFrames)ResourceLoader.Load("res://resources/sprites/characters/anim/light_anim.tres");
		darkFrames = (SpriteFrames)ResourceLoader.Load("res://resources/sprites/characters/anim/dark_anim.tres");

		player = (Player)GetNode("Player");
		camera = (MainCamera)GetNode("MainCamera");
		debug = (DebugOverlay)GetNode("UILayer/DebugOverlay");
		audioSource = (AudioSourceImplementation)GetNode("AudioManager");

		debug.AddStat("Fps: ", this, "GetFps", true);
		debug.AddStat("Nearby Items: ", player, "GetSurroundingItems", true);
		debug.AddStat("Static Memory Usage: ", this, "GetStaticMemory", true);
		debug.AddStat("Dynamic Memory usage: ", this, "GetDynamicMemory", true);

		camera.Initialize(player);

		switch (Main.player)
		{
			case 2:
				audioSource.FadeInTrack(AudioSourceImplementation.lightPlayerTheme, 0);
				player.Initialize(lightFrames);
				break;
			case 1:
				audioSource.FadeInTrack(AudioSourceImplementation.darkPlayerTheme, 0);
				player.Initialize(darkFrames);
				break;
			default:
				GD.Print("Error while choosing player frames.");
				break;
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
