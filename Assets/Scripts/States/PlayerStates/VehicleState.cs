using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using Unity.Cinemachine;

namespace Assets.Scripts.States.PlayerStates
{
    public class VehicleState : IState
    {
        readonly PlayerController _playerController;

        private float timer = 0f; // Sayaç başlangıç değeri
        private bool isCounting = false; // Sayaç çalışıyor mu?

        public VehicleState(PlayerController playerController)
        {
            _playerController = playerController;
        }

        public void Enter()
        {
            Debug.Log("Vehicle State");

            _playerController.raycastDistance = 0f;
            _playerController.audioSource.Stop();


            timer = 0f;
            isCounting = true;

            _playerController.vehicle.transform.Find("VehicleCam").gameObject.SetActive(true);

            _playerController.vehicle.GetComponent<CarController>().enabled = true;

            _playerController.characterController.enabled = false;
            _playerController.cam.gameObject.SetActive(false);
            _playerController.body.SetActive(false);
            _playerController.interactionText.text = "Press F to Exit";


            _playerController.vehicle.GetComponent<CarController>().raycastDistance= 10f;

            _playerController.cinemachineCamera.Follow = _playerController.vehicle.transform;
            _playerController.cinemachineCamera.gameObject.GetComponent<CinemachineOrbitalFollow>().Radius = 12f;






        }

        public void Exit()
        {
            




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

            if (Input.GetKeyDown(KeyCode.F) && !isCounting)
            {
                _playerController.isInVehicle = false;
            }
        }




    }
}