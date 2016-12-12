using UnityEngine;
using System.Collections;

public class AreaTower : Tower
{
    public GameObject SFX_Object;
    public Effect effect;
    public float duration; // how long
    public float amount; // the amount of effect

    public int damage; // how much damage if any

    public float speed = 0.5f; // how often applied. Probably leave this alone if not damage.
    float sTimer = 0;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        sTimer += Time.deltaTime;
        if (sTimer > speed)
        {
            sTimer -= speed;
            

            for (int i = 0; i < Game.enemies.Count; i++)
            {
                bool startEffect = false;
                

                if (inRange(Game.enemies[i]))
                {
                    // Create only 1 effect, and only if atleast 1 enemy is in range.
                    if(!startEffect)
                    {
                        GameObject sfx = GameObject.Instantiate(SFX_Object);
                        sfx.transform.position = new Vector3(transform.position.x, transform.position.y, -5);
                    }

                    if (effect == Effect.None)
                    {
                        Game.enemies[i].TakeDamage(damage);
                    }
                    else if (effect == Effect.SlowMovement)
                    {
                        Game.enemies[i].ApplySlow(amount, duration);
                    }
                }
            }
        }
	}
}
