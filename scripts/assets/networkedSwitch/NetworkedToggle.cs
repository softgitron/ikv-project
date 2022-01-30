using Godot;
using System;

public class NetworkedToggle : Area2D, Toggleable, StateSync<Boolean>
{
	[Export]
	public string handleName = "";
	[Export]
	public string openablePath = "";
	[Export]
	public bool defaultState = false;
	private StateSyncImpl<Boolean> stateSync;

	public override void _Ready()
	{
		stateSync = new StateSyncImpl<Boolean>(this, defaultState, handleName);
	}

	public void Toggle() {
		stateSync.SetState(!stateSync.GetState());
		Node node = GetNode(openablePath);
		Openable toggleable = (Openable) node;
		toggleable.Open(stateSync.GetState());
	}

	public void StateTrigger(Boolean newState) {
		Node node = GetNode(openablePath);
		Openable toggleable = (Openable) node;
		toggleable.Open(newState);
	}
}
