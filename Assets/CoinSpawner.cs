using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public GameObject coinPrefab;
    public int anzahlMuenzen = 3;

    // Erreichbare Sprunghöhen (Boden liegt bei Y = -2)
    public float[] moeglicheHoehen = new float[] { -1.3f, -0.5f, -0.1f, 2f, 2.8f };

    // Bereich innerhalb des Chunks von links nach rechts
    public float minX = -6f;
    public float maxX = 6f;

    void Start()
    {
        if (coinPrefab == null)
        {
            return;
        }

        for (int i = 0; i < anzahlMuenzen; i++)
        {
            if (Random.value > 0.3f)
            {
                float zufallsX = Random.Range(minX, maxX);
                int zufallsIndex = Random.Range(0, moeglicheHoehen.Length);
                float zufallsY = moeglicheHoehen[zufallsIndex];

                Vector3 spawnPos = new Vector3(transform.position.x + zufallsX, zufallsY, 0f);

                if (Physics2D.OverlapCircle(spawnPos, 0.15f) != null)
                {
                    continue;
                }
                Instantiate(coinPrefab, spawnPos, Quaternion.identity, transform);
            }
        }
    }
}