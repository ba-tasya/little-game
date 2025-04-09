using UnityEngine;

public class DoorHell : MonoBehaviour
{
    [SerializeField] private Transform teleportLocation; // Позиция для перемещения в новую комнату
    [SerializeField] private GameObject player; // Игрок (ссылка на объект игрока)

    private bool isNearDoor = false;

    void Update()
    {
        // Если игрок рядом с дверью и нажал E, перемещаем его
        if (isNearDoor && Input.GetKeyDown(KeyCode.E))
        {
            TeleportPlayer();
            SwitchToTopdownView();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Проверяем, что это игрок
        if (other.CompareTag("Player"))
        {
            isNearDoor = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isNearDoor = false;
        }
    }

    private void TeleportPlayer()
    {
        // Перемещаем игрока в новую комнату (по трансформу)
        player.transform.position = teleportLocation.position;
    }

    private void SwitchToTopdownView()
    {
        // Переключаем режим управления на topdown
        CharacterController characterController = player.GetComponent<CharacterController>();
        characterController.currentMode = CharacterController.ControlMode.TopDown;

        // Изменяем спрайт персонажа на для topdown режима
        characterController.GetComponent<SpriteRenderer>().sprite = characterController.topDownSprite;
    }
}
