using UnityEngine;

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

    private void Update()
    {
        // 4 yöne hareket girdilerini alıyoruz
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        movementInput = new Vector2(moveX, moveY).normalized;
    }

    private void FixedUpdate()
    {
        // Anlık güncel yürüme hızını PlayerStats'tan alıyoruz
        float currentSpeed = playerStats.GetStat(StatType.MoveSpeed);

        // Fizik motoru ile hareket
        rb.linearVelocity = movementInput * currentSpeed;
    }
}