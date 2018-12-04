using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Menus = MenuHandler.MenuOverlay;

public class mnu_RemovePlayer : MonoBehaviour 
{
 
    [SerializeField] Button trash;
    MenuHandler uiMenus;
    PlayerSelector playerSelector;
    PlayerController playerController;
    Player editPlayer;
    List<Player> playerRoster;
    List<Player> activePlayers;

    private void Awake()
    {
        playerController = PlayerController.playerController;
        playerSelector = PlayerSelector.playerSelector;
        uiMenus = MenuHandler.uiMenus;
    }

    private void OnEnable()
    {
        uiMenus = MenuHandler.uiMenus;
        editPlayer = playerSelector.editPlayerButton.refPlayer;
        playerRoster = playerController.playerRoster;
        activePlayers = playerSelector.playersActive;

        trash.interactable = false;
        if(playerRoster.Count > 0)
        {
            trash.interactable = true;
        }
	}

    // Trash the player data from the player roster
    public void RemoveFromRoster()
    {
       if (playerRoster.Contains(editPlayer))
        {
            playerRoster.Remove(editPlayer);
            RemoveFromGame();
        }
    }

    // Remove the player from the current game and reset
    // the player data
    public void RemoveFromGame()
    {
        //       if(activePlayers.Contains(editPlayer))
        //       {
        ////           editPlayer.ResetData();
        //    //editPlayer.gameObject.SetActive(false);
        //    PlayerSelection.playerSelect.UpdatePlayerList();
        //    playerRemoved = true;
        //    CloseMenu();
        //}
        playerSelector.editPlayerButton.gameObject.SetActive(false);
        playerController.playersActive.Remove(editPlayer);
        CloseMenu();
    }

    // Close the menu
    public void CloseMenu ()
    {
        uiMenus.CloseMenu(Menus.RemovePlayer);
        //if (!playerRemoved)
        //{
        //    uiMenu.ShowMenu(Menus.PlayerInfo);
        //}
    }
}
