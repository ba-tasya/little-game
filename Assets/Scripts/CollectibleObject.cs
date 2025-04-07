
using UnityEngine;

public class CollectibleObject : MonoBehaviour
{
    [SerializeField] HUDManager hudManager;
    [SerializeField] InventoryManager inventoryManager;
    [SerializeField] CharacterController characterController;

    public Sprite itemSprite;
    public string itemCategory; // Категория предмета, например, "water", "fire" и т.д.

    private bool isInRange = false; // Флаг, чтобы проверить, находится ли игрок рядом

    public Sprite GetItemSprite()
    {
        return itemSprite;
    }

    public string GetItemCategory()
    {
        return itemCategory;
    }

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         Collect();
    //         Destroy(gameObject);
    //     }
    // }

    void Update()
    {
        // Проверка нажатия клавиши E, когда игрок рядом с объектом
        if (isInRange && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Нажата клавиша E");
            AddToInventory(itemCategory, itemSprite);
            HidePickText(); // Скрываем текст "Pick"
            Destroy(gameObject); // Уничтожаем объект после того, как он был подобран
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Проверяем, является ли объект подбираемым
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Коллизия с игроком");
            isInRange = true; // Устанавливаем флаг, что игрок рядом
            ShowPickText(); // Показать текст "Pick"
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = false; // Сбросить флаг, если игрок покидает область
            HidePickText(); // Скрыть текст "Pick"
        }
    }
    private void Collect()
    {
        Debug.Log("Объект собран!");
    }

    private void AddToInventory(string category, Sprite itemSprite)
    {
        if (hudManager == null)
        {
            Debug.LogError("HUDManager не назначен!");
            return;
        }

        for (int i = 0; i < characterController.inventory.Length; i++)
        {
            if (characterController.inventory[i] == null) // Если слот пуст
            {

                characterController.inventory[i] = itemSprite; // Добавляем предмет в инвентарь
                hudManager.AddItemToHUD(itemSprite, i, category); // Обновляем HUD
                inventoryManager.AddItemToInventory(category, itemSprite, i);
                Debug.Log($"Предмет добавлен в слот {i + 1}");
                return;
            }
        }

        Debug.Log("Нет свободных слотов в инвентаре!");
    }

    [SerializeField] private GameObject pickText;

    private void ShowPickText()
    {
        if (pickText != null)
        {
            pickText.SetActive(true);
        }
        else
        {
            Debug.LogError("PickText не назначен!");
        }
    }
    private void HidePickText()
    {
        if (pickText != null)
        {
            pickText.SetActive(false);
        }
    }
}
