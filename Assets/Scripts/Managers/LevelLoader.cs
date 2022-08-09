using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] float fadeRate = 1;

    PlayerInputActions _playerControls;

    void Awake()
    {
        _playerControls = new PlayerInputActions();
    }

    void Start()
    {
        // #if !UNITY_EDITOR
        StartCoroutine(StartLevel());
        // #endif
    }

    void OnEnable()
    {
        _playerControls.Player.RestartLevel.Enable();
        _playerControls.Player.RestartLevel.performed += (_) => ReloadScene();
    }

    void OnDisable()
    {
        _playerControls.Player.RestartLevel.Disable();
    }

    public void ReloadScene()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadScene(string scene)
    {
        StartCoroutine(FadeToScene(scene));
    }

    IEnumerator StartLevel()
    {
        var lights = FindObjectsOfType<Light2D>();
        var startingIntensities = lights.ToDictionary(l => l, l => l.intensity);

        foreach (var l in lights)
        {
            l.intensity = 0;
        }

        var t = 0f;
        while (t < 1)
        {
            t += fadeRate * Time.deltaTime;
            foreach (var l in lights)
            {
                l.intensity = Mathf.Lerp(0f, startingIntensities[l], t);
            }

            yield return null;
        }
    }

    IEnumerator FadeToScene(string scene)
    {
        var lights = FindObjectsOfType<Light2D>();
        var startingIntensities = lights.ToDictionary(l => l, l => l.intensity);

        foreach (var l in lights)
        {
            var flicker = l.GetComponent<LightFlicker>();
            if (flicker)
                flicker.enabled = false;
        }

        var t = 0f;
        while (t < 1)
        {
            t += fadeRate * Time.deltaTime;
            foreach (var l in lights)
            {
                l.intensity = Mathf.Lerp(startingIntensities[l], 0f, t);
            }

            yield return null;
        }

        SceneManager.LoadScene(scene);
    }
}