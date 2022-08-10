using UnityEngine;
using UnityEngine.InputSystem;

public class Wand : MonoBehaviour
{
    [SerializeField] Transform shootPoint;
    [SerializeField] Bolt bolt1Prefab;
    [SerializeField] Bolt bolt2Prefab;

    public void Shoot(ShootType shootType)
    {
        var boltPrefab = shootType == ShootType.Type1 ? bolt1Prefab : bolt2Prefab;
        var point = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        var direction = ((Vector2)(point - shootPoint.position)).normalized;
        var bolt = Instantiate(boltPrefab, shootPoint.position, Quaternion.identity);
        bolt.SetDirection(direction);
    }

    public enum ShootType
    {
        Type1,
        Type2,
    }
}