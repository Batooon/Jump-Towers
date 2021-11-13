using System.Collections.Generic;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private PlayerSettings _playerSettings;
    [SerializeField] private Player _player;
    [SerializeField] private PlatformSpawner _platformSpawner;

    private void Awake()
    {
        _platformSpawner.Init(_playerSettings);
        
        _player.Init(_platformSpawner.Platforms, _playerSettings);
    }
}

public class PlatformWay
{
    private readonly Queue<Platform> _platforms = new Queue<Platform>();

    public void AddPlatform(Platform newPlatform)
    {
        _platforms.Enqueue(newPlatform);
    }

    public Transform GetNextDestinationPoint()
    {
        return _platforms.Dequeue().JumpPoint;
    }

    public Platform GetNextPlatform()
    {
        return _platforms.Dequeue();
    }
}