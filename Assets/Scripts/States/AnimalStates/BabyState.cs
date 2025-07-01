using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace Assets.Scripts.States.AnimalStates
{
    public class BabyState : IState
    {
        readonly AnimalController _animalController;

        public BabyState(AnimalController animalController)
        {
            _animalController = animalController;
        }

        public void Enter()
        {
            PlayerPrefs.SetInt(_animalController.gameObject.GetInstanceID().ToString() + "State", 0);



            Debug.Log("BabyState");

            _animalController.adultBody.SetActive(false);
            _animalController.babyBody.SetActive(true);


            _animalController.isBaby = true;
            _animalController.isTeen = false;
            _animalController.isAdult = false;

            _animalController.animalState = "Baby";

            _animalController.animalSellPrice = _animalController.animalBuyPrice / 2;

            _animalController.babyBody.transform.DOScale(_animalController.babyBody.transform.localScale*1.5f,_animalController.babyToTeenTime);
            _animalController.StartCoroutine(_animalController.SwitchAnimalState(_animalController.babyToTeenTime));

            _animalController.animalUI.transform.Find("stateUI").GetComponent<TMPro.TextMeshProUGUI>().text = _animalController.animalState;
            _animalController.animalUI.transform.Find("priceUI").GetComponent<TMPro.TextMeshProUGUI>().text = _animalController.animalSellPrice.ToString();

           

        }

        public void Exit()
        {
            _animalController.StopAllCoroutines();

        }

        public void Tick()
        {
        }
    }
}