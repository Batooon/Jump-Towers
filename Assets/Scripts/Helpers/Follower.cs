using UnityEngine;

public class Follower : MonoBehaviour
{
    [SerializeField] private Transform _target;
    private Vector3 _previousPosition;
    private Vector3 _currentPosition;
    private Transform _transform;

    private void Start()
    {
        _transform = GetComponent<Transform>();
        _currentPosition = _transform.position;
    }

    private void Update()
    {
        var position = _target.position;
        _currentPosition.x += position.x - _previousPosition.x;
        _transform.position = _currentPosition;
        _previousPosition = position;
    }
}