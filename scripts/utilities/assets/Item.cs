using Godot;
using System;

public class Item : Area2D
{
	[Export]
	public string itemName = "Item";
	private KinematicBody2D owner = null;
	public override void _Ready()
	{
		SetProcess(false);
	}
	public void PickUp(KinematicBody2D _owner)
	{
		owner = _owner;
		SetProcess(true);
	}
	public void SetDown()
	{
		Position = owner.Position + new Vector2(0, 250);
		owner = null;
		SetProcess(false);
	}
	public override void _Process(float delta)
	{
		if (owner != null)
		{
			Position = owner.Position;
		}
	}
}
