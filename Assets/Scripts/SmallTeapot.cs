using UnityEngine;
using System.Collections;

public class SmallTeapot : MonoBehaviour
{
    [SerializeField] private Wizard wizard;
    public Sprite teapotWithLidSprite;
    public GameObject lidObject;
    public BoilingTeapot stoveKettle;
    private SpriteRenderer sr;
    public Sprite for_reset;

    private bool hasTeaLeaves = false;
    private bool hasBoilingWater = false;
    private bool hasFilter = false;
    private bool isComplete = false;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (stoveKettle.IsReadyToPour())
        {
            hasBoilingWater = true;
        }
        TryBrewTea();
    }

    public void AddTeaLeaves()
    {
        hasTeaLeaves = true;
        TryBrewTea();
    }

    public void AddBoilingWater()
    {
        if (stoveKettle.IsReadyToPour())
        {
            hasBoilingWater = true;
            stoveKettle.ResetTeapot();
            TryBrewTea();
        }
    }

    public void AddFilter()
    {
        hasFilter = true;
        TryBrewTea();
    }

    private void TryBrewTea()
    {
        if (hasTeaLeaves && hasBoilingWater && hasFilter && !isComplete)
        {
            StartCoroutine(CompleteTea());
        }
        // else if (hasTeaLeaves && hasBoilingWater && !hasFilter)
        // {
        //     wizard.SayLine("не люблю мутный чай");
        // }
    }

    private IEnumerator CompleteTea()
    {
        lidObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        sr.sprite = teapotWithLidSprite;
        isComplete = true;
        yield return wizard.MoveToTableSmooth();
        wizard.SayLine("Как же вкусно...");
    }

    public void ResetTeapot()
    {
        hasTeaLeaves = false;
        hasBoilingWater = false;
        hasFilter = false;
        isComplete = false;
        lidObject.SetActive(true);
        sr.sprite = for_reset;
    }

    public bool HasTeaLeaves() => hasTeaLeaves;
    public bool HasBoilingWater() => hasBoilingWater;
    public bool HasFilter() => hasFilter;
}