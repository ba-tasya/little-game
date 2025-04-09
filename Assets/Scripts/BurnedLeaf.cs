using UnityEngine;

public class BurnedLeaf : MonoBehaviour
{
    [SerializeField] HUDManager hudManager;
    [SerializeField] CharacterController characterController;
    public GameObject livingLeaf;
    public SmallTeapot teapot;

    void Start()
    {
        livingLeaf.SetActive(false);
    }


    void OnMouseDown()
    {
        if (IsWaterSelected())
        {
            gameObject.SetActive(false);
            livingLeaf.SetActive(true);
            livingLeaf.transform.position = teapot.transform.position; // Автоперенос к чайнику
            teapot.AddFilter();
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
}
