using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Keyboard.current.dKey.isPressed)
            rb.linearVelocityX = speed;

        else if (Keyboard.current.aKey.isPressed)
            rb.linearVelocityX = -speed;

        else
            rb.linearVelocityX = 0f;
    }
}