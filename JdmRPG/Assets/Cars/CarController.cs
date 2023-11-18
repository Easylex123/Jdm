using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    // Speed is here 300km/h
    public float maxSpeed = 300.0f;
    public float maxAcceleration = 30.0f;
    public float breakAcceleration = 50.0f;

    public List<Wheel> wheels;
    
    private float _moveInput;
    private Rigidbody _carBody;
    
    void Start()
    {
        _carBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        GetInputs();
    }

    void Move()
    {
        foreach (var wheel in wheels)
        {
            wheel.wheelCollider.motorTorque = _moveInput * maxAcceleration * Time.deltaTime;
        }
    }

    void GetInputs()
    {
        _moveInput = Input.GetAxis("Vertical");
    }

    public enum Axel
    {
        Front,
        Rear
    }
    [Serializable]
    public struct Wheel
    {
        public GameObject wheelModel;
        public WheelCollider wheelCollider;
        public Axel axel;
    }
    
    private void LateUpdate()
    {
        Move();
    }
}
