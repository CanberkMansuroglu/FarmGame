using System.Collections;
using UnityEngine;

namespace Assets.Scripts.States.PlayerStates
{
    public class GetOutVehicleState : IState
    {
        readonly PlayerController _playerController;


        public GetOutVehicleState(PlayerController playerController)
        {
            _playerController = playerController;
        }
        public void Enter()
        {
            Debug.Log("Get Out Vehicle State");
            _playerController.vehicle.transform.Find("VehicleCam").gameObject.SetActive(false);
            _playerController.vehicle.GetComponent<CarController>().enabled = false;
            _playerController.characterController.enabled = true;
            _playerController.cam.gameObject.SetActive(true);
            _playerController.body.SetActive(true);
            _playerController.audioSource.Stop();

            _playerController.transform.position = _playerController.vehicle.GetComponent<CarController>().playerSp.position;


        }

        public void Exit()
        {

        }

        public void Tick()
        {
        }
    }
}