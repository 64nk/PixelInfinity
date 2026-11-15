using UnityEngine;

public class Coin : MonoBehaviour
{
    public int punkteWert = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(punkteWert);
            }
            Destroy(gameObject);
        }
    }
}