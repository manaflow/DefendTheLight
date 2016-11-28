using UnityEngine;
using System.Collections;

public class OrangeTower : Tower
{
    public LayerMask enemyMask;
    public GameObject effect; // the animation for attack
    public float speed;
    public int damage;
    float time = 0;
	// Use this for initialization
	void Start ()
    {
        Setup(AbilityType.Orange);
    }
	
	// Update is called once per frame
	void Update ()
    {
        TowerUpdate();

        if (time <= 0)
        {
            RaycastHit2D[] hit = Physics2D.CircleCastAll(transform.position, range, Vector2.right, 0, enemyMask);
            for(int i = 0; i < hit.Length; i++)
            {
                hit[i].collider.gameObject.GetComponent<Enemy>().TakeDamage(damage * (1 + chargeLevel));
                GameObject obj = GameObject.Instantiate(effect);
                obj.transform.position = transform.position;
                obj.transform.localEulerAngles = new Vector3(2, 2, 1);
            }
            time += speed;
        }
        else time -= Time.deltaTime * Game.GameSpeed * speedBoost;

        speedBoost = 1;
    }

    
}
