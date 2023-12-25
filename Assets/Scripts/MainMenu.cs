using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button playBtn;
    [SerializeField] private Button quitBtn;
    [SerializeField] private Button goBtn;
    [SerializeField] private Button backBtn;
    [SerializeField] private Button nextCarBtn;
    [SerializeField] private Button previousCarBtn;

    [SerializeField] private CanvasGroup mainMenu;
    [SerializeField] private CanvasGroup carSelect;

    [SerializeField] private Transform[] cameraLookList;
    [SerializeField] private GameObject[] carVisuals;

    private int currentSelectedVisual = 0;

    private void Awake()
    {
        mainMenu.transform.DOScale(1, 0);
        carSelect.transform.DOScale(0, 0);
        AddEventForButtons();

        for (int i = 0; i < carVisuals.Length; i++)
        {
            if (i != currentSelectedVisual) carVisuals[i].SetActive(false);
        }

    }

    private void AddEventForButtons()
    {
        playBtn.onClick.AddListener(() =>
        {
            ToggleButton(false);
            Camera.main.transform.DOLookAt(cameraLookList[1].position, 1f);
            mainMenu.transform.DOScale(0, 0.5f).OnComplete(() =>
            {
                carSelect.transform.DOScale(1, 0.5f);
                ToggleButton(true);
            });

        });
        quitBtn.onClick.AddListener(() =>
        {
            ToggleButton(false);
            Application.Quit();
        });
        goBtn.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.GameScene);
        });
        backBtn.onClick.AddListener(() =>
        {
            ToggleButton(false);
            Camera.main.transform.DOLookAt(cameraLookList[0].position, 1f);
            carSelect.transform.DOScale(0, 0.5f).OnComplete(() =>
            {
                mainMenu.transform.DOScale(1, 0.5f);
                ToggleButton(true);
            });
        });
        nextCarBtn.onClick.AddListener(() => { NextCar(); });
        previousCarBtn.onClick.AddListener(() => { PreviousCar(); });
    }

    private void NextCar()
    {
        carVisuals[currentSelectedVisual].SetActive(false);
        currentSelectedVisual = (currentSelectedVisual + 1) % carVisuals.Length;
        carVisuals[currentSelectedVisual].SetActive(true);
        PlayerCarSpawner.Instance.SetPlayerCatIndex(currentSelectedVisual);
    }

    private void PreviousCar()
    {
        carVisuals[currentSelectedVisual].SetActive(false);
        currentSelectedVisual--;
        if (currentSelectedVisual < 0) currentSelectedVisual += carVisuals.Length;
        carVisuals[currentSelectedVisual].SetActive(true);
        PlayerCarSpawner.Instance.SetPlayerCatIndex(currentSelectedVisual);
    }

    private void ToggleButton(bool on)
    {
        playBtn.interactable = on;
        quitBtn.interactable = on;
    }
}
