using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using Unity.Cinemachine;

public class CarController : MonoBehaviour
{

    PlayerController _playerController;

    public bool isTractor;


    private float horizontalInput, verticalInput;
    private float currentSteerAngle, currentbreakForce;
    private bool isBreaking;


    public TextMeshProUGUI interactionText;

    public Transform attachPoint;
    public Transform playerSp;

    public float raycastDistance = 3f;  // Raycast mesafesi
    public LayerMask layerMask;         // Raycast'ın geçebileceği katmanlar

    // Settings
    [SerializeField] private float motorForce, breakForce, maxSteerAngle;

    // Wheel Colliders
    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    // Wheels
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;

    public GameObject trailer;

    [SerializeField] private Transform attacPointVehicle;

    public GameObject cutter;

    AudioSource AudioSource;


    private void Awake()
    {
        AudioSource = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        AudioSource.Play();
    }



    private void Start()
    {
        _playerController = FindAnyObjectByType<PlayerController>();


    }

    private void Update()
    {
        Vector3 rayDirection = attachPoint.forward;
        Vector3 rayStart = new Vector3(attachPoint.position.x, attachPoint.position.y, attachPoint.position.z);

        Debug.DrawRay(rayStart, rayDirection * raycastDistance, Color.red);

        RaycastHit hit;

        if (Physics.Raycast(rayStart, rayDirection, out hit, raycastDistance, layerMask))
        {
            if (hit.collider.CompareTag("Plow")&&isTractor)
            {
                trailer = hit.collider.gameObject;


                if (!_playerController.isVehiclePlow)
                {
                    interactionText.text = "attach";

                }
                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactionText.text = "attached";
                    AttachTrailer(trailer);

                }
            }
            else
            {

                interactionText.text = " ";

            }


        }

        
    }

    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();


        
    }
    #region Input
    private void GetInput()
    {
        // Steering Input
        horizontalInput = Input.GetAxis("Horizontal");

        // Acceleration Input
        verticalInput = Input.GetAxis("Vertical");

        // Breaking Input
        isBreaking = Input.GetKey(KeyCode.Space);
    }
    #endregion
    #region Motor
    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        currentbreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();
    }

    public void ApplyBreaking()
    {
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }
    #endregion

    public void AttachTrailer(GameObject _trailer)
    {
        _playerController.isVehiclePlow = true;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(_trailer.transform.DOMove(new Vector3(attachPoint.position.x, attachPoint.position.y-0.7f, attachPoint.position.z), 1f));
        sequence.Join(_trailer.transform.DORotate(attachPoint.rotation.eulerAngles, 1f));
        _trailer.transform.parent = attachPoint;

    }
    private void OnDisable()
    {
        AudioSource.Pause();
    }
}

