using UnityEngine;

public class SpeedTracker : MonoBehaviour
{
    float _multiplier = 1f;

    public float Multiplier
    {
        get => _multiplier;
        set => _multiplier = Mathf.Clamp(value, 0.5f, 2f);
    }
}