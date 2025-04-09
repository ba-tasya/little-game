using UnityEngine;

public class LeftSupport : MonoBehaviour
{
    [SerializeField] HUDManager hudManager;
    [SerializeField] CharacterController characterController;
    public ShelfController shelfController;

    void OnMouseDown()
    {
        if (IsFireSelected())  // Проверка, выбрана ли руна огня
        {
            shelfController.ApplyFireToSupport(false); // Это правое крепление
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
}
