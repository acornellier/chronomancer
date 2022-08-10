using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpeedTracker))]
public class Arrow : MonoBehaviour
{
    [SerializeField] float baseSpeed = 15f;

    Vector2 _direction;

    public Vector2 Direction
    {
        set
        {
            _direction = value;
            UpdatePhysics();
        }
    }

    public SpeedTracker SpeedTracker { get; private set; }

    Rigidbody2D _body;
    LayerMask _contactMask;

    void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _contactMask = LayerMask.GetMask("Ground");
        SpeedTracker = GetComponent<SpeedTracker>();
    }

    void OnEnable()
    {
        SpeedTracker.OnChange += UpdatePhysics;
    }

    void OnDisable()
    {
        SpeedTracker.OnChange -= UpdatePhysics;
    }

    void UpdatePhysics()
    {
        _body.velocity = baseSpeed * SpeedTracker.Multiplier * transform.up;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        var player = col.GetComponent<Player>();
        if (player)
            player.Die();

        if (player || _contactMask.Contains(col.gameObject.layer))
            Destroy(gameObject);

        // some stuff i was trying to get arrows to flop down after hitting something
        // GetComponent<Collider2D>().isTrigger = false;
        // _body.bodyType = RigidbodyType2D.Dynamic;
    }
}