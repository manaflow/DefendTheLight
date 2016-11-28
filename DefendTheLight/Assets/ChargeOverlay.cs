using UnityEngine;
using System.Collections;

public class ChargeOverlay : MonoBehaviour
{
    public Sprite[] sprites;
    public AbilityType color;
    int level;
    SpriteRenderer sr;

	// Use this for initialization
	void Start ()
    {
        sr = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void SetCharge(AbilityType c, int Level)
    {
        level = Mathf.Clamp(Level, 0, 3);
        color = c;

        if(color == AbilityType.Red)
        {
            if (level == 0) sr.enabled = false;
            else if(level == 1)
            {
                sr.enabled = true;
                sr.sprite = sprites[0];
            }
            else if (level == 2)
            {
                sr.enabled = true;
                sr.sprite = sprites[1];
            }
            else if (level == 3)
            {
                sr.enabled = true;
                sr.sprite = sprites[2];
            }
        }
        else if (color == AbilityType.Blue)
        {
            if (level == 0) sr.enabled = false;
            else if (level == 1)
            {
                sr.enabled = true;
                sr.sprite = sprites[3];
            }
            else if (level == 2)
            {
                sr.enabled = true;
                sr.sprite = sprites[4];
            }
            else if (level == 3)
            {
                sr.enabled = true;
                sr.sprite = sprites[5];
            }
        }
        else if (color == AbilityType.Yellow)
        {
            if (level == 0) sr.enabled = false;
            else if (level == 1)
            {
                sr.enabled = true;
                sr.sprite = sprites[6];
            }
            else if (level == 2)
            {
                sr.enabled = true;
                sr.sprite = sprites[7];
            }
            else if (level == 3)
            {
                sr.enabled = true;
                sr.sprite = sprites[8];
            }
        }
        else if (color == AbilityType.Green)
        {
            if (level == 0) sr.enabled = false;
            else if (level == 1)
            {
                sr.enabled = true;
                sr.sprite = sprites[9];
            }
            else if (level == 2)
            {
                sr.enabled = true;
                sr.sprite = sprites[10];
            }
            else if (level == 3)
            {
                sr.enabled = true;
                sr.sprite = sprites[11];
            }
        }
        else if (color == AbilityType.Purple)
        {
            if (level == 0) sr.enabled = false;
            else if (level == 1)
            {
                sr.enabled = true;
                sr.sprite = sprites[12];
            }
            else if (level == 2)
            {
                sr.enabled = true;
                sr.sprite = sprites[13];
            }
            else if (level == 3)
            {
                sr.enabled = true;
                sr.sprite = sprites[14];
            }
        }
        else if (color == AbilityType.Orange)
        {
            if (level == 0) sr.enabled = false;
            else if (level == 1)
            {
                sr.enabled = true;
                sr.sprite = sprites[15];
            }
            else if (level == 2)
            {
                sr.enabled = true;
                sr.sprite = sprites[16];
            }
            else if (level == 3)
            {
                sr.enabled = true;
                sr.sprite = sprites[17];
            }
        }

    }

}
