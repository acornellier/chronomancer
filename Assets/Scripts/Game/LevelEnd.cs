using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class LevelEnd : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        var player = col.GetComponent<Player>();
        if (player)
            player.OnLevelEnd();
    }
}