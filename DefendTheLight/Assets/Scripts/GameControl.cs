using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameControl : MonoBehaviour
{

    public GameObject enemyPref;
    public GameObject[] enemyPrefs;
    public GameObject selectPref;
    public GameObject dragPref;
    public Text moneyText;
    public Image UI_TowerMenu;
    public Sprite rangeSprite;

    GameObject selector;
    
    bool isTower;    // whether tower or ability.
    GameObject dragObject;

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

    void OnGUI()
    {
        //if(dragObject != null)
        //GUI.DrawTexture(new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, 64, 64), dragSprite.texture);

        //Debug.Log(Input.mousePosition);
    }
	
	// Update is called once per frame
	void Update ()
    {

        moneyText.text = "$" + Game.money;
      
        // Spawn enemy for testing
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Game.enemies.Add(GameObject.Instantiate(enemyPrefs[0].GetComponent<Enemy>()));
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Game.enemies.Add(GameObject.Instantiate(enemyPrefs[1].GetComponent<Enemy>()));
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Game.enemies.Add(GameObject.Instantiate(enemyPrefs[2].GetComponent<Enemy>()));
        }

        // 3 Types of behavior. Clicking, Dragging, Release Drag

        if(Input.GetMouseButtonDown(0)) // Click
        {
            HandleClick();
        }
        else if(Input.GetMouseButton(0) && dragObject != null) // Drag - Mouse Down and Drag object valid
        {
            //Debug.Log("Drag");
            HandleDrag();
        }
        else if(Input.GetMouseButtonUp(0) && dragObject != null) // Release Drag - Mouse Up and Drag object valid 
        {
            HandleRelease();
        }
    }

    void HandleClick()
    {
        // Create a raycast using the mouse position and test against the UI
        PointerEventData pointer = new PointerEventData(EventSystem.current);
        pointer.position = Input.mousePosition;
        List<RaycastResult> raycast = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, raycast);

        if (raycast.Count == 0) return; // found nothing

        // You can click either a UI element, or Tower on the board.
        // First check if selecting UI element - Towers, Abilities, Pause, Speed, Menu
        
        for(int i = 0; i < raycast.Count; i++)
        {
            TowerButton towerBtn = raycast[i].gameObject.GetComponent<TowerButton>();
            if (towerBtn != null) // Tower UI Element found
            {
                if(Game.money >= towerBtn.cost) // make sure u have enough money
                dragObject = GameObject.Instantiate(dragPref);
                DragControl dc = dragObject.GetComponent<DragControl>();
                dc.obj = towerBtn.Tower;                
                dc.sprite = towerBtn.Tower.GetComponent<SpriteRenderer>().sprite;
                isTower = true;
                dc.cost = towerBtn.cost;
                break;
            }
            AbilityBtn abilityBtn = raycast[i].gameObject.GetComponent<AbilityBtn>();
            if(abilityBtn != null)
            {
                if(Game.energy >= abilityBtn.cost)
                {
                    dragObject = GameObject.Instantiate(dragPref);
                    DragControl dc = dragObject.GetComponent<DragControl>();
                    dc.obj = abilityBtn.abilityPref;
                    dc.sprite = abilityBtn.abilityPref.GetComponent<SpriteRenderer>().sprite;
                    isTower = false;
                    dc.cost = abilityBtn.cost;
                    break;
                }
            }
            SpeedBtn speedBtn = raycast[i].gameObject.GetComponent<SpeedBtn>();
            if(speedBtn != null)
            {
                speedBtn.Click();
            }
            
            if(raycast[i].gameObject.tag == "Pause")
            {
                if (Game.isPaused)
                {
                    Game.isPaused = false;
                    Game.GameSpeed = Game.Speed;
                    Time.timeScale = Game.Speed;
                }
                else
                {
                    Game.isPaused = true;
                    Time.timeScale = 0;
                    Game.GameSpeed = 0;
                }
            }

        }
        
    }
    void HandleDrag()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100, gridMask))
        {
            if (hit.collider != null)
            {
                // From Bottom Left - Board is currently 19.2 x 8.4 with 32x14 slots, 0.6 or 60 pixels
                int i = (int)(hit.point.y / 0.6f); // row
                int j = (int)(hit.point.x / 0.6f); // col
                
                dragObject.transform.position = new Vector3(j * 0.6f + 0.3f, i * 0.6f + 0.3f, -1);

                GameObject obj = gameGrid.CheckGrid(i, j);
                if(obj == null)
                {

                }
            }
        }
    }
    void HandleRelease()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100, gridMask))
        {
            if (hit.collider != null)
            {
                // From Bottom Left - Board is currently 19.2 x 8.4 with 32x14 slots, 0.6 or 60 pixels
                int i = (int)(hit.point.y / 0.6f); // row
                int j = (int)(hit.point.x / 0.6f); // col

                if (isTower)
                {

                    GameObject obj = gameGrid.CheckGrid(i, j);
                    if (obj == null)
                    {

                        DragControl dc = dragObject.GetComponent<DragControl>();
                        GameObject tower = GameObject.Instantiate(dc.obj);
                        tower.transform.position = new Vector3(j * 0.6f + 0.3f, i * 0.6f + 0.3f, -1);
                        gameGrid.SetGrid(i, j, tower);
                        Game.money -= dc.cost;
                        Destroy(dragObject);
                        return;
                    }
                }
                else
                {
                    DragControl dc = dragObject.GetComponent<DragControl>();
                    GameObject abilityObject = GameObject.Instantiate(dc.obj);
                    abilityObject.transform.position = new Vector3(j * 0.6f + 0.3f, i * 0.6f + 0.3f, -3);
                    Game.energy -= dc.cost;
                    Destroy(dragObject);
                    return;
                }

            }
        }

        Destroy(dragObject);

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
