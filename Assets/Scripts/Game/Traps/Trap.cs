using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpeedTracker))]
[RequireComponent(typeof(SpriteRenderer))]
public class Trap : MonoBehaviour
{
    protected SpeedTracker SpeedTracker;

    Collider2D _collider;
    SpriteRenderer _renderer;

    protected virtual void Awake()
    {
        SpeedTracker = GetComponent<SpeedTracker>();
        _collider = GetComponent<Collider2D>();
        _renderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void Update()
    {
        _renderer.color = SpeedTracker.Multiplier switch
        {
            < 1 => Color.blue,
            > 1 => Color.red,
            _ => Color.white,
        };
    }
}