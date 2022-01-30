public interface PlayerState
{
    PlayerState ProcessState(Player player);
    void ExitState(Player player);
}