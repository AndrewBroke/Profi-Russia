using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    public bool isStarted;

    [SerializeField] private float motorForce;
    [SerializeField] private float speed;
    [SerializeField] private float fanForce;

    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider behindLeftWheelCollider, behindRightWheelCollider;

    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] private Transform behindLeftWheelTransform, behindRightWheelTransform;

    private Rigidbody rbCar;

    private void Start()
    {
        rbCar = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(isStarted == true)
        {
            HandleMotor();
        }
    }
    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = speed * motorForce;
        frontRightWheelCollider.motorTorque = speed * motorForce;
        behindLeftWheelCollider.motorTorque = speed * motorForce;
        behindRightWheelCollider.motorTorque = speed * motorForce;

    }
}
