using UnityEngine;
using UnityEngine.InputSystem;

public class Blade : MonoBehaviour
{
    private bool cutting;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Collider bladeCollider;
    public float minVelo = 0.1f;
    private Vector2 direction;

    private TrailRenderer bladeTrail;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        mainCamera = Camera.main;
        bladeCollider = GetComponent<Collider>();
        bladeTrail = GetComponentInChildren<TrailRenderer>();
    }
    private void OnEnable(){
        StopCutting();
    }
    private void OnDisable(){
        StopCutting();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            StartCutting();
        }else if (Input.GetMouseButtonUp(0)){
            StopCutting();
        }else if (cutting){
            ContCutting();
        }
    }

    private void StartCutting(){
        Vector2 newPosition = (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition);
        transform.position = newPosition;

        cutting = true;
        bladeCollider.enabled = true;
        bladeTrail.enabled = true;
        bladeTrail.Clear();
    }

    private void StopCutting(){
        cutting = false;
        bladeCollider.enabled = true;
    }

    private void ContCutting(){
        Vector2 newPosition = (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition);
        direction = newPosition - (Vector2)transform.position;

        float velo = direction.magnitude / Time.deltaTime;
        bladeCollider.enabled = velo > minVelo;
        
        transform.position = newPosition;
    }
}
