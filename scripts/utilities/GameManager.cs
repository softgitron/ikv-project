using Godot;
using System;

public class GameManager : Node
{
	private Player player;
	private MainCamera camera;
	private AudioSource audioSource;
	public override void _Ready()
	{
		player = (Player)GetNode("Player");
		camera = (MainCamera)GetNode("MainCamera");
		audioSource = (AudioSourceImplementation) GetNode("AudioManager");

		camera.Initialize(player);
		player.Initialize();
	}
}
