using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] HUDManager hudManager;

    public GameObject inventoryPanel; // Панель с инвентарем
    public Image[] topSlots = new Image[5]; // Слоты в верхней части (5 слотов)
    public Image[] bottomSlots = new Image[6]; // Слоты в нижней части (6 слотов)
    public Sprite defaultSprite;

    private bool isInventoryOpen = false; // Флаг для отслеживания состояния инвентаря
    private Sprite[] inventoryItems = new Sprite[6]; // Массив для хранения предметов в нижних слотах
    private Dictionary<string, int> tagToSlotMap = new Dictionary<string, int>(); // Словарь для сопоставления тегов и индексов слотов

    void Start()
    {
        // Скрываем инвентарь при старте
        inventoryPanel.SetActive(false);

        for (int i = 0; i < inventoryItems.Length; i++)
        {
            inventoryItems[i] = null;  // Изначально все слоты пустые
            // bottomSlots[i].sprite = defaultSprite; // Устанавливаем пустой спрайт
        }

        tagToSlotMap.Add("water", 0);  // Тег "water" будет связан со слотом 1
        tagToSlotMap.Add("fire", 1);   // Тег "fire" будет связан со слотом 2
        tagToSlotMap.Add("air", 2);   // Тег "air" будет связан со слотом 3
    }

    void Update()
    {
        // Открываем или закрываем инвентарь по нажатию Tab
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }
    }

    // Метод для переключения состояния инвентаря
    private void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;

        // Показываем или скрываем панель инвентаря
        inventoryPanel.SetActive(isInventoryOpen);
    }

    public void AddItemToInventory(string category, Sprite itemSprite, int ind)
    {
        if (tagToSlotMap.ContainsKey(category))
        {
            int slotIndex = tagToSlotMap[category];

            // Если слот пустой, добавляем предмет в этот слот
            if (inventoryItems[slotIndex] == null)
            {
                inventoryItems[slotIndex] = itemSprite; // Добавляем предмет в инвентарь
                bottomSlots[slotIndex].sprite = itemSprite; // Обновляем спрайт слота
                bottomSlots[slotIndex].gameObject.SetActive(true); // Отображаем слот
                topSlots[ind].sprite = hudManager.slots[ind].sprite;
                topSlots[ind].gameObject.SetActive(true);
                Debug.Log($"Предмет с категорией {category} добавлен в слот {slotIndex + 1}");
            }
            else
            {
                Debug.Log("Этот слот уже занят!");
            }
        }
        else
        {
            Debug.LogError($"Нет слота для предмета с категорией {category}");
        }
    }
}

