using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    public int chargeValue = 1; // value for defeating
    public int health;
    public float speed; // move
    public int attack;
    public float attackSpeed;
    float attackTime = 0;
    Facing facing = Facing.Right; // start facing right

    bool attackState = false;
    Tower target = null;

    int i = 6;
    int j = 0;
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
              
        GetNextTile();
    }

    // Use this for initialization
    void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (i == 6 && j == 31)
        {
            // Reached goal
            Game.Health--;
            Destroy(this.gameObject);
            return;
        }


        // Normal state of not attacking, move toward next node
        if (!attackState)
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


            if (curDistance + newDistance > 0.6f)
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
        else // attack state.
        {
            if(target == null)
            {                
                GetNextTile();
                attackState = false;
                return;
            }
            if (attackTime <= 0)
            {
                target.Damage(attack);
                attackTime += attackSpeed;
            }
            else attackTime -= Time.deltaTime;
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
        int dir = Game.grid.GetNextPath(facing,i, j, ref target);

        if(dir < 0) movementVector = Vector2.zero;
        else if( dir == 5)
        {
            movementVector = Vector2.zero;
            attackState = true;
        }
        else if (dir == (int)Facing.Down) { i--; facing = Facing.Down; movementVector = new Vector2(0, -1); transform.localEulerAngles = new Vector3(0, 0, -90); }
        else if (dir == (int)Facing.Right) { j++; facing = Facing.Right; movementVector = new Vector2(1, 0); transform.localEulerAngles = new Vector3(0, 0, 0); }
        else if (dir == (int)Facing.Up) { i++; facing = Facing.Up; movementVector = new Vector2(0, 1); transform.localEulerAngles = new Vector3(0, 0, 90); }
        else if (dir == (int)Facing.Left) { j--; facing = Facing.Left; movementVector = new Vector2(-1, 0); transform.localEulerAngles = new Vector3(0, 0, 180); }
        
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

    void OnDestroy()
    {
        Game.chargeCounter += chargeValue;
    }
}
