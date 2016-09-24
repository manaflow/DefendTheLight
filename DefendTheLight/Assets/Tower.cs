using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tower : MonoBehaviour
{
    public float attackSpeed;
    float attackTime = 0;
    public float attackDamage;
    public float attackRange;
    // effects


    public GameObject turret;
    public GameObject projectile;
    Enemy target = null;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        // No target, search for a target
        if (target == null)
        {
            if (Game.enemies.Count > 0)
            {
                // Get closest enemy in range.
                for (int i = 0; i < Game.enemies.Count; i++)
                {
                    if (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), new Vector2(Game.enemies[i].transform.position.x, Game.enemies[i].transform.position.y))
                        <= attackRange * 0.64f + 0.32f)
                    {
                        target = Game.enemies[i];
                        break;
                    }
                }
            }
        }
        else
        {
            // Check if target is in range.
            if (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), new Vector2(target.transform.position.x, target.transform.position.y))
                        <= attackRange * 0.64f + 0.32f)
            {
                // Rotate turret if there is one
                if (turret != null && target != null)
                {
                    // Create a vector on the x,z plane. To map the rotation treat z as x, x as y
                    Vector2 vect = new Vector2(target.transform.position.x - transform.position.x, target.transform.position.y - transform.position.y).normalized;

                    float rotate = Mathf.Sign(vect.y) * Mathf.Acos(vect.x) * 180 / Mathf.PI;
                    turret.transform.localEulerAngles = new Vector3(0, 0, rotate - 90);
                }


            }
            else
            {
                // No target in range
                target = null;
            }
        }        

        // Handle attack 
        if (attackTime > 0) attackTime -= Time.deltaTime;
        else
        {
            if (target != null)
            {
                Vector2 vect = new Vector2(target.transform.position.x - transform.position.x, target.transform.position.y - transform.position.y).normalized;

                float rotate = Mathf.Sign(vect.y) * Mathf.Acos(vect.x) * 180 / Mathf.PI;

                Projectile shot = GameObject.Instantiate(projectile).GetComponent<Projectile>();
                shot.transform.position = new Vector3(transform.position.x, transform.position.y, -2);
                shot.transform.localEulerAngles = new Vector3(0, 0, rotate - 90);
                shot.target = target;

                // Ajust timer
                attackTime += attackSpeed;
            }
            else attackTime = 0;
        }

        



        // End Update
    }
}
