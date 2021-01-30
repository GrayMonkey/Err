#pragma warning disable 649   // Disable [SerializeField] warnings CS0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MenuHandler : MonoBehaviour 
{
    public static MenuHandler instance;
    public GameObject helpButton;
    public enum MenuOverlay { Options, FailAnswer, CorrectAnswer, NewQuestion, WinningPlayer, 
        GameResults, QuitGame, Instructions, Credits }; // RemovePlayer, PlayerStats, PlayerInfo

    [SerializeField] private GameObject backPanel;
    [SerializeField] private GameObject[] menuArray;
    // List<MenuOverlay> menuStack; //Not sure if this is going to be needed but useful for returning to previous menus?

    private void Awake()
    {
        instance = this;
    }

    public void ShowMenu(MenuOverlay newMenu)
    {
        int iD = (int)newMenu;
        menuArray[iD].SetActive(true);
        backPanel.SetActive(true);
        //menuStack.Add(newMenu);
    }

    public void CloseMenu(MenuOverlay oldMenu)
    {
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
}
