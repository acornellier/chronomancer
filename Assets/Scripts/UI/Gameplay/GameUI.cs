using System.Collections;
using TMPro;
using UnityEngine;
using Zenject;

public class GameUI : MonoBehaviour
{
    [SerializeField] TMP_Text highscoreText;
    [SerializeField] TMP_Text heightText;
    [SerializeField] float initialFadeInDelay = 1f;
    [SerializeField] float fadeRate = 2f;

    [Inject] HeightTracker _heightTracker;

    void Awake()
    {
        StartCoroutine(FadeIn());
    }

    void LateUpdate()
    {
        heightText.text = $"{_heightTracker.currentHeight}ft";
        highscoreText.text = $"{Mathf.Round(_heightTracker.maxHeight)}ft";
    }

    IEnumerator FadeIn()
    {
        var alpha = 0f;
        highscoreText.alpha = alpha;
        heightText.alpha = alpha;

        yield return new WaitForSeconds(initialFadeInDelay);

        while (alpha < 1)
        {
            alpha += fadeRate * Time.deltaTime;
            highscoreText.alpha = alpha;
            heightText.alpha = alpha;
            yield return null;
        }
    }
}
