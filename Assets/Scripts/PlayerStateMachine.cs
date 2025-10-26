public abstract class PlayerState
{
    public abstract void Enter(PlayerControllerRB player);
    public abstract void Update(PlayerControllerRB player);
}

public class WalkingState : PlayerState
{
    public override void Enter(PlayerControllerRB player) { }

    public override void Update(PlayerControllerRB player)
    {
        player.HandleMovement();

        if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Space))
            player.ChangeState(new JumpingState());
    }
}

public class JumpingState : PlayerState
{
    public override void Enter(PlayerControllerRB player)
    {
        player.PerformJump();
    }

    public override void Update(PlayerControllerRB player)
    {
        if (player.IsGrounded)
            player.ChangeState(new WalkingState());
    }
}
