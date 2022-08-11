using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpells : MonoBehaviour
{
    [SerializeField] Transform shootPoint;
    [SerializeField] Bolt bolt1Prefab;
    [SerializeField] Bolt bolt2Prefab;

    [SerializeField] AudioSource shootAudioSource;
    [SerializeField] AudioClip shootClip;

    public void Shoot(ShootType shootType)
    {
        var boltPrefab = shootType == ShootType.Type1 ? bolt1Prefab : bolt2Prefab;
        var point = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        var direction = ((Vector2)(point - shootPoint.position)).normalized;
        var bolt = Instantiate(boltPrefab, shootPoint.position, Quaternion.identity);
        bolt.SetDirection(direction);

        shootAudioSource.Play();
        shootAudioSource.PlayOneShot(shootClip);
    }

    public enum ShootType
    {
        Type1,
        Type2,
    }
}