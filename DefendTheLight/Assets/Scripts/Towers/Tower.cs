using UnityEngine;
using System.Collections;



// All towers inherit from this class
public abstract class Tower : MonoBehaviour
{
    float chargeTime = 0;

    [HideInInspector]
    public int chargeLevel = 0;
    [HideInInspector]
    public float speedBoost = 1;
    public int heath;
    public int cHealth;

    AbilityType color;

    public int CurrentHealth { get { return cHealth; } }

    public float range; // Distance from center that attacks or affects can reach.

    ChargeOverlay overlay;

    protected void Setup(AbilityType c)
    {
        cHealth = heath;
        color = c;
        overlay = transform.GetChild(0).GetComponent<ChargeOverlay>();
    }

    protected void TowerUpdate()
    {       
        if (chargeLevel > 0)
        {
            if (chargeTime <= 0)
            {
                overlay.SetCharge(color, 0);
                chargeLevel = 0;
            }
            else
                chargeTime -= Time.deltaTime;
        }
    }

    public void Damage(int amount)
    {
        cHealth -= amount;
        hpCheck();
    }
    public void Heal(int amount)
    {
        cHealth = Mathf.Clamp(cHealth + amount, 0, heath);
    }

    protected void hpCheck()
    {
        if(cHealth <= 0)
        {
            Destroy(this.gameObject);
            Game.grid.UpdateCostGrid();
        }
    }

    protected void GetTarget(ref Enemy target)
    {
        if (Game.enemies.Count > 0)
        {         
            // Get closest enemy in range.
            for (int i = 0; i < Game.enemies.Count; i++)
            {
                if (inRange(Game.enemies[i]))
                {
                    target = Game.enemies[i];
                    break;
                }
            }
        }
    }
    
    protected bool inRange(Enemy target)
    {
        if (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), new Vector2(target.transform.position.x, target.transform.position.y))
                        <= range * 0.64f + 0.32f) return true;
        return false;
    }

    /// <summary>
    /// Return false if maxed out, otherwise true. Use for spending charge.
    /// </summary>
    /// <returns></returns>
    public bool ChargeTower()
    {
        // Already maxed
        if (chargeLevel >= 3) return false;

        chargeLevel++;
        overlay.SetCharge(color, chargeLevel);
        if (chargeLevel == 1) chargeTime = 30;
        else chargeTime += 5;

        return true;
    }

    public void SellTower()
    {        
        Destroy(this.gameObject);
    }
    
}



