using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class KillPlayerHitbox : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        var player = col.gameObject.GetComponent<Player>();
        if (player)
            player.Die();
    }
}