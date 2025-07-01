using Assets.Scripts.States.AnimalStates;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SocialPlatforms;
using UnityEngine.Audio;

public class AnimalController : MonoBehaviour
{
    StateMachine _stateMachine;

    public GameObject babyBody;
    public GameObject adultBody;

    public GameObject animalUI;

    public bool isBaby;
    public bool isTeen;
    public bool isAdult;


    public int animalBuyPrice;


    public string animalName;
    public string animalType;
    public string animalState;

    public int animalSellPrice;


    public Animator chikenAnim;

    public float babyToTeenTime = 30f;
    public float teenToAdultTime = 120f;

    public float _productSpawnTime = 3f;

    public int animalStateNumber;


    public CoopController coopController;

    AudioSource audioSource;

    private void Awake()
    {
        _stateMachine = new StateMachine();

        if (!PlayerPrefs.HasKey(gameObject.GetInstanceID().ToString()+"State"))
        {
            PlayerPrefs.SetInt(gameObject.GetInstanceID().ToString() + "State", 0);
        }



    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        StartCoroutine(PlayAudioWithRandomDelay());

        if (!PlayerPrefs.HasKey(gameObject.GetInstanceID().ToString() + "Name"))
        {
            PlayerPrefs.SetString(gameObject.GetInstanceID().ToString() + "Name", animalName);
        }

        animalName = PlayerPrefs.GetString(gameObject.GetInstanceID().ToString() + "Name");
        

        IState babyState = new BabyState(this);
        IState teenState = new TeenState(this);
        IState adultState = new AdultState(this);

        _stateMachine.SetNormalState(babyState,teenState,()=>isTeen);
        _stateMachine.SetNormalState(teenState, adultState, () => isAdult);

        if (PlayerPrefs.GetInt(gameObject.GetInstanceID().ToString() + "State") ==0)
        {
            _stateMachine.SetState(babyState);

        }
        else if(PlayerPrefs.GetInt(gameObject.GetInstanceID().ToString() + "State") == 1)
        {
            _stateMachine.SetState(teenState);

        }
        else
        {
            _stateMachine.SetState(adultState);
        }


        GameObject uýChick = Instantiate(babyBody,animalUI.transform.Find("bodyUI"));

        if (!coopController.isCoop)
        {
            uýChick.transform.localScale = uýChick.transform.localScale/2;
        }

        uýChick.layer = 5;
        uýChick.AddComponent<RotateForever>();
        foreach (Transform child in uýChick.transform)
        {
            child.gameObject.layer = 5;
            foreach (Transform child2 in child)
            {
                child2.gameObject.layer = 5;
            }
        }

        PlayerPrefs.SetString(gameObject.GetInstanceID().ToString() + "Name", animalName);

        animalUI.transform.Find("nameUI").GetComponent<TMPro.TextMeshProUGUI>().text = animalName;
        animalUI.transform.Find("typeUI").GetComponent<TMPro.TextMeshProUGUI>().text = animalType;
        animalUI.transform.Find("Button").gameObject.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(SellAnimal);

    }

    private void Update()
    {
        _stateMachine.Tick();
    }



    public IEnumerator SwitchAnimalState(float delay)
    {

        while (true)
        {
            yield return new WaitForSeconds(delay);

            if (isBaby && coopController.currentFeed > 0 && coopController.currentWater > 0)
            {
                isBaby = false;
                isTeen = true;
                coopController.Feed();
                coopController.DrinkWater();

                UIThing();



            }

            else if (isTeen && coopController.currentFeed > 0 && coopController.currentWater > 0)
            {
                isTeen = false;
                isAdult = true;
                coopController.Feed();
                coopController.DrinkWater();
            }
        }
        


    }

    public IEnumerator ProductSpawn(float productSpawnTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(productSpawnTime);

            if (coopController.currentFeed>0 && coopController.currentWater>0)
            {
                coopController.EggCounter(1);
                coopController.Feed();
                coopController.DrinkWater();

            }

        }


    }
    IEnumerator PlayAudioWithRandomDelay()
    {
        while (true) // Döngüyle sürekli rastgele sürelerle ses çalabilir
        {
            float randomDelay = Random.Range(3f, 100f); // Rastgele süreyi belirle (20-30 sn)
            yield return new WaitForSeconds(randomDelay); // Rastgele süre kadar bekle

            audioSource.Play(); // Ses çal
        }
    }

    public void SellAnimal()
    {
        coopController.playerController.SellAnimal(animalSellPrice);

        Destroy(animalUI);

       

        coopController.animals.Add(gameObject);
        coopController.activeAnimals.Remove(gameObject);

        PlayerPrefs.SetInt(coopController.isCoop ? "ChikenCount" : "CowCount", coopController.activeAnimals.Count);

        coopController.animalText.text = $"buy\n({coopController.animalIndex}/{coopController.maxAnimalCount})";
        gameObject.SetActive(false);
    }


    public void UIThing()
    {
        //Destroy(animalUI.transform.Find("bodyUI").GetChild(0).gameObject);

        GameObject uýChick = Instantiate(adultBody, animalUI.transform.Find("bodyUI"));
        if (!coopController.isCoop)
        {
            uýChick.transform.localScale = uýChick.transform.localScale / 2;
        }
        uýChick.SetActive(true);
        uýChick.layer = 5;
        uýChick.AddComponent<RotateForever>();
        foreach (Transform child in uýChick.transform)
        {
            child.gameObject.layer = 5;
            child.gameObject.layer = 5;
            foreach (Transform child2 in child)
            {
                child2.gameObject.layer = 5;
            }
        }
    }
    public void SaveState()
    {
        PlayerPrefs.SetInt("AnimalState", animalStateNumber);

    }
}
