using Godot;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public abstract class TCPImpl : Node
{
    public static Action ConnectedListener {get; set;}
    protected static StreamPeerTCP client;
    protected string currentBytes;
    private static Dictionary<string, TCPAction> listeners = new Dictionary<string, TCPAction>();

    public static void SendCommand(string command, string parameters) {
        string tcpCommand = CommandToTCPMessage(command, parameters);
        client.PutData(tcpCommand.ToAscii());
    }

    public static void RegisterListener(string command, TCPAction action) {
        listeners.Add(command, action);
    }

    protected static string CommandToTCPMessage(string command, string parameters) {
        Dictionary<string, string> jsonFields = new Dictionary<string, string>();
        jsonFields.Add("command", command);
        jsonFields.Add("parameters", parameters);
        string jsonCommand = jsonCommand = JsonConvert.SerializeObject(jsonFields);
        string tcpCommand = jsonCommand + ";";
        return tcpCommand;
    }

    protected static string ProcessBytes(string currentBytes) {
        string[] commands = currentBytes.Split(";");

        // Process complete messages
        for (int messageIndex = 0; messageIndex < commands.Length - 1; messageIndex++) {
            processMessage(commands[messageIndex]);
        }

        // Return remaining bytes.
        return commands[commands.Length - 1];
    }

    private static void processMessage(string message) {
        Dictionary<string, string> jsonFields = JsonConvert.DeserializeObject<Dictionary<string, string>>(message);
        string command;
        jsonFields.TryGetValue("command", out command);
        string parameters;
        jsonFields.TryGetValue("parameters", out parameters);

        // Run TCP action.
        TCPAction action;
        listeners.TryGetValue(command, out action);
        if (action != null) {
            action.TCPAction(parameters);
        }
    }

    public abstract override void _Process(float delta);
}
