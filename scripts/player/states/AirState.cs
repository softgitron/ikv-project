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
				player.velocity.y -= player.jump;
				player.doubleJump--;
			}
		}

		return null;
	}

	public void ExitState(Player player)
	{
		if (player.type.Equals("light"))
		{
			player.doubleJump = 1;
		}
	}
}
