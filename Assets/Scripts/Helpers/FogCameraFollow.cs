using System;
using UnityEngine;

public class FogCameraFollow : MonoBehaviour
{
    [SerializeField] private Camera _followCamera;
    private Vector3 _previousPosition;
    private Vector3 _currentPosition;
    private Transform _transform;
    private Transform _cameraTransform;
    
    private void Start()
    {
        _currentPosition = transform.position;
        _transform=GetComponent<Transform>();
        _cameraTransform = _followCamera.transform;
    }

    private void Update()
    {
        var position = _cameraTransform.position;
        _currentPosition.x += position.x - _previousPosition.x;
        _transform.position = _currentPosition;
        _previousPosition = position;
    }
}