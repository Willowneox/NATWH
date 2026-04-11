using UnityEngine;

public class Plant : MonoBehaviour
{   
    private Collider PlantCollider;
    private PlantMiniGame spawner;
    private RectTransform rt;
    private Canvas canvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rt = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(PlantMiniGame spawner)
    {
        this.spawner = spawner;
    }

    private void OnTriggerEnter(Collider o){
        if(o.CompareTag("Blade")){
            spawner.CutPlant(gameObject);
        }
    }
}
