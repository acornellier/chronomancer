using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : Menu
{
    [SerializeField] Image alphaCover;
    [SerializeField] float fadeRate = 1f;

    AsyncOperation _asyncLoad;

    void Awake()
    {
        menuManager.OpenMenu(gameObject);
    }

    void Start()
    {
        _asyncLoad = SceneManager.LoadSceneAsync("Game", LoadSceneMode.Additive);
        _asyncLoad.allowSceneActivation = false;
    }

    public void PlayGame()
    {
        StartCoroutine(PlayGameWhenLoaded());
    }

    IEnumerator PlayGameWhenLoaded()
    {
        alphaCover.gameObject.SetActive(true);
        var color = alphaCover.color;
        while (alphaCover.color.a < 1)
        {
            color.a += fadeRate * Time.deltaTime;
            alphaCover.color = color;
            yield return null;
        }

        _asyncLoad.allowSceneActivation = true;
        yield return _asyncLoad;

        var asyncUnload = SceneManager.UnloadSceneAsync("StartMenu");
        yield return asyncUnload;
    }

    public void Quit()
    {
        GameUtilities.Quit();
    }
}
