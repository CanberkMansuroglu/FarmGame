using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class CowControllers : MonoBehaviour
    {

        public CoopController coopController;


        void Start()
        {
            coopController = GetComponentInParent<CoopController>();

            StartCoroutine(EggSpawn(5f));
        }

        IEnumerator EggSpawn(float eggSpawmTime)
        {
            while (true)
            {
                yield return new WaitForSeconds(eggSpawmTime);

                if (coopController.currentFeed > 0 && coopController.currentWater>0)
                {
                    coopController.Feed();
                    coopController.EggCounter(1);
                    coopController.DrinkWater();


                }

            }


        }
    }
}