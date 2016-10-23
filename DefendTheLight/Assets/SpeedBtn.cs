using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class SpeedBtn : MonoBehaviour
{
    public Sprite x1;
    public Sprite x2;
    public Sprite x4;

    int index = 0;
    Image sr;
    public void Click()
    {
        index++;

        if (index == 1)
        {
            sr.sprite = x2;
            Game.Speed = 2;
            if (!Game.isPaused)
            {
                Game.GameSpeed = 2;
                Time.timeScale = 2;
            }
        }
        else if(index == 2)
        {
            sr.sprite = x4;
            Game.Speed = 4;
            if (!Game.isPaused)
            {
                Game.GameSpeed = 4;
                Time.timeScale = 4;
            }
        }
        else
        {
            index = 0;
            sr.sprite = x1;
            Game.Speed = 1;
            if (!Game.isPaused)
            {
                Game.GameSpeed = 1;
                Time.timeScale = 1;
            }
        }
    }

	// Use this for initialization
	void Start ()
    {
        sr = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
