using UnityEngine;
using System.Collections;

public class GreenTower : Tower
{
    public GameObject boostPref;
    SpriteRenderer northSR, eastSR, southSR, westSR;
    int i, j;    
    Tower north, east, south, west;

    // Use this for initialization
    void Start()
    {
        Setup(AbilityType.Green);

        // Get current position on grid
        Game.grid.GetPosition(this.gameObject, ref i, ref j);

        // Boost animations

        northSR = GameObject.Instantiate(boostPref).GetComponent<SpriteRenderer>();
        northSR.transform.position = transform.position + new Vector3(0, 0.6f, -2);

        eastSR = GameObject.Instantiate(boostPref).GetComponent<SpriteRenderer>();
        eastSR.transform.position = transform.position + new Vector3(0.6f, 0, -2);

        southSR = GameObject.Instantiate(boostPref).GetComponent<SpriteRenderer>();
        southSR.transform.position = transform.position + new Vector3(0, -0.6f, -2);

        westSR = GameObject.Instantiate(boostPref).GetComponent<SpriteRenderer>();
        westSR.transform.position = transform.position + new Vector3(-0.6f, 0, -2);
    }

    // Update is called once per frame
    void Update()
    {
        TowerUpdate();

        northSR.enabled = false;
        eastSR.enabled = false;
        southSR.enabled = false;
        westSR.enabled = false;

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
            northSR.enabled = true;
        }
        if (east != null && east.speedBoost < boost)
        {
            east.speedBoost = boost * (1 + chargeLevel);
            eastSR.enabled = true;
        }
        if (south != null && south.speedBoost < boost)
        {
            south.speedBoost = boost * (1 + chargeLevel);
            southSR.enabled = true;
        }
        if (west != null && west.speedBoost < boost)            
        {
            west.speedBoost = boost * (1 + chargeLevel);
            westSR.enabled = true;
        }


    }

    Tower GetTower(int row, int col)
    {
        GameObject obj = Game.grid.CheckGrid(row, col);
        if (obj != null) return obj.GetComponent<Tower>();
        return null;
    }

    void OnDestroy()
    {

    }
}

