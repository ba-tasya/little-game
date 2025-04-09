using UnityEngine;
using System.Collections;

public class StrawPiece : MonoBehaviour
{
    [SerializeField] HUDManager hudManager;
    [SerializeField] CharacterController characterController;
    [SerializeField] private float maxDistanceToInteract = 5f;
    [SerializeField] private float pushForce = 2f;

    private bool isFixed = false; // Заблокирована ли соломина
    private SpriteRenderer sr;
    private Color originalColor;
    private Rigidbody2D rb;

    [SerializeField] private float airEffectRadius = 2f; // Радиус воздействия руны воздуха
    private LayerMask strawLayer; // Слой, на котором находятся все кучки соломы

    private bool isPushed = false; // Проверка, был ли толчок

    void Start()
    {
        if (hudManager == null)
        {
            hudManager = FindObjectOfType<HUDManager>();
        }
        if (characterController == null)
        {
            characterController = FindObjectOfType<CharacterController>();
        }
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
        rb = GetComponent<Rigidbody2D>();
        strawLayer = LayerMask.GetMask("Straw"); // Убедись, что все кучки соломы находятся на этом слое

        // Устанавливаем сопротивление воздуха для замедления
        rb.drag = 1f; // Можно регулировать для большего или меньшего замедления
    }

    void OnMouseDown()
    {
        if (IsAirSelected() && !isFixed)
        {
            ApplyAirEffect();
        }
        else if (IsWaterSelected() && !isFixed)
        {
            FixPosition();
        }
    }

    private bool IsWaterSelected()
    {
        int selectedSlot = characterController.selectedSlot;
        return hudManager.tags[selectedSlot] == "water";
    }

    private bool IsAirSelected()
    {
        int selectedSlot = characterController.selectedSlot;
        return hudManager.tags[selectedSlot] == "air";
    }

    // Применить эффект руны воздуха
    private void ApplyAirEffect()
    {
        Debug.Log("Apply Air Effect");

        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = -1;

        Vector3 playerPos = characterController.transform.position;
        float distanceToPlayer = Vector3.Distance(playerPos, transform.position);

        if (distanceToPlayer > maxDistanceToInteract)
        {
            Debug.Log("Too far from player");
            return;
        }

        Vector2 direction = (mouseWorld - playerPos).normalized;

        // Получаем все кучки в радиусе (большой круг между игроком и точкой клика)
        Collider2D[] nearbyStraws = Physics2D.OverlapCircleAll(playerPos, maxDistanceToInteract, strawLayer);

        foreach (var straw in nearbyStraws)
        {
            var strawPiece = straw.GetComponent<StrawPiece>();
            if (strawPiece != null && !strawPiece.isFixed)
            {
                Vector3 toStraw = straw.transform.position - playerPos;
                float angle = Vector3.Angle(direction, toStraw);

                // Кучка считается "по пути", если угол между направлением и вектором к кучке маленький
                if (angle < 20f)
                {
                    strawPiece.PushAwayFrom(playerPos);
                }
            }
        }
    }

    public void PushAwayFrom(Vector3 source)
    {
        if (!isPushed)  // Проверяем, был ли уже применен толчок
        {
            Vector2 direction = (transform.position - source).normalized;
            rb.velocity = Vector2.zero;  // Останавливаем текущую скорость перед применением силы
            rb.AddForce(direction * pushForce, ForceMode2D.Impulse);  // Применяем импульс только один раз

            // isPushed = true;  // Запрещаем повторный толчок

            // Можно также ограничить максимальную скорость
            float maxSpeed = 5f;  // Например, максимальная скорость 5 единиц в секунду
            if (rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity = rb.velocity.normalized * maxSpeed;  // Ограничиваем скорость
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Проверяем, столкнулась ли кучка с объектом на слоях Straw или Ground
        if (collision.gameObject.layer == LayerMask.NameToLayer("Straw") ||
            collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            // Останавливаем движение кучки
            rb.velocity = Vector2.zero; // Обнуляем скорость
            rb.angularVelocity = 0;     // Останавливаем вращение (если необходимо)
        }
    }

    // Фиксируем соломину
    private void FixPosition()
    {
        Debug.Log("Fix Position");
        isFixed = true;
        DarkenColor(); // Становится темнее
        StartCoroutine(ResetFixPosition());
    }

    // Фиксируем цвет (становится темнее)
    private void DarkenColor()
    {
        Color darkenedColor = originalColor * 0.7f; // Уменьшаем яркость (умножаем на 0.7 для темности)
        sr.color = darkenedColor;
    }

    // Восстанавливаем возможность движения через 10 секунд
    private IEnumerator ResetFixPosition()
    {
        yield return new WaitForSeconds(10f);
        isFixed = false;
        sr.color = originalColor; // Восстановим оригинальный цвет
    }

    // Проверка, зафиксирована ли соломина
    public bool IsFixed() => isFixed;

    // Для визуализации в редакторе
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, airEffectRadius);
    }
}
