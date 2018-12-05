#pragma warning disable 649   // Disable [SerializeField] warnings CS0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Menu = MenuHandler.MenuOverlay;

public class PlayerInfo : MonoBehaviour
{
    public PlayerObject editPlayerButton;

    [SerializeField] InputField playerName;
    [SerializeField] Text playerTurn;
    [SerializeField] Toggle logData;
    [SerializeField] Button updateButton;
    [SerializeField] GameObject nameGood;
    [SerializeField] GameObject nameBad;
    MenuHandler uiMenu;
    PlayerSelector playerSelect;
    Player editPlayer;
    List<Player> playerRoster;
    List<Player> activePlayers;
    int currentTurn;
    int newTurn;
    string currentName;
    string newName;
    bool currentLogData;

    // Use this when the object becomes enabled
    void OnEnable()
    {
        uiMenu = MenuHandler.uiMenus;
        playerSelect = PlayerSelector.playerSelector;
        editPlayerButton = playerSelect.editPlayerObject;
        editPlayer = editPlayerButton.refPlayer;
        currentName = editPlayer.playerName;
        currentTurn = editPlayerButton.transform.GetSiblingIndex() + 1;
        //currentLogData = editPlayer.saveData;
        newName = currentName;
        newTurn = currentTurn;

        playerName.placeholder.GetComponent<Text>().text = currentName;
        playerName.textComponent.GetComponent<Text>().text = "";
        playerName.text = "";
        playerTurn.text = "# " + currentTurn.ToString();
        //logData.isOn = editPlayer.saveData;
        updateButton.interactable = false;
    }

    // === Turn Order Buttons ===
    // Change the turn order
    public void AlterTurn(int change)
    {
        newTurn = Mathf.Clamp(newTurn + change, 1, playerSelect.playersActive.Count);
        playerTurn.text = "# " + newTurn.ToString();
        ActivateUpdateButton();
    }

    // === Return Button ===
    // Return with or without updating the information (newInfo)
    public void CloseMenu(bool update)
    {
        if (update)
        {
            //editPlayerButton.UpdateButtonInfo(newName, newTurn, logData.isOn);
        }
        
        uiMenu.CloseMenu(Menu.PlayerInfo);
    }

    // If the player name or turn order has changed then check to 
    // activate/deactivate the update button
    public void ActivateUpdateButton()
    {
        bool buttonOn = false;
        newName = playerName.textComponent.text;
        if (newName == "") { newName = currentName; }
        if (newName != currentName || newTurn != currentTurn || logData.isOn != currentLogData) 
        { 
            buttonOn = true;
        }
        updateButton.interactable = buttonOn;
    }

    // Call the menu to confirm removal of the player
    public void RemovePlayer()
    {
        uiMenu.CloseMenu(Menu.PlayerInfo);
        uiMenu.ShowMenu(Menu.RemovePlayer);
    }

    // Check to see if a playerName already exists
    public void CheckName()
    {
        //playerRoster = PlayerRoster.playerRoster;
        activePlayers = playerSelect.playersActive;
        newName = playerName.text;
        bool nameClash = false;

        // Check for an existing name in the playerRoster
        if (playerRoster != null)
        {
            foreach (Player player in playerRoster)
            {
                if (newName == player.playerName)
                {
                    nameClash = true;
                    break;
                }
            }
        }

        // Check for an existing name in the active players
        foreach(Player player in activePlayers)
        {
            if (newName == player.playerName)
            {
                nameClash = true;
                break;
            }
        }

        if (nameClash)
        {
            updateButton.interactable = false;
            nameGood.SetActive(false);
            nameBad.SetActive(true);
            return;
        }

        logData.isOn = true;
        nameGood.SetActive(true);
        nameBad.SetActive(false);
        ActivateUpdateButton();
    }
}