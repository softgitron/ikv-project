using Godot;
using System;

public class Main : Node
{
	public static int player;

	public static int otherPlayer {
		get {
			return (Main.player == 1) ? 2 : 1;
		}
		private set {
			player = value;
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
