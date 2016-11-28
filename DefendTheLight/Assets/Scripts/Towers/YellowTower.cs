using UnityEngine;
using System.Collections;

public class YellowTower : Tower
{
    public GameObject SFX_Object;
       
    public float duration; // how long
    public float amount; // the amount of effect    

    public float speed = 0.5f; // how often applied.
    float sTimer = 0;

    // Use this for initialization
    void Start ()
    {
        Setup(AbilityType.Yellow);
    }

    // Update is called once per frame
    void Update()
    {
        TowerUpdate();   
        if (sTimer <= 0)
        {
            sTimer += speed;


            for (int i = 0; i < Game.enemies.Count; i++)
            {
                bool startEffect = false;


                if (inRange(Game.enemies[i]))
                {
                    // Create only 1 effect, and only if atleast 1 enemy is in range.
                    if (!startEffect)
                    {
                        GameObject sfx = GameObject.Instantiate(SFX_Object);
                        sfx.transform.position = new Vector3(transform.position.x, transform.position.y, -5);
                    }
                    // normal 50 % slow : charge 1-3 = 40%, 30%, 20%
                    Game.enemies[i].ApplySlow(amount - (chargeLevel * 0.1f), duration * speedBoost);
                    
                }
            }
        }
        else sTimer -= Time.deltaTime * Game.GameSpeed * speedBoost;

        speedBoost = 1;
    }
}
