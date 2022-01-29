using Godot;
using System;

public class LabelReceiver : Label, TCPAction
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        TCPImpl.RegisterListener("label", this);
    }

    public void TCPAction(string parameters) {
        Text = parameters;
    }
}
