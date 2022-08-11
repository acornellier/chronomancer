using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
    [SerializeField] ColliderTrigger trigger;
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip clip;

    bool _triggered;

    void OnEnable()
    {
        trigger.OnTriggered += OnTriggered;
    }

    void OnDisable()
    {
        trigger.OnTriggered -= OnTriggered;
    }

    void OnTriggered()
    {
        source.PlayOneShot(clip);
    }
}