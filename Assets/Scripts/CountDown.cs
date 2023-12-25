using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class CountDown : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countDownText;
    [SerializeField] private float countDownTimer = 0.3f;
    private void Start()
    {
        gameObject.SetActive(false);

        DoCountDown();
    }

    public void DoCountDown()
    {
        gameObject.SetActive(true);
        countDownText.gameObject.SetActive(false);
        Time.timeScale = 0;

        Sequence sequence = DOTween.Sequence().SetUpdate(true);
        countDownText.text = "1";
        sequence.Append(countDownText.transform.DOScale(1.5f, 0).OnComplete(() => { countDownText.gameObject.SetActive(true); }));
        sequence.Append(countDownText.transform.DOScale(1, countDownTimer));
        sequence.Append(countDownText.transform.DOScale(1.5f, 0).OnComplete(() => { countDownText.text = "2"; }));
        sequence.Append(countDownText.transform.DOScale(1, countDownTimer));
        sequence.Append(countDownText.transform.DOScale(1.5f, 0).OnComplete(() => { countDownText.text = "3"; }));
        sequence.Append(countDownText.transform.DOScale(1, countDownTimer).OnComplete(() =>
        {
            gameObject.SetActive(false);
            Time.timeScale = 1;
        }));
    }
}
