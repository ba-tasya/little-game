using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class FirePillar : MonoBehaviour
{
    [SerializeField] InventoryManager inventoryManager;
    [SerializeField] HUDManager hudManager;
    [SerializeField] CharacterController characterController;

    public TextMeshPro textBubble;

    void Start()
    {
        textBubble.gameObject.SetActive(false);
        textBubble.text = "";
    }

    void OnMouseDown()
    {
        if (hudManager != null && IsWaterSelected())
        {
            if (inventoryManager.Contains("water") && inventoryManager.Contains("fire") && inventoryManager.Contains("air"))
            {

                StartCoroutine(Wait(true));

            }
            else
            {

                StartCoroutine(Wait(false));
            }
        }
    }

    public IEnumerator Wait(bool flag)
    {
        if (flag)
        {
            textBubble.gameObject.SetActive(true);
            textBubble.text = "Что ж, проходи...";
            GetComponent<Collider2D>().enabled = false;
            gameObject.GetComponent<SpriteRenderer>().sprite = null;
            yield return new WaitForSeconds(2f);
            textBubble.gameObject.SetActive(false);
            textBubble.text = "";
        }
        else
        {
            textBubble.gameObject.SetActive(true);
            textBubble.text = "Р а н о...";
            yield return new WaitForSeconds(2f);
            textBubble.gameObject.SetActive(false);
            textBubble.text = "";
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