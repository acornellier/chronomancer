using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpeedTracker))]
public class Elevator : MonoBehaviour
{
    [SerializeField] ColliderTrigger trigger;
    [SerializeField] Transform destination;
    [SerializeField] float maxSpeed = 0.5f;
    [SerializeField] float acceleration = 0.5f;

    SpeedTracker _speedTracker;
    Rigidbody2D _body;
    bool _triggered;
    float _speed;

    void Awake()
    {
        _speedTracker = GetComponent<SpeedTracker>();
        _body = GetComponent<Rigidbody2D>();
    }

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
        _triggered = true;
    }

    void FixedUpdate()
    {
        if (!_triggered)
            return;

        if (Vector3.Distance(transform.position, destination.position) < 0.1f)
        {
            transform.position = destination.position;
            _speed = 0;
            _body.velocity = Vector2.zero;
            _triggered = false;
            return;
        }

        _speed = Mathf.Min(_speed + acceleration * Time.fixedDeltaTime, maxSpeed);
        var direction = (destination.position - transform.position).normalized;
        _body.velocity = _speed * _speedTracker.Multiplier * direction;
    }
}