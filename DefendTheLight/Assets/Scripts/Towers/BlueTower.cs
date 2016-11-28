using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlueTower : Tower
{
    int i, j;
    public int healAmount;
    public float healTime;
    float time;

    Tower north, east, south, west;

	// Use this for initialization
	void Start ()
    {
        Setup(AbilityType.Blue);

        // Get current position on grid
        Game.grid.GetPosition(this.gameObject,ref i,ref j);
	}
	
	// Update is called once per frame
	void Update ()
    {
        TowerUpdate();

        // Time elapsed, heal
	    if(time <= 0)
        {
            // Get towers adj
            if (north == null) north = GetTower(i + 1, j);
            if (east == null) east = GetTower(i, j + 1);
            if (south == null) south = GetTower(i - 1, j);
            if (west == null) west = GetTower(i, j - 1);

            // Get the tower with lowest health
            Tower tower = null; 
            if (north != null)
            {
                tower = north;               
            }
            if (east != null)
            {
                if (tower == null) tower = east;
                else if (tower.CurrentHealth > east.CurrentHealth) tower = east;
            }
            if (south != null)
            {
                if (tower == null) tower = south;
                else if (tower.CurrentHealth > south.CurrentHealth) tower = south;
            }
            if (west != null)
            {
                if (tower == null) tower = west;
                else if (tower.CurrentHealth > west.CurrentHealth) tower = west;
            }

            // Heal the tower, then reset timer
            if(tower != null)
            {
                tower.Heal(healAmount * (1 + chargeLevel));
                time += healTime;
            }
           


        }
        else
        {
            time -= Time.deltaTime * Game.GameSpeed * speedBoost;
        }

        speedBoost = 1;
	}

    Tower GetTower(int row, int col)
    {
        GameObject obj = Game.grid.CheckGrid(row, col);
        if (obj != null) return obj.GetComponent<Tower>();
        return null;
    }
}
