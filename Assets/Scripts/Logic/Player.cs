using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _jumpDuration;
    [SerializeField] private float _randomizerRepeatRate;

    public Renderer Renderer
    {
        get;
        private set;
    }

    private PlayerRandomizer _playerRandomizer;
    private List<PlayerRandomizer> _availableRandomizeStates;
    private Vector3 _nextDestination;
    private Platform _nextPlatform;
    private PlatformWay _platformWay;
    private bool _isHoldingState;

    public void Init(PlatformWay platformWay, PlayerSettings playerSettings)
    {
        _platformWay = platformWay;
        Renderer = GetComponent<Renderer>();
        _availableRandomizeStates = new List<PlayerRandomizer>
        {
            new ColorRandomizer(this, playerSettings)
        };
        SetRandomizer<ColorRandomizer>();
        StartCoroutine(Randomize());
    }

    public void SetRandomizer<T>() where T : PlayerRandomizer
    {
        var state = _availableRandomizeStates.FirstOrDefault(s => s is T);
        _playerRandomizer = state;
    }

    private void Start()
    {
        GetNextDestinationPoint();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && _isHoldingState == false)
        {
            _isHoldingState = true;
        }

        if (Input.GetMouseButtonUp(0) && _isHoldingState)
        {
            _isHoldingState = false;
            StartCoroutine(Randomize());
        }
    }

    private void GetNextDestinationPoint()
    {
        if (_nextPlatform != null && _nextPlatform.TryAccept(this) == false)
        {
            Debug.Log("You Loose");
            return;
        }
        _nextPlatform = _platformWay.GetNextPlatform();
        _nextDestination = _nextPlatform.JumpPoint.position;
        Jump();
    }

    private void Jump()
    {
        transform.DOJump(_nextDestination, _jumpHeight, 1, _jumpDuration).OnComplete(GetNextDestinationPoint)
            .SetEase(Ease.Linear);
    }

    private IEnumerator Randomize()
    {
        while (_isHoldingState == false)
        {
            yield return new WaitForSecondsRealtime(_randomizerRepeatRate);
            if(_isHoldingState == false)
                _playerRandomizer.Randomize();
        }
    }
}