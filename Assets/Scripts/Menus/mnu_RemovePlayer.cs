using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Menus = MenuHandler.MenuOverlay;

public class mnu_RemovePlayer : MonoBehaviour 
{
 
    [SerializeField] Button trash;
    [SerializeField] PlayerRosterSelect playerRosterSelect;
    [SerializeField] GameObject playerTrashConfirm;

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
        playerRosterSelect = PlayerRosterSelect.playerRosterSelect;
        uiMenus = MenuHandler.uiMenus;
    }

    private void OnEnable()
    {
        uiMenus = MenuHandler.uiMenus;
        editPlayer = playerSelector.hasFocus.refPlayer;
        playerRoster = playerController.playerRoster;
        activePlayers = playerSelector.playersActive;

        trash.interactable = false;
        if(playerRoster.Count > 0)
        {
            trash.interactable = true;
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
        playerController.playersActive.Remove(editPlayer);
        //playerRosterSelect.Hide();
        CloseMenu();
    }

    public void ConfirmTrash()
    {
        playerTrashConfirm.SetActive(true);
    }

    // Trash the player data from the player roster
    // TODO Remove the player from the PlayerRosterSelector
    public void RemoveFromRoster(bool trashPlayer)
    {
       if (playerRoster.Contains(editPlayer))
        {
            if (trashPlayer)
            {
                playerRosterSelect.TrashPlayer(editPlayer);
                RemoveFromGame();
            }
        }
        playerTrashConfirm.SetActive(false);

        if (trashPlayer) { CloseMenu(); }
    }


    // Close the menu
    public void CloseMenu ()
    {
        uiMenus.CloseMenu(Menus.RemovePlayer);
    }
}
