using UnityEngine;
using TMPro;
using System.Collections;

public class Wizard : MonoBehaviour
{
    public TextMeshPro textBubble;
    public Transform finalPosition;
    public Transform textPosition;
    public Transform startPosition;
    public SmallTeapot teapot;

    public CollectibleObject rune;
    public Pinwheel pinwheel;

    private SpriteRenderer spriteRenderer;

    // private bool hasSpoken = false;
    private bool isInRange = false;
    private Coroutine moveRoutine;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rune.gameObject.SetActive(false);
        pinwheel.gameObject.SetActive(false);
        textBubble.gameObject.SetActive(true);
        textBubble.text = "";
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        // Проверяем, является ли объект подбираемым
        if (other.CompareTag("Player"))
        {
            isInRange = true; // Устанавливаем флаг, что игрок рядом
            GiveHint();
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = false; // Сбросить флаг, если игрок покидает область
            textBubble.text = "";
        }
    }

    public void SayLine(string line)
    {
        if (textBubble != null)
        {
            textBubble.text = line;
            // textBubble.gameObject.SetActive(true);
        }
    }

    public IEnumerator MoveToTableSmooth()
    {
        float time = 0;
        Vector3 start = transform.position;
        Vector3 end = finalPosition.position;
        while (time < 1f)
        {
            transform.position = Vector3.Lerp(start, end, time);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = end;
        textBubble.transform.position = textPosition.position;
        // hasSpoken = true;
    }

    public void ResetWizard()
    {
        // hasSpoken = false;
        transform.position = startPosition.position;
        textBubble.text = "";
        // textBubble.gameObject.SetActive(false);
    }

    public void GiveHint()
    {
        bool a = teapot.HasBoilingWater();
        bool b = teapot.HasTeaLeaves();
        bool c = teapot.HasFilter();

        if (!a && !b && !c) SayLine("Который час?");
        else if (a && !b && !c) SayLine("Я хочу чай, а не воду!");
        else if (!a && b && !c) SayLine("Что-то суховато получается");
        else if (!a && !b && c) SayLine("Ты предлагаешь мне пить воздух? Ну хоть свежий...");
        else if (a && b && !c) SayLine("Не люблю мутный чай");
        else if (a && !b && c) SayLine("Я хочу чай, а не воду, пускай и чистую!");
        else if (!a && b && c) SayLine("Что-то суховато получается");
        else if (a && b && c)
        {
            SayLine("Как же вкусно...");
            StartCoroutine(Disappear());
        }
    }

    public IEnumerator Disappear()
    {
        yield return new WaitForSeconds(2f);
        SayLine("В благодарность за чай держи подарок");
        rune.gameObject.SetActive(true);
        pinwheel.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        SayLine("Используй его с умом");
        yield return new WaitForSeconds(2f);

        // Плавное исчезновение за 5 секунд
        float duration = 5f;
        float elapsed = 0f;
        Color originalColor = spriteRenderer.color;

        while (elapsed < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }

        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        textBubble.text = "";
        textBubble.gameObject.SetActive(false);
        GetComponent<Collider2D>().enabled = false;
        textBubble.text = "";
    }
}
