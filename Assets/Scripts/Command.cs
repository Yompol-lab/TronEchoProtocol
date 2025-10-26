using UnityEngine;

public interface ICommand
{
    void Execute(PlayerControllerRB player);
}

public class MoveCommand : ICommand
{
    private Vector2 input;
    public MoveCommand(Vector2 moveInput) => input = moveInput;

    public void Execute(PlayerControllerRB player)
    {
        player.Move(input);
    }
}

public class JumpCommand : ICommand
{
    public void Execute(PlayerControllerRB player)
    {
        player.Jump();
    }
}
