using UnityEngine;
using UnityEngine.Rendering;

public class SpeedEffekte : MonoBehaviour
{
    public ParticleSystem funken;
    public Volume speedVolume;
    public Volume todVolume;

    public float minFunken = 5f;
    public float maxFunken = 80f;

    void Start()
    {
        if (speedVolume != null)
        {
            speedVolume.weight = 0f;
        }

        if (todVolume != null)
        {
            todVolume.weight = 0f;
        }
    }

    void Update()
    {
        if (GameManager.Instance == null)
        {
            return;
        }

        if (GameManager.Instance.gameOver)
        {
            if (todVolume != null)
            {
                todVolume.weight = 1f;
            }

            if (funken != null)
            {
                funken.Stop();
            }

            return;
        }

        float start = GameManager.Instance.startGeschwindigkeit;
        float max = GameManager.Instance.maxGeschwindigkeit;
        float aktuell = GameManager.Instance.aktuelleGeschwindigkeit;
        float tempo = (aktuell - start) / (max - start);

        if (speedVolume != null)
        {
            speedVolume.weight = tempo;
        }

        if (funken != null)
        {
            ParticleSystem.EmissionModule emission = funken.emission;
            emission.rateOverTime = minFunken + (maxFunken - minFunken) * tempo;
        }
    }
}
