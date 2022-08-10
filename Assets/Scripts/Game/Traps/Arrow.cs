using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpeedTracker))]
public class Arrow : MonoBehaviour
{
    [SerializeField] float baseSpeed = 1f;

    Vector2 _direction;

    public Vector2 Direction
    {
        get => _direction;
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
        transform.rotation = Quaternion.LookRotation(Vector3.forward, Direction);
        _body.velocity = baseSpeed * SpeedTracker.Multiplier * Direction;
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