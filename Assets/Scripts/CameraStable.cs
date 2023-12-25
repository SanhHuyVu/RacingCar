using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStable : MonoBehaviour
{
    [SerializeField] private GameObject carGO;
     private float carX;
     private float carY;
     private float carZ;
    private void Update()
    {
        carX = carGO.transform.eulerAngles.x;
        carY = carGO.transform.eulerAngles.y;
        carZ = carGO.transform.eulerAngles.z;

        transform.eulerAngles = new Vector3(carX - carX, carY, carZ - carZ);
    }
}
