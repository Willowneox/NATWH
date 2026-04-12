using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TrashClickSpawner : MonoBehaviour
{
    public GameObject trashPrefab;

    public int numOfTrash = 4;
    private void Start()
    {
        for (int i = 0; i < numOfTrash; i++)
        {
            int x_rand = Random.Range(-960,960);
            int y_rand = Random.Range(-540,540);

            GameObject btn = Instantiate(trashPrefab, gameObject.transform);
            RectTransform rect = btn.GetComponent<RectTransform>();
            rect.localPosition = Vector3.zero;
            rect.localScale = Vector3.one;

            rect.anchoredPosition = new Vector2 (x_rand, y_rand);

            btn.GetComponent<Button>().onClick.AddListener(() => PickupTrash(btn));
        }
    }

    public void PickupTrash(GameObject trash)
    {
        numOfTrash--;
        Destroy(trash);
        if(numOfTrash <=0)
        {
            MinigameSpawner.Instance.EndMinigame();
            Destroy(gameObject);
        }
    }
}
