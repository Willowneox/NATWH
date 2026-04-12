using System.Collections.Generic;
using UnityEngine;

public class Props : Obstacle
{
    PolygonCollider2D polygonCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = availableSprites[Random.Range(0, availableSprites.Length)];
        polygonCollider = GetComponent<PolygonCollider2D>();

        int dir = Random.Range(1, 3);
        if (dir == 2)
        {
            spriteRenderer.flipX = true;
        }

        SyncCollider();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter2D(Collision2D player)
    {
        // don't damage the player
        return;    
    }

    // Makes the collider match the sprite shape
    private void SyncCollider()
    {
        polygonCollider.pathCount = spriteRenderer.sprite.GetPhysicsShapeCount();
        List<Vector2> path = new List<Vector2>();

        for (int i = 0; i < polygonCollider.pathCount; i++)
        {
            path.Clear();
            spriteRenderer.sprite.GetPhysicsShape(i, path);
            polygonCollider.SetPath(i, path.ToArray());
        }
    }
}
