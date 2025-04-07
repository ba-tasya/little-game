using UnityEngine;
using System.Collections;

public class TreeController : MonoBehaviour
{
    [SerializeField] private GameObject smallTree;
    [SerializeField] private GameObject bigTree;
    [SerializeField] private float growthTime = 0.5f;
    [SerializeField] private float shrinkDelay = 10f;
    [SerializeField] private Transform topOfBigTree;
    [SerializeField] HUDManager hudManager;
    [SerializeField] CharacterController characterController;

    private bool isGrowing = false;
    private bool playerOnTree = false;

    void Start()
    {
        hudManager = FindObjectOfType<HUDManager>();
        if (hudManager == null)
        {
            Debug.LogError("HUDManager не найден в сцене!");
        }
        smallTree.SetActive(true);
        bigTree.SetActive(false);
    }

    void OnMouseDown()
    {
        if (hudManager != null && IsWaterSelected() && !isGrowing)
        {
            CheckIfPlayerOnTree();
            StartCoroutine(GrowTree());
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

    private void CheckIfPlayerOnTree()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Collider2D playerCol = player.GetComponent<Collider2D>();
            Collider2D treeCol = smallTree.GetComponent<Collider2D>();
            if (playerCol != null && treeCol != null)
            {
                playerOnTree = playerCol.IsTouching(treeCol);
            }
        }
    }

    private IEnumerator GrowTree()
    {
        isGrowing = true;

        float timeElapsed = 0f;
        Vector3 initialScale = smallTree.transform.localScale;
        Vector3 targetScale = new Vector3(0.6f, 0.6f, 1); // изначально (0.1, 0.1, 1)

        while (timeElapsed < growthTime)
        {
            smallTree.transform.localScale = Vector3.Lerp(initialScale, targetScale, timeElapsed / growthTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        smallTree.transform.localScale = targetScale;

        smallTree.GetComponent<SpriteRenderer>().enabled = false;
        smallTree.GetComponent<Collider2D>().enabled = false;

        bigTree.SetActive(true);
        bigTree.GetComponent<SpriteRenderer>().enabled = true;
        bigTree.GetComponent<Collider2D>().enabled = true;

        if (playerOnTree)
        {
            MovePlayerToTopOfBigTree();
        }

        // Debug.Log("Tree fully grown, waiting to shrink...");
        yield return new WaitForSeconds(shrinkDelay);

        // Debug.Log("ShrinkTree about to start");
        ShrinkTree();
        isGrowing = false;
        playerOnTree = false;
    }

    private void MovePlayerToTopOfBigTree()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && topOfBigTree != null)
        {
            player.transform.position = topOfBigTree.position;
        }
    }

    private void ShrinkTree()
    {
        bigTree.GetComponent<SpriteRenderer>().enabled = false;
        bigTree.GetComponent<Collider2D>().enabled = false;

        smallTree.GetComponent<SpriteRenderer>().enabled = true;
        smallTree.GetComponent<Collider2D>().enabled = true;
        smallTree.transform.localScale = new Vector3(0.1f, 0.1f, 1); // исходный размер дерева
    }
}
