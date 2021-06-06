using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Menus = MenuHandler.MenuOverlay;

public class mnu_FailAnswer : MonoBehaviour
{
    //ToDo: Is this class required anymore?
    
    GameManager gameManager;
    CardSetCollection csCollection;
    MenuHandler uiMenus;

    private void Awake()
    {
        gameManager = GameManager.instance;
        csCollection = CardSetCollection.instance;
        uiMenus = MenuHandler.instance;
    }

    public void Home()
    {
        gameManager.SetGameState(gameManager.gameState.cardSetCollection);
//        uiMenus.CloseMenu(Menus.FailAnswer);
    }

    public void NextQuestion()
    {
//        csCollection.SelectCardSet();
//        uiMenus.CloseMenu(Menus.FailAnswer);
    }
}
