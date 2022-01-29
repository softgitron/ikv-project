using Godot;
using System;

public class ButtonSender : Button
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
		Connect("button_down", this, "buttonePressed");
    }

    private void buttonePressed() {
		TCPImpl.SendCommand("label", "button press from the server");
	}
}
