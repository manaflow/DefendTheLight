using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fireball : MonoBehaviour
{
    public LayerMask enemyMask;
    Vector2 direction;
    public int damage;
    public float speed;
    List<GameObject> hitList; // which enemies have been hit already, no repeat damage.

    float boxX;
    float boxY;

    public void Init(Enemy target, float multipler)
    {
        // Take a target location and set movement vector.
        direction = new Vector2(target.transform.position.x - transform.position.x, target.transform.position.y - transform.position.y);
        direction.Normalize();
        damage = (int)(damage * multipler);
    }
	// Use this for initialization
	void Start ()
    {
        hitList = new List<GameObject>();
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        boxX = box.size.x;
        boxY = box.size.y;
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Check if out of bounds, if yes destory
        if(Game.OutOfBounds(transform.position))
        {
            Destroy(this.gameObject);
        }

        // Move
        Vector3 myTransform = transform.position;
        myTransform.x += direction.x * speed * Game.Speed;
        myTransform.y += direction.y * speed * Game.Speed;
        transform.position = myTransform;

        
        CheckCollision();
        
	}

    void CheckCollision()
    {
        // Create a vertical and horizontal line thru the object for collision
        // horz
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x - (boxX / 2), transform.position.y), Vector2.right, boxX, enemyMask);
        if (hit.collider != null)
        {
            for(int i = 0; i < hitList.Count; i++)
            {
                // If already on the list 
                if (hit.collider.gameObject == hitList[i]) return; 
            }
            hitList.Add(hit.collider.gameObject);
            hit.collider.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            Debug.Log("Hit");
            return;
        }
        // vert
        hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - (boxY / 2)), Vector2.up, boxY, enemyMask);
        if(hit.collider != null)
        {
            for(int i = 0; i < hitList.Count; i++)
            {
                // If already on the list 
                if (hit.collider.gameObject == hitList[i])  return; 
            }
            hitList.Add(hit.collider.gameObject);
            hit.collider.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            return;
        }
    }
}
