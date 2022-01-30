using Godot;
using System;

public class Door : Interactive
{
    protected override void Trigger(Area2D area)
    {
        GD.Print("Door triggered");
        base.Trigger(area);
    }
}
