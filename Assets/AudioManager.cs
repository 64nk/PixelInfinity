using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource musikQuelle;
    public AudioSource effektQuelle;

    public AudioClip sprungSound;
    public AudioClip gameOverSound;

    public float sprungLautstaerke = 0.25f;
    public float gameOverLautstaerke = 0.8f;

    void Awake()
    {
        Instance = this;
    }

    public void SpieleSprung()
    {
        if (effektQuelle != null && sprungSound != null)
        {
            effektQuelle.PlayOneShot(sprungSound, sprungLautstaerke);
        }
    }

    public void SpieleGameOver()
    {
        if (musikQuelle != null)
        {
            musikQuelle.Stop();
        }

        if (effektQuelle != null && gameOverSound != null)
        {
            effektQuelle.PlayOneShot(gameOverSound, gameOverLautstaerke);
        }
    }
}
