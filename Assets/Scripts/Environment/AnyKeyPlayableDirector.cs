using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
public class AnyKeyPlayableDirector : MonoBehaviour
{
    PlayableDirector _playableDirector;

    void Awake()
    {
        _playableDirector = GetComponent<PlayableDirector>();

        InputSystem.onAnyButtonPress.CallOnce((_) => _playableDirector.Play());
    }
}