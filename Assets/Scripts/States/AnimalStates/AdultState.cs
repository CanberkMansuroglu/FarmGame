using System.Collections;
using UnityEngine;

namespace Assets.Scripts.States.AnimalStates
{
    public class AdultState : IState
    {

        readonly AnimalController _animalController;

        public AdultState(AnimalController animalController)
        {
            _animalController = animalController;
        }

        public void Enter()
        {
            PlayerPrefs.SetInt(_animalController.gameObject.GetInstanceID().ToString() + "State", 2);

            Debug.Log("AdultState");

            _animalController.babyBody.SetActive(false);
            _animalController.adultBody.SetActive(true);

            _animalController.isBaby = false;
            _animalController.isTeen = false;
            _animalController.isAdult = true;

            _animalController.animalState = "Adult";

            _animalController.chikenAnim.SetFloat("Speed_f", 0.6f);

            _animalController.UIThing();

            _animalController.animalSellPrice = _animalController.animalBuyPrice * 2;

            _animalController.StartCoroutine(_animalController.ProductSpawn(_animalController._productSpawnTime));

            _animalController.animalUI.transform.Find("stateUI").GetComponent<TMPro.TextMeshProUGUI>().text = _animalController.animalState;
            _animalController.animalUI.transform.Find("priceUI").GetComponent<TMPro.TextMeshProUGUI>().text = _animalController.animalSellPrice.ToString();

            



        }

        public void Exit()
        {
        }

        public void Tick()
        {
        }
    }
}