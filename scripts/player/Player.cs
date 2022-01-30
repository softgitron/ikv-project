using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public class Player : KinematicBody2D
{
    public Vector2 velocity = Vector2.Zero;
    [Export]
    private int maxSpeed = 100;
    [Export]
    public int jump = 1500;
    [Export]
    private float acceleration = 100;
    [Export]
    private int gravity = 100;
    public AnimatedSprite sprite;
    public List<Area2D> surroundingInteractives = new List<Area2D>();
    private List<Hittable> closeByAreas = new List<Hittable>();
    private Item pickedItem;
    public int ladders = 0;
    public int doubleJump = 0;
    private int attack = 0;
    private List<PlayerState> stateStack = new List<PlayerState>();
    private PlayerState currentState = new IdleState();
    private Toggleable toggle;
    private PositionSync positionSync;
    private float maxAngle = 0.9f;
    private PlayerThemeSync themeSync;

    public override void _Ready()
    {
        SetPhysicsProcess(false);
    }
    public void Initialize(SpriteFrames frames)
    {
        sprite = (AnimatedSprite)GetNode("AnimatedSprite");
        AudioSource audioSource = ((GameManager)GetParent()).audioSource;

        sprite.Frames = frames;

        switch (Main.player)
        {
            case 2:
                doubleJump++;
                this.themeSync = new PlayerThemeSync(audioSource, AudioSourceImplementation.darkPlayerTheme);
                break;
            case 1:
                jump = (int)(0.6 * jump);
                attack++;
                this.themeSync = new PlayerThemeSync(audioSource, AudioSourceImplementation.lightPlayerTheme);
                break;
            default:
                break;
        }
        SetPhysicsProcess(true);
        positionSync = new PositionSync(this, Main.player, "player" + Main.player.ToString());
    }
    public override void _PhysicsProcess(float delta)
    {

        PlayerState newState = currentState.ProcessState(this);
        if (newState != null)
        {
            ChangeState(newState);
        }

        if (Input.IsActionJustPressed("attack_button") && Main.player == 1)
        {
            Attack();
        }

        if (ladders > 0)
        {
            HandleLadders();
        }
        // IMPORTANT!
        positionSync.Update();
    }

    public void GroundMove(int direction)
    {
        velocity.y += gravity;
        velocity.x = maxSpeed * direction;
        velocity = MoveAndSlide(velocity, Vector2.Up, stopOnSlope: true, floorMaxAngle: maxAngle);
    }
    public void AirMove(int direction)
    {
        velocity.y += gravity;
        velocity.x = maxSpeed * direction;
        velocity = MoveAndSlide(velocity, Vector2.Up, stopOnSlope: true, floorMaxAngle: maxAngle);
        if (IsOnFloor())
        {
            ChangeState(new MoveState());
        }
    }

    public void Drop()
    {
        Position = new Vector2(Position.x, Position.y + 1);
    }

    public void HandleInteract()
    {
        // IMPORTANT!
        if (toggle != null)
        {
            toggle.Toggle();
        }

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

    private void HandleLadders()
    {
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

    private void Attack()
    {
        GD.Print("Attacking");
        sprite.Stop();
        sprite.Play("attack");
        foreach (Hittable area in closeByAreas)
        {
            if (!IsInstanceValid(area))
            {
                closeByAreas.Remove(area);
                return;
            }
            GD.Print(string.Format("Attack: {0}", area.Name));
            area.Hit();
        }
    }


    private void ChangeState(PlayerState playerState)
    {
        playerState.ExitState(this);
        stateStack.Insert(0, playerState);
        currentState = stateStack[0];
        currentState.EnterState(this);
        GD.Print(currentState);
    }

    private void moveToPreviousState()
    {
        stateStack.RemoveAt(0);
        currentState = stateStack[0];
    }

    // Handles interacting with items.
    private void _OnVicinityEntered(Area2D area)
    {
        if (area is Toggleable)
        {
            toggle = (Toggleable)area;
        }

        if (area is Item item)
        {
            surroundingInteractives.Add(item);
        }

        if (area.Name.EndsWith("Theme"))
        {
            ((GameManager)GetParent()).audioSource.FadeInTrack(area.Name);
        }

        if (area.Name == "PlayerThemeSync")
        {
            this.themeSync.SetThemeState(true);
        }
    }
    private void _OnVicinityExited(Area2D area)
    {
        if (area == toggle)
        {
            toggle = null;
        }
        surroundingInteractives.Remove(area);

        if (area.Name.EndsWith("Theme"))
        {
            ((GameManager)GetParent()).audioSource.FadeOutTrack(area.Name);
        }

        if (area.Name == "PlayerThemeSync")
        {
            this.themeSync.SetThemeState(false);
        }
    }
    private void _AttackAreaEntered(object area)
    {
        if (area is Hittable hittable)
        {
            closeByAreas.Add(hittable);
        }
    }
    private void _AttackAreaExited(object area)
    {
        if (area is Hittable hittable)
        {
            closeByAreas.Remove(hittable);
        }
    }
    private void _OnAnimationFinished()
    {
        currentState.HandleAnimationFinished(this);
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
