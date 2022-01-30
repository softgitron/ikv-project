using Godot;
using System;

public class MainMenu: Control
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	
	private Button Player1Button;
	private Button Player2Button;
	private TextEdit AddressTextEdit;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Player1Button = (Button) GetNode("Player1Button");
		Player1Button.Connect("button_down", this, "player1ButtonPressed");

		Player2Button = (Button) GetNode("Player2Button");
		Player2Button.Connect("button_down", this, "player2ButtonPressed");

		AddressTextEdit = (TextEdit) GetNode("AddressTextEdit");
	}

	private void player1ButtonPressed() {
		Main.player = 1;
		SceneManagement.GetInstance().ChangeScene(SceneManagement.Scenes.WaitingPlayer2);
	}

	private void player2ButtonPressed() {
		Main.player = 2;
		TCPClientImpl.ConnectToServer(AddressTextEdit.Text);

		TCPClientImpl.ConnectedListener = new Action(() => {
			SceneManagement.GetInstance().ChangeScene(SceneManagement.Scenes.World);
		});
	}
}
