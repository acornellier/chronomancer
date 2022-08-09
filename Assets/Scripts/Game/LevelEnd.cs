using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class LevelEnd : MonoBehaviour
{
    [SerializeField] SceneAsset nextScene;

    void OnTriggerEnter2D(Collider2D col)
    {
        var player = col.GetComponent<Player>();
        if (player)
            player.OnLevelEnd(nextScene);
    }
}