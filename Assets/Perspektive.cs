using UnityEngine;

public class Perspektive : MonoBehaviour
{
    public Transform spieler;
    public float abstand = 5f;

    void LateUpdate()
    {
        if (spieler == null)
        {
            return;
        }

        transform.position = new Vector3(
            spieler.position.x + abstand,
            transform.position.y,
            transform.position.z
        );
    }
}
