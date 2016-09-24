using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameControl : MonoBehaviour
{
    public GameObject enemyPref;
    public GameObject selectPref;
    public Image UI_TowerMenu;
    GameObject selector;

    public LayerMask gridMask;
    GameGrid gameGrid;
    Vector3 org;
    Vector3 dir;

    int selectX;
    int selectY;

    public LayerMask UImask;
    bool towerMenu = false;

	// Use this for initialization
	void Start ()
    {
        gameGrid = new GameGrid();
        Game.grid = gameGrid;
        Game.enemies = new List<Enemy>();

        // Start with menu offscreen
        UI_TowerMenu.rectTransform.position = new Vector3(-Screen.width / 2, Screen.height / 2, 0);
    }
	
	// Update is called once per frame
	void Update ()
    {
     
     
      
        // Spawn enemy
        if (Input.GetKeyDown(KeyCode.A)) { Game.enemies.Add(GameObject.Instantiate(enemyPref.GetComponent<Enemy>())); }

       
        // Left mouse click
        if(Input.GetMouseButtonDown(0))
        {

            // Menu not open, click the board
            if (!towerMenu)
            {
                int x = (int)Input.mousePosition.x / (Screen.width / 25);
                int y = (int)Input.mousePosition.y / (Screen.height / 14);
                y -= 1; // border at bottom
                if (y < 0) return;

                if (selector == null) selector = GameObject.Instantiate(selectPref);
                selector.transform.position = new Vector3(x * 0.64f + 0.32f, y * 0.64f + 0.32f + 0.64f, -1);



                selectX = x;
                selectY = y;

                // For now only select empty location
                GameObject o = gameGrid.CheckGrid(x, y);
                if (o == null)
                {
                    towerMenu = true;
                    Debug.Log(UI_TowerMenu.rectTransform.position);
                    UI_TowerMenu.rectTransform.position = new Vector3(Screen.width/2, Screen.height/2, 0);
                    Debug.Log(UI_TowerMenu.rectTransform.position);
                }
            }
            else
            {               


                // Create a raycast using the mouse position and test against the UI
                PointerEventData pointer = new PointerEventData(EventSystem.current);
                pointer.position = Input.mousePosition;

                List<RaycastResult> raycast = new List<RaycastResult>();

                EventSystem.current.RaycastAll(pointer, raycast);

                // Successfully clicked a UI element
                if(raycast.Count > 0)
                {
                    TowerButton btn = raycast[0].gameObject.GetComponent<TowerButton>();
                    if(btn != null)
                    {
                        GameObject tower = GameObject.Instantiate(btn.Tower);
                        tower.transform.position = new Vector3(selector.transform.position.x, selector.transform.position.y, -1);
                        gameGrid.SetGrid(selectY, selectX, tower);
                    }
                    Debug.Log(raycast[0].gameObject.name);
                }

                // Clicking anywhere else will close menu also
                towerMenu = false;
                // Reposition offscreen
                UI_TowerMenu.rectTransform.position = new Vector3(-Screen.width / 2, Screen.height / 2, 0);
                selector.transform.position = new Vector3(-1, 0, -1);
            }


        }

    }

    public void AddTower()
    {

    }
    public void AddWall()
    {
        
        Debug.Log("Click");

        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = new Vector3(selectX * 0.64f + 0.32f, selectY * 0.64f + 0.32f + 0.64f, -2);
        cube.transform.localScale = new Vector3(0.64f, 0.64f, 0.64f);
        cube.name = "(" + selectX.ToString() + "," + selectY.ToString() + ")";
        gameGrid.SetGrid(selectY, selectX, cube);
    }
}
