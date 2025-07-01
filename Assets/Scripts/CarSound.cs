using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class CarSound : MonoBehaviour
    {
        private Rigidbody carRB;
        private AudioSource carAudioSource;

        public float minSpeed;
        public float maxSpeed;
        private float currentSpeed;

        public float minPitch;
        public float maxPitch;
        private float pitchFromCar;

        private void Start()
        {
            carRB = GetComponent<Rigidbody>();
            carAudioSource = GetComponent<AudioSource>();
        }

        private void LateUpdate()
        {
            EngineSound();


        }

        void EngineSound()
        {
            currentSpeed = carRB.linearVelocity.magnitude;
            pitchFromCar = carRB.linearVelocity.magnitude / 5f;

            

            if (currentSpeed < minSpeed)
            {
                carAudioSource.pitch = minPitch;

            }

            if (currentSpeed > minSpeed && currentSpeed<maxSpeed) 
            { 
                carAudioSource.pitch= minPitch+pitchFromCar;

            }

            if (currentSpeed > maxSpeed)
            {
                carAudioSource.pitch = maxPitch;
            }

        }

    }
}