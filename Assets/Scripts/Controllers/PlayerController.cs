using Assets.Scripts;
using Assets.Scripts.Managers;
using Assets.Scripts.States.PlayerStates;
using DG.Tweening;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;



public class PlayerController : MonoBehaviour
{
    StateMachine _stateMachine;


    [Header("References")]
    public Animator animator; // Animator bileþeni
    public CharacterController characterController; // CharacterController bileþeni
    public Camera cam; // Kamera transform referansý
    public Camera UICam; // UI Kamera transform referansý
    public CinemachineCamera cinemachineCamera;

    public GameObject CoopPanel;
    public GameObject TradePanel;
    public GameObject AhýrPanel;

    public GameObject camPivot; // Kamera pivot referansý

    public int money=1000;


    public int wheatCount = 0;
    public int strawCount = 0;




    [Header("UI")]
    public TextMeshProUGUI interactionText; // Etkileþim metni
    public TextMeshProUGUI moneyText; // Etkileþim metni

    public TMP_Text wheatText;
    public TMP_Text strawText;

    public GameObject model; // Model referansý
    public GameObject body; // Vücut referansý
    public GameObject vehicle; // Araç referansý

    public float horizontal;
    public float vertical;
    public float runSpeed = 11f;
    public float walkSpeed = 6f;



    public float raycastDistance = 25f;  // Raycast mesafesi
    public LayerMask layerMask;         // Raycast'ýn geçebileceði katmanlar



    public float dump;

    public float rotSpeed = 5f;

    public Vector3 moveDirection;

    public bool isInVehicle = false;

    public bool isVehiclePlow=false;

    public bool canBuy=false;


    public AudioSource audioSource;




    private void Awake()
    {
        _stateMachine = new StateMachine();

    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        money = PlayerPrefs.GetInt("Money");
        wheatCount = PlayerPrefs.GetInt("PlayerWheatCount");
        strawCount = PlayerPrefs.GetInt("PlayerStrawCount");


        DOTween.SetTweensCapacity(3125, 500);

        Cursor.visible = false;

        if (cam == null)
        {
            cam = Camera.main;
        }

        IState idleState = new IdleState(this);
        IState runState = new RunState(this);
        IState walkState = new WalkState(this);
        IState vehicleState = new VehicleState(this);
        IState vehiclePlowState = new VehiclePlowState(this);
        IState getOutVehicleState = new GetOutVehicleState(this);

        _stateMachine.SetNormalState(idleState, runState, () => Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Vertical") > 0);
        _stateMachine.SetNormalState(runState, idleState, () => Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0);
        _stateMachine.SetNormalState(runState, walkState, () => Input.GetKeyDown(KeyCode.LeftShift));
        _stateMachine.SetNormalState(walkState, idleState, () => Input.GetKeyUp(KeyCode.LeftShift));

        _stateMachine.SetAnyState(vehicleState, () => isInVehicle && !isVehiclePlow);
        _stateMachine.SetNormalState(vehicleState, getOutVehicleState, () => !isInVehicle);
        _stateMachine.SetNormalState(getOutVehicleState, idleState, () => true);

        _stateMachine.SetNormalState(vehicleState, vehiclePlowState, () => isVehiclePlow);
        _stateMachine.SetNormalState(vehiclePlowState, vehicleState, () => !isVehiclePlow);

        _stateMachine.SetState(idleState);

        moneyText.text=money.ToString();

        strawText.text = strawCount.ToString();
        wheatText.text = wheatCount.ToString();
    }

    private void Update()
    {

        _stateMachine.Tick();

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        moveDirection = new Vector3(horizontal, 0, vertical).normalized;

        #region Raycast

        Vector3 rayOrigin = cam.transform.position;

        Vector3 rayDirection = cam.transform.forward;

        Debug.DrawRay(rayOrigin, rayDirection * raycastDistance, Color.red);

        RaycastHit hit;

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, raycastDistance, layerMask))
        {
            if (hit.collider.CompareTag("Arac") && !isInVehicle)
            {
                interactionText.text = "Press F to enter vehicle";

                if (Input.GetKeyDown(KeyCode.F))
                {
                    vehicle = hit.collider.gameObject;
                    isInVehicle = true;
                }
            }
            else if (hit.collider.CompareTag("PartDirt") && !isInVehicle)
            {
                if (hit.transform.gameObject.GetComponent<FieldController>().isProcessed)
                {
                    interactionText.text = "press e for plant";

                    Transform field = hit.collider.transform;
                    if (Input.GetKeyDown(KeyCode.E) && wheatCount>5)
                    {
                        field.gameObject.GetComponent<FieldController>().isGrowing = true;
                        WheatCounter(-5);
                    }
                }
               

            }

            else if (hit.collider.CompareTag("Coop"))
            {
                interactionText.text = "Press e to enter the coop";
                if (Input.GetKeyDown(KeyCode.E))
                {
                    cam.gameObject.SetActive(false);
                    UICam.gameObject.SetActive(true);
                    CoopPanel.SetActive(true);

                    Cursor.visible = true;
                }
            }

            else if (hit.collider.CompareTag("Market"))
            {
                interactionText.text = "Press E to shop";
                if (Input.GetKeyDown(KeyCode.E))
                {
                    cam.gameObject.SetActive(false);
                    UICam.gameObject.SetActive(true);
                    TradePanel.SetActive(true);
                    Cursor.visible = true;
                }
            }

            else if (hit.collider.CompareTag("Ahýr"))
            {
                interactionText.text = "press e to enter barn";
                if (Input.GetKeyDown(KeyCode.E))
                {
                    cam.gameObject.SetActive(false);
                    UICam.gameObject.SetActive(true);
                    AhýrPanel.SetActive(true);
                    Cursor.visible = true;
                }
            }

            else if (hit.collider.CompareTag("Home"))
            {
                interactionText.text = "press e to save";
                if (Input.GetKeyDown(KeyCode.E))
                {
                    hit.transform.gameObject.GetComponent<SaveManager>().SaveScene();
                }
            }

            else
            {
                interactionText.text = " ";
            }

        }



