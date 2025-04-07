using UnityEngine;

public class ColdAirClickPlatform : MonoBehaviour
{
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private float platformLifetime = 3f;
    [SerializeField] HUDManager hudManager;
    [SerializeField] CharacterController characterController;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
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
            Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(mouseWorldPos);

            if (hit != null && hit.gameObject == gameObject)
            {
                CreatePlatformAt(mouseWorldPos);
            }
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

    void CreatePlatformAt(Vector2 position)
    {
        GameObject platform = Instantiate(platformPrefab, position, Quaternion.identity);
        Destroy(platform, platformLifetime);
    }
}
