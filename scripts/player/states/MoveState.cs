using System;
using Godot;

public class MoveState : PlayerState
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
        else
        {
            return new IdleState();
        }
        if (!player.IsOnFloor())
        {
            return new AirState();
        }

        if (Input.IsActionJustPressed("jump_button"))
        {
            player.velocity.y = -player.jump;
            player.sprite.Stop();
            player.sprite.Play("jump");
            return new AirState();
        }

        player.GroundMove(direction);

        if (Input.IsActionJustPressed("interact_button"))
        {
            player.HandleInteract();
        }

        if (Input.IsActionJustPressed("crouch_button"))
        {
            player.Drop();
        }

        return null;
    }
    public void HandleAnimationFinished(Player player)
    {
        player.sprite.Play("walk");
    }

    public void ExitState(Player player)
    {

    }
    public void EnterState(Player player)
    {
        player.sprite.Play("walk");
    }
}