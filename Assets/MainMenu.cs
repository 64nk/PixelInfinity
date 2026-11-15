using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public GameObject scoresPanel;
    public TMP_Text lastScoresText;

    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void OpenScores()
    {
        if (scoresPanel != null)
        {
            scoresPanel.SetActive(true);
        }

        if (lastScoresText != null)
        {
            int score1 = PlayerPrefs.GetInt("Highscore", 0);
            int score2 = PlayerPrefs.GetInt("Highscore2", 0);
            int score3 = PlayerPrefs.GetInt("Highscore3", 0);

            lastScoresText.text = "HIGHSCORES:\n\n" +
                                  "1. " + score1 + "\n" +
                                  "2. " + score2 + "\n" +
                                  "3. " + score3;
        }
    }

    public void CloseScores()
    {
        if (scoresPanel != null)
        {
            scoresPanel.SetActive(false);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Spiel beendet!");
    }
}