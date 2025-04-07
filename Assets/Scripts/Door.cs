using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] Fire fire;

    public Sprite openDoorSprite; // Спрайт открытой двери
    private SpriteRenderer spriteRenderer;
    private bool isOpen = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("На двери отсутствует компонент SpriteRenderer!");
        }
    }

    void Update()
    {
        if (fire == null && !isOpen)
        {
            Open();
        }
    }

    public void Open()
    {
        if (!isOpen)
        {
            spriteRenderer.sprite = openDoorSprite;
            isOpen = true;
            Debug.Log("Дверь открыта!");

            GetComponent<Collider2D>().enabled = false;
        }
    }
}
