using System;
using Godot;

public class AirState : PlayerState
{
	public PlayerState ProcessState(Player player)
	{
		int direction = Math.Sign(Input.GetActionStrength("right_button") - Input.GetActionStrength("left_button"));
		if (direction > 0)
		{
			player.sprite.FlipH = false;
		}
		else if (direction < 0)
		{
			player.sprite.FlipH = true;
		}
		player.AirMove(direction);
		if (Input.IsActionJustPressed("interact_button"))
		{
			player.HandleInteract();
		}
		if (Input.IsActionJustPressed("jump_button"))
		{
			if (player.doubleJump > 0)
			{
				player.velocity.y = -player.jump;
				player.sprite.Stop();
				player.sprite.Play("jump");
				player.doubleJump--;
			}
		}

		return null;
	}
	public void HandleAnimationFinished(Player player)
	{
		player.sprite.Play("idle"); // Fall animation here
	}

	public void ExitState(Player player)
	{
		if (Main.player == 2)   // If player is light
		{
			player.doubleJump = 1;
		}
	}
	public void EnterState(Player player)
	{
		player.sprite.Stop();
		player.sprite.Play("jump");
	}
}
