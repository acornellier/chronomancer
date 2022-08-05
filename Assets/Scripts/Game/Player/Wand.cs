﻿using UnityEngine;

public class Wand : MonoBehaviour
{
    [SerializeField] Transform wandTip;
    [SerializeField] Bolt bolt1Prefab;
    [SerializeField] Bolt bolt2Prefab;

    public void Shoot(ShootType shootType)
    {
        var boltPrefab = shootType == ShootType.Type1 ? bolt1Prefab : bolt2Prefab;
        var point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var direction = ((Vector2)(point - wandTip.position)).normalized;
        var bolt = Instantiate(boltPrefab, wandTip.position, Quaternion.identity);
        bolt.Direction = direction;
    }

    public enum ShootType
    {
        Type1,
        Type2,
    }
}