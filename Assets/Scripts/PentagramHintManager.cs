using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PentagramHintManager : MonoBehaviour
{
    public GameObject hintStarPrefab;
    public List<Transform> hintPositions;

    private GameObject currentStar;

    void Start()
    {
        StartCoroutine(ShowHintRoutine());
    }

    IEnumerator ShowHintRoutine()
    {
        while (true)
        {
            if (currentStar != null)
                Destroy(currentStar);

            int index = Random.Range(0, hintPositions.Count);
            Vector3 pos = hintPositions[index].position;

            currentStar = Instantiate(hintStarPrefab, pos, Quaternion.identity);
            currentStar.transform.localScale = Vector3.one * 0.2f;

            SpriteRenderer sr = currentStar.GetComponent<SpriteRenderer>();
            if (sr != null)
                yield return StartCoroutine(FadeSprite(sr, 0f, 1f, 0.5f)); // плавное появление

            yield return new WaitForSeconds(1.5f); // звезда остаётся видимой

            if (sr != null)
                yield return StartCoroutine(FadeSprite(sr, 1f, 0f, 0.5f)); // плавное исчезновение

            Destroy(currentStar);
            yield return new WaitForSeconds(1.5f);
        }
    }

    IEnumerator FadeSprite(SpriteRenderer sr, float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        Color color = sr.color;

        while (elapsed < duration)
        {
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            sr.color = new Color(color.r, color.g, color.b, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }

        sr.color = new Color(color.r, color.g, color.b, endAlpha);
    }
}
