using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace Assets.Scripts.States.AnimalStates
{
    public class TeenState : IState
    {
        readonly AnimalController _animalController;

        public TeenState(AnimalController animalController)
        {
            _animalController = animalController;
        }

        public void Enter()
        {
            PlayerPrefs.SetInt(_animalController.gameObject.GetInstanceID().ToString() + "State", 1);



            Debug.Log("TeenState");

            _animalController.babyBody.SetActive(false);
            _animalController.adultBody.SetActive(true);

            _animalController.isBaby = false;
            _animalController.isTeen = true;
            _animalController.isAdult = false;

            _animalController.animalState = "Teen";

            _animalController.chikenAnim.SetFloat("Speed_f", 0.6f);

            _animalController.animalSellPrice = _animalController.animalBuyPrice;

            _animalController.UIThing();


            _animalController.adultBody.transform.DOScale(_animalController.adultBody.transform.localScale/1.5f,_animalController.teenToAdultTime).From();
            _animalController.StartCoroutine(_animalController.SwitchAnimalState(_animalController.teenToAdultTime));
            _animalController.StartCoroutine(_animalController.ProductSpawn(_animalController._productSpawnTime*2));

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