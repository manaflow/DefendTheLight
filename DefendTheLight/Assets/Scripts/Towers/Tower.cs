using UnityEngine;
using System.Collections;

// All towers inherit from this class
public abstract class Tower : MonoBehaviour
{
    public int ward; // Used to determine weight for enemy movement
    public int heath;

    public float range; // Distance from center that attacks or affects can reach.


    protected void hpCheck()
    {
        if(heath <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    
    protected bool inRange(Enemy target)
    {
        if (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), new Vector2(target.transform.position.x, target.transform.position.y))
                        <= range * 0.64f + 0.32f) return true;
        return false;
    }
}
