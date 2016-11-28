using UnityEngine;
using System.Collections;

public class DefenseTower : Tower
{
    public int defense; // reduce incoming damage

	// Use this for initialization
	void Start ()
    {
        cHealth = heath;
    }
	
	// Update is called once per frame
	void Update ()
    {
        hpCheck();
	}
}
