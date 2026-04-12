using UnityEngine;
using UnityEngine.UI;
public class GAMBLING : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject lvr;
    void Start()
    {
        lvr.GetComponent<Button>().onClick.AddListener(() => spitOut());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void spitOut(){
        //if(scrap >= 5){
        //  int roll = Random.Range(0, 100);
            // if(roll > 95){
            //     //give 30
            // }else if(roll > 70){
            //     //give 15
            // }else if(roll > 50){
            //     //give 7
            // }else if(roll > 30){
            //     //give 4
            // }else{
            //     //give 2
            // }
        //}
    }
}
