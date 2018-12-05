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
    public PlayerObject editPlayerObject;
    public List<Player> playersActive;
    public GameObject playerDragged;
    public int newPlayerCount = 0;
    public bool loadPlayerActive = false;

    [SerializeField] GameObject playersPanel;
    [SerializeField] GameObject playerObject;
    [SerializeField] Text playerCountLabel;
    [SerializeField] Button loadPlayers;
    [SerializeField] Button addPlayer;
    [SerializeField] Button startGame;

    PlayerController playerController;
    GameOptions gameOptions;
    MenuHandler uiMenus;
    List<Player> playerData = new List<Player>();

    // Use this for initialization
    private void Start()
    {
        playerSelector = this;
        playerController = PlayerController.playerController;
        playersActive = playerController.playersActive;
        gameOptions = GameOptions.gameOptions;
        uiMenus = MenuHandler.uiMenus;
        loadPlayers.interactable = false;
    }

    //private void OnEnable()
    //{
    //    // Clear out exist playerButton objects
    //    PlayerButton[] activePlayerButtons = playersPanel.GetComponentsInChildren<PlayerButton>();
    //    foreach (PlayerButton activePlayerButton in activePlayerButtons) 
    //    { 
    //        Destroy(activePlayerButton.gameObject); 
    //    }

    //    // Check if there are activePlayers (from a previous game)
    //    // and populate accordingly
    //    if (playersActive.Count > 0)
    //    {
    //        foreach (Player player in playersActive) 
    //        {
    //            //playersActive.Remove(player); // player gets readded as part of ActivatePlayerButton
    //            ActivatePlayerButton(player); 
    //        }
    //    }
    //}

    private void Update()
    {
        string updateCount = "UI_PlayerCount: %%playerCount / 10";
        updateCount = updateCount.Replace("%%playerCount", playersActive.Count.ToString());
        playerCountLabel.text = updateCount;

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

    // Add a new player object and activate a new button
    public void AddNewPlayer()
    {
        newPlayerCount++;
        Player newPlayer = playerController.AddNewPlayer();
        ActivatePlayerButton(newPlayer);
    }

    // Activate a new player button object
    public void ActivatePlayerButton(Player player)
    {
        playerController.activePlayer = player;
        GameObject newPlayerButton = Instantiate(playerObject, playersPanel.transform);
        newPlayerButton.GetComponent<PlayerObject>().refPlayer = player;
        if (!playersActive.Contains(player)) { playersActive.Add(player); }

        //for (int i = 0; i < playersPanel.transform.childCount; i++)
        //{
        //    GameObject playerButtonObject = playersPanel.transform.GetChild(i).gameObject;
        //    if (!playerButtonObject.activeInHierarchy)
        //    {
        //        playerButtonObject.SetActive(true);
        //        playerButtonObject.GetComponent<PlayerButton>().refPlayer = player;
        //        playersActive.Add(player);
        //        return;
        //    }
        //}
    }

    public void UpdatePlayerList()
    {
        playersActive.Clear();

        for (int i = 0; i < playersPanel.transform.childCount; i++)
        {
            PlayerObject playerButton = playersPanel.transform.GetChild(i).gameObject.GetComponent<PlayerObject>();
            if (playerButton != null)
            {
                playerButton.AddToPlayerList();
            }
        }

 
        // Deactivate the add player button if playersActive.count is 10
        addPlayer.interactable = true;
        if (playersActive.Count == playersPanel.transform.childCount)
        {
            addPlayer.interactable = false;
        }
    }

    //public void StartButton()
    //{
    //    // Update stats for all the players
    //    foreach (Player player in playersActive)
    //    {
    //        //player.gamesTotal++;
    //        player.answersThisGame = 0;
    //        player.questionsThisGame = 0;
    //        player.pointsThisGame = 0;
    //    }

    //    playerSelector.gameObject.SetActive(false);
    //    playerController.NextPlayer();
    //}
}
