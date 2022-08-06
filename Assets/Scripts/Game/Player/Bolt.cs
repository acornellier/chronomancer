using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Bolt : MonoBehaviour
{
    [SerializeField] float speedMultiplier = 1;
    [SerializeField] float baseSpeed = 1f;

    Vector3 _direction;

    LayerMask _contactMask;

    void Awake()
    {
        _contactMask = LayerMask.GetMask("Ground", "Trap");
    }

    void FixedUpdate()
    {
        transform.position += baseSpeed * Time.fixedDeltaTime * _direction;
    }

    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
        transform.right = _direction;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        var speedTracker = col.GetComponent<SpeedTracker>();
        if (speedTracker)
            speedTracker.Multiplier *= speedMultiplier;

        if (col.GetComponent<Player>())
            return;

        if (_contactMask == (_contactMask | (1 << col.gameObject.layer)))
            Destroy(gameObject);
    }
}