using Godot;
using System;

public class GameManager : Node
{
	private Player player;
	private MainCamera camera;
	public override void _Ready()
	{
		player = (Player)GetNode("Player");
		camera = (MainCamera)GetNode("MainCamera");

		camera.Initialize(player);
		player.Initialize();
	}
}
