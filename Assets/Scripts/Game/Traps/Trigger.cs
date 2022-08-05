using UnityEngine;

public class Trigger : MonoBehaviour
{
    public bool Triggered { get; private set; }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<Player>())
            Triggered = true;
    }
}