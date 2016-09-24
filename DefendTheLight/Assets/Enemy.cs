﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    public int health;
    public float speed;

    int i = 6;
    int j = 0;
    List<int> path;
    Vector3 myTransform;


    float curDistance = 0; // distance traveled so far to next tile
    Vector2 movementVector = Vector2.zero;


    // Use this for initialization
    void Start ()
    {
        transform.position = new Vector3(0.32f, 4.8f, -2);
        path = Game.grid.GetPath(i, j);
        string str = "";
        for(int x = 0; x < path.Count; x++)
        {
            str += path[x] + ", ";
        }
        GetNextTile();
	}
	
	// Update is called once per frame
	void Update ()
    {
        float newDistance = speed * 0.64f * Time.deltaTime * Game.GameSpeed;
        

        if(curDistance + newDistance > 0.64f)
        {
            Move(0.64f - curDistance); // move to 1.
            GetNextTile();          // get new tile and direction
            curDistance += newDistance;
            Move(curDistance - 0.64f);  // move remaining distance in new direction
            curDistance -= 0.64f;
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

    void GetNextTile()
    {
        if (path.Count > 0)
        {
            int dir = path[0];
            path.RemoveAt(0);

            if (dir == 1) movementVector = new Vector2(0, -1);
            else if (dir == 2) movementVector = new Vector2(1, 0);
            else if (dir == 3) movementVector = new Vector2(0, 1);
            else if (dir == 4) movementVector = new Vector2(-1, 0);
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