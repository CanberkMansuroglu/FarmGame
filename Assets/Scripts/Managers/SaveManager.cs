using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class SaveManager : MonoBehaviour
    {
        public int money;
        public int playerWheatCount;
        public int playerStrawCount;

        public int coopWaterCount;
        public int coopWheatCount;

        public int barnWaterCount;
        public int barnStrawCount;

        public int farmState;

        public PlayerController playerController;
        public CoopController coopController;
        public CoopController barnController;

        private void Awake()
        {

            money = playerController.money;
            playerWheatCount = playerController.wheatCount;
            playerStrawCount = playerController.strawCount;

            coopWaterCount = coopController.currentWater;
            coopWheatCount = coopController.currentFeed;

            barnStrawCount = barnController.currentFeed;
            barnWaterCount = barnController.currentWater;

            //---------------   PLAYER    --------------------------------------

            if (!PlayerPrefs.HasKey("Money"))
            {
                PlayerPrefs.SetInt("Money",money);
            }

            if (!PlayerPrefs.HasKey("PlayerWheatCount"))
            {
                PlayerPrefs.SetInt("PlayerWheatCount", playerWheatCount);
            }

            if (!PlayerPrefs.HasKey("PlayerStrawCount"))
            {
                PlayerPrefs.SetInt("PlayerStrawCount", playerStrawCount);
            }

            //---------------   COOP    --------------------------------------
            if (!PlayerPrefs.HasKey("CoopWaterCount"))
            {
                PlayerPrefs.SetInt("CoopWaterCount", coopWaterCount);
            }
            if (!PlayerPrefs.HasKey("CoopWheatCount"))
            {
                PlayerPrefs.SetInt("CoopWheatCount", coopWheatCount);
            }

            //---------------   BARN    --------------------------------------

            if (!PlayerPrefs.HasKey("BarnWaterCount"))
            {
                PlayerPrefs.SetInt("BarnWaterCount", barnWaterCount);
            }
            if (!PlayerPrefs.HasKey("BarnStrawCount"))
            {
                PlayerPrefs.SetInt("BarnStrawCount", barnStrawCount);
            }

            //---------------   FARM    --------------------------------------

            if (!PlayerPrefs.HasKey("FarmState"))
            {
                PlayerPrefs.SetInt("FarmState", farmState);

            }

        }


        public void SaveScene()
        {
            money = playerController.money;
            playerWheatCount = playerController.wheatCount;
            playerStrawCount = playerController.strawCount;

            coopWaterCount = coopController.currentWater;
            coopWheatCount = coopController.currentFeed;

            barnStrawCount = barnController.currentFeed;
            barnWaterCount = barnController.currentWater;


            PlayerPrefs.SetInt("Money", money);

            PlayerPrefs.SetInt("PlayerWheatCount", playerWheatCount);
            PlayerPrefs.SetInt("PlayerStrawCount", playerStrawCount);

            PlayerPrefs.SetInt("CoopWaterCount", coopWaterCount);
            PlayerPrefs.SetInt("CoopWheatCount", coopWheatCount);

            PlayerPrefs.SetInt("BarnWaterCount", barnWaterCount);
            PlayerPrefs.SetInt("BarnStrawCount", barnStrawCount);








        }


    }
}