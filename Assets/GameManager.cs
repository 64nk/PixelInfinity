using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float startGeschwindigkeit = 5f;
    public float maxGeschwindigkeit = 11f;
    public float schwierigkeitsZuwachs = 0.15f;
    public int score = 0;

    public TMP_Text scoreText;
    public TMP_Text gameOverInfoText;
    public GameObject gameOverPanel;
    public GameObject pausePanel;

    public bool gameOver = false;
    public bool isPaused = false;

    public float aktuelleGeschwindigkeit;
    private int highscore;
    public PlayerSteuerung spieler;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        aktuelleGeschwindigkeit = startGeschwindigkeit;
        score = 0;
        gameOver = false;
        isPaused = false;
        Time.timeScale = 1f;

        highscore = PlayerPrefs.GetInt("Highscore", 0);

        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
    }

    void Update()
    {
        if (!gameOver && Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }

        if (gameOver || isPaused)
        {
            return;
        }

        if (aktuelleGeschwindigkeit < maxGeschwindigkeit)
        {
            aktuelleGeschwindigkeit += schwierigkeitsZuwachs * Time.deltaTime;
        }

        if (spieler != null)
        {
            spieler.laufgeschwindigkeit = aktuelleGeschwindigkeit;
        }
    }

    public void AddScore(int punkte)
    {
        if (gameOver)
        {
            return;
        }

        score += punkte;

        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;

        if (pausePanel != null)
        {
            pausePanel.SetActive(isPaused);
        }
    }

    public void GameOver()
    {
        if (gameOver)
        {
            return;
        }

        gameOver = true;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SpieleGameOver();
        }

        int platz1 = PlayerPrefs.GetInt("Highscore", 0);
        int platz2 = PlayerPrefs.GetInt("Highscore2", 0);
        int platz3 = PlayerPrefs.GetInt("Highscore3", 0);

        if (score > platz1)
        {
            platz3 = platz2;
            platz2 = platz1;
            platz1 = score;
        }
        else if (score > platz2)
        {
            platz3 = platz2;
            platz2 = score;
        }
        else if (score > platz3)
        {
            platz3 = score;
        }

        PlayerPrefs.SetInt("Highscore", platz1);
        PlayerPrefs.SetInt("Highscore2", platz2);
        PlayerPrefs.SetInt("Highscore3", platz3);
        PlayerPrefs.Save();

        highscore = platz1;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        if (gameOverInfoText != null)
        {
            gameOverInfoText.text = "GAME OVER\nScore: " + score + "\nHighscore: " + highscore;
        }

        Time.timeScale = 0f;
    }

    public void Neustart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ZumMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}