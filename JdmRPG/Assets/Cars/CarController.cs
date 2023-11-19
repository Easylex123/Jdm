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
    public float turnSensitivity = 1.0f;
    public float maxSteerAngle = 30.0f;
    public Vector3 centerOfMass;

    public List<Wheel> wheels;
    
    private float _moveInput;
    private float _steerInput;
    private Rigidbody _carBody;
    
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
    
    void Start()
    {
        _carBody = GetComponent<Rigidbody>();
        _carBody.centerOfMass = centerOfMass;
    }

    void Update()
    {
        GetInputs();
        AnimatedWheels();
    }

    void Move()
    {
        var currentSpeed = _carBody.velocity.sqrMagnitude;
        Debug.Log(currentSpeed + " | " +  maxSpeed);

        if (currentSpeed < maxSpeed)
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.motorTorque = _moveInput * 600 * maxAcceleration;
            }
        }
    }

    void GetInputs()
    {
        _moveInput = Input.GetAxis("Vertical");
        _steerInput = Input.GetAxis("Horizontal");
    }

    private void LateUpdate()
    {
        Steer();
    }

    private void FixedUpdate()
    {
        Move();
        Breake();
    }

    private void Steer()
    {
        foreach (var wheel in wheels)
        {
            if (wheel.axel == Axel.Front)
            {
                var steerAngle = _steerInput * turnSensitivity * maxSteerAngle;
                wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, steerAngle, 0.6f);
            }
        }
    }

    private void Breake()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = 300 * breakAcceleration;
            }
        }
        else
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = 0;
            }
        }
    }

    private void AnimatedWheels()
    {
        foreach (var wheel in wheels)
        {
            wheel.wheelCollider.GetWorldPose(out var position, out var rotation);
            wheel.wheelModel.transform.position = position;
            wheel.wheelModel.transform.rotation = rotation;
        }
    }
}
