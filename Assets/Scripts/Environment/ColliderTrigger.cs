using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ColliderTrigger : MonoBehaviour
{
    public event Action OnTriggered;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<Player>())
            OnTriggered?.Invoke();
    }
}