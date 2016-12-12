using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lightning : MonoBehaviour
{
    public GameObject bolt;

    List<GameObject> boltList;

    float distance = 1;
    List<float> points;
    Vector2 source;
    Vector2 dest;
    // Use this for initialization
    void Start()
    {
        source = transform.position;
        dest = transform.position + new Vector3(0, 1, 0);

        // Create a vector on the x,z plane. To map the rotation treat z as x, x as y
        //Vector2 vect = new Vector2(target.transform.position.x - transform.position.x, target.transform.position.y - transform.position.y).normalized;
        // Vector2 vect = new Vector2(0, 1).normalized;

        //float rotate = Mathf.Sign(vect.y) * Mathf.Acos(vect.x) * 180 / Mathf.PI;
        //beam.transform.localEulerAngles = new Vector3(0, 0, rotate);

        boltList = new List<GameObject>();
        for(int i = 0; i < 10; i++)
        {
            boltList.Add(GameObject.Instantiate(bolt));
        }


        GenerateBolt();

    }

    void GenerateBolt()
    {
        float previousDisplacement = 0;
        List<float> positions = new List<float>();
        for (int i = 0; i < 10; i++)
        {
            positions.Add(Random.Range(0.15f * distance, 0.85f * distance));            
        }

        positions.Sort();

        for (int i = 9; i >= 0 ; i--)
        {
            float displacement = Random.Range(-0.15f, 0.15f);
           // displacement -= (previousDisplacement - displacement);
            boltList[i].transform.position = transform.position + new Vector3(positions[i], displacement, 0);
            previousDisplacement = displacement;

            Vector2 vect;
            if (i < 9) vect = new Vector2(boltList[i + 1].transform.position.x - boltList[i].transform.position.x, boltList[i + 1].transform.position.y - boltList[i].transform.position.y).normalized;
            else vect = new Vector2(dest.x - boltList[i].transform.position.x, dest.y - boltList[i].transform.position.y);
            float rotate = Mathf.Sign(vect.y) * Mathf.Acos(vect.x) * 180 / Mathf.PI;
            boltList[i].transform.localEulerAngles = new Vector3(0, 0, rotate + 90);
            boltList[i].transform.localScale = new Vector3(10, 0.1f, 1);
        }
    }


    
}
