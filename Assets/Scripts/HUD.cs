using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public static HUD Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI bestTimeText;
    [SerializeField] private TextMeshProUGUI lapCountText;
    [SerializeField] private TextMeshProUGUI placeText;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void UpdateLapCount(string lapCount)
    {
        lapCountText.text = lapCount;
    }
    public void UpdateTime(string time)
    {
        timeText.text = time;
    }
    public void UpdateBestTime(string bestTime)
    {
        bestTimeText.text = bestTime;
    }
    public void UpdatePlacementText(int placement)
    {
        string placementText = "";
        switch (placement)
        {
            case 1:
                placementText = $"{placement}st Place";
                break;
            case 2:
                placementText = $"{placement}nd Place";
                break;
            case 3:
                placementText = $"{placement}rd Place";
                break;
            case 4:
                placementText = $"{placement}th Place";
                break;
            default:
                placementText = $"ERROR Place";
                break;
        }
        placeText.text = placementText;
    }
}
