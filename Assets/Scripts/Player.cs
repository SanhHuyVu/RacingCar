using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CarController))]
public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    private const string HORIZONTAl = "Horizontal";
    private const string VERTICAL = "Vertical";

    private float horizontalInput;
    private float verticalInput;

    public Placement PlayerPlacement { get; private set; }

    public int CurrentPlace { get; private set; } = 1;

    private CarController carController;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        PlayerPlacement = GetComponent<Placement>();
        carController = GetComponent<CarController>();
    }

    private void LateUpdate()
    {
        GetInput();
        carController.HandleMotor(verticalInput);
        carController.ApplyBreaking(Input.GetKey(KeyCode.Space));
        carController.HandleSteering(horizontalInput);
        carController.UpdateWheels();
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis(HORIZONTAl);
        verticalInput = Input.GetAxis(VERTICAL);
    }
}
