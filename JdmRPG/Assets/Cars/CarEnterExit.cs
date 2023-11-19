using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class CarEnterExit : MonoBehaviour
{
    public Transform player;
    public Transform car;
    public GameObject driveUi;
    
    private CarController _carController;
    private bool _canDrive;
    private bool _driving;

    [Header("Cameras")]
    public CinemachineFreeLook cinemachineFreeLookCameraCar;
    public CinemachineFreeLook cinemachineFreeLookCameraPlayer;

    void Start()
    {
        driveUi.gameObject.SetActive(false);
        _carController.enabled = false;
    }

    private void Awake()
    {
        _carController = GetComponent<CarController>();
    }

    void Update()
    {
        if (_canDrive && !_driving)
        {
            driveUi.gameObject.SetActive(true);
        }
        else
        {
            driveUi.gameObject.SetActive(false);
        }

        if (Input.GetKey(KeyCode.E) && _canDrive)
        {
            _carController.enabled = true;

            driveUi.gameObject.SetActive(false);
            _driving = true;
            
            player.transform.SetParent(car);
            player.gameObject.SetActive(false);

            if (cinemachineFreeLookCameraCar.gameObject.CompareTag("Rx7Cam") && _driving)
            {
                cinemachineFreeLookCameraCar.enabled = true;
            }
            else if (cinemachineFreeLookCameraCar.gameObject.CompareTag("CarCamera")&& _driving)
            {
                cinemachineFreeLookCameraCar.enabled = true;
            }
            
            cinemachineFreeLookCameraPlayer.enabled = false;
        }

        if (Input.GetKey(KeyCode.G))
        {
            _driving = false;
            _carController.enabled = false;
            player.transform.SetParent(null);
            player.transform.gameObject.SetActive(true);

            cinemachineFreeLookCameraCar.enabled = false;
            cinemachineFreeLookCameraPlayer.enabled = true;
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player" ))
        {
            driveUi.gameObject.SetActive(true);
            _canDrive = true;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            driveUi.gameObject.SetActive(false);
            _canDrive = false;
        }
    }
}
