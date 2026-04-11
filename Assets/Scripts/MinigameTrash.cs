using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MinigameTrash : MonoBehaviour
{
    public Button trashPrefab;

    public int numOfTrash = 4;
    private void Start()
    {
        for (int i = 0; i < numOfTrash; i++)
        {
            int x_rand = Random.Range(0, 1920);
            int y_rand = Random.Range(0, 1080);

            Instantiate(trashPrefab, new Vector3(x_rand, y_rand, 0), Quaternion.identity);
        }
    }

    public void PickupTrash(GameObject trash)
    {
        numOfTrash--;
        Destroy(trash);
        if(numOfTrash <=0)
        {
            // scrap reward
            Destroy(gameObject);
        }
    }
}
