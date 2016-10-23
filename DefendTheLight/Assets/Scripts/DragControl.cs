using UnityEngine;
using System.Collections;


public class DragControl : MonoBehaviour
{
    [HideInInspector]
    public Sprite sprite
    { get
        {
            if (sr == null) sr = GetComponent<SpriteRenderer>();
            return sr.sprite;
        }
        set
        {
            if (sr == null) sr = GetComponent<SpriteRenderer>();
            sr.sprite = value;
        }
    }
   // public Sprite sprite; // Sprite to display on drag
    [HideInInspector]
    public GameObject obj; // the object to create on release
    [HideInInspector]
    public int cost;        // cost to use
    [HideInInspector]
    public SpriteRenderer sr;
    //[HideInInspector]
    //Color color;

    // Use this for initialization
    void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
