using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placement : MonoBehaviour
{
    [SerializeField] private int numberOfCheckPointPassed = 0;
    [SerializeField] private bool isPlayer = false;

    public bool IsPlayer => isPlayer;
    public int NumberOfCheckPointPassed => numberOfCheckPointPassed;

    public void PassACheckPoint()
    {
        numberOfCheckPointPassed++;
        TrackCheckPoints.Instance.UpdatePlayerPlacement();
    }
}
