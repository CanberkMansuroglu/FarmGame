using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
public class CoopController : MonoBehaviour
{
    public bool isCoop;


    public PlayerController playerController;

    [Header("UI")]
    public TMP_Text animalText;
    public TMP_Text productText;

    [Header("GameObjects")]

    public int maxAnimalCount;

    public int maxFeed = 100; // Kümesin maksimum alabileceði yem miktarý
    public int currentFeed = 0; // Kümesin þu anki yem miktarý
    public int inCoopYem = 0; // Kümese eklenecek yem miktarý

    public int maxWater;
    public int currentWater;

    public string productName;

    public List<GameObject> animals;
    public List<GameObject> activeAnimals;

    public Image yemBar;
    public Image waterBar;

    public int animalIndex;

    public int eggCount;

    public float delay = 5f;

    [SerializeField] Transform pathParent;

    #region UI

    public GameObject parentUI;



    public GameObject sellAnimalUI;

    public static string[] animalNames ={
            "Bella", "Max", "Charlie", "Luna", "Rocky", "Lucy", "Cooper", "Daisy", "Milo", "Bailey",
            "Zoe", "Buddy", "Lola", "Teddy", "Molly", "Oliver", "Sophie", "Toby", "Chloe", "Leo",
            "Sadie", "Oscar", "Ruby", "Coco", "Jack", "Rosie", "Simba", "Maggie", "Finn", "Milo",
            "Jasper", "Lily", "Duke", "Ellie", "Marley", "Mia", "Ginger", "Archie", "Roxy", "Louie",
            "Hazel", "Tank", "Poppy", "Bentley", "Nala", "Winston", "Pepper", "Murphy", "Abby", "Zeus",
            "Harley", "Penny", "Frankie", "Dakota", "Remy", "Honey", "Shadow", "Mocha", "Scout", "Minnie",
            "Bear", "Olive", "Ranger", "Misty", "Gus", "Juno", "Willow", "Rufus", "Sugar", "Simba",
            "Riley", "Sasha", "Baxter", "Ivy", "Benny", "Nina", "Ace", "Holly", "Tucker", "Cleo",
            "Rusty", "Muffin", "King", "Lulu", "Blue", "Maple", "Diesel", "Maisie", "Storm", "Django",
            "Fiona", "Oreo", "Thor", "Bonnie", "Otis", "Cinnamon", "Bambi", "Athena", "Casper", "Sunny"
        };


    #endregion

    private void Awake()
    {
        if (isCoop)
        {
            if (!PlayerPrefs.HasKey("ChikenCount"))
            {
                PlayerPrefs.SetInt("ChikenCount", activeAnimals.Count);
            }
        }
        else
        {
            if (!PlayerPrefs.HasKey("CowCount"))
            {
                PlayerPrefs.SetInt("CowCount", activeAnimals.Count);


            }
        }
    }

    private void Start()
    {
        if (isCoop)
        {
            currentFeed = PlayerPrefs.GetInt("CoopWheatCount");
            currentWater = PlayerPrefs.GetInt("CoopWaterCount");
        }
        else
        {
            currentFeed = PlayerPrefs.GetInt("BarnStrawCount");
            currentWater = PlayerPrefs.GetInt("BarnWaterCount");
        }
        animalText.text = $"buy \n({activeAnimals.Count}/{maxAnimalCount})";
        productText.text = $" sell {productName}  {eggCount} ";

        playerController = FindAnyObjectByType<PlayerController>();

        yemBar.fillAmount = (float)currentFeed / maxFeed;
        waterBar.fillAmount = (float)currentWater / maxWater;

        for (int i = 0; i < PlayerPrefs.GetInt(isCoop ? "ChikenCount" : "CowCount"); i++)
        {
            CreateAnimal();
            
        }


    }

    public void BuyAnimal()
    {

        if (playerController.canBuy && activeAnimals.Count < maxAnimalCount)
        {
            CreateAnimal();
            PlayerPrefs.SetInt(isCoop ? "ChikenCount" : "CowCount", activeAnimals.Count);

        }

    }

