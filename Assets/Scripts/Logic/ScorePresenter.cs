using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScorePresenter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _highScoreText;
    [SerializeField] private string _highScoreTemplate;
    [SerializeField] private TextMeshProUGUI _currentScoreText;
    [SerializeField] private string _currentScoreTemplate;

    private Player _player;
    private PlayerData _playerData;

    public void Init(Player player, PlayerData playerData)
    {
        _player = player;
        _playerData = playerData;
    }

    private void OnEnable()
    {
        _player.ScoreChanged += OnScoreChanged;
        _playerData.Changed += OnHighScoreChanged;
        OnScoreChanged(0);
        OnHighScoreChanged(_playerData);
    }

    private void OnDisable()
    {
        _player.ScoreChanged -= OnScoreChanged;
        _playerData.Changed -= OnHighScoreChanged;
    }

    private void OnScoreChanged(int newScore)
    {
        _currentScoreText.text = string.Format(_currentScoreTemplate, newScore.ToString());
    }

    private void OnHighScoreChanged(PlayerData newPlayerData)
    {
        _highScoreText.text = string.Format(_highScoreTemplate, newPlayerData.HighScore.ToString());
    }
}