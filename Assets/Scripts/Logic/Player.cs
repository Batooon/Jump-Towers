using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using Deform;
using System.IO;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    public event Action Lost;
    public event Action<int> ScoreChanged;
    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _jumpDuration;
    [SerializeField] private float _randomizerRepeatRate;
    [SerializeField] private int _scoreAddition;

    [field: SerializeField]
    public Renderer Renderer
    {
        get;
        private set;
    }

    [field: SerializeField]
    public SpherifyDeformer Deformer
    {
        get;
        private set;
    }

    public Shapes CurrentShape
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
    private bool _playing;
    private Transform _transform;
    private PlatformType _rules;
    private PlayerData _playerData;
    private int _currentScore;
    private PlayerSettings _playerSettings;

    public void Init(PlatformWay platformWay, PlayerSettings playerSettings, PlayerData playerData)
    {
        _playerSettings = playerSettings;
        _playerData = playerData;
        _platformWay = platformWay;
        _transform = GetComponent<Transform>();
        _availableRandomizeStates = new List<PlayerRandomizer>
        {
            new ColorRandomizer(this, playerSettings),
            new ShapeRandomizer(this, playerSettings)
        };
        _rules = playerSettings.StartingRules;
        RandomizeParameters();
    }

    public void SetupCube()
    {
        SetRules(_rules);
        StartCoroutine(Randomize());
        _playing=true;
    }

    public void ChangeRules()
    {
        switch (_rules)
        {
            case PlatformType.ColorChanging:
            {
                _rules = PlatformType.FormChanging;
                SetRules(_rules);
                break;
            }
            case PlatformType.FormChanging:
            {
                _rules = PlatformType.ColorChanging;
                SetRules(_rules);
                break;
            }
        }
    }

    private void SetRules(PlatformType newType)
    {
        switch (newType)
        {
            case PlatformType.ColorChanging:
                SetRandomizer<ColorRandomizer>();
                break;
            case PlatformType.FormChanging:
                SetRandomizer<ShapeRandomizer>();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void SetRandomizer<T>() where T : PlayerRandomizer
    {
        var state = _availableRandomizeStates.FirstOrDefault(s => s is T);
        _playerRandomizer = state;
    }

    public void SetNewShape(Shapes newShape)
    {
        CurrentShape = newShape;
    }

    public void Die()
    {
        Time.timeScale = 0f;
        Lost?.Invoke();
    }

    private void Start()
    {
        GetNextDestinationPoint();
    }

    private void Update()
    {
        if(_playing==false)
            return;
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
            Die();
            return;
        }

        _currentScore += _scoreAddition;
        if (_currentScore > _playerData.HighScore)
        {
            _playerData.HighScore = _currentScore;
        }
        
        ScoreChanged?.Invoke(_currentScore);
        _nextPlatform = _platformWay.GetNextPlatform();
        _nextDestination = _nextPlatform.JumpPoint.position;
        Jump();
    }

    private void Jump()
    {
        _transform.DOJump(_nextDestination, _jumpHeight, 1, _jumpDuration).OnComplete(GetNextDestinationPoint)
            .SetEase(Ease.Linear);
    }

    private IEnumerator Randomize()
    {
        while (_isHoldingState == false)
        {
            yield return new WaitForSeconds(_randomizerRepeatRate);
            if(_isHoldingState == false)
                _playerRandomizer.Randomize();
        }
    }

    private void RandomizeParameters()
    {
        var randomColor = Random.Range(0, _playerSettings.Colors.Count);
        Renderer.material.color = _playerSettings.Colors[randomColor];
    }
}

[SerializeField]
public class PlayerData
{
    public event Action<PlayerData> Changed;
    [SerializeField] private int _highScore;

    public int HighScore
    {
        get => _highScore;
        set
        {
            _highScore = value;
            Changed?.Invoke(this);
        }
    }
}

public static class JsonSaver
{
    public static T Load<T>(string filename) where T : class
    {
        var path = GetFilePath(filename);
        return FileExists(path) ? JsonUtility.FromJson<T>(File.ReadAllText(path)) : default;
    }

    public static void Save<T>(string filename, T data) where T : class
    {
        var path = GetFilePath(filename);
        File.WriteAllText(path, JsonUtility.ToJson(data));
    }

    public static bool FileExists(string path)
    {
        return File.Exists(path);
    }
    
    private static string GetFilePath(string filename)
    {
#if UNITY_STANDALONE
        var path = Path.Combine(Application.dataPath, filename);
#elif UNITY_IOS || UNITY_ANDROID
        var path = Path.Combine(Application.persistentDataPath, filename);
#endif
        return path;
    }
}