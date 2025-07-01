using UnityEngine;
using UnityEngine.EventSystems;

public class RunState : IState
{
    readonly PlayerController _playerController;

    public RunState(PlayerController playerController)
    {
        _playerController = playerController;
    }
    public void Enter()
    {
        
        Debug.Log("RunState");
        _playerController.InputMove(_playerController.runSpeed);
        _playerController.audioSource.Play();
        _playerController.audioSource.pitch = 1.8f;


    }

    public void Exit()
    {
        _playerController.InputMove(0);
    }

    public void Tick()
    {

       _playerController.InputMove(_playerController.runSpeed);
    }

   

}
