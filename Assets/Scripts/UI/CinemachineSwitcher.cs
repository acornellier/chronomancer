using Cinemachine;
using UnityEngine;

public class CinemachineSwitcher : MonoBehaviour
{
    [SerializeField] ColliderTrigger trigger;
    [SerializeField] CinemachineVirtualCamera cameraToSwitchTo;

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
        var allCameras = FindObjectsOfType<CinemachineVirtualCamera>();
        foreach (var cam in allCameras)
        {
            cam.Priority = 0;
        }

        cameraToSwitchTo.Priority = 1;
    }
}