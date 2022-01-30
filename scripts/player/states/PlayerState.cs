public interface PlayerState
{
    PlayerState ProcessState(Player player);
    void ExitState(Player player);
    void EnterState(Player player);
    void HandleAnimationFinished(Player player);
}