using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameControl : MonoBehaviour
{
    public GameObject lightObj;
    public GameObject selectPref;
    public GameObject dragPref;
    public GameObject waveObjPref;
    public GameObject projectilePref;
    public GameObject instructionPref;  

    // Counters for abilities
    public Text redCounter;
    public Text blueCounter;
    public Text yellowCounter;
    public Text purpleCounter;
    public Text orangeCounter;
    public Text greenCounter;

    // Selection
    public Text chargeText;
    public Image chargeImage;
    public Image towerSelect;
    public Image towerCharge;
    public Image towerSell;

    bool selected = false;
    Tower selectedTower = null;

    public Image UI_TowerMenu;
    public Sprite rangeSprite;
    public int seed = 0;
    bool start = false;

    public int chargeMax = 30;  // 30 kills

    public Text healthUI;
    public Text gameoverUI;

    public GameObject targeting;

    [HideInInspector]
    public WaveControl waveControl; 

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
        Game.control = this;
        gameGrid = new GameGrid();
        Game.grid = gameGrid;
        Game.enemies = new List<Enemy>();

        Game.charges = 10;
        if (Game.difficulty == Difficulty.Easy) chargeMax = 20; 
        else if (Game.difficulty == Difficulty.Hard) chargeMax = 40;  
    }
	
	// Update is called once per frame
	void Update ()
    {
        redCounter.text = Game.redCount.ToString();
        blueCounter.text = Game.blueCount.ToString();
        yellowCounter.text = Game.yellowCount.ToString();
        purpleCounter.text = Game.purpleCount.ToString();
        greenCounter.text = Game.greenCount.ToString();
        orangeCounter.text = Game.orangeCount.ToString();

        healthUI.text = "Health: " + Game.Health; 
        if(Game.Health <= 0)
        {
            gameoverUI.enabled = true;
            Time.timeScale = 0;
        }

        if (Game.chargeCounter >= chargeMax)
        {
            Game.charges++;
            Game.chargeCounter -= chargeMax;        
        }

        if (Game.TargetEnemy != null)
        {
            targeting.transform.position = Game.TargetEnemy.transform.position + new Vector3(0, 0, -1);
        }
        else targeting.transform.position = new Vector3(-100, 0, 0);


        chargeImage.fillAmount = ((float)Game.chargeCounter) / chargeMax;
        chargeText.text = Game.charges.ToString();     
           
     
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
        // Clicking anywhere will remove instructions
        if (instructionPref != null) Destroy(instructionPref);

        // Start Game 
        if (!start && waveControl == null)
        {
            GameObject waveObj = GameObject.Instantiate(waveObjPref);
            waveControl = waveObj.GetComponent<WaveControl>();
            waveControl.Init();
            start = true;
        }

        // Check board selection
        int x = (int)Input.mousePosition.x / (Screen.width / 32); // 32 cells
        int y = (int)Input.mousePosition.y / (Screen.height / 18); // 14 cells +2+2 on for top and bottom UI
        y -= 2; // Board starts 2 cells up.



        GameObject obj = Game.grid.CheckGrid(y, x);
        if (obj != null && selectedTower == null)
        {
            selectedTower = obj.GetComponent<Tower>();
            #region Tower
            if (!selected)
            {
                float width = Screen.width / 32;
                float height = Screen.height / 18;
                selected = true;
                towerSelect.rectTransform.position = new Vector3((x + 0.5f) * width, (y + 2.5f) * height, 0);
                // Drawing the Selection UI varies if near the edge. Take care of specific cases
                // NW corner
                if (x < 2 && y > 11)
                {
                    towerCharge.rectTransform.position = towerSelect.rectTransform.position + new Vector3(45, 0, 0);
                    towerSell.rectTransform.position = towerSelect.rectTransform.position + new Vector3(0, -45, 0);
                }
                // NE corner
                else if(x > 29 && y > 11)
                {
                    towerCharge.rectTransform.position = towerSelect.rectTransform.position + new Vector3(-45, 0, 0);
                    towerSell.rectTransform.position = towerSelect.rectTransform.position + new Vector3(0, -45, 0);
                }
                // SE corner
                else if (x > 29 && y > 11)
                {
                    towerCharge.rectTransform.position = towerSelect.rectTransform.position + new Vector3(0, 45, 0);
                    towerSell.rectTransform.position = towerSelect.rectTransform.position + new Vector3(-45, 0, 0);
                }
                // Top
                else if (y > 11)
                {
                    towerCharge.rectTransform.position = towerSelect.rectTransform.position + new Vector3(45, 0, 0);
                    towerSell.rectTransform.position = towerSelect.rectTransform.position + new Vector3(0, -45, 0);
                }
                else if(x > 29)
                {
                    towerCharge.rectTransform.position = towerSelect.rectTransform.position + new Vector3(-45, 0, 0);
                    towerSell.rectTransform.position = towerSelect.rectTransform.position + new Vector3(0, -45, 0);
                }
                else
                {
                    towerCharge.rectTransform.position = towerSelect.rectTransform.position + new Vector3(0, 45, 0);
                    towerSell.rectTransform.position = towerSelect.rectTransform.position + new Vector3(45, 0, 0);
                }
            }
            #endregion
        }        
        else // Try looking at UI objects
        {
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(Input.mousePosition.x / Screen.width * 19.2f, Input.mousePosition.y / Screen.height * 10.8f - 1.2f), Vector2.right, 0.1f);
            
            if (hit.collider != null)
            {
                //Game.TargetEnemy = hit.collider.gameObject.GetComponent<Enemy>();
            }
            
                #region UI

           // Create a raycast using the mouse position and test against the UI
           PointerEventData pointer = new PointerEventData(EventSystem.current);
            pointer.position = Input.mousePosition;
            List<RaycastResult> raycast = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointer, raycast);

            if (raycast.Count == 0)
            {
                selected = false;
                towerSelect.rectTransform.position = new Vector3(-100, 0);
                selectedTower = null;
                return; // found nothing
            }

            // You can click either a UI element, or Tower on the board.
            // First check if selecting UI element - Towers, Abilities, Pause, Speed, Menu

            for (int i = 0; i < raycast.Count; i++)
            {
                Debug.Log(raycast[0]);


                TowerButton towerBtn = raycast[i].gameObject.GetComponent<TowerButton>();
                if (towerBtn != null) // Tower UI Element found
                {
                    if (Game.charges < towerBtn.cost) return; // make sure u have enough money
                    dragObject = GameObject.Instantiate(dragPref);
                    DragControl dc = dragObject.GetComponent<DragControl>();
                    //dc.obj = towerBtn.Tower;     
                    dc.buttonRef = towerBtn;
                    dc.sprite = towerBtn.Tower.GetComponent<SpriteRenderer>().sprite;
                    isTower = true;
                    dc.cost = towerBtn.cost;
                    break;
                }
                AbilityBtn abilityBtn = raycast[i].gameObject.GetComponent<AbilityBtn>();
                if (abilityBtn != null)
                {
                    if (Game.charges > 0)
                    {
                        if(abilityBtn.abilityType == AbilityType.Red && Game.redCount > 0)
                        {
                            lightObj.GetComponent<LightObject>().StartRed();
                            Game.redCount--;
                        }
                        else if (abilityBtn.abilityType == AbilityType.Blue && Game.blueCount > 0)
                        {
                            lightObj.GetComponent<LightObject>().StartBlue();
                            Game.blueCount--;
                        }
                        else if (abilityBtn.abilityType == AbilityType.Yellow && Game.yellowCount > 0)
                        {
                            lightObj.GetComponent<LightObject>().StartYellow();
                            Game.yellowCount--;
                        }
                        else if (abilityBtn.abilityType == AbilityType.Purple && Game.purpleCount > 0)
                        {
                            lightObj.GetComponent<LightObject>().StartPurple();
                            Game.purpleCount--;
                        }
                        else if (abilityBtn.abilityType == AbilityType.Orange && Game.orangeCount > 0)
                        {
                            lightObj.GetComponent<LightObject>().StartOrange();
                            Game.orangeCount--;
                        }
                        else if (abilityBtn.abilityType == AbilityType.Green && Game.greenCount > 0)
                        {
                            lightObj.GetComponent<LightObject>().StartGreen();
                            Game.greenCount--;
                        }
                        break;
                    }
                }
                SpeedBtn speedBtn = raycast[i].gameObject.GetComponent<SpeedBtn>();
                if (speedBtn != null)
                {
                    speedBtn.Click();
                }

                if (raycast[i].gameObject.tag == "Pause")
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

                if(raycast[i].gameObject.tag == "Charge")
                {
                    if(selectedTower != null)
                    {
                        if (selectedTower.ChargeTower()) Game.charges--; 
                    }
                }
                else if(raycast[i].gameObject.tag == "Sell")
                {
                    if(selectedTower != null)
                    {
                        // Destory tower, get half half a charge
                        selectedTower.SellTower();
                        Game.chargeCounter += chargeMax / 2;
                    }
                }
            }
            #endregion

            // Remove tower selection
            selected = false;
            towerSelect.rectTransform.position = new Vector3(-100, 0);
            selectedTower = null;
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
                        if (dc.buttonRef != null)
                        {
                            GameObject tower = GameObject.Instantiate(dc.buttonRef.Tower);
                            tower.transform.position = new Vector3(j * 0.6f + 0.3f, i * 0.6f + 0.3f, -1);
                            gameGrid.SetGrid(i, j, tower);
                            Game.charges -= dc.cost;
                            Destroy(dragObject);
                            Game.grid.UpdateCostGrid();
                            return;
                        }
                    }
                }
            }
        }

        Destroy(dragObject);

    }   
   
}
