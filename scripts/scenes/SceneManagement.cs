using Godot;
using System;

public class SceneManagement : Node
{
    public enum Scenes
    {
        MainMenu,
        Player1,
        Player2,
        WaitingPlayer2,
        DarkWorld,
        LightWorld
    }

    public static PackedScene CurrentScene {get; private set;}
    private static Node currentSceneNode;
    private static SceneManagement instance;
    private Node sceneNode;

    private SceneManagement() {
        
    }

    public override void _Ready()
    {
        instance = this;

        sceneNode = GetNode("SceneNode");

        // Change immidiately to main menu.
        ChangeScene(SceneManagement.Scenes.MainMenu);
    }

    public static SceneManagement GetInstance() {
        return instance;
    }

    public void ChangeScene(Scenes scene) {
        // Remove previous scene.
        if (CurrentScene != null) {
            sceneNode.RemoveChild(currentSceneNode);
            currentSceneNode.Dispose();
        }

        // Load new scene.
        CurrentScene = (PackedScene)ResourceLoader.Load($"res://scenes/{scene.ToString()}Scene.tscn");
        currentSceneNode = CurrentScene.Instance();
        sceneNode.AddChild(currentSceneNode);
    }
}
