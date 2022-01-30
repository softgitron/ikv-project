using Godot;
using System;

public class Hittable : StaticBody2D
{
	public void Hit()
	{
		Node2D parent = (Node2D)GetParent();
		parent.Scale = parent.Scale + new Vector2(0, -0.25f);
		parent.Position = parent.Position + new Vector2(0, 50);
		if (parent.Scale.y < 0.5)
		{
			parent.QueueFree();
		}
	}
}
