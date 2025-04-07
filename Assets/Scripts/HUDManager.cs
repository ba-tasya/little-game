using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public Image[] slots; // Ссылки на окошки в HUD
    public string[] tags = new string[5]; // Теги того, что лежит в мини-инвентаре

    public Sprite defaultSprite; // Спрайт пустого слота
    public Sprite[] itemSprites; // Спрайты объектов, которые можно поднимать
    public Text[] slotKeys; // Тексты над слотами для отображения клавиш

    private void Start()
    {
        // Установите клавиши над слотами
        for (int i = 0; i < slotKeys.Length; i++)
        {
            slotKeys[i].text = (i + 1).ToString(); // Клавиши 1-5
        }
    }

    // Метод для добавления объекта в HUD
    public void AddItemToHUD(Sprite itemSprite, int slotIndex, string category)
    {
        if (slotIndex < 0 || slotIndex >= slots.Length)
        {
            Debug.LogError("Неверный индекс слота!");
            return;
        }

        slots[slotIndex].sprite = itemSprite;
        tags[slotIndex] = category;
    }

    public void ResetSlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= slots.Length)
        {
            Debug.LogError("Неверный индекс слота!");
            return;
        }

        slots[slotIndex].sprite = defaultSprite;
    }

    // Метод для сброса всех слотов (например, при старте или смерти игрока)
    public void ResetHUD()
    {
        foreach (var slot in slots)
        {
            slot.sprite = defaultSprite; // Сбрасываем спрайт в дефолтный
        }
    }
}

