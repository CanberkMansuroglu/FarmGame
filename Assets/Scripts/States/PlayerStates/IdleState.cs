using Unity.Cinemachine;
using UnityEngine;

public class IdleState : IState
{
    readonly PlayerController _playerController;

    public IdleState( PlayerController playerController)
    {
        _playerController = playerController;
    }

    public void Enter()
    {
        Debug.Log("IdleState");
        _playerController.animator.SetFloat("Speed_f",0);
        _playerController.audioSource.Stop();


        _playerController.raycastDistance = 50f;

        _playerController.cinemachineCamera.Follow = _playerController.camPivot.transform;
        _playerController.cinemachineCamera.gameObject.GetComponent<CinemachineOrbitalFollow>().Radius = 6f;

    }

    public void Exit()
    {

    }

    public void Tick()
    {

    }
}
