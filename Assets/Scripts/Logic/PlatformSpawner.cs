using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [SerializeField] private Vector3 _offset;
    [SerializeField] private Platform _platformPrefab;
    [SerializeField] private int _poolAmount;
    [SerializeField] private Transform _startSpawn;
    private ObjectPool<Platform> _platformPool;
    
    private Vector3 _nextPlatformPosition;

    public void Init()
    {
        _platformPool = new ObjectPool<Platform>(_platformPrefab, _poolAmount);
        _nextPlatformPosition = _startSpawn.position;
    }

    public Platform SpawnPlatform()
    {
        var platform = _platformPool.SpawnObjectAt(_nextPlatformPosition);
        _nextPlatformPosition += _offset;
        return platform;
    }
}
