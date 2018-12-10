using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Menu = MenuHandler.MenuOverlay;

public class PlayerObject : MonoBehaviour
{
    public Player refPlayer;
    //public int turn;

    [SerializeField] Button loadPlayer;
    [SerializeField] Text playerName;
    [SerializeField] InputField nameInputField;
    [SerializeField] Image buttonImage;

    PlayerRosterSelect playerRosterSelect;
    PlayerSelector playerSelector;
    PlayerController playerController;
    MenuHandler uiMenus;
    bool uniqueName;
    Color darkGreen = new Color(0.0f, 0.9f, 0.0f);
    Color lightRed = new Color(1.0f, 0.35f, 0.35f);

    private void Awake()
    {
        uiMenus = MenuHandler.uiMenus;
        playerSelector = PlayerSelector.playerSelector;
        playerController = PlayerController.playerController;
        playerRosterSelect = PlayerRosterSelect.playerRosterSelect;
    }

    private void Start()
    {
        refPlayer = playerController.activePlayer;
        loadPlayer.interactable = playerController.playerDataExists;
        playerName.text = refPlayer.playerName;
    }

   public void AddToPlayerList()
    {
        if (this.gameObject.activeSelf)
        {
            playerController.playersActive.Add(refPlayer);
        }
    }

    public void RemovePlayer()
    {
        playerSelector.editPlayerObject = this;
        uiMenus.ShowMenu(Menu.RemovePlayer);
    }

    public void NameCheck(string text)
    {
        buttonImage.color = lightRed;
        text = nameInputField.text;
        if (playerController.UniqueNameCheck(text, refPlayer))
        { 
            buttonImage.color = darkGreen; 
        }
    }

    public void UpdatePlayerName()
    {
        string newName = nameInputField.text;

        // check if the new name is blank and if so reset it to the original name
        // or if the new name is not unique
        if (!playerController.UniqueNameCheck(newName, refPlayer)) {newName = playerName.text;}

        refPlayer.playerName = newName;
        playerName.text = newName;
        playerName.gameObject.SetActive(true);
        nameInputField.text = newName;
        nameInputField.gameObject.SetActive(false);
        buttonImage.color = Color.white;
    }

    public void ShowPlayerRoster()
    {
        int index;
        index = transform.GetSiblingIndex()+1;
        playerRosterSelect.transform.SetParent(transform);
        //playerRosterSelect.transform.SetSiblingIndex(index);
        playerRosterSelect.transform.localPosition = new Vector3 (0.0f, 110f, 0f);
        playerRosterSelect.gameObject.SetActive(true);
    }
}