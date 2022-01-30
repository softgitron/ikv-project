using Godot;
using System;

public class NetworkedDoor : Area2D, Openable
{
    public void Open(bool opened) {
        this.Visible = opened;
    }
}
