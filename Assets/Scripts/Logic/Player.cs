using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

//[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _jumpDuration;

    public Renderer Renderer
    {
        get;
        private set;
    }

    private PlayerRandomizer _playerRandomizer;
    private List<PlayerRandomizer> _availableRandomizeStates;
    private Rigidbody _rigidbody;
    private Vector3 _nextDestination;
    private PlatformWay _platformWay;

    public void Init(PlatformWay platformWay, PlayerSettings playerSettings)
    {
        _platformWay = platformWay;
        Renderer = GetComponent<Renderer>();
        _availableRandomizeStates = new List<PlayerRandomizer>
        {
            new ColorRandomizer(this, playerSettings)
        };
        SetRandomizer<ColorRandomizer>();
        InvokeRepeating(nameof(Randomize), 0f, .5f);
    }

    public void SetRandomizer<T>() where T : PlayerRandomizer
    {
        var state = _availableRandomizeStates.FirstOrDefault(s => s is T);
        _playerRandomizer = state;
    }

    private void Awake()
    {
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

    private void Randomize()
    {
        _playerRandomizer.Randomize();
    }
}