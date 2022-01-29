using Godot;
using System;

public class MovingClouds : ParallaxLayer
{
	[Export]
	public float cloudSpeed = -25;
	public override void _Process(float delta)
	{
		MotionOffset += new Vector2(cloudSpeed * delta, 0);
	}
}
