using Godot;
using System;

public class TCPServerImpl : TCPImpl
{
    private static TCP_Server server = new TCP_Server();

    public static void StartServer() {
        server.Listen(4242);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (server.IsConnectionAvailable()) {
            client = server.TakeConnection();
            // Inform that client was connected.
            ConnectedListener();
        }

        // Process client messages.
        if (client != null) {
            int bytesAvailable = client.GetAvailableBytes();
            currentBytes = currentBytes + client.GetString(bytesAvailable);
            currentBytes = ProcessBytes(currentBytes);
        }
    }
}
