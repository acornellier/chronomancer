using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] float parralaxEffect;

    float _startPos;

    void Start()
    {
        _startPos = transform.position.x;
    }

    void FixedUpdate()
    {
        var dist = Camera.main.transform.position.x * parralaxEffect;
        transform.position = new Vector3(
            _startPos + dist,
            Camera.main.transform.position.y,
            transform.position.z
        );
    }
}