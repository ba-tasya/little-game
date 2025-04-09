using UnityEngine;
using System.Collections;

public class BoilingTeapot : MonoBehaviour
{
    [SerializeField] HUDManager hudManager;
    [SerializeField] CharacterController characterController;

    public Sprite fullSprite;
    public Sprite hotSprite;
    public GameObject fireEffect;
    public Sprite for_reset;

    private bool isFull = false;
    private bool isBoiled = false;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        fireEffect.SetActive(false);
    }

    void OnMouseDown()
    {
        if (hudManager != null && IsWaterSelected())
        {
            sr.sprite = fullSprite;
            isFull = true;
        }
        else if (hudManager != null && IsFireSelected())
        {
            StartCoroutine(SetFire());
        }
    }

    private bool IsWaterSelected() => hudManager.tags[characterController.selectedSlot] == "water";
    private bool IsFireSelected() => hudManager.tags[characterController.selectedSlot] == "fire";

    private IEnumerator SetFire()
    {
        fireEffect.SetActive(true);
        yield return new WaitForSeconds(5f);
        fireEffect.SetActive(false);
        if (isFull)
        {
            sr.sprite = hotSprite;
            isBoiled = true;
        }
    }

    public bool IsReadyToPour() => isBoiled;

    public void ResetTeapot()
    {
        isFull = false;
        isBoiled = false;
        fireEffect.SetActive(false);
        sr.sprite = for_reset;
    }
}