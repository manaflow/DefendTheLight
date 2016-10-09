using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    public Enemy target;
    public int damage;
    public int speed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (target == null) Destroy(this.gameObject);
        else
        {
            Vector3 myTransform = transform.position;
            myTransform = Vector3.MoveTowards(myTransform, target.transform.position, speed * Time.deltaTime);
            if (myTransform == target.transform.position)
            {
                target.TakeDamage(damage);
                Destroy(this.gameObject);
            }
            transform.position = myTransform;
        }
	}
}
