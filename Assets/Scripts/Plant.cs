using UnityEngine;
using System.Collections;

public class Plant : MonoBehaviour
{
    [SerializeField] HUDManager hudManager;
    [SerializeField] CharacterController characterController;
    public GameObject burnedLeaf;
    public GameObject smileyEffect;
    public Sprite emptyPotSprite;
    public Sprite start;

    private bool isBurned = false;

    void Start()
    {
        burnedLeaf.SetActive(false);
        smileyEffect.SetActive(false);
    }


    void OnMouseDown()
    {
        if (IsWaterSelected())
        {
            if (isBurned)
            {
                GetComponent<SpriteRenderer>().sprite = start;
            }
            StartCoroutine(ShowSmiley());
        }
        else if (IsFireSelected() && !isBurned)
        {
            isBurned = true;
            GetComponent<SpriteRenderer>().sprite = emptyPotSprite;
            burnedLeaf.SetActive(true);
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

    IEnumerator ShowSmiley()
    {
        smileyEffect.SetActive(true);
        yield return new WaitForSeconds(2);
        smileyEffect.SetActive(false);
    }

    public void ResetPlant()
    {
        burnedLeaf.SetActive(false);
        smileyEffect.SetActive(false);
        GetComponent<SpriteRenderer>().sprite = start;
    }
}
