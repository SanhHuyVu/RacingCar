using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGameScreen : MonoBehaviour
{
    public static EndGameScreen Instance { get; private set; }
    [SerializeField] private Button mainmenuBtn;
    [SerializeField] private Button quitBtn;
    [SerializeField] private CanvasGroup cg;
    [SerializeField] private TextMeshProUGUI placementText;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        ToggleButtons(false);

        cg.transform.DOScale(0, 0);
        mainmenuBtn.onClick.AddListener(() =>
        {
            ToggleButtons(false);
            Time.timeScale = 1;
            Loader.Load(Loader.Scene.MainMenuScene);
        });
        quitBtn.onClick.AddListener(() =>
        {
            ToggleButtons(false);
            Application.Quit();
        });
    }

    public void EndGame()
    {
        ToggleButtons(true);
        cg.transform.DOScale(1, 1.5f).OnComplete(() =>
        {
            Time.timeScale = 0;
            int playerPlacement = TrackCheckPoints.Instance.UpdatePlayerPlacement();
            switch (playerPlacement)
            {
                case 1:
                    placementText.text = $"Finished as<br> {playerPlacement}st placement";
                    break;
                case 2:
                    placementText.text = $"Finished as<br> {playerPlacement}nd placement";
                    break;
                case 3:
                    placementText.text = $"Finished as<br> {playerPlacement}rd placement";
                    break;
                case 4:
                    placementText.text = $"Finished as<br> {playerPlacement}th placement";
                    break;
                default:
                    break;
            }
        });
    }

    public void ToggleButtons(bool on)
    {
        mainmenuBtn.interactable = on;
        quitBtn.interactable = on;
    }
}
