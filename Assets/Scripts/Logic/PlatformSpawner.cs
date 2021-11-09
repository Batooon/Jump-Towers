using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [SerializeField] private Vector3 _offset;
    [SerializeField] private GameObject _platformPrefab;
    [SerializeField] private int _poolAmount;
    [SerializeField] private Transform _startSpawn;
    private ObjectPool _platformPool;
    
    private Vector3 _nextPlatformPosition;

    private void Awake()
    {
        _platformPool = new ObjectPool(_platformPrefab, _poolAmount);
        _nextPlatformPosition = _startSpawn.position;
        for (var i = 0; i < 5; i++)
        {
            SpawnPlatform();
        }
    }

    public void SpawnPlatform()
    {
        _platformPool.SpawnObjectAt(_nextPlatformPosition);
        _nextPlatformPosition += _offset;
    }
}