        #endregion

    }

    

    public void InputMove(float speed)
    {
        float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotSpeed, 0.1f);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        characterController.Move(moveDir.normalized * speed * Time.deltaTime);

        animator.SetFloat("Speed_f", speed / 15); // Koþma animasyonu
    }



    #region Trade
    public void SellStraw(int adet)
    {

        if (strawCount - 10*adet >= 0 )
        {
            MoneyCounter(30*adet);
            StrawCounter(-adet);
            strawText.text = strawCount.ToString();
            moneyText.text = money.ToString();

           
        }
    }

    public void SellWheat(int adet)
    {
        if (wheatCount - 10*adet >= 0)
        {
            MoneyCounter(30*adet);
            WheatCounter(-10*adet);
            wheatText.text = wheatCount.ToString();
            moneyText.text = money.ToString();
        }
    }

    

    public void BuyAnimal(int cost)
    {
        if (money >= cost)
        {
            canBuy = true;
            cost = -1 * cost;
            Debug.Log(cost);
            MoneyCounter(cost);
        }
        else canBuy = false;
    }

    public void SellAnimal(int price)
    {
        MoneyCounter(price);
    }

    public void BuyStraw()
    {
        if (money >= 50)
        {
            MoneyCounter(-50);
            StrawCounter(10);
            strawText.text = strawCount.ToString();
            moneyText.text = money.ToString();
        }
    }
    public void BuyWheat()
    {
        if (money >= 50)
        {
            MoneyCounter(-50);
            WheatCounter(20);
            wheatText.text = wheatCount.ToString();
            moneyText.text = money.ToString();
        }
    }

    public void StrawCounter(int straw)
    {
        strawCount += straw;
        strawText.text = strawCount.ToString();
        PlayerPrefs.SetInt("PlayerStrawCount",strawCount);
    }

    public void WheatCounter(int wheat)
    {
        wheatCount += wheat;
        wheatText.text = wheatCount.ToString();
        PlayerPrefs.SetInt("PlayerWheatCount", wheatCount);

    }

    public void MoneyCounter(int cost)
    {
        money += cost;
        moneyText.text = money.ToString();
        PlayerPrefs.SetInt("Money", money);

    }

    #endregion



}
