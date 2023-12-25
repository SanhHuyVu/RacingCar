using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointSingle : MonoBehaviour
{
    [SerializeField] private bool isGoalCheckPoint = false;
    TrackCheckPoints trackCheckPoints;

    public bool IsGoalCheckPoint => isGoalCheckPoint;
    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            trackCheckPoints.GoThroughCheckPoint(this);
        }
        if (isGoalCheckPoint) LapTimeManager.Instance.SetBestTime();
    }

    public void SetTrackCheckPoints(TrackCheckPoints trackCheckPoints)
    {
        this.trackCheckPoints = trackCheckPoints;
    }
}
