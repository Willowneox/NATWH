using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public Sprite[] availableSprites;
    private SpriteRenderer spriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = availableSprites[Random.Range(0, availableSprites.Length)];
        int dir = Random.Range(1,3);
        if(dir == 2){
            spriteRenderer.flipX = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D player){
        if(player.gameObject.CompareTag("Player")){
            player.gameObject.GetComponent<Player>().dmg(Random.Range(1.5f, 2.5f));
        }
    }
}
