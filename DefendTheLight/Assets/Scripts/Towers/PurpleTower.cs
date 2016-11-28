using UnityEngine;
using System.Collections;

public class PurpleTower : Tower
{
    Enemy target;
    GameObject beam;
    SpriteRenderer beamRender;

    public int damage;
    public float beamSpeed;
    float time = 0;

    // Use this for initialization
    void Start ()
    {
        Setup(AbilityType.Purple);

        beam = transform.GetChild(1).gameObject;
        beamRender = beam.GetComponent<SpriteRenderer>();
	}
    void OnGUI()
    {
        
        /*
        GL.Begin(GL.LINES);
        GL.Color(Color.blue);
        GL.Vertex(transform.position);
        GL.Vertex(transform.position + new Vector3(0, 1, 0));
        GL.End();
        GL.PopMatrix();
        */
    }

    // Update is called once per frame
    void Update ()
    {
        TowerUpdate();
        hpCheck();

        if (Game.TargetEnemy != null)
        {
            if (inRange(Game.TargetEnemy)) { target = Game.TargetEnemy; }
        }


        // No target, search for a target
        if (target == null)
        {
            beamRender.enabled = false;
            GetTarget(ref target);
        }
        else if(Vector2.Distance(transform.position, target.transform.position) > range)
        {
            beamRender.enabled = false;
            GetTarget(ref target);
        }
        else
        {
            beamRender.enabled = true;
            float distance = Vector2.Distance(transform.position, target.transform.position);
            beam.transform.localScale = new Vector3(distance * -25, 1, 1);

            // Create a vector on the x,z plane. To map the rotation treat z as x, x as y
            Vector2 vect = new Vector2(target.transform.position.x - transform.position.x, target.transform.position.y - transform.position.y).normalized;

            float rotate = Mathf.Sign(vect.y) * Mathf.Acos(vect.x) * 180 / Mathf.PI;
            beam.transform.localEulerAngles = new Vector3(0, 0, rotate);

            // Damage
            if (time <= 0)
            {
                target.TakeDamage(damage * (chargeLevel + 1));
                time += beamSpeed;
            }
            else time -= Time.deltaTime * Game.GameSpeed * speedBoost;
        }

        speedBoost = 1;
    }
}
