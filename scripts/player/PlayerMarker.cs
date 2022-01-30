using Godot;
using System;

public class PlayerMarker : Node2D
{
    private PositionSync positionSync;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        positionSync = new PositionSync(this, Main.otherPlayer, "player" + Main.otherPlayer.ToString());
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
    {
		positionSync.Update();
    }
}
