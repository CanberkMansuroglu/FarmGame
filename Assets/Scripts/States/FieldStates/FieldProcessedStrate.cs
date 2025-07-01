using System.Collections;
using UnityEngine;

namespace Assets.Scripts.States.FieldStates
{
    public class FieldProcessedStrate : IState
    {

        readonly FieldController _fieldController;

        public FieldProcessedStrate(FieldController fieldController)
        {
            _fieldController = fieldController;
        }

        public void Enter()
        {

            PlayerPrefs.SetInt(_fieldController.farmID, 1);

            Debug.Log("FieldProcessedStrate");

            _fieldController.isEmpty = false;
            _fieldController.isProcessed = true;

            _fieldController.emptyField.SetActive(false);
            _fieldController.processedField.SetActive(true);
            _fieldController.growingField.SetActive(false);
            _fieldController.seededField.SetActive(false);

        }

        public void Exit()
        {

        }

        public void Tick()
        {

        }
    }
}