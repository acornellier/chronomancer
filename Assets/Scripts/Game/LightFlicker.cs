using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class LightFlicker : MonoBehaviour
{
    [SerializeField] float minIntensity;
    [SerializeField] float maxIntensity = 1f;
    [Range(1, 50)] [SerializeField] int smoothing = 5;

    Light2D _light;

    Queue<float> _smoothQueue;
    float _lastSum;

    void Start()
    {
        _light = GetComponent<Light2D>();
        _smoothQueue = new Queue<float>(smoothing);
    }

    void Update()
    {
        if (_light == null)
            return;

        while (_smoothQueue.Count >= smoothing)
        {
            _lastSum -= _smoothQueue.Dequeue();
        }

        var newVal = Random.Range(minIntensity, maxIntensity);
        _smoothQueue.Enqueue(newVal);
        _lastSum += newVal;
        _light.intensity = _lastSum / _smoothQueue.Count;
    }
}