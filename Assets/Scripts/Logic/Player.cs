using UnityEngine;
using DG.Tweening;

//[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _jumpLength;
    [SerializeField] private float _jumpDuration;
    
    private Rigidbody _rigidbody;
    private Vector3 _nextDestination;
    private PlatformWay _platformWay;

    public void Init(PlatformWay platformWay)
    {
        _platformWay = platformWay;
    }
    
    private void Awake()
    {
        //_rigidbody = GetComponent<Rigidbody>();
        GetNextDestinationPoint();
    }

    private void GetNextDestinationPoint()
    {
        _nextDestination = _platformWay.GetNextDestinationPoint().position;
        Jump();
    }

    private void Jump()
    {
        transform.DOJump(_nextDestination, _jumpHeight, 1, _jumpDuration).OnComplete(GetNextDestinationPoint)
            .SetEase(Ease.Linear);
    }
}