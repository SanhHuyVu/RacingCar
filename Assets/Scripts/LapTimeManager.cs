using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LapTimeManager : MonoBehaviour
{
    public static LapTimeManager Instance { get; private set; }


    private int minuteCount;
    private int secondCount;
    private float millicount;

    private string minuteStr;
    private string secondStr;
    private string milliStr;

    private string lapTimeDisplay;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        millicount += Time.deltaTime * 10;
        lapTimeDisplay = millicount.ToString("F0");
        milliStr = lapTimeDisplay;

        if (millicount >= 10)
        {
            millicount = 0;
            secondCount++;
        }

        if (secondCount <= 9)
            secondStr = $"0{secondCount}.";
        else
            secondStr = $"{secondCount}.";

        if (secondCount >= 60)
        {
            secondCount = 0;
            minuteCount++;
        }

        if (minuteCount <= 9)
            minuteStr = $"0{minuteCount}:";
        else
            minuteStr = $"{minuteCount}:";

        lapTimeDisplay = $"{minuteStr}{secondStr}{milliStr}";
        HUD.Instance?.UpdateTime(lapTimeDisplay);
    }

    public void SetBestTime()
    {
        HUD.Instance?.UpdateBestTime(lapTimeDisplay);
        minuteCount = 0;
        secondCount = 0;
        millicount = 0;
    }

}
