using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerStats))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerStats playerStats;
    private Vector2 movementInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerStats = GetComponent<PlayerStats>();

        // 2D Bullet Hell için yerçekimini kapat
        rb.gravityScale = 0f;
    }

    public void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }

    private void FixedUpdate()
    {
        // Anlık güncel yürüme hızını PlayerStats'tan alıyoruz
        float currentSpeed = playerStats.GetStat(StatType.MoveSpeed);

        // Karakter hareketi
        Vector2 movement  = new Vector2(movementInput.x,movementInput.y);
        rb.MovePosition(rb.position + (movement * Time.fixedDeltaTime * currentSpeed));
    }
}