﻿#pragma warning disable 649   // Disable [SerializeField] warnings CS0649

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
    public int newPlayerCount = 0;
    public bool loadPlayerActive = false;
    public PlayerObject hasFocus;

    [SerializeField] private GameObject playersPanelContent;
    [SerializeField] private GameObject playerObject;
    [SerializeField] private Text playerCountLabel;
    [SerializeField] private Button loadPlayers;
    [SerializeField] private Button addPlayer;
    [SerializeField] private Button startGame;

    PlayerController playerController;
    GameOptions gameOptions;
    MenuHandler uiMenus;
    List<Player> playerData = new List<Player>();

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
        loadPlayers.interactable = false;
    }

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
        hasFocus = null;
    }

    // Activate a new player button object
    public void ActivatePlayerButton(Player player)
    {
        playerController.activePlayer = player;
        GameObject newPlayerButton = Instantiate(playerObject, playersPanelContent.transform);
        newPlayerButton.GetComponent<PlayerObject>().refPlayer = player;
        if (!playersActive.Contains(player)) { playersActive.Add(player); }
    }

    public void UpdatePlayerList()
    {
        playersActive.Clear();

        for (int i = 0; i < playersPanelContent.transform.childCount; i++)
        {
            PlayerObject playerButton = playersPanelContent.transform.GetChild(i).gameObject.GetComponent<PlayerObject>();
            if (playerButton != null)
            {
                playerButton.AddToPlayerList();
            }
        }

 
        // Deactivate the add player button if playersActive.count is 10
        addPlayer.interactable = true;
        if (playersActive.Count == playersPanelContent.transform.childCount)
        {
            addPlayer.interactable = false;
        }
    }
}
