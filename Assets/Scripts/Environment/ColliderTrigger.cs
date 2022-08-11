using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class ColliderTrigger : MonoBehaviour
{
    [SerializeField] bool repeatable;

    public event Action OnTriggered;

    bool _triggered;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (_triggered && !repeatable) return;

        if (col.GetComponent<Player>())
        {
            _triggered = true;
            OnTriggered?.Invoke();
        }
    }
}