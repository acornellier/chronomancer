using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
public class AnyKeyPlayableDirector : MonoBehaviour
{
    [SerializeField] PlayableDirector nextPlayableDirector;

    PlayableDirector _playableDirector;

    void Awake()
    {
        _playableDirector = GetComponent<PlayableDirector>();
    }

    public void EnableTrigger()
    {
        InputSystem.onAnyButtonPress.CallOnce(
            (_) =>
            {
                if (_playableDirector)
                    _playableDirector.Stop();

                if (nextPlayableDirector)
                    nextPlayableDirector.Play();
            }
        );
    }
}