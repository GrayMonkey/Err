using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Menus = MenuHandler.MenuOverlay;

public class mnu_Winner : MonoBehaviour 
{
    //ToDo: Is this class required anymore?
    
    [SerializeField] Text winningPlayer;

    QuestionManager questionManager;
    PlayerController playerController;
    MenuHandler uiMenu;
    Player activePlayer;

    private void Awake()
    {
        questionManager = QuestionManager.instance;
        playerController = PlayerController.instance;
        uiMenu = MenuHandler.instance;
    }

    private void OnEnable()
    {
        activePlayer = playerController.activePlayer;
        winningPlayer.text = activePlayer.playerName;
    }

    public void CloseMenu(bool home)
    {
        if (home)
        {
            // Update player stats
            activePlayer.gamesWon++;
            activePlayer.questionsThisGame++;
            activePlayer.answersThisGame++;
            activePlayer.pointsThisGame += questionManager.activeQuestion.maxPoints;
            playerController.SavePlayerData();
//            uiMenu.CloseMenu(Menus.WinningPlayer);
//            uiMenu.ShowMenu(Menus.GameResults);
        } else {
//            uiMenu.CloseMenu(Menus.WinningPlayer);
//            uiMenu.ShowMenu(Menus.CorrectAnswer);
        }
    }
}
