using Godot;
using System;

public class WaitingPlayer2 : Node
{
	private Label playerIpLabel;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		playerIpLabel = (Label) GetNode("PlayerIpLabel");
		string possibleAddresses = IP.GetLocalAddresses().ToString();
		playerIpLabel.Text = $"Use one of the following addresses for player 2: {possibleAddresses}";

		TCPServerImpl.StartServer();
	TCPServerImpl.ConnectedListener = new Action(() => {
			SceneManagement.GetInstance().ChangeScene(SceneManagement.Scenes.World);
		});
	}
}
