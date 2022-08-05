using UnityEngine;

[RequireComponent(typeof(SpeedTracker))]
[RequireComponent(typeof(SpriteRenderer))]
public class ArrowTrap : Trap
{
    [SerializeField] float cooldown = 1f;
    [SerializeField] Arrow arrowPrefab;

    float _cooldownRemaining;

    void Start()
    {
        _cooldownRemaining = cooldown;
    }

    protected override void Update()
    {
        base.Update();

        _cooldownRemaining -= Time.deltaTime;

        if (_cooldownRemaining <= 0)
        {
            Shoot();
            _cooldownRemaining = cooldown / SpeedTracker.Multiplier;
        }
    }

    void Shoot()
    {
        var arrow = Instantiate(arrowPrefab, transform);
        arrow.Direction = Vector2.right;
        arrow.SpeedTracker.Multiplier = SpeedTracker.Multiplier;
    }
}