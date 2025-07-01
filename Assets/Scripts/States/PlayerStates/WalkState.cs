using UnityEngine;

public class WalkState : IState
{

    readonly PlayerController _playerController;

    public WalkState(PlayerController playerController)
    {
        _playerController = playerController;
    }
    public void Enter()
    {
        Debug.Log("WalkState");
        _playerController.InputMove(_playerController.walkSpeed);
        _playerController.audioSource.Play();
        _playerController.audioSource.pitch = 1;


    }

    public void Exit()
    {
    }

    public void Tick()
    {
        _playerController.InputMove(_playerController.walkSpeed);
    }
}
