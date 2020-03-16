using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// работа с UI и контроль очков
/// </summary>
public class GameController : MonoBehaviour
{
    [SerializeField] private Text _pointsText;

    [SerializeField] private GameObject _panelPause;

    [SerializeField] private GameObject _panelGameOver;

    [SerializeField] private Image[] _healths;

    private int _points;

    /// <summary>
    /// Победные очки
    /// </summary>
    public int Points
    {
        get
        {
            return _points;
        }
        set
        {
            _points = value;
            _pointsText.text = "Points: " + _points;
        }
    }

    private void Start()
    {
        Points = 0;
    }

    #region UI

    public void GameOver()
    {
        Time.timeScale = 0;
        AudioListener.volume = 0;
        _panelGameOver.SetActive(true);
    }

    public void ChangeHealth(int health)
    {
        for (int i = 2; i - health >= 0; i--)
        {
            _healths[i].enabled = false;
        }
    }

    public void Restart()
    {
        Time.timeScale = 1;
        AudioListener.volume = 1f;
        SceneManager.LoadScene("Game");
    }

    public void Play()
    {
        Time.timeScale = 1;
        _panelPause.SetActive(false);
        AudioListener.volume = 1f;
    }

    public void Pause()
    {
        Time.timeScale = 0;
        _panelPause.SetActive(true);
        AudioListener.volume = 0;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    #endregion
}
