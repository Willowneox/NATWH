using UnityEngine;
using UnityEngine.InputSystem;

public class Blade : MonoBehaviour
{
    public bool cutting;
    [SerializeField] public Camera mainCamera;
    [SerializeField] private Collider2D bladeCollider;
    //public float minVelo = 0.001f;
    private Vector2 direction;
    //private TrailRenderer bladeTrail;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        mainCamera = Camera.main;
        bladeCollider = GetComponent<Collider2D>();
        //bladeTrail = GetComponentInChildren<TrailRenderer>();
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
        if(Mouse.current.leftButton.wasPressedThisFrame){
            StartCutting();
        }else if (!Mouse.current.leftButton.isPressed){
            StopCutting();
        }else if (cutting){
            ContCutting();
        }
    }

    private void StartCutting(){
        Vector2 newPosition = (Vector2)Mouse.current.position.ReadValue();
        transform.position = newPosition;

        cutting = true;
        bladeCollider.enabled = true;
        // bladeTrail.enabled = true;
        // bladeTrail.Clear(); 
    }

    private void StopCutting(){
        cutting = false;
        bladeCollider.enabled = false;
        // bladeTrail.enabled = false;
    }

    private void ContCutting(){
        Vector2 newPosition = (Vector2)Mouse.current.position.ReadValue();
        direction = newPosition - (Vector2)transform.position;

        float velo = direction.magnitude / Time.deltaTime;
        //bladeCollider.enabled = velo > minVelo;
        
        transform.position = newPosition;
    }
}
