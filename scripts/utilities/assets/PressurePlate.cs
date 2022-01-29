using Godot;
using System;

public class PressurePlate : Area2D
{
	private Area2D triggeredEntity;
	public override void _Ready()
	{
		triggeredEntity = GetParentOrNull<Area2D>();
		if (triggeredEntity != null)
		{
			Connect("area_entered", triggeredEntity, "Trigger");
		}
	}
}
