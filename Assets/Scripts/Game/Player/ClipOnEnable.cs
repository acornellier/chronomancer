using Animancer;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(AnimancerComponent))]
public class ClipOnEnable : MonoBehaviour
{
    [SerializeField] ClipTransition clip;

    AnimancerComponent _animancer;

    void Awake()
    {
        _animancer = GetComponent<AnimancerComponent>();
        clip.Events.OnEnd += () => gameObject.SetActive(false);
    }

    void OnEnable()
    {
        _animancer.Play(clip);
    }
}