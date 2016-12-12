using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LightObject : MonoBehaviour
{
    float blueTimer = 30;
    float blueTimer2;
    GameObject healObject;

    SpriteRenderer sr;
    public Sprite full;
    public Sprite mid;
    public Sprite low;
    public Sprite dead;

    public GameObject redAbility;
    public GameObject blueAbility;
    public GameObject yellowAbility;
    public GameObject purpleAbility;
    public GameObject orangeAbility;
    public GameObject greenAbility;


    // Use this for initialization
    void Start ()
    {
        sr = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(blueTimer >= 0)
        {
            if(blueTimer2 <= 0)
            {
                List<Tower> towers = Game.grid.GetAllTowers();
                for(int i = 0; i < towers.Count; i++)
                {
                    towers[i].Heal(1);
                }
                blueTimer2 = 1;
            }
            else
            {
                blueTimer2 -= Time.deltaTime;
            }
            blueTimer -= Time.deltaTime;
        }
        else if(healObject != null)
        {
            Destroy(healObject);
        }

        if (Game.Health >= 15) sr.sprite = full;
        else if (Game.Health >= 10) sr.sprite = mid;
        else if (Game.Health > 0) sr.sprite = low;
        else sr.sprite = dead;
    }

    public void StartRed()
    {
        // 5 random fireballs.
        if (Game.enemies.Count == 0) return;

        Enemy e1 = Game.enemies[Random.Range(0, Game.enemies.Count)];
        Enemy e2 = Game.enemies[Random.Range(0, Game.enemies.Count)];
        Enemy e3 = Game.enemies[Random.Range(0, Game.enemies.Count)];
        Enemy e4 = Game.enemies[Random.Range(0, Game.enemies.Count)];
        Enemy e5 = Game.enemies[Random.Range(0, Game.enemies.Count)];

        GameObject fireball = GameObject.Instantiate(redAbility);
        fireball.GetComponent<Fireball>().Init(e1, 1);
        fireball.transform.position = transform.position;

        fireball = GameObject.Instantiate(redAbility);
        fireball.GetComponent<Fireball>().Init(e2, 1);
        fireball.transform.position = transform.position;

        fireball = GameObject.Instantiate(redAbility);
        fireball.GetComponent<Fireball>().Init(e3, 1);
        fireball.transform.position = transform.position;

        fireball = GameObject.Instantiate(redAbility);
        fireball.GetComponent<Fireball>().Init(e4, 1);
        fireball.transform.position = transform.position;

        fireball = GameObject.Instantiate(redAbility);
        fireball.GetComponent<Fireball>().Init(e5, 1);
        fireball.transform.position = transform.position;

    }
    public void StartBlue()
    {
        if (healObject == null) healObject = blueAbility;
        blueTimer = 30;
        blueTimer2 = 1;
    }
    public void StartYellow()
    {
        for(int i = 0; i < Game.enemies.Count; i++)
        {
            Game.enemies[i].ApplySlow(0.5f, 30);
        }
    }
    public void StartPurple()
    {
        if (Game.enemies.Count == 0) return;
        Enemy highest = Game.enemies[0];
        for (int i = 0; i < Game.enemies.Count; i++)
        {
            if (Game.enemies[i].health > highest.health) highest = Game.enemies[i];
        }
        highest.TakeDamage(100);
    }
    public void StartOrange()
    {
        for (int i = 0; i < Game.enemies.Count; i++)
        {
            Game.enemies[i].TakeDamage(10);
        }
    }
    public void StartGreen()
    {
        List<Tower> towers = Game.grid.GetAllTowers();
        for (int i = 0; i < towers.Count; i++)
        {
            towers[i].speedBoost = 2;
        }
    }
}
