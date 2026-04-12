using UnityEngine;

public class PlantMiniGame : MonoBehaviour
{   
    public GameObject prefab;
    private int numOfPlants;
    void Awake()
    {
        numOfPlants = Random.Range(4,6);
        for(int i = 0; i < numOfPlants; i++){
            int x_rand = Random.Range(-500,500);
            int y_rand = Random.Range(-500,-100);

            GameObject plant = Instantiate(prefab, gameObject.transform);
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
            MinigameSpawner.Instance.EndMinigame();
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