using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CanvasGroup))]
public class FadeOnInput : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;
    [SerializeField] InputActionReference actionRef;
    [SerializeField] float fadeRate = 1f;
    [SerializeField] float fadeInDelay = 2f;

    CanvasGroup _canvasGroup;

    Coroutine _fadeInCoroutine;

    void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    void Start()
    {
        var action = playerInput.actions.FindAction(actionRef.action.id.ToString());
        action.performed += (_) => StartCoroutine(FadeOut());
        _fadeInCoroutine = StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        _canvasGroup.alpha = 0f;
        yield return new WaitForSeconds(fadeInDelay);

        while (_canvasGroup.alpha < 1f)
        {
            _canvasGroup.alpha += fadeRate * Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator FadeOut()
    {
        StopCoroutine(_fadeInCoroutine);

        while (_canvasGroup.alpha > 0f)
        {
            _canvasGroup.alpha -= fadeRate * Time.deltaTime;
            yield return null;
        }
    }
}