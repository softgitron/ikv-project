using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public class Player : KinematicBody2D
{
    private Vector2 velocity = Vector2.Zero;
    [Export]
    private int maxSpeed = 100;
    [Export]
    private int jump = 1500;
    private float currentSpeed = 0;
    [Export]
    private float acceleration = 100;
    [Export]
    private int gravity = 100;
    private Sprite sprite;
    public List<Area2D> surroundingInteractives = new List<Area2D>();
    private Item pickedItem;
    public bool ladder_on = false;

    public override void _Ready()
    {
        SetPhysicsProcess(false);
    }
    public void Initialize()
    {
        sprite = (Sprite)GetNode("Sprite");
        SetPhysicsProcess(true);
    }
    public override void _PhysicsProcess(float delta)
    {
        int direction = Math.Sign(Input.GetActionStrength("right_button") - Input.GetActionStrength("left_button"));
        if (direction > 0)
        {
            sprite.FlipH = false;
        }
        else if (direction < 0)
        {
            sprite.FlipH = true;
        }
        if (Input.IsActionJustPressed("jump_button"))
        {
            velocity.y -= jump;
        }
        if (Input.IsActionJustPressed("interact_button"))
        {
            Item item;
            if (pickedItem != null)
            {
                DropItem();
            }
            else if ((item = (Item)surroundingInteractives.FirstOrDefault(x => x is Item)) != null)
            {
                PickItem(item);
            }
        }
        velocity.y += gravity;
        currentSpeed = maxSpeed;
        velocity.x = currentSpeed * direction;

        velocity = MoveAndSlide(velocity);

        if (ladder_on == true)
        {
            gravity = 0;
            if (Input.IsActionPressed("jump_button"))
            {
                velocity.y = -maxSpeed;
            }
            else if (Input.IsActionPressed("crouch_button"))
            {
                velocity.y = maxSpeed;
            }
            else
            {
                velocity.y = 0;
            }
        }
        else
        {
            gravity = 100;
        }
    }

    // Handles interacting with items.
    private void _OnVicinityEntered(Area2D area)
    {
        if (area is Item item)
        {
            surroundingInteractives.Add(item);
        }
    }
    private void _OnVicinityExited(Area2D area)
    {
        surroundingInteractives.Remove(area);
    }
    private void PickItem(Item item)
    {
        GD.Print(string.Format("Item {0} picked", item.itemName));
        pickedItem = item;
        item.PickUp(this);
    }
    private void DropItem()
    {
        GD.Print(string.Format("Item {0} dropped", pickedItem.itemName));
        pickedItem.SetDown();
        pickedItem = null;
    }
    public List<Area2D> GetSurroundingItems()
    {
        return surroundingInteractives;
    }
}
