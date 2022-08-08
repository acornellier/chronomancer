using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CanvasGroup))]
public class LevelLoader : MonoBehaviour
{
    [SerializeField] float fadeRate = 1;

    CanvasGroup _canvasGroup;
    // AsyncOperation _asyncLoad;

    public void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();

        // _asyncLoad = SceneManager.LoadSceneAsync(_settings.nextLevel.name);
        // _asyncLoad.allowSceneActivation = false;
    }

    void Start()
    {
        // #if !UNITY_EDITOR
        StartCoroutine(StartLevel());
        // #endif
    }

    public void LoadScene(string scene)
    {
        StartCoroutine(FadeToScene(scene));
    }

    IEnumerator StartLevel()
    {
        _canvasGroup.alpha = 1;
        while (_canvasGroup.alpha > 0)
        {
            _canvasGroup.alpha -= fadeRate * Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator FadeToScene(string scene)
    {
        _canvasGroup.alpha = 0;
        while (_canvasGroup.alpha < 1)
        {
            _canvasGroup.alpha += fadeRate * Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadScene(scene);
    }
}