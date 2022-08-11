using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class LightFlicker : MonoBehaviour
{
    [SerializeField] float minIntensityMultiplier = 1f;
    [SerializeField] float maxIntensityMultiplier = 1.5f;
    [Range(1, 50)] [SerializeField] int smoothing = 5;

    Light2D _light;
    float _initialIntensity;

    Queue<float> _smoothQueue;
    float _lastSum;

    void Awake()
    {
        _light = GetComponent<Light2D>();
        _initialIntensity = _light.intensity;
        _smoothQueue = new Queue<float>(smoothing);
    }

    void Update()
    {
        while (_smoothQueue.Count >= smoothing)
        {
            _lastSum -= _smoothQueue.Dequeue();
        }

        var newVal = Random.Range(
            _initialIntensity * minIntensityMultiplier,
            _initialIntensity * maxIntensityMultiplier
        );
        _smoothQueue.Enqueue(newVal);
        _lastSum += newVal;
        _light.intensity = _lastSum / _smoothQueue.Count;
    }
}