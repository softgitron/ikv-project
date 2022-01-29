using Godot;
using System;

public class TCPClientImpl : TCPImpl
{
    private static bool connectionInformed;

    public static void ConnectToServer(string host) {
        client = new StreamPeerTCP();
        client.ConnectToHost(host, 4242);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (client == null || client.GetStatus() != StreamPeerTCP.Status.Connected) {
            return;
        }

        if (!connectionInformed && client.IsConnectedToHost()) {
            connectionInformed = true;
            ConnectedListener();
        }

        // Process server messages.
        int bytesAvailable = client.GetAvailableBytes();
        currentBytes = currentBytes + client.GetString(bytesAvailable);
        currentBytes = ProcessBytes(currentBytes);
    }
}
