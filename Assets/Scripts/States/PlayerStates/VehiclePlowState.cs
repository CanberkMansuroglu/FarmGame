using System.Collections;
using UnityEngine;

namespace Assets.Scripts.States.PlayerStates
{
    public class VehiclePlowState : IState
    {
        readonly PlayerController _playerController;


        private float timer = 0f; // Sayaç başlangıç değeri
        private bool isCounting = false; // Sayaç çalışıyor mu?

        public VehiclePlowState(PlayerController playerController)
        {
            _playerController = playerController;
        }

        public void Enter()
        {
            Debug.Log("Vehicle Plow State");
            _playerController.vehicle.GetComponent<CarController>().raycastDistance = 0f;



        }

        public void Exit()
        {
            _playerController.vehicle.GetComponent<CarController>().trailer.transform.parent=null;
            _playerController.raycastDistance = 10f;


        }

        public void Tick()
        {
            if (timer >= 2f)
            {
                timer = 0f;
                isCounting = false;
            }
            else
            {
                timer += Time.deltaTime;

            }

            if (Input.GetKeyDown(KeyCode.E) && !isCounting)
            {
                _playerController.isVehiclePlow = false;
            }
        }
    }
}