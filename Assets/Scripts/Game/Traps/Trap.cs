using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpeedTracker))]
[RequireComponent(typeof(SpriteRenderer))]
public class Trap : MonoBehaviour
{
    protected SpeedTracker SpeedTracker;

    Collider2D _collider;
    SpriteRenderer _renderer;

    readonly Color _slowColor = new(0.25f, 0.25f, 0.75f);
    readonly Color _fastColor = new(0.75f, 0.25f, 0.25f);

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
            < 1 => _slowColor,
            > 1 => _fastColor,
            _ => Color.white,
        };
    }
}