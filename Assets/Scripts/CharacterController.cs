
using UnityEngine;
using System;

public class CharacterController : MonoBehaviour
{

    Rigidbody2D rb;
    [SerializeField] HUDManager hudManager;
    [SerializeField] InventoryManager inventoryManager;

    public Sprite[] inventory = new Sprite[5]; // Инвентарь игрока
    [Header("Движение")]
    public float moveSpeed = 10f;         // Максимальная скорость по горизонтали
    public float smoothTime = 0.1f;       // Время сглаживания изменения скорости

    [Header("Прыжок")]
    public float jumpForce = 10f;         // Сила прыжка (начальная скорость по Y)
    public float fallMultiplier = 2.5f;   // Множитель гравитации при падении
    public float lowJumpMultiplier = 2f;  // Множитель гравитации, если кнопка прыжка отпущена раньше

    [Header("Проверка земли")]
    public Transform groundCheck;         // Позиция для проверки земли (пустышка внизу игрока)
    public float groundCheckRadius = 0.2f;  // Радиус проверки
    public LayerMask groundLayer;         // Слой объектов, считающихся землёй
    private float velocityXSmoothing;
    private bool isGrounded;

    [Header("Спрайты для режимов управления")]
    public Sprite platformerSprite;
    public Sprite topDownSprite;
    private SpriteRenderer spriteRenderer;

    public enum ControlMode { Platformer, TopDown }
    public ControlMode currentMode;

    /// <summary>
    /// Плавное горизонтальное перемещение с использованием SmoothDamp
    /// </summary>
    // void HandleMovement()
    // {
    //     float input = Input.GetAxisRaw("Horizontal");
    //     float targetVelocityX = input * moveSpeed;
    //     float smoothedVelocityX = Mathf.SmoothDamp(rb.linearVelocity.x, targetVelocityX, ref velocityXSmoothing, smoothTime);
    //     rb.linearVelocity = new Vector2(smoothedVelocityX, rb.linearVelocity.y);
    // }

    /// <summary>
    /// Обработка прыжка с проверкой на землю
    /// </summary>
    void HandleJump()
    {
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            Debug.Log("Jumped");
        }
    }

    /// <summary>
    /// Применение дополнительных сил гравитации для более реалистичного прыжка
    /// </summary>
    void ApplyBetterJumpGravity()
    {
        if (rb.linearVelocity.y < 0)
        {
            // При падении увеличиваем силу гравитации для более быстрого падения
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !Input.GetButton("Jump"))
        {
            // Если игрок отпустил кнопку прыжка раньше, уменьшаем подъемную силу для более короткого прыжка
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    /// <summary>
    /// Визуализация области проверки земли в редакторе
    /// </summary>
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    public int selectedSlot = 0;

    void Start()
    {
        currentMode = ControlMode.Platformer;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (hudManager == null)
        {
            hudManager = FindObjectOfType<HUDManager>();
            if (hudManager == null)
            {
                Debug.LogError("HUDManager не найден в сцене!");
            }
        }
    }

    void Update()
    {
        if (currentMode == ControlMode.Platformer)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
            HandleMovementPlatformer();
            HandleJump();
            ApplyBetterJumpGravity();
            EnableGravityForPlatformer();
        }
        else if (currentMode == ControlMode.TopDown)
        {
            HandleMovementTopDown();
            DisableGravityForTopDown();
        }

        HandleInput();
    }

    private void HandleMovementPlatformer()
    {
        float input = Input.GetAxisRaw("Horizontal");
        float targetVelocityX = input * moveSpeed;
        float smoothedVelocityX = Mathf.SmoothDamp(rb.linearVelocity.x, targetVelocityX, ref velocityXSmoothing, smoothTime);
        rb.linearVelocity = new Vector2(smoothedVelocityX, rb.linearVelocity.y);
    }

    private void HandleMovementTopDown()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");
        Vector2 movement = new Vector2(inputX, inputY).normalized * moveSpeed;
        rb.linearVelocity = movement;
    }

    private void HandleInput()
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                selectedSlot = i;
                Debug.Log($"Выбран слот {i + 1}");
            }
        }

        if (Input.GetMouseButtonDown(0) && selectedSlot >= 0)
        {
            UseSelectedItem();
        }
    }

    private void UseSelectedItem()
    {
        if (inventory[selectedSlot] != null)
        {
            Debug.Log($"Использован предмет из слота {selectedSlot + 1}");
        }
    }

    private void DisableGravityForTopDown()
    {
        // Отключаем влияние гравитации для режима TopDown
        rb.gravityScale = 0f;
    }

    private void EnableGravityForPlatformer()
    {
        // Включаем гравитацию обратно для платформенного режима
        rb.gravityScale = 1f;
    }

    // private void HandleSlotSelection()
    // {
    //     if (Input.GetKeyDown(KeyCode.Alpha1)) activeSlot = 0;
    //     if (Input.GetKeyDown(KeyCode.Alpha2)) activeSlot = 1;
    //     if (Input.GetKeyDown(KeyCode.Alpha3)) activeSlot = 2;
    //     if (Input.GetKeyDown(KeyCode.Alpha4)) activeSlot = 3;
    //     if (Input.GetKeyDown(KeyCode.Alpha5)) activeSlot = 4;
    // }


}
