using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] HUDManager hudManager;
    [SerializeField] CharacterController characterController;

    // public ParticleSystem extinguishEffect; // Эффект тушения огня

    void Start()
    {
        hudManager = FindObjectOfType<HUDManager>();
        if (hudManager == null)
        {
            Debug.LogError("HUDManager не найден в сцене!");
        }
    }

    void OnMouseDown()
    {
        if (hudManager != null && IsWaterSelected())
        {
            Extinguish();
        }
        else
        {
            Debug.Log("Выбранный предмет не может потушить огонь!");
        }
    }

    private bool IsWaterSelected()
    {
        int selectedSlot = characterController.selectedSlot;
        if (hudManager.tags[selectedSlot] == "water")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // Метод для тушения огня
    private void Extinguish()
    {
        Debug.Log("Огонь потушен!");

        // Запускаем эффект тушения, если он задан
        // if (extinguishEffect != null)
        // {
        //     Instantiate(extinguishEffect, transform.position, Quaternion.identity);
        // }

        // Удаляем объект огня
        Destroy(gameObject);
    }

}
