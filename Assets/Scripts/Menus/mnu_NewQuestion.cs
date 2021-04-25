#pragma warning disable 649   // Disable [SerializeField] warnings CS0649

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
    GameManager gameManager;
    PlayerController playerController;
    QuestionManager questionManager;
    MenuHandler uiMenus;

    private void Awake()
    {
        gameManager = GameManager.instance;
        playerController = PlayerController.instance;
        questionManager = QuestionManager.instance;
        uiMenus = MenuHandler.instance;
    }

    private void OnEnable()
    {
        //set the next question
        questionManager.GetNewQuestion();

        //fill the menu
        playerName.text = playerController.activePlayer.playerName;
        deckName.text = LocManager.instance.GetLocText(questionManager.activeCardSet.cardSetTitleKey);
        deckIcon.sprite = questionManager.activeCardSet.cardSetIcon.sprite;
    }

    public void CloseMenu()
    {
        uiMenus.CloseMenu(Menu.NewQuestion);
        gameManager.SetGameState(gameManager.gameState.question);
    }
}