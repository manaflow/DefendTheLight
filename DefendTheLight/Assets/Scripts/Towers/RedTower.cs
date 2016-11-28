using UnityEngine;
using System.Collections;

public class RedTower : Tower
{
    public GameObject fireball;
    public float speed;
    float time;
    Enemy target;

	// Use this for initialization
	void Start ()
    {
        Setup(AbilityType.Red);
    }
	
	// Update is called once per frame
	void Update ()
    {
        TowerUpdate();

        if (Game.TargetEnemy != null)
        {
            if (inRange(Game.TargetEnemy)) { target = Game.TargetEnemy; return; }
        }

        if (target == null)
        {
            GetTarget(ref target);
        }
        else
        {
            Debug.Log(speedBoost);
            if (time <= 0)
            {
                Fireball fb = GameObject.Instantiate(fireball).GetComponent<Fireball>();
                fb.transform.position = transform.position;
                fb.Init(target, 1 + chargeLevel);
                time += speed;
            }
            else time -= Time.deltaTime * Game.GameSpeed * speedBoost;
        }

        speedBoost = 1;
    }
}
