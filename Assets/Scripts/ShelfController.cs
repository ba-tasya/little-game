using UnityEngine;
using System.Collections;

public class ShelfController : MonoBehaviour
{
    public GameObject teaJar;               // Банка с заваркой
    public GameObject floorPosition;        // Точка, куда падает банка при ошибке
    public GameObject teapotPosition;       // Точка, куда должна упасть банка в чайник
    public SmallTeapot teapotManager;     // Ссылка на заварочный чайник
    public Wizard wizardManager;     // Ссылка на волшебника

    private bool actionTaken = false;       // Чтобы не допустить повторной активации

    // Применение руны огня к креплению
    public void ApplyFireToSupport(bool isRightSupport)
    {
        Debug.Log("ApplyFireToSupport");
        if (actionTaken) { return; }
        actionTaken = true;

        if (isRightSupport)
        {
            // Успех — банка падает в чайник
            teaJar.transform.position = teapotPosition.transform.position;
            teapotManager.AddTeaLeaves();
        }
        else
        {
            // Ошибка — банка падает на пол
            teaJar.transform.position = floorPosition.transform.position;
            wizardManager.SayLine("Ничего тебе доверить нельзя.");
            // yield return new WaitForSeconds(2);
            ResetRoom();
        }
    }

    // Сброс состояния всех объектов
    public void ResetRoom()
    {
        teaJar.transform.position = transform.position;
        actionTaken = false;
        teapotManager.ResetTeapot();
        FindObjectOfType<BoilingTeapot>().ResetTeapot();
        FindObjectOfType<Plant>().ResetPlant();
        wizardManager.ResetWizard();
    }
}
