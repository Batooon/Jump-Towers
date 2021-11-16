using System.Collections.Generic;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private PlayerSettings _playerSettings;
    [SerializeField] private Player _player;
    [SerializeField] private PlatformSpawner _platformSpawner;
    [SerializeField] private string _playerDataFile;
    [SerializeField] private ScorePresenter _scorePresenter;
    [SerializeField] private MainMenuPresenter _mainMenuPresenter;
    [SerializeField] private LooseMenuPresenter _looseMenuPresenter;
    private PlayerData _playerData;

    private void Awake()
    {
        Time.timeScale = 0f;
        _playerSettings.StartingRules = (PlatformType)Random.Range(0, 2);
        _playerData = JsonSaver.Load<PlayerData>(_playerDataFile) ?? new PlayerData();
        _platformSpawner.Init(_playerSettings);
        _mainMenuPresenter.Init(_playerData);
        _looseMenuPresenter.Init(_player);
        
        _scorePresenter.Init(_player, _playerData);
        _player.Init(_platformSpawner.Platforms, _playerSettings, _playerData);
    }

    private void OnApplicationQuit()
    {
        JsonSaver.Save(_playerDataFile, _playerData);
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