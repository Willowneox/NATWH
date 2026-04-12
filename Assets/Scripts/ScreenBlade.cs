using UnityEngine;
using UnityEngine.InputSystem;
public class ScreenBlade : MonoBehaviour
{
    private Blade Bladeparent;
    private TrailRenderer bladeTrail;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Bladeparent = this.GetComponentInParent<Blade>();
        bladeTrail = GetComponentInChildren<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 pos = (Vector2)Bladeparent.mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        transform.position = pos;
        if(this.GetComponentInParent<Blade>().cutting){
            bladeTrail.enabled = true;
        }else{
            bladeTrail.enabled = false;
            bladeTrail.Clear();        
        }
    }
}
