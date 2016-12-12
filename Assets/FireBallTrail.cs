using UnityEngine;
using System.Collections;

public class FireBallTrail : MonoBehaviour
{
    SpriteRenderer sr;
    public float fadeTime;
    float time;
	// Use this for initialization
	void Start ()
    {
        sr = GetComponent<SpriteRenderer>();
        time = fadeTime;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (time <= 0) Destroy(this.gameObject);
        else time -= Time.deltaTime;
        sr.color = new Color(1, 1, 1, time / fadeTime);
	}
}
