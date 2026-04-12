using UnityEngine;

public class PaperDragSpawner : MonoBehaviour
{
    [SerializeField] private DropZone dropZone;
    [SerializeField] private GameObject trashPrefab;

    public int numOfTrash = 4;

    private void Start()
    {
        for (int i = 0; i < numOfTrash; i++)
        {

            int x_rand = Random.Range(-800, 460);
            int y_rand = Random.Range(-210, 350);

            GameObject trash = Instantiate(trashPrefab, gameObject.transform);
            RectTransform rect = trash.GetComponent<RectTransform>();
            rect.localPosition = Vector3.zero;
            rect.localScale = Vector3.one;

            rect.anchoredPosition = new Vector2(x_rand, y_rand);

            trash.GetComponent<DraggablePaper>().Init(this, dropZone);
        }
    }
    public void PickupTrash()
    {
        numOfTrash--;
        if (numOfTrash <= 0)
        {
            // scrap reward
            MinigameSpawner.Instance.EndMinigame();
            Destroy(gameObject);
        }
    }
}
