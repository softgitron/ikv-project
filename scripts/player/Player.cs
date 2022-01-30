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
	public Sprite sprite;
	public List<Area2D> surroundingInteractives = new List<Area2D>();
	private Item pickedItem;
	public int ladders = 0;
	public string type = "light";
	public int doubleJump = 0;
	private int attack = 0;
	private List<PlayerState> stateStack = new List<PlayerState>();
	private PlayerState currentState = new IdleState();
	private Toggleable toggle;
	private PositionSync positionSync;
	
	public override void _Ready()
	{
		SetPhysicsProcess(false);
	}
	public void Initialize()
	{
		sprite = (Sprite)GetNode("Sprite");
		switch (type)
		{
			case "light":
				doubleJump++;
				break;
			case "dark":
				attack++;
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
		GD.Print(currentState);

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
		velocity = MoveAndSlide(velocity);
	}
	public void AirMove(int direction)
	{
		velocity.y += gravity;
		velocity.x = maxSpeed * direction;
		velocity = MoveAndSlide(velocity, Vector2.Up);
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
		if (toggle != null) {
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


	private void ChangeState(PlayerState playerState)
	{
		playerState.ExitState(this);
		stateStack.Insert(0, playerState);
		currentState = stateStack[0];
	}
	
	private void moveToPreviousState()
	{
		stateStack.RemoveAt(0);
		currentState = stateStack[0];
	}

	// Handles interacting with items.
	private void _OnVicinityEntered(Area2D area)
	{
		if (area is Toggleable) {
			toggle = (Toggleable) area;
		}

		if (area is Item item)
		{
			surroundingInteractives.Add(item);
		}

		if (area.Name.EndsWith("Theme"))
		{
			((GameManager) GetParent()).audioSource.FadeInTrack(area.Name);
		}
	}
	private void _OnVicinityExited(Area2D area)
	{
		if (area == toggle) {
			toggle = null;
		}
		surroundingInteractives.Remove(area);
		
		if (area.Name.EndsWith("Theme"))
		{
			((GameManager) GetParent()).audioSource.FadeOutTrack(area.Name);
		}
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
