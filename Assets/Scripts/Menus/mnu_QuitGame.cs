using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Menu = MenuHandler.MenuOverlay;

public class mnu_QuitGame : MonoBehaviour 
{
    //ToDo: Is this class required anymore?
    
    //[SerializeField] Button homeButton; NLR
    //[SerializeField] Button returnButton; NLR
    GameManager gameManager;
    MenuHandler uiMenus;

    private void Start()
    {
        gameManager = GameManager.instance;
        uiMenus = MenuHandler.instance;
    }

    public void QuitCurrentGame (bool quit)
    {
        if (quit)
        {
            gameManager.gameInProgress = false;
           //questionManager.UpdateGameState(GameManager.GameState.Home);
            uiMenus.CloseMenu(Menu.Options);
        }

        //uiMenus.CloseMenu(Menu.QuitGame);
    }
}
