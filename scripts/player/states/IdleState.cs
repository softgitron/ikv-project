using Godot;

public class IdleState : PlayerState
{
    public void HandleAnimationFinished(Player player)
    {
        player.sprite.Play("idle");
    }
    public void ExitState(Player player)
    {

    }
    public void EnterState(Player player)
    {
        player.sprite.Play("idle");
    }
    public PlayerState ProcessState(Player player)
    {
        if (Input.IsActionJustPressed("left_button") || Input.IsActionJustPressed("right_button"))
        {
            return new MoveState();
        }

        if (Input.IsActionJustPressed("jump_button"))
        {
            player.velocity.y -= player.jump;
            return new MoveState();
        }

        if (Input.IsActionJustPressed("interact_button"))
        {
            player.HandleInteract();
        }

        if (Input.IsActionJustPressed("crouch_button"))
        {
            player.Drop();
        }

        player.GroundMove(0);

        if (player.velocity.y != 0)
        {
            return new AirState();
        }

        return null;
    }
}
