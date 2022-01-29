using Godot;
using System;

public class Ladder_2D : Area2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	//  public override void _Process(float delta)
	//  {
	//      
	//  }


	private void _on_Ladder_2D_body_entered(object body)
	{
	if (body.GetType().Name.ToString() == "Player") {
			Player player = (Player) body;
			player.ladder_on = true;
		}
	}


	private void _on_Ladder_2D_body_exited(object body)
	{
	if (body.GetType().Name.ToString() == "Player") {
			Player player = (Player) body;
			player.ladder_on = false;
		}
	}
}



