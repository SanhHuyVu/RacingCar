using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private float velocityLose = 1.005f;
    [SerializeField] private float velocityDamper = 1.015f;

    [SerializeField] private float motorForce = 6000f;
    [SerializeField] private float breakForce = 3000f;
    [SerializeField] private float maxSteerAngle = 45f;

    [Header("Wheel transforms")]
    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheelTransform;
    [SerializeField] private Transform backLeftWheelTransform;
    [SerializeField] private Transform backRightWheelTransform;

    [Header("Wheel colliders")]
    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider backLeftWheelCollider;
    [SerializeField] private WheelCollider backRightWheelCollider;

    private float currentSteerAngle;
    private float currentbreakForce;
    private Rigidbody carRigidbody;
    private Transform carTransform;

    void Start()
    {
        carRigidbody = GetComponent<Rigidbody>();
        carTransform = transform;
        carRigidbody.centerOfMass += new Vector3(0, -0.5f, 0);
    }

    public void HandleMotor(float verticalInput)
    {
        verticalInput = Mathf.Clamp(verticalInput, -1, 1);

        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;

        if (verticalInput == 0) carRigidbody.velocity = carRigidbody.velocity / velocityLose;

        if (verticalInput * Vector3.Dot(carTransform.forward, carRigidbody.velocity) < 0)
            carRigidbody.velocity = carRigidbody.velocity / velocityDamper;
    }
    public void ApplyBreaking(bool isBreaking)
    {
        currentbreakForce = isBreaking ? breakForce : 0f;

        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        backLeftWheelCollider.brakeTorque = currentbreakForce;
        backRightWheelCollider.brakeTorque = currentbreakForce;
    }
    public void HandleSteering(float horizontalInput)
    {
        horizontalInput = Mathf.Clamp(horizontalInput, -1, 1);
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }
    public void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(backRightWheelCollider, backRightWheelTransform);
        UpdateSingleWheel(backLeftWheelCollider, backLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        wheelCollider.GetWorldPose(out Vector3 pos, out Quaternion rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }
}
