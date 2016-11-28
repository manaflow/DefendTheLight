using UnityEngine;
using System.Collections;

public class GreenTower : Tower
{
    int i, j;    
    Tower north, east, south, west;

    // Use this for initialization
    void Start()
    {
        Setup(AbilityType.Green);

        // Get current position on grid
        Game.grid.GetPosition(this.gameObject, ref i, ref j);
    }

    // Update is called once per frame
    void Update()
    {
        TowerUpdate();

        // Get towers adj
        if (north == null) north = GetTower(i + 1, j);
        else
        if (east == null) east = GetTower(i, j + 1);
        if (south == null) south = GetTower(i - 1, j);
        if (west == null) west = GetTower(i, j - 1);

        // Boost all towers adj
        float boost = 1.5f + chargeLevel * 0.5f;
        if (north != null && north.speedBoost < boost)
        {
            north.speedBoost = boost * (1 + chargeLevel);
        }
        if (east != null && east.speedBoost < boost)
        {
            east.speedBoost = boost * (1 + chargeLevel);
        }
        if (south != null && south.speedBoost < boost)
        {
            south.speedBoost = boost * (1 + chargeLevel);
        }
        if (west != null && west.speedBoost < boost)
        {
            west.speedBoost = boost * (1 + chargeLevel);
        }


    }

    Tower GetTower(int row, int col)
    {
        GameObject obj = Game.grid.CheckGrid(row, col);
        if (obj != null) return obj.GetComponent<Tower>();
        return null;
    }
}