    public void CreateAnimal()
    {

        Vector3[] randomChildren = SelectRandomChildPositions(pathParent, 10);

        animals[0].SetActive(true);
        GameObject animal = animals[0];
        activeAnimals.Add(animal);
        animals.RemoveAt(0);



        animal.transform.position = randomChildren[0];
        animalIndex++;
        GameObject animalUI = Instantiate(parentUI, sellAnimalUI.transform);


        animal.GetComponent<AnimalController>().coopController = this;

        animal.GetComponent<AnimalController>().animalUI = animalUI;

        animal.GetComponent<AnimalController>().animalName = animalNames[Random.Range(0, animalNames.Length - 1)];


        // randomChildren[randomChildren.Length - 1] = randomChildren[0];
        animalText.text = $"buy \n({activeAnimals.Count}/{maxAnimalCount})";

        Sequence sequence = DOTween.Sequence();

        for (int i = 1; i < randomChildren.Length;i++)
        {


            animal.GetComponentInChildren<Animator>().SetFloat("Speed_f", 0f);

            sequence.Append(animal.transform.DOMove(randomChildren[i], Random.Range(7,13))
                .SetEase(Ease.Linear)
                .SetDelay(delay)
                .OnStart(() =>
                {
                    animal.GetComponentInChildren<Animator>().SetFloat("Speed_f", 0f);
                })
                .OnPlay(() =>
                {
                    animal.GetComponentInChildren<Animator>().SetFloat("Speed_f", 0.6f);
                })
                .OnComplete(() =>
                {
                    animal.GetComponentInChildren<Animator>().SetFloat("Speed_f", 0f);
                })

                );
            sequence.Join(animal.transform.DOLookAt(randomChildren[i], 0.1f));

            

        }
    }


    Vector3[] SelectRandomChildPositions(Transform parent, int count)
    {
        // Parent'in tüm çocuklarýný listeye ekle
        List<Transform> children = new List<Transform>();
        foreach (Transform child in parent)
        {
            children.Add(child);
        }

        // Rastgele eleman seçimi
        List<Vector3> selectedPositions = new List<Vector3>();
        List<int> usedIndexes = new List<int>();
        System.Random random = new System.Random();

        while (selectedPositions.Count < count && selectedPositions.Count < children.Count)
        {
            int randomIndex = random.Next(0, children.Count);

            if (!usedIndexes.Contains(randomIndex))
            {
                selectedPositions.Add(children[randomIndex].position); // Çocuðun pozisyonunu ekle
                usedIndexes.Add(randomIndex);
            }
        }

        return selectedPositions.ToArray(); // Listeyi diziye dönüþtürüp döndür
    }

    public void EggCounter(int egg)
    {
        eggCount += egg;
        productText.text = $"sell  {productName}  {eggCount} ";


    }

    public void SellProduct(int cost)
    {
        if (eggCount > 0)
        {
            eggCount--;
            productText.text = $"sell {productName}  {eggCount} ";

            playerController.MoneyCounter(cost);
        }
    }

    public void Feed()
    {
        currentFeed--;
        yemBar.fillAmount = (float)currentFeed / maxFeed;
        if (isCoop)
        {
            PlayerPrefs.SetInt("CoopWheatCount", currentFeed);
        }
        else
        {
            PlayerPrefs.SetInt("BarnStrawCount", currentFeed);


        }


    }

    public void YemCounter()
    {
        inCoopYem = maxFeed - currentFeed;

        if (isCoop)
        {

            if (playerController.wheatCount >= inCoopYem)
            {
                playerController.WheatCounter(-inCoopYem);
                currentFeed = maxFeed;

            }
            else
            {
                currentFeed += playerController.wheatCount;
                playerController.WheatCounter(-playerController.wheatCount);
            }
            PlayerPrefs.SetInt("CoopWheatCount", currentFeed);


        }

        else
        {
            if (playerController.strawCount >= inCoopYem)
            {
                playerController.StrawCounter(-inCoopYem);
                currentFeed = maxFeed;
            }
            else
            {
                currentFeed += playerController.strawCount;
                playerController.StrawCounter(-playerController.strawCount);
            }

            PlayerPrefs.SetInt("BarnStrawCount", currentFeed);

        }
        yemBar.fillAmount = (float)currentFeed / maxFeed;
    }

    public void DrinkWater()
    {
        currentWater--;
        waterBar.fillAmount = (float)currentWater / maxWater;
        if (isCoop)
        {
            PlayerPrefs.SetInt("CoopWaterCount", currentFeed);
        }
        else
        {
            PlayerPrefs.SetInt("BarnWaterCount", currentFeed);


        }

    }

    public void WaterCounter()
    {
        currentWater = maxWater;
        waterBar.fillAmount = (float)currentWater / maxWater;

        if (isCoop)
        {
            PlayerPrefs.SetInt("CoopWaterCount", currentFeed);
        }
        else
        {
            PlayerPrefs.SetInt("BarnWaterCount", currentFeed);


        }
    }
}
