using System;
using UnityEngine;
using GD.MinMaxSlider;
using Random = UnityEngine.Random;

public class PlatformSpawner : MonoBehaviour
{
    [SerializeField] private Vector3 _offset;
    [SerializeField] private Platform _platformPrefab;
    [SerializeField] private int _poolAmount;
    [SerializeField] private Transform _startSpawn;
    [SerializeField, MinMaxSlider(3,20)] private Vector2Int _ruleChangerPlatformRange;
    [SerializeField] private PlatformFactory _platformFactory;
    [SerializeField, Range(3, 5)] private int _amountToGenerateFirst;

    private PlatformType _currentPlatformType = PlatformType.ColorChanging;
    private int _platformCounter;
    private int _nextPlatformRuleChanger;

    public PlatformWay Platforms
    {
        get;
        private set;
    }
    
    private ObjectPool<Platform> _platformPool;

    private Vector3 _nextPlatformPosition;

    public void Init(PlayerSettings playerSettings)
    {
        _platformFactory.Init(playerSettings);
        Platforms = new PlatformWay();
        _platformPool = new ObjectPool<Platform>(_platformPrefab, _poolAmount);
        
        var colorChangingPlatform = _platformFactory.GetConfig(PlatformType.ColorChanging).Prefab;
        AddPlatformToPool(colorChangingPlatform);
        var formChangingPlatform = _platformFactory.GetConfig(PlatformType.FormChanging).Prefab;
        AddPlatformToPool(formChangingPlatform);
        var rulesChangingPlatform = _platformFactory.GetConfig(PlatformType.RulesChanging).Prefab;
        AddPlatformToPool(rulesChangingPlatform, 1);
        
        _nextPlatformPosition = _startSpawn.position;
        RandomizeRuleChangerValue();
        
        for (var i = 0; i < _amountToGenerateFirst; i++)
        {
            PlacePlatform();
        }
    }

    private void SpawnPlatform(Platform platformToSpawn)
    {
        var platform = _platformPool.ShowObjectAt(platformToSpawn, _nextPlatformPosition);
        SetupPlatform(platform);
        platform.OnDisappeared += ReplacePlatform;
    }

    private void AddPlatformToPool(Platform platform, int amount = 5)
    {
        for (var i = 0; i < amount; i++)
        {
            _platformPool.AddObject(platform);
        }
    }

    private void PlacePlatform()
    {
        _platformCounter += 1;
        var platformType = GetNextPlatformType();
        var platform = _platformPool.GetObject(_platformFactory.GetConfig(platformType).Prefab);
        SetupPlatform(platform);
        platform.OnDisappeared += ReplacePlatform;
        platform.gameObject.SetActive(true);
    }

    private void ReplacePlatform(Platform platform)
    {
        if(platform == null)
            return;
        platform.OnDisappeared -= ReplacePlatform;
        _platformCounter += 1;
        var newPlatformType = GetNextPlatformType();
        platform = _platformPool.GetObject(_platformFactory.GetConfig(newPlatformType).Prefab);
        SetupPlatform(platform);
        platform.OnDisappeared += ReplacePlatform;
        platform.gameObject.SetActive(true);
    }

    private void SetupPlatform(Platform platform)
    {
        platform.SetArgs(_platformFactory.GetArgs(_currentPlatformType));
        platform.transform.position = _nextPlatformPosition;
        Platforms.AddPlatform(platform);
        _nextPlatformPosition += _offset;
    }

    private PlatformType GetNextPlatformType()
    {
        var newPlatformType = _currentPlatformType;
        if (_platformCounter != _nextPlatformRuleChanger)
            return newPlatformType;
            
        _platformCounter = 0;
        newPlatformType = PlatformType.RulesChanging;
        ChangePlatformType();
        RandomizeRuleChangerValue();
        
        return newPlatformType;
    }

    private void ChangePlatformType()
    {
        _currentPlatformType = _currentPlatformType switch
        {
            PlatformType.ColorChanging => PlatformType.FormChanging,
            PlatformType.FormChanging => PlatformType.ColorChanging,
            _ => _currentPlatformType
        };
    }

    private void RandomizeRuleChangerValue()
    {
        _nextPlatformRuleChanger = Random.Range(_ruleChangerPlatformRange.x, _ruleChangerPlatformRange.y + 1);
    }
}

public enum PlatformType
{
    ColorChanging = 0,
    FormChanging = 1,
    RulesChanging = 2
}

[Serializable]
public class PlatformConfig
{
    public Platform Prefab;
    // public Color PlatformColor;
}

[Serializable]
public class PlatformArgs
{
    public Color PlatformColor;
}
