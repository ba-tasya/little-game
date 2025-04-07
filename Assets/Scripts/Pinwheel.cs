using UnityEngine;
using System.Collections;

public class Pinwheel : MonoBehaviour
{
    [SerializeField] private GameObject windZonePrefab;
    [SerializeField] HUDManager hudManager;
    [SerializeField] CharacterController characterController;

    private bool isActive = false;

    void Start()
    {
        hudManager = FindObjectOfType<HUDManager>();
        if (hudManager == null)
        {
            Debug.LogError("HUDManager не найден в сцене!");
        }
    }

    private void OnMouseDown()
    {
        if (hudManager != null && IsAirSelected() && !isActive)
        {
            StartCoroutine(SpawnWindZone());
        }
    }

    private bool IsAirSelected()
    {
        int selectedSlot = characterController.selectedSlot;
        if (hudManager.tags[selectedSlot] == "air")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private IEnumerator SpawnWindZone()
    {
        isActive = true;

        // создаём поток на позиции этого объекта
        GameObject windZone = Instantiate(windZonePrefab, transform.position, Quaternion.identity);

        // через 5 секунд удаляем поток
        yield return new WaitForSeconds(5f);
        Destroy(windZone);

        isActive = false;
    }
}
