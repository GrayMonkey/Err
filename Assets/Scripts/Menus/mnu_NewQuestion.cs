﻿#pragma warning disable 649   // Disable [SerializeField] warnings CS0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Menu = MenuHandler.MenuOverlay;

public class mnu_NewQuestion : MonoBehaviour
{
    [SerializeField] Text playerName;
    [SerializeField] Text deckName;
    [SerializeField] Image deckIcon;
    PlayerController playerController;
    GameManager gameManager;
    MenuHandler uiMenus;

    private void Awake()
    {
        playerController = PlayerController.playerController;
        gameManager = GameManager.gameManager;
        uiMenus = MenuHandler.uiMenus;
    }

    private void OnEnable()
    {
        //set the next question
        playerController.GetNewQuestion();

        //fill the menu
        playerName.text = playerController.activePlayer.playerName;
        deckName.text = gameManager.activeCardSet.name;
        deckIcon.sprite = gameManager.activeCardSet.cardSetIcon.sprite;
    }

    public void CloseMenu()
    {
        uiMenus.CloseMenu(Menu.NewQuestion);
        gameManager.SetGameState(gameManager.gameState.question);
    }
}