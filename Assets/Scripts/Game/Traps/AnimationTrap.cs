using Animancer;
using UnityEngine;

public class AnimationTrap : MonoBehaviour
{
    SoloAnimation _animation;
    SpeedTracker _speedTracker;

    void Awake()
    {
        _animation = GetComponent<SoloAnimation>();
        _speedTracker = GetComponent<SpeedTracker>();
    }

    void Update()
    {
        _animation.Speed = _speedTracker.Multiplier;
    }
}