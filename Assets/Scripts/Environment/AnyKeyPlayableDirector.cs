using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Playables;

public class AnyKeyPlayableDirector : MonoBehaviour
{
    [SerializeField] PlayableDirector prevPlayableDirector;
    [SerializeField] PlayableDirector nextPlayableDirector;

    PlayableDirector _playableDirector;

    public void EnableTrigger()
    {
        print("EnableTrigger");
        InputSystem.onAnyButtonPress.CallOnce(
            (_) =>
            {
                if (prevPlayableDirector)
                    prevPlayableDirector.Stop();

                if (nextPlayableDirector)
                    nextPlayableDirector.Play();
            }
        );
    }
}