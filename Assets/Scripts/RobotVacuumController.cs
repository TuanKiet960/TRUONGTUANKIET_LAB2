using UnityEngine;
using UnityEngine.InputSystem;    // nhớ import thư viện Input System

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public class RobotVacuumController : MonoBehaviour
{
    [Header("Cài đặt di chuyển thủ công")]
    [Tooltip("Tốc độ di chuyển (units/giây) khi dùng phím điều khiển")]
    public float moveSpeed = 3f;

    private Rigidbody2D rb;
    private Vector2 inputDir;

    // Flag để khóa di chuyển khi win
    private bool locked = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        gameObject.tag = "Player";  // đảm bảo tag "Player" để Collectible trigger đúng
    }

    void Update()
    {
        if (locked)
        {
            inputDir = Vector2.zero;
            return;
        }

        // --- LẤY INPUT TỪ New Input System ---
        // Keyboard.current trả về instance bàn phím
        var keyboard = Keyboard.current;
        if (keyboard == null)
        {
            inputDir = Vector2.zero;
            return;
        }

        float h = 0, v = 0;

        // A / ←
        if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed) h -= 1;
        // D / →
        if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed) h += 1;
        // W / ↑
        if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed) v += 1;
        // S / ↓
        if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed) v -= 1;

        inputDir = new Vector2(h, v);
        if (inputDir.sqrMagnitude > 1f)
            inputDir = inputDir.normalized;
    }

    void FixedUpdate()
    {
        // Di chuyển Rigidbody2D
        Vector2 newPos = rb.position + inputDir * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPos);
    }

    /// <summary>
    /// Khóa di chuyển robot khi Win
    /// </summary>
    public void LockMovement()
    {
        locked = true;
    }
}