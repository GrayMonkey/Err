#pragma warning disable 649   // Disable [SerializeField] warnings CS0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Menu = MenuHandler.MenuOverlay;

public class mnu_NewQuestion : MonoBehaviour
{
    PlayerController playerController;
    MenuHandler uiMenus;
    [SerializeField] Text playerName;

    private void Awake()
    {
        playerController = PlayerController.playerController;
        uiMenus = MenuHandler.uiMenus;
    }

    private void OnEnable()
    {
        playerName.text = playerController.activePlayer.playerName;
    }

    public void CloseMenu()
    {
        playerController.GetNewQuestion();
        uiMenus.CloseMenu(Menu.NewQuestion);
    }
}