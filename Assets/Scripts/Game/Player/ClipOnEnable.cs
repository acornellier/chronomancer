using Animancer;
using UnityEngine;

[RequireComponent(typeof(AnimancerComponent))]
public class ClipOnEnable : MonoBehaviour
{
    [SerializeField] ClipTransition _clip;
    
    AnimancerComponent _animancer;

    void Awake()
    {
        _animancer = GetComponent<AnimancerComponent>();
        _clip.Events.OnEnd += () => gameObject.SetActive(false);
    }

    void OnEnable()
    {
        _animancer.Play(_clip);
    }
}
