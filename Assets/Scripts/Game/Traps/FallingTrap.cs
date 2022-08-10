using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FallingTrap : MonoBehaviour
{
    [SerializeField] SpeedTracker speedTracker;
    [SerializeField] Trigger trigger;
    [SerializeField] float acceleration = 10f;

    Rigidbody2D _body;

    readonly RaycastHit2D[] _hits = new RaycastHit2D[16];

    bool _triggered;

    void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (trigger.Triggered)
            _body.velocity += acceleration * speedTracker.Multiplier * Time.fixedDeltaTime *
                              Vector2.down;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        var player = col.gameObject.GetComponent<Player>();
        if (player)
            player.Die();
    }
}