using UnityEngine;
using System.Collections;

public class WaveControl : MonoBehaviour
{
    public GameObject[] enemyPrefs;
    float time = 0;
    float spawnTime = 0;
	// Use this for initialization
	void Start ()
    {
	    if(Game.difficulty == Difficulty.Easy)
        {
            spawnTime = 1;
        }
        else if (Game.difficulty == Difficulty.Normal)
        {
            spawnTime = 0.75f;
        }
        else if (Game.difficulty == Difficulty.Hard)
        {
            spawnTime = 0.5f;
        }
        else if (Game.difficulty == Difficulty.VeryHard)
        {
            spawnTime = 0.25f;
        }

        Enemy newEnemy = GameObject.Instantiate(enemyPrefs[1].GetComponent<Enemy>());
        Game.enemies.Add(newEnemy);
        newEnemy.Init(1, Game.wave);
    }
	
	// Update is called once per frame
	void Update ()
    {
        time += Time.deltaTime;

        if(time > spawnTime)
        {
            // Reset timer
            time -= spawnTime;
            // Pick a random tile on the left spawn side.
            int tileY = Random.Range(0, 13);

            // For now spawn only normal types - index 1
            Enemy newEnemy = GameObject.Instantiate(enemyPrefs[1].GetComponent<Enemy>());
            Game.enemies.Add(newEnemy);
            newEnemy.Init(tileY, Game.wave);
        }

    }

    public void Init()
    {

    }
}
