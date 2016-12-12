using UnityEngine;
using System.Collections;

public class YellowTower : Tower
{
    public float duration; // how long
    public float amount; // the amount of effect    

    public float speed = 0.5f; // how often applied.
    float sTimer = 0;

    SpriteRenderer slowAnimation;

    // Use this for initialization
    void Start ()
    {
        Setup(AbilityType.Yellow);
        slowAnimation = transform.GetChild(1).GetComponent<SpriteRenderer>();
        slowAnimation.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        TowerUpdate();   
        if (sTimer <= 0)
        {
            sTimer += speed;

            bool canRun = false;
            for (int i = 0; i < Game.enemies.Count; i++)
            {

                if (inRange(Game.enemies[i]))
                {
                    canRun = true; // play animation so long as one enemies is in range
                    // normal 50 % slow : charge 1-3 = 40%, 30%, 20%
                    Game.enemies[i].ApplySlow(amount - (chargeLevel * 0.1f), duration * speedBoost);                    
                }
                
            }

            if(canRun) slowAnimation.enabled = true;
            else slowAnimation.enabled = false;
        }
        else sTimer -= Time.deltaTime * Game.GameSpeed * speedBoost;

        speedBoost = 1;
    }
}
