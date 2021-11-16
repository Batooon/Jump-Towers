using TMPro;
using UnityEngine;

public class MainMenuPresenter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _highScore;
    [SerializeField] private string _highScoreTemplate;
    [SerializeField] private GameObject _scorePanel;
    
    public void Init(PlayerData playerData)
    {
        _highScore.text = string.Format(_highScoreTemplate, playerData.HighScore.ToString());
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        _scorePanel.SetActive(true);
        gameObject.SetActive(false);
    }
}