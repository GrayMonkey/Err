#pragma warning disable 649   // Disable [SerializeField] warnings CS0649

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;
using Menus = MenuHandler.MenuOverlay;

public class PlayerSelector : MonoBehaviour
{
    public static PlayerSelector playerSelector;
    public List<Player> playersActive;
    public GameObject playerDragged;
    public GameObject dummyPlayer;
    public int newPlayerCount = 0;
    public bool loadPlayerActive = false;
    public PlayerObject selectedPlayer;
    public GameObject playersActiveContent;

    [SerializeField] GameObject homeObject;
    [SerializeField] GameObject playersRosterContent;
    [SerializeField] GameObject playerObject;
    [SerializeField] Text playerCountLabel;
    [SerializeField] Button addPlayer;
    [SerializeField] Button startGame;
    [SerializeField] GameObject noActivePlayers;

    PlayerController playerController;
    GameOptions gameOptions;
    MenuHandler uiMenus;
    List<Player> playerData = new List<Player>();
    string updateCount;

    private void Awake()
    {
        playerSelector = this;
    }

    // Use this for initialization
    private void Start()
    {
        playerController = PlayerController.playerController;
        playersActive = playerController.playersActive;
        gameOptions = GameOptions.gameOptions;
        uiMenus = MenuHandler.uiMenus;
        updateCount = LocManager.locManager.GetLocText("str_PlayerCount");
        CheckPlayerLists();
    }

    private void CheckPlayerLists()
    {
        // If players don't exist in the active players then display the no players text
        noActivePlayers.SetActive(false);
        if (playerController.playersActive.Count == 0)
            noActivePlayers.SetActive(true);

        // If no players are in the player roster then don't show the player roster
        GameObject playersRoster = playersRosterContent.transform.parent.gameObject;
        playersRoster.SetActive(true);
        if (playerController.playerRoster.Count == 0)
            playersRoster.SetActive(false);
    }

    private void Update()
    {
        string players = playersActive.Count.ToString() + "/10";
        playerCountLabel.text = updateCount + ": " + players;

        switch (playersActive.Count)
        {
            case 0:
            case 1:
                startGame.interactable = false;
                break;
            case 10:
                addPlayer.interactable = false;
                break;
            default:
                startGame.interactable = true;
                addPlayer.interactable = true;
                break;
        }
    }

    public void ResizeActivePlayerHolderCollider()
    {
        // Resize the height of the Collider box to accomodate new players
        RectTransform rect = playersActiveContent.GetComponent<RectTransform>();
        BoxCollider2D collider2D = playersActiveContent.GetComponent<BoxCollider2D>();
        LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
        float height = rect.sizeDelta.y;
        Vector2 size = new Vector2(rect.sizeDelta.x, height);
        collider2D.size = size;
    }

    // Add a new player object and activate a new button
    public void AddNewPlayer()
    {
        newPlayerCount++;
        Player newPlayer = playerController.AddNewPlayer();
        ActivatePlayerButton(newPlayer);
        selectedPlayer = null;
        CheckPlayerLists();
        ResizeActivePlayerHolderCollider();
    }

    // Activate a new player button object
    public void ActivatePlayerButton(Player player)
    {
        playerController.activePlayer = player;
        GameObject newPlayerButton = Instantiate(playerObject, playersActiveContent.transform);
        newPlayerButton.SetActive(true);
        newPlayerButton.GetComponent<PlayerObject>().refPlayer = player;
        if (!playersActive.Contains(player))
            playersActive.Add(player);
    }

    public void UpdatePlayerList()
    {
        playersActive.Clear();

        for (int i = 0; i < playersActiveContent.transform.childCount; i++)
        {
            PlayerObject playerButton = playersActiveContent.transform.GetChild(i).gameObject.GetComponent<PlayerObject>();
            if (playerButton != null)
            {
                playerButton.AddToPlayerList();
            }
        }

 
        // Deactivate the add player button if playersActive.count is 10
        addPlayer.interactable = true;
        if (playersActive.Count == playersActiveContent.transform.childCount)
        {
            addPlayer.interactable = false;
        }

        CheckPlayerLists();
    }

    public void AddPlayerFromRoster()
    {
        // If a player is clicked on and is in the roster then add them to the active players

        CheckPlayerLists();
    }

    //public void ProceedWithPlayers()
    //{
    //    this.gameObject.SetActive(false);
    //    homeObject.SetActive(true);
    //}
}
