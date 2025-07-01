using Assets.Scripts.States.FieldStates;
using System.Collections;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

namespace Assets.Scripts
{
    public class FieldController : MonoBehaviour
    {

        public float growTime = 5f;
        public float growSpeed = 15f;

        public PlayerController playerController;

        StateMachine _stateMachine;

        public GameObject emptyField;
        public GameObject growingField;
        public GameObject processedField;
        public GameObject seededField;

        //public GameObject doneField;

        public bool isEmpty = true;
        public bool isProcessed = false;
        public bool isGrowing = false;
        public bool isDone = false;

        public string farmID;

        public AudioClip plantSound;
        public AudioClip bicmeSound;

        public AudioSource audioSource;



        private void Awake()
        {
            farmID = gameObject.GetInstanceID().ToString();

            _stateMachine = new StateMachine();
            playerController = FindAnyObjectByType<PlayerController>();

            if (!PlayerPrefs.HasKey(farmID))
            {
                PlayerPrefs.SetInt(farmID,0);
            }

        }

        private void Start()
        {
            IState emptyState = new FieldEmptyState(this);
            IState processedState = new FieldProcessedStrate(this);
            IState growingState = new FieldGrowingState(this);
            IState doneState = new FieldDoneState(this);

            _stateMachine.SetNormalState(emptyState, processedState, () => isProcessed);
            _stateMachine.SetNormalState(processedState, growingState, () => isGrowing);
            _stateMachine.SetNormalState(growingState, doneState, () => isDone);
            _stateMachine.SetNormalState(doneState, emptyState, () => isEmpty);

            if (PlayerPrefs.GetInt(farmID) == 0)
            {
                _stateMachine.SetState(emptyState);

            }
            else if (PlayerPrefs.GetInt(farmID) == 1)
            {
                _stateMachine.SetState(processedState);

            }
            else if (PlayerPrefs.GetInt(farmID) == 2)
            {
                _stateMachine.SetState(growingState);

            }
            else 
            {
                _stateMachine.SetState(doneState);

            }



            audioSource = GetComponent<AudioSource>();





        }

        private void Update()
        {
            _stateMachine.Tick();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<CarController>() != null)
            {
                if (!other.gameObject.GetComponent<CarController>().isTractor && isDone)
                {
                    isEmpty = true;

                }
            }

        }




    }
}