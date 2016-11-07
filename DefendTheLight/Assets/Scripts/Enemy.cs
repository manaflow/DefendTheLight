using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    public int health;
    public float speed; // move
    public int attack;
    public float attackSpeed;
    float attackTime = 0;

    int i = 6;
    int j = 0;
    List<int> path;
    Vector3 myTransform;

    float slowDuration = 0;
    float slowAmount = 0;

    float curDistance = 0; // distance traveled so far to next tile
    Vector2 movementVector = Vector2.zero;

    public void Init(int tileY, int wave)
    {
        // tiles are 60pixels high
        i = tileY;
        // modifiy hp
        health = health * ((wave / 10) + 1);
        transform.position = new Vector3(0.3f, i * 0.6f + 0.3f, -2);
        path = Game.grid.GetPath(i, j);
        string str = "";
        for (int x = 0; x < path.Count; x++)
        {
            str += path[x] + ", ";
        }
        GetNextTile();
    }

    // Use this for initialization
    void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        float newDistance;

        if (slowDuration > 0)
        {

            slowDuration -= Time.deltaTime;
            newDistance = speed * 0.6f * slowAmount * Time.deltaTime * Game.GameSpeed;
        }
        else
        {
            slowAmount = 0;
            newDistance = speed * 0.6f * Time.deltaTime * Game.GameSpeed;
        }
        

        if(curDistance + newDistance > 0.6f)
        {
            Move(0.6f - curDistance); // move to 1.
            GetNextTile();          // get new tile and direction
            curDistance += newDistance;
            Move(curDistance - 0.6f);  // move remaining distance in new direction
            curDistance -= 0.6f;
        }
        else
        {
            curDistance += newDistance;
            Move(newDistance);
        }
       
        
	}

    void Move(float distance)
    {
        transform.position = transform.position + new Vector3(distance * movementVector.x, distance * movementVector.y, 0);
    }

    public void ApplySlow(float amount, float duration)
    {
        if (slowAmount <= amount)
        {
            slowDuration = duration;
            slowAmount = amount;
        }
    }

    void GetNextTile()
    {
        if (path.Count > 0)
        {
            int dir = path[0];
            path.RemoveAt(0);



            if (dir == 1) { movementVector = new Vector2(0, -1); transform.localEulerAngles = new Vector3(0, 0, -90); }
            else if (dir == 2) { movementVector = new Vector2(1, 0); transform.localEulerAngles = new Vector3(0, 0, 0); }
            else if (dir == 3) { movementVector = new Vector2(0, 1); transform.localEulerAngles = new Vector3(0, 0, 90); }
            else if (dir == 4) { movementVector = new Vector2(-1, 0); transform.localEulerAngles = new Vector3(0, 0, 180); }
            }
        else movementVector = Vector2.zero;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Game.enemies.Remove(this);
            Destroy(this.gameObject);
        }
    }
}
