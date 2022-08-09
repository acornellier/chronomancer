using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Elevator : MonoBehaviour
{
    [SerializeField] ColliderTrigger trigger;
    [SerializeField] float speed;

    Rigidbody2D _body;

    bool _triggered;

    void Awake()
    {
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
        _body.velocity = speed * Vector2.down;
    }
}