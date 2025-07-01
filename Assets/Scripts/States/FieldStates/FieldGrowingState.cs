using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace Assets.Scripts.States.FieldStates
{
    public class FieldGrowingState : IState
    {
        readonly FieldController _fieldController;

        public FieldGrowingState(FieldController fieldController)
        {
            _fieldController = fieldController;
        }

        public void Enter()
        {

            PlayerPrefs.SetInt(_fieldController.farmID, 2);

            Debug.Log("FieldGrowingState");

            _fieldController.isProcessed = false;   
            _fieldController.isGrowing = true;

            _fieldController.emptyField.SetActive(false);
            _fieldController.processedField.SetActive(false);
            _fieldController.growingField.SetActive(true);
            _fieldController.seededField.SetActive(false);


            DOVirtual.DelayedCall(_fieldController.growTime, () => Grow());


        }

        public void Exit()
        {
        }

        public void Tick()
        {
        }

        public void Grow()
        {
            Debug.Log("büyüme aşamasıdna");
            _fieldController.seededField.SetActive(true);
            _fieldController.growingField.SetActive(false);



            foreach (Transform child in _fieldController.seededField.transform)
            {
                child.localScale = Vector3.zero;

                child.DOScale(new Vector3(5f, 1f, 1f), _fieldController.growSpeed).OnComplete(() => _fieldController.isDone = true);

            }



        }
    }
}