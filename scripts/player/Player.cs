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
	[Export]
	private float acceleration = 100;
	[Export]
	private int gravity = 100;
	private Sprite sprite;
	public List<Area2D> surroundingInteractives = new List<Area2D>();
	private Item pickedItem;
	public int ladders = 0;
	private string type = "light";
	private int doubleJump = 0;
	private int attack = 0;

	private string[] states =
	{
		"idle",
		"move",
		"air"
	};
	private List<string> stateStack = new List<string>();
	private string currentState = "idle";

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
	}
	public override void _PhysicsProcess(float delta)
	{

		switch (currentState)
		{
			case "idle":
				HandleIdleState();
				break;
			case "move":
				HandleMoveState();
				break;
			case "air":
				HandleAirState();
				break;
			default:
				GD.Print("Kyrpä");
				break;
		}
		GD.Print(currentState);

		if (ladders > 0)
		{
			HandleLadders();
		}
	}

	private void GroundMove(int direction)
	{
		velocity.y += gravity;
		velocity.x = maxSpeed * direction;
		velocity = MoveAndSlide(velocity);
	}
	private void AirMove(int direction)
	{
		velocity.y += gravity;
		velocity.x = maxSpeed * direction;
		velocity = MoveAndSlide(velocity, Vector2.Up);
		if (IsOnFloor())
		{
			ChangeState("move");
		}
	}

	private void HandleIdleState()
	{
		if (Input.IsActionJustPressed("left_button") || Input.IsActionJustPressed("right_button"))
		{
			ChangeState("move");
			return;
		}

		if (Input.IsActionJustPressed("jump_button"))
		{
			velocity.y -= jump;
			ChangeState("air");
			return;
		}

		if (Input.IsActionJustPressed("interact_button"))
		{
			HandleInteract();
		}
		if (velocity.y != 0)
		{
			ChangeState("air");
		}
		GroundMove(0);
	}
	private void HandleMoveState()
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
		else
		{
			ChangeState("idle");
			return;
		}
		if (velocity.y != 0)
		{
			ChangeState("air");
			return;
		}

		if (Input.IsActionJustPressed("jump_button"))
		{
			velocity.y -= jump;
			ChangeState("air");
			return;
		}

		GroundMove(direction);

		if (Input.IsActionJustPressed("interact_button"))
		{
			HandleInteract();
		}

	}
	private void HandleAirState()
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
		AirMove(direction);
		if (Input.IsActionJustPressed("interact_button"))
		{
			HandleInteract();
		}
		if (Input.IsActionJustPressed("jump_button"))
		{
			if (doubleJump > 0)
			{
				velocity.y -= jump;
				doubleJump--;
			}
		}
	}

	private void HandleInteract()
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


	private void ChangeState(string stateName)
	{
		ExitState(currentState);
		switch (stateName)
		{
			case "idle":
				stateStack.Insert(0, "idle");
				break;
			case "move":
				stateStack.Insert(0, "move");
				break;
			case "air":
				stateStack.Insert(0, "air");
				break;
			case "previous":
				stateStack.RemoveAt(0);
				currentState = stateStack[0];
				break;
			default:
				GD.Print("Error while changing state");
				break;
		}
		currentState = stateStack[0];
	}
	private void ExitState(string state)
	{
		switch (state)
		{
			case "idle":
				break;
			case "move":
				break;
			case "air":
				if (type.Equals("light"))
				{
					doubleJump = 1;
				}
				break;
			default:
				break;
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
