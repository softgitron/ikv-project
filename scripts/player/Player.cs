using Godot;
using System;
using System.Linq;

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
	private PositionSync positionSync;
	private Toggleable toggle;
	public bool ladder_on = false;

	public override void _Ready()
	{
		SetPhysicsProcess(false);
	}
	public void Initialize()
	{
		sprite = (Sprite)GetNode("Sprite");
		SetPhysicsProcess(true);
		positionSync = new PositionSync(this, Main.player, "player" + Main.player.ToString());
	}
	public override void _PhysicsProcess(float delta)
	{
		positionSync.Update();
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
		velocity.y += gravity;
		currentSpeed = maxSpeed;
		velocity.x = currentSpeed * direction;

		velocity = MoveAndSlide(velocity);

		if (ladder_on == true) {
			gravity = 0;
			if (Input.IsActionPressed("jump_button")) {
				velocity.y = -maxSpeed;
			}
			else if (Input.IsActionPressed("crouch_button")) {
				velocity.y = maxSpeed;
			}
			else
			{
				velocity.y = 0; 
			}
		}
		else {
			gravity = 100;
		}
	}
	private void _on_Player_2D_area_entered(Area2D area)
	{
	if (area is Toggleable) {
			toggle = (Toggleable) area;
		}
	}

	private void _on_Player_2D_area_exited(Area2D area)
	{
		if (area == toggle) {
			toggle = null;
		}
	}

	public override void _Input(InputEvent inputEvent)
	{
		if (inputEvent.IsActionPressed("interact") && toggle != null)
		{
			toggle.Toggle();
		}
	}
}
