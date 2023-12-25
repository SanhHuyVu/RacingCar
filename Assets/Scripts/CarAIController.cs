using UnityEngine;

public class CarAIController : MonoBehaviour
{
    [SerializeField] private float maxTopSpeed = 18;
    [SerializeField] private float minTopSpeed = 14;
    [SerializeField] private float speedMin = -15f;
    [SerializeField] private float acceleration = 2.5f;
    [SerializeField] private float brakeSpeed = 40f;
    [SerializeField] private float reverseSpeed = 15f;
    [SerializeField] private float idleSlowdown = 10f;

    [SerializeField] private float turnSpeedMax = 300f;
    [SerializeField] private float turnSpeedAcceleration = 300f;
    [SerializeField] private float turnIdleSlowdown = 500f;

    [Header("Wheel transforms")]
    [SerializeField] private Transform frontLeftWheelTF;
    [SerializeField] private Transform frontRightWheelTF;
    [SerializeField] private Transform backLeftWheelTF;
    [SerializeField] private Transform backRightWheelTF;

    private float topSpeed = 50f;
    private float speed;
    private float turnSpeed;
    private float forwardAmount;
    private float turnAmount;
    private Rigidbody carRigidbody;

    private float wheelRoolRate;
    private float wheelTurnRate;

    private void Start()
    {
        carRigidbody = GetComponent<Rigidbody>();
        carRigidbody.centerOfMass += new Vector3(0, -0.5f, 0);
        topSpeed = Random.Range(minTopSpeed, maxTopSpeed);
    }

    private void Update()
    {
        if (forwardAmount > 0)
        {
            // Accelerating
            speed += forwardAmount * acceleration * Time.deltaTime;
            wheelRoolRate += forwardAmount * brakeSpeed * Time.deltaTime; ;
        }
        if (forwardAmount < 0)
        {
            if (speed > 0)
            {
                // Braking
                speed += forwardAmount * brakeSpeed * Time.deltaTime;
                wheelRoolRate += forwardAmount * brakeSpeed * Time.deltaTime; ;
            }
            else
            {
                // Reversing
                speed += forwardAmount * reverseSpeed * Time.deltaTime;
                wheelRoolRate += forwardAmount * brakeSpeed * Time.deltaTime; ;
            }
        }

        if (forwardAmount == 0)
        {
            // Not accelerating or braking
            if (speed > 0)
            {
                speed -= idleSlowdown * Time.deltaTime;
            }
            if (speed < 0)
            {
                speed += idleSlowdown * Time.deltaTime;
            }
        }

        speed = Mathf.Clamp(speed, speedMin, topSpeed);

        //transform.position += transform.forward * speed * Time.deltaTime;
        carRigidbody.velocity = transform.forward * speed;// * Time.deltaTime;

        if (speed < 0)
        {
            // Going backwards, invert wheels
            turnAmount = turnAmount * -1f;
        }

        if (turnAmount > 0 || turnAmount < 0)
        {
            // Turning
            if ((turnSpeed > 0 && turnAmount < 0) || (turnSpeed < 0 && turnAmount > 0))
            {
                // Changing turn direction
                float minTurnAmount = 20f;
                turnSpeed = turnAmount * minTurnAmount;
            }
            turnSpeed += turnAmount * turnSpeedAcceleration * Time.deltaTime;
        }
        else
        {
            // Not turning
            if (turnSpeed > 0)
            {
                turnSpeed -= turnIdleSlowdown * Time.deltaTime;
            }
            if (turnSpeed < 0)
            {
                turnSpeed += turnIdleSlowdown * Time.deltaTime;
            }
            if (turnSpeed > -1f && turnSpeed < +1f)
            {
                // Stop rotating
                turnSpeed = 0f;
            }
        }

        float speedNormalized = speed / topSpeed;
        float invertSpeedNormalized = Mathf.Clamp(1 - speedNormalized, .75f, 1f);

        turnSpeed = Mathf.Clamp(turnSpeed, -turnSpeedMax, turnSpeedMax);

        //transform.Rotate(0, turnSpeed * (invertSpeedNormalized * 1f) * Time.deltaTime, 0);

        carRigidbody.angularVelocity = new Vector3(0, turnSpeed * (invertSpeedNormalized * 1f) * Mathf.Deg2Rad, 0);

        if (transform.eulerAngles.x > 2 || transform.eulerAngles.x < -2 || transform.eulerAngles.z > 2 || transform.eulerAngles.z < -2)
        {
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        }

        UpdateWheels();
    }

    public void SetInputs(float forwardAmount, float turnAmount, float wheelTurnRate)
    {
        this.forwardAmount = forwardAmount;
        this.turnAmount = turnAmount;
        this.wheelTurnRate = wheelTurnRate;
    }

    public void ApplyBrake()
    {
        speed -= brakeSpeed * Time.deltaTime;
    }

    public void ClearTurnSpeed()
    {
        turnSpeed = 0f;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public void ReRandomSpeed()
    {
        topSpeed = Random.Range(minTopSpeed, maxTopSpeed);
    }

    public void StopCompletely()
    {
        speed = 0f;
        turnSpeed = 0f;
    }

    public void UpdateWheels()
    {
        if (wheelRoolRate >= 360f || wheelRoolRate <= -360f) wheelRoolRate = 0f;
        wheelTurnRate = Mathf.Clamp(wheelTurnRate, -45, 45);
        frontLeftWheelTF.localEulerAngles = new Vector3(wheelRoolRate, wheelTurnRate, frontLeftWheelTF.localEulerAngles.z);
        frontRightWheelTF.localEulerAngles = new Vector3(wheelRoolRate, wheelTurnRate, frontRightWheelTF.localEulerAngles.z);

        backLeftWheelTF.localEulerAngles = new Vector3(wheelRoolRate, backLeftWheelTF.localEulerAngles.y, backLeftWheelTF.localEulerAngles.z);
        backRightWheelTF.localEulerAngles = new Vector3(wheelRoolRate, backRightWheelTF.localEulerAngles.y, backRightWheelTF.localEulerAngles.z);
    }
}

