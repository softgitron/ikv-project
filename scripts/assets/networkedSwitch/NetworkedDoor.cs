using Godot;
using System;

public class NetworkedDoor : StaticBody2D, Openable
{
    [Export]
	public string openSprite = "";
    [Export]
	public string closedSprite = "";
    public void Open(bool opened) {
        CallDeferred("updateDoor", opened);
    }

    public void updateDoor(bool opened) {
        Sprite sprite = (Sprite) GetNode("Sprite");
        CollisionShape2D collision = (CollisionShape2D) GetNode("Collision");
        if (opened) {
            Texture newTexture = (Texture)ResourceLoader.Load(openSprite);
            sprite.Texture = newTexture;
        } else {
            Texture newTexture = (Texture)ResourceLoader.Load(closedSprite);
            sprite.Texture = newTexture;
        }
        collision.Disabled = opened;
    }
}
