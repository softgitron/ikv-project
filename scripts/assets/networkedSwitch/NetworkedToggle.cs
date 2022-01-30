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
    [Export]
	public string offSprite = "";
    [Export]
	public string onSprite = "";
    public StateSyncImpl<Boolean> stateSync;

	public override void _Ready()
	{
		stateSync = new StateSyncImpl<Boolean>(this, defaultState, handleName);
	}

    public void Toggle() {
        stateSync.SetState(!stateSync.GetState());
        Node node = GetNode(openablePath);
        Openable toggleable = (Openable) node;
        toggleable.Open(stateSync.GetState());
        updateSprite();
    }

    public void StateTrigger(Boolean newState) {
        Node node = GetNode(openablePath);
        Openable toggleable = (Openable) node;
        toggleable.Open(newState);
        updateSprite();
    }

    private void updateSprite() {
        bool opened = stateSync.GetState();
        Sprite sprite = (Sprite) GetNode("Sprite");
        if (opened) {
            Texture newTexture = (Texture)ResourceLoader.Load(onSprite);
            sprite.Texture = newTexture;
        } else {
            Texture newTexture = (Texture)ResourceLoader.Load(offSprite);
            sprite.Texture = newTexture;
        }
    }
}
