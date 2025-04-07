using UnityEngine;

public class Trapdoor : MonoBehaviour
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
        if (hudManager != null && IsFireSelected())
        {
            Extinguish();
        }
        else
        {
            Debug.Log("Выбранный предмет не может сжучь люк!");
        }
    }

    private bool IsFireSelected()
    {
        int selectedSlot = characterController.selectedSlot;
        if (hudManager.tags[selectedSlot] == "fire")
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
        Debug.Log("Люк сожжён");

        // Запускаем эффект тушения, если он задан
        // if (extinguishEffect != null)
        // {
        //     Instantiate(extinguishEffect, transform.position, Quaternion.identity);
        // }

        // Удаляем объект огня
        Destroy(gameObject);
    }

}
