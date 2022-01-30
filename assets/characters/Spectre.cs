using Godot;
using System;

public class Spectre : RigidBody2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var animSprite2D = GetNode<AnimatedSprite>("AnimatedSprite");
		animSprite2D.Playing = true;
		string[] mobTypes = animSprite2D.Frames.GetAnimationNames();
		animSprite2D.Animation = mobTypes[1];

	}

	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	//  public override void _Process(float delta)
	//  {
	//      
	//  }
	private void _on_VisibilityNotifier2D_screen_exited()
	{
		QueueFree();
	}

	private void _on_Area2D_body_entered(object body)
	{
		var animSprite2D = GetNode<AnimatedSprite>("AnimatedSprite");
		string[] mobTypes = animSprite2D.Frames.GetAnimationNames();
		animSprite2D.Animation = mobTypes[0];
	}

	private void _on_Area2D_body_exited(object body)
	{
		var animSprite2D = GetNode<AnimatedSprite>("AnimatedSprite");
		string[] mobTypes = animSprite2D.Frames.GetAnimationNames();
		animSprite2D.Animation = mobTypes[1];
	}
}









