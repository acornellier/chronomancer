using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SpeedTracker : MonoBehaviour
{
    [SerializeField] new SpriteRenderer renderer;
    [SerializeField] SpriteRenderer[] otherRenderers;

    public Action OnChange;

    readonly Color _slowColor = new(0.25f, 0.25f, 0.75f);
    readonly Color _fastColor = new(0.75f, 0.25f, 0.25f);

    float _multiplier = 1f;

    public float Multiplier
    {
        get => _multiplier;
        set
        {
            _multiplier = Mathf.Clamp(value, 0.5f, 2f);
            UpdateRenderers();
            OnChange?.Invoke();
        }
    }

    void Awake()
    {
        if (renderer == null)
            renderer = GetComponent<SpriteRenderer>();
    }

    void UpdateRenderers()
    {
        if (renderer)
            UpdateRenderer(renderer);

        foreach (var rend in otherRenderers)
        {
            UpdateRenderer(rend);
        }
    }

    void UpdateRenderer(SpriteRenderer rend)
    {
        rend.color = _multiplier switch
        {
            < 1 => _slowColor,
            > 1 => _fastColor,
            _ => Color.white,
        };
    }
}