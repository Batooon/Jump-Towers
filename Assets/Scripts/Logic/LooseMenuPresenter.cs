using UnityEngine;
using UnityEngine.SceneManagement;

public class LooseMenuPresenter : MonoBehaviour
{
    [SerializeField] private GameObject _lostMenu;
    private Player _player;
    
    public void Init(Player player)
    {
        _player = player;
    }

    private void OnEnable()
    {
        _player.Lost += OnLost;
    }

    private void OnDisable()
    {
        _player.Lost -= OnLost;
    }

    public void Retry()
    {
        _lostMenu.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnLost()
    {
        _lostMenu.SetActive(true);
    }
}