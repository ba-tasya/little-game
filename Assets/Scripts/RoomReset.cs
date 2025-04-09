using UnityEngine;

public class RoomReset : MonoBehaviour
{
    public static RoomReset Instance;

    public GameObject jar;
    public GameObject jarOnFloor;
    public SmallTeapot teapot;
    public BoilingTeapot kettle;
    public GameObject lid;
    public GameObject burnedLeaf;
    public GameObject livingLeaf;
    public SpriteRenderer plantRenderer;
    public Sprite fullPlantSprite;

    void Awake()
    {
        Instance = this;
    }

    public void ResetRoom()
    {
        jar.SetActive(true);
        jarOnFloor.SetActive(false);
        teapot.ResetTeapot();
        kettle.ResetTeapot();
        lid.SetActive(true);
        burnedLeaf.SetActive(false);
        livingLeaf.SetActive(false);
        plantRenderer.sprite = fullPlantSprite;
    }
}
