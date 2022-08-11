using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
public class TimelineTrigger : ColliderTrigger
{
    PlayableDirector _playableDirector;

    bool _triggered;

    void Awake()
    {
        _playableDirector = GetComponent<PlayableDirector>();
    }

    void OnEnable()
    {
        OnTriggered += HandleTriggered;
    }

    void OnDisable()
    {
        OnTriggered -= HandleTriggered;
    }

    void HandleTriggered()
    {
        _playableDirector.Play();
    }
}