using UnityEngine;
using Zenject;

public class HeightTracker : ITickable
{
    [Inject] Player _player;

    public float currentHeight { get; private set; }
    public float maxHeight { get; private set; }

    float _minHeight;

    public void Tick()
    {
        if (_player.transform.position.y < _minHeight)
            _minHeight = _player.transform.position.y;

        currentHeight = Mathf.Round(_player.transform.position.y - _minHeight);
        if (currentHeight >= maxHeight)
            maxHeight = currentHeight;
    }
}
