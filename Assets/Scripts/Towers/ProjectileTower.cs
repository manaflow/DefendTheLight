using UnityEngine;
using System.Collections;

public class ProjectileTower : Tower {

    public GameObject projectile;   // the projectile to fire 
    public GameObject turret;       // optional, a turret obj that turns to face target
   

    public float attackSpeed;             // how often projectile fires. value of 1 = 1 second
    float attackTime = 0;
    Enemy target;

	// Use this for initialization
	void Start ()
    {
	    
	}

    // Update is called once per frame
    void Update()
    {
        if (projectile == null)
        {
            Debug.Log("Lost");
            projectile = Game.control.projectilePref; // having a weird issue where prefab is disappearing. Temp solution
            
        }

        

        hpCheck();

        // No target, search for a target
        if (target == null)
        {
            if (Game.enemies.Count > 0)
            {
                // Get closest enemy in range.
                for (int i = 0; i < Game.enemies.Count; i++)
                {
                    if(inRange(Game.enemies[i]))
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
            if(inRange(target))
            {
                // Rotate turret if there is one
                if (turret != null && target != null)
                {
                    // Create a vector on the x,z plane. To map the rotation treat z as x, x as y
                    Vector2 vect = new Vector2(target.transform.position.x - transform.position.x, target.transform.position.y - transform.position.y).normalized;

                    float rotate = Mathf.Sign(vect.y) * Mathf.Acos(vect.x) * 180 / Mathf.PI;
                    turret.transform.localEulerAngles = new Vector3(0, 0, rotate);

                    if (rotate < 0) Debug.Log(360 + rotate);
                    else
                    Debug.Log(rotate);
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
                shot.transform.localEulerAngles = new Vector3(0, 0, rotate);
                shot.target = target;

                // Ajust timer
                attackTime += attackSpeed;
            }
            else attackTime = 0;
        }
        
        // End Update
    }
}
