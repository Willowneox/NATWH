using UnityEngine;

public class PlantMiniGame : MonoBehaviour
{
    int numOfPlants = Random.Range(4,6);
    void Start()
    {
        for(int i = 0; i < numOfPlants; i++){
            int x_rand = Random.Range(-960,960);
            int y_rand = Random.Range(-540,540);

            GameObject plant = Instantiate(plantPrefab, gameObject.transform);
            RectTransform rect = plant.GetComponent<RectTransform>();
            rect.localPosition = Vector3.zero;
            rect.localScale = Vector3.one;

            rect.anchoredPosition = new Vector2(x_rand, y_rand);

            plant.GetComponent<Plant>().Init(this);
        }
    }
    public void CutPlant(GameObject Plant)
    {
        numOfPlants--;
        Destroy(Plant);
        if(numOfPlants <=0)
        {
            // scrap reward
            // MinigameSpawner.Instance.EndMinigame();
            Destroy(gameObject);
        }
    }
    private void OnEnable(){

    }
    private void OnDisable(){
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}