using Godot;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public class PositionSync : TCPAction
{
    private const int yOffset = 15500;
    public const int offsetActive = 2;

    public Node2D controllObject;
    private int playerAuthority;

    private string handleName;

    private bool isSwitched = false;

    public PositionSync(Node2D controllObject, int playerAuthority, string handleName)
    {
        this.controllObject = controllObject;
        this.playerAuthority = playerAuthority;
        this.handleName = handleName;
        initialize();
    }

    private void initialize()
    {
        if (playerAuthority != Main.player)
        {
            TCPImpl.RegisterListener(handleName, this);
        }
    }

    public void Update()
    {
        if (playerAuthority == Main.player)
        {
            Dictionary<string, float> jsonFields = new Dictionary<string, float>();
            jsonFields.Add("positionX", controllObject.Position.x);
            jsonFields.Add("positionY", controllObject.Position.y);
            string jsonCommand = jsonCommand = JsonConvert.SerializeObject(jsonFields);
            TCPImpl.SendCommand(handleName, jsonCommand);
        }
    }

    public void TCPAction(string parameters)
    {
        Dictionary<string, float> jsonFields = JsonConvert.DeserializeObject<Dictionary<string, float>>(parameters);
        float positionX;
        jsonFields.TryGetValue("positionX", out positionX);
        float positionY;
        jsonFields.TryGetValue("positionY", out positionY);

        // Apply offset if needed.
        if (Main.player == (isSwitched ?  1 : offsetActive))
        {
            positionY = yOffset;
        }

        controllObject.Position = new Vector2(positionX, positionY);
    }

    public void Swap()
    {
        isSwitched = !isSwitched;
    }
}
