using Godot;
using System;

public class PressurePlate : Area2D, Toggleable, StateSync<Boolean>
{
	[Export]
	public string handleName = "";
	[Export]
	public string openablePath = "";
	[Export]
	public string openSprite = "";
	public StateSyncImpl<Boolean> stateSync;

	public override void _Ready()
	{
		stateSync = new StateSyncImpl<Boolean>(this, false, handleName);
	}

	public void _on_Pressure_Plate_area_entered(Area2D area) {
		Toggle();
	}

	public void Toggle() {
		updateButton();
		stateSync.SetState(true);
		Node node = GetNode(openablePath);
		Openable toggleable = (Openable) node;
		toggleable.Open(true);
	}

	public void StateTrigger(Boolean newState) {
		updateButton();
		Node node = GetNode(openablePath);
		Openable toggleable = (Openable) node;
		toggleable.Open(true);
	}

	private void updateButton() {
		Sprite sprite = (Sprite) GetNode("Sprite");
		Texture newTexture = (Texture)ResourceLoader.Load(openSprite);
		sprite.Texture = newTexture;
	}
}
