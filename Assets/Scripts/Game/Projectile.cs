using UnityEngine;


public class Projectile : MonoBehaviour
{
    [SerializeField] float baseSpeed = 1f;

    public Vector3 Direction { get; set; }

    void FixedUpdate()
    {
        transform.Translate(baseSpeed * Time.fixedDeltaTime * Direction);
    }
}