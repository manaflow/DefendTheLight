using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlueTower : Tower
{
    int i, j;
    public int healAmount;
    public float healTime;
    float time;

    SpriteRenderer healObject;
    SpriteRenderer healBeam;

    Tower north, east, south, west;

	// Use this for initialization
	void Start ()
    {
        Setup(AbilityType.Blue);

        // Get current position on grid
        Game.grid.GetPosition(this.gameObject,ref i,ref j);

        healObject = transform.GetChild(1).GetComponent<SpriteRenderer>();
        healBeam = transform.GetChild(2).GetComponent<SpriteRenderer>();

        healBeam.enabled = false;
                healObject.enabled = false;
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
                healBeam.transform.localEulerAngles = new Vector3(0, 0, 0); // point beam tower object
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
            if(tower != null && tower.CurrentHealth < tower.heath)
            {
                // Point beam toward object
                if(tower == east) healBeam.transform.localEulerAngles = new Vector3(0, 0, -90); 
                else if (tower == west) healBeam.transform.localEulerAngles = new Vector3(0, 0, 90);
                else if (tower == north) healBeam.transform.localEulerAngles = new Vector3(0, 0, 0);
                else if (tower == south) healBeam.transform.localEulerAngles = new Vector3(0, 0, 180);
                healBeam.enabled = true;
                healObject.enabled = true;
                healObject.transform.position = new Vector3(tower.transform.position.x, tower.transform.position.y, -5);
                tower.Heal(healAmount * (1 + chargeLevel));
                time += healTime;
            }
            else
            {
                healBeam.enabled = false;
                healObject.enabled = false;
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
