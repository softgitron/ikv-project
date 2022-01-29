using Godot;
using System;

public class MainCamera : Camera2D
{
	public Node2D target { get; private set; }
	public override void _Ready()
	{
		SetProcess(false);
	}
	public void Initialize(Node2D _target)
	{
		target = _target;
		if (target != null)
		{
			SetProcess(true);
		}
	}
	public override void _Process(float delta)
	{
		Position = target.Position;
	}
}
