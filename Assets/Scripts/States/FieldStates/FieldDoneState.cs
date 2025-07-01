using System.Collections;
using UnityEngine;

namespace Assets.Scripts.States.FieldStates
{
    public class FieldDoneState : IState
    {
        readonly FieldController _fieldController;

        public FieldDoneState(FieldController fieldController)
        {
            _fieldController = fieldController;
        }

        public void Enter()
        {
           

            PlayerPrefs.SetInt(_fieldController.farmID, 3);


            Debug.Log("FieldDoneState");

            _fieldController.emptyField.SetActive(false);
            _fieldController.processedField.SetActive(false);
            _fieldController.growingField.SetActive(false);
            _fieldController.seededField.SetActive(true);
            _fieldController.isGrowing = false;
            _fieldController.isDone = true;

        }

        public void Exit()
        {
            _fieldController.playerController.StrawCounter(5);
            _fieldController.playerController.WheatCounter(25);

            _fieldController.audioSource.PlayOneShot(_fieldController.bicmeSound);

        }

        public void Tick()
        {

        }
    }
}