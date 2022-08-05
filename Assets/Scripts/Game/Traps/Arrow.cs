using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(SpeedTracker))]
public class Arrow : MonoBehaviour
{
    [SerializeField] float baseSpeed = 1f;

    public Vector3 Direction { get; set; }
    public SpeedTracker SpeedTracker { get; private set; }

    LayerMask _contactMask;
    SpriteRenderer _renderer;

    void Awake()
    {
        _contactMask = LayerMask.GetMask("Ground");
        _renderer = GetComponent<SpriteRenderer>();
        SpeedTracker = GetComponent<SpeedTracker>();
    }

    void Update()
    {
        _renderer.color = SpeedTracker.Multiplier switch
        {
            < 1 => Color.blue,
            > 1 => Color.red,
            _ => Color.green,
        };
    }

    void FixedUpdate()
    {
        transform.Translate(baseSpeed * SpeedTracker.Multiplier * Time.fixedDeltaTime * Direction);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        var player = col.GetComponent<Player>();
        if (player)
            player.Die();

        if (_contactMask.Contains(col.gameObject.layer))
            Destroy(gameObject);
    }
}