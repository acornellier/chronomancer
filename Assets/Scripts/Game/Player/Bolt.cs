using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Bolt : MonoBehaviour
{
    [SerializeField] float speedMultiplier = 1;
    [SerializeField] float baseSpeed = 1f;

    public Vector3 Direction { get; set; }

    LayerMask _contactMask;

    void Awake()
    {
        _contactMask = LayerMask.GetMask("Ground", "Trap");
    }

    void FixedUpdate()
    {
        transform.Translate(baseSpeed * Time.fixedDeltaTime * Direction);
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