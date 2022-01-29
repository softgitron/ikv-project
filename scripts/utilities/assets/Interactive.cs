using Godot;
using System;

public class Interactive : Area2D
{
    private Sprite sprite;
    private CollisionShape2D collisionBox;
    private bool open = false;
    public override void _Ready()
    {
        sprite = (Sprite)GetNode("Sprite");
        collisionBox = (CollisionShape2D)GetNode("Collision/CollisionBox");
    }
    protected virtual void Trigger(Area2D area)
    {
        open = open ? false : true;
        CallDeferred("SetVisibility", open);
        CallDeferred("SetActiveStatus", open);
    }
    private void SetVisibility(bool _open)
    {
        sprite.Visible = !_open;
    }
    private void SetActiveStatus(bool _open)
    {
        collisionBox.Disabled = _open;
    }
}
