using System.Collections;
using UnityEngine;

namespace Assets.Scripts.States.FieldStates
{
    public class FieldEmptyState : IState
    {
        readonly FieldController _fieldController;

        public FieldEmptyState(FieldController fieldController)
        {
            _fieldController = fieldController;
        }

        public void Enter()
        {
            PlayerPrefs.SetInt(_fieldController.farmID,0);

            Debug.Log("FieldEmptyState");

            _fieldController.isDone = false;

            _fieldController.emptyField.SetActive(true);
            _fieldController.processedField.SetActive(false);
            _fieldController.growingField.SetActive(false);
            _fieldController.seededField.SetActive(false);
        }

        public void Exit()
        {
            _fieldController.audioSource.PlayOneShot(_fieldController.plantSound);

        }

        public void Tick()
        {

        }
    }
}