using UnityEngine;

[RequireComponent(typeof(SpeedTracker))]
public class ArrowTrap : MonoBehaviour
{
    [SerializeField] SpeedTracker speedTracker;
    [SerializeField] Transform spawnPoint;
    [SerializeField] Arrow arrowPrefab;
    [SerializeField] Vector2 direction = Vector2.right;
    [SerializeField] float cooldown = 1f;
    [SerializeField] float initialCooldown;

    float _cooldownRemaining;

    void Start()
    {
        _cooldownRemaining = initialCooldown;
    }

    void Update()
    {
        _cooldownRemaining -= Time.deltaTime;

        if (_cooldownRemaining <= 0)
        {
            Shoot();
            _cooldownRemaining = cooldown;
            // _cooldownRemaining = cooldown / SpeedTracker.Multiplier;
        }
    }

    void Shoot()
    {
        var arrow = Instantiate(arrowPrefab, spawnPoint.position, Quaternion.identity, transform);
        arrow.Direction = direction;
        arrow.SpeedTracker.Multiplier = speedTracker.Multiplier;
    }
}