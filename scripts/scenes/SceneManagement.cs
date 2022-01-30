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
        World
    }

    public static PackedScene CurrentScene {get; private set;}
    private static Node currentSceneNode;
    private static SceneManagement instance;

    private SceneManagement() {
        
    }

    public override void _Ready()
    {
        instance = this;

        // Change immidiately to main menu.
        ChangeScene(SceneManagement.Scenes.MainMenu);
    }

    public static SceneManagement GetInstance() {
        return instance;
    }

    public void ChangeScene(Scenes scene) {
        Node sceneNode = GetNode("SceneNode");

        string newSceneName = $"res://scenes/{scene.ToString()}Scene.tscn";
        if (CurrentScene != null && CurrentScene.ResourcePath == newSceneName) {
            return;
        }

        // Remove previous scene.
        if (CurrentScene != null) {
            sceneNode.RemoveChild(currentSceneNode);
            currentSceneNode.Dispose();
        }

        // Load new scene.
        CurrentScene = (PackedScene)ResourceLoader.Load(newSceneName);
        currentSceneNode = CurrentScene.Instance();
        sceneNode.AddChild(currentSceneNode);
    }
}
