using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Menu = MenuHandler.MenuOverlay;

public class mnu_QuitGame : MonoBehaviour 
{
    [SerializeField] Button homeButton;
    [SerializeField] Button returnButton;
    GameManager gameManager;
    MenuHandler uiMenus;

    private void Start()
    {
        gameManager = GameManager.gameManager;
        uiMenus = MenuHandler.uiMenus;
    }

    private void QuitCurrentGame (bool quit)
    {
        if (quit)
        {
            gameManager.gameInProgress = false;
            gameManager.UpdateGameState(GameManager.GameState.Home);
            uiMenus.CloseMenu(Menu.Options);
        }

        uiMenus.CloseMenu(Menu.QuitGame);
    }
}
