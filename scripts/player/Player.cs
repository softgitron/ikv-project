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
	public bool ladder_on = false;
	private AudioSource audioSource;

	public override void _Ready()
	{
		SetPhysicsProcess(false);
	}
	public void Initialize()
	{
		sprite = (Sprite)GetNode("Sprite");
		SetPhysicsProcess(true);
		this.audioSource = new AudioSourceImplementation(this);
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
}
