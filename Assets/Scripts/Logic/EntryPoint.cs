using System.Collections.Generic;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField]private PlayerSettings _playerSettings;
    [SerializeField] private Player _player;
    [SerializeField] private PlatformSpawner _platformSpawner;
    private PlatformWay _platformWay = new PlatformWay();

    private void Awake()
    {
        _platformSpawner.Init();
        for (var i = 0; i < 10; i++)
        {
            _platformWay.AddPlatform(_platformSpawner.SpawnPlatform());
        }
        
        _player.Init(_platformWay, _playerSettings);
    }
}

public class PlatformWay
{
    private Queue<Platform> _platforms = new Queue<Platform>();

    public void AddPlatform(Platform newPlatform)
    {
        _platforms.Enqueue(newPlatform);
    }

    public Transform GetNextDestinationPoint()
    {
        return _platforms.Dequeue().JumpPoint;
    }
}