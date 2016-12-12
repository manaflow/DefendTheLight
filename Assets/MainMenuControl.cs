using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuControl : MonoBehaviour
{
    public Image newGameBtn;
    public Image helpBtn;
    public Image easyBtn;
    public Image normalBtn;
    public Image hardBtn;

    public Image backBtn;
    public Image helpTowerBtn;
    public Image helpChargeBtn;
    public Image helpAbilityBtn;

    public Sprite mainMenuSprite;
    public Sprite helpMenuTowerSprite;
    public Sprite helpMenuChargeSprite;
    public Sprite helpMenuAbilitySprite;
    public GameObject background;
    SpriteRenderer sr;

    enum MainMenuState {Main, Difficulty, HelpTower, HelpCharge, HelpAbility};
    MainMenuState menuState = MainMenuState.Main;

    // Use this for initialization
    void Start ()
    {
        sr = background.GetComponent<SpriteRenderer>();
        helpTowerBtn.enabled = false;
        helpChargeBtn.enabled = false;
        helpAbilityBtn.enabled = false;
    }

    void PressBackBtn()
    {
        if (menuState == MainMenuState.Main)
        {

        }
        else if (menuState == MainMenuState.Difficulty)
        {
            ChangeToMain();
        }
        else if (menuState == MainMenuState.HelpTower)
        {
            ChangeToMain();
        }
    }
    
    // Update is called once per frame
    void Update ()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PressBackBtn();            
        }

        if (Input.GetMouseButtonDown(0)) // Click
        {
            HandleClick();
        }
    }

    public void HandleClick()
    {
        PointerEventData pointer = new PointerEventData(EventSystem.current);
        pointer.position = Input.mousePosition;
        List<RaycastResult> raycast = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, raycast);

        for (int i = 0; i < raycast.Count; i++)
        {
            if (raycast[i].gameObject.tag == "NewGameBtn")
            {
                ChangeToDifficulty();
            }
            else if (raycast[i].gameObject.tag == "HelpBtn")
            {
                ChangeToHelp();
            }
            else if (raycast[i].gameObject.tag == "EasyBtn")
            {
                Game.difficulty = Difficulty.Easy;
                StartGame();
            }
            else if (raycast[i].gameObject.tag == "NormalBtn")
            {
                Game.difficulty = Difficulty.Normal;
                StartGame();
            }
            else if (raycast[i].gameObject.tag == "HardBtn")
            {
                Game.difficulty = Difficulty.Hard;
                StartGame();
            }
            else if (raycast[i].gameObject.tag == "BackBtn")
            {
                PressBackBtn();
            }
            else if (raycast[i].gameObject.tag == "HelpTowerBtn")
            {
                sr.sprite = helpMenuTowerSprite;
            }
            else if (raycast[i].gameObject.tag == "HelpChargeBtn")
            {
                sr.sprite = helpMenuChargeSprite;
            }
            else if (raycast[i].gameObject.tag == "HelpAbilityBtn")
            {
                sr.sprite = helpMenuAbilitySprite;
            }

        }
    }

    void StartGame()
    {
        SceneManager.LoadScene("Main");
    }

    void ChangeToMain()
    {
        menuState = MainMenuState.Main;
        sr.sprite = mainMenuSprite;
        easyBtn.enabled = false;
        normalBtn.enabled = false;
        hardBtn.enabled = false;
        newGameBtn.enabled = true;
        helpBtn.enabled = true;

        helpTowerBtn.enabled = false;
        helpChargeBtn.enabled = false;
        helpAbilityBtn.enabled = false;
    }

    void ChangeToDifficulty()
    {
        menuState = MainMenuState.Difficulty;
        sr.sprite = mainMenuSprite;
        easyBtn.enabled = true;
        normalBtn.enabled = true;
        hardBtn.enabled = true;
        newGameBtn.enabled = false;
        helpBtn.enabled = false;

        helpTowerBtn.enabled = false;
        helpChargeBtn.enabled = false;
        helpAbilityBtn.enabled = false;
    }  

    void ChangeToHelp()
    {
        menuState = MainMenuState.HelpTower;
        sr.sprite = helpMenuTowerSprite;
        easyBtn.enabled = false;
        normalBtn.enabled = false;
        hardBtn.enabled = false;
        newGameBtn.enabled = false;
        helpBtn.enabled = false;

        helpTowerBtn.enabled = true;
        helpChargeBtn.enabled = true;
        helpAbilityBtn.enabled = true;
    }

    
}
