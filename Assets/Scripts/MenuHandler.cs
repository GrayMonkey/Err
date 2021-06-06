#pragma warning disable 649   // Disable [SerializeField] warnings CS0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MenuHandler : MonoBehaviour 
{
    public static MenuHandler instance;
    public GameObject helpButton;
    public enum MenuOverlay {Options, Answer, Instructions}; // RemovePlayer, PlayerStats, PlayerInfo, NewQuestion, WinningPlayer, GameResults, QuitGame , Credits, NextQuestion, 

    GameManager gameManager;
    [SerializeField] GameObject backPanel;
    [SerializeField] GameObject[] menuArray;

    GameObject gameState;
    // List<MenuOverlay> menuStack; //Not sure if this is going to be needed but useful for returning to previous menus?

    private void Awake()
    {
        instance = this;
        gameManager = GameManager.instance;
    }

    public void DisableMenus()
    {
        for (int i = 0; i < menuArray.Length; i++)
            menuArray[i].gameObject.SetActive(false);
    }

    public void ShowMenu(MenuOverlay newMenu)
    {
        if (gameManager == null)
            gameManager = GameManager.instance;
        gameState = gameManager.currGameState;
        gameState.SetActive(false);
        int iD = (int)newMenu;
        menuArray[iD].SetActive(true);
        backPanel.SetActive(true);
        //menuStack.Add(newMenu);
    }

    public void CloseMenu(MenuOverlay oldMenu)
    {
        gameState.SetActive(true);
        int iD = (int)oldMenu;
        menuArray[iD].SetActive(false);
        backPanel.SetActive(false);

/*        // If there is more than one menu in the stack then presumably
        // the next oldest menu needs to be displayed
        menuStack.Remove(oldMenu);
        if (menuStack.Count>0)
        {
            ShowMenu((MenuOverlay)menuStack.Count - 1);
            menuStack.RemoveAt(menuStack.Count - 1);
        
        }*/
    }

    public void CloseMenu(MenuOverlay oldMenu, GameObject newGameState)
    {
        gameState = newGameState;
        CloseMenu(oldMenu);
    }
}
