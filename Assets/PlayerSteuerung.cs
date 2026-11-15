using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerSteuerung : MonoBehaviour
{
    public float sprungkraft = 5f;
    public float laufgeschwindigkeit = 3f;
    public int maxSpruenge = 2;
    private int aktuelleSpruenge;  
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (GameManager.Instance != null && (GameManager.Instance.isPaused || GameManager.Instance.gameOver))
        {
            return;
        }

        if (aktuelleSpruenge < maxSpruenge && Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, sprungkraft);
            aktuelleSpruenge++;

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.SpieleSprung();
            }
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(laufgeschwindigkeit, rb.linearVelocity.y);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (rb.linearVelocity.y > 0.1f)
        {
            return;
        }

        for (int i = 0; i < collision.contactCount; i++)
        {
            if (collision.GetContact(i).normal.y > 0.5f)
            {
                aktuelleSpruenge = 0;
                return;
            }
        }
    }
}