using UnityEngine;
using System.Collections;

public class Line : MonoBehaviour
{
    // Start
    public Vector2 A;
    // End
    public Vector2 B;
    //Thinkness
    public float Thickness;

    // Children taht contain the pieces that make up the line
    public GameObject StartCapChild, LineChild, EndCapChild;
    
    // New Line
    public Line(Vector2 a, Vector2 b, float thickness)
    {
        A = a;
        B = b;
        Thickness = thickness;
    }

    // Used to set the color of the line
    public void SetColor(Color color)
    {
        StartCapChild.GetComponent<SpriteRenderer>().color = color;
        LineChild.GetComponent<SpriteRenderer>().color = color;
        EndCapChild.GetComponent<SpriteRenderer>().color = color;
    }

    // Will actually draw line
    public void Draw()
    {
        Vector2 difference = B - A;
        float rotation = Mathf.Atan2(difference.y, difference.x);

        // 
    }
    
	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
