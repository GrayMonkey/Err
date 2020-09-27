using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Home : MonoBehaviour
{
    [SerializeField] Text instructions;
    [SerializeField] Button playerSelect;
    [SerializeField] Button cardSetSelect;
    [SerializeField] Button startGame;

    GameManager gameManager;
    PlayerController playerController;
    
    Color proceed = new Color(0.7f, 1.0f, 0.4f);
    Color select = new Color(1.0f, 0.7f, 0.7f);
    Image playerBtnImage;
    Image cardSetBtnImage;
    Text playerBtnText;
    Text cardSetBtnText;

    private void Awake()
    {
        gameManager = GameManager.gameManager;
        playerController = PlayerController.playerController;
        playerBtnImage = playerSelect.GetComponent<Image>();
        cardSetBtnImage = cardSetSelect.GetComponent<Image>();
        playerBtnText = playerSelect.GetComponentInChildren<Text>();
        cardSetBtnText = cardSetSelect.GetComponentInChildren<Text>();
    }

    void OnEnable()
    {
        bool players = false;
        bool cardsets = false;
        startGame.interactable = false;
        cardSetBtnImage.color = select;
        cardSetBtnText.text = gameManager.defaultCardSets.Count.ToString();
        playerBtnImage.color = select;
        playerBtnText.text = playerController.playersActive.Count.ToString();
        playerSelect.interactable = false;

        instructions.text = LocManager.locManager.GetLocText("UI_CardSetSelect");

        if (gameManager.defaultCardSets.Count > 0)
        {
            cardSetBtnImage.color = proceed;
            cardsets = true;
            instructions.text = LocManager.locManager.GetLocText("str_BtnHelpSelectPlayers");
            playerSelect.interactable = true;
        }

        if (playerController.playersActive.Count > 1 && cardsets)
        {
            if (playerController.playersActive.Count > 1) 
                playerBtnImage.color = proceed;
            players = true;
        }

        if (players && cardsets)
        {
            instructions.text = "";
            startGame.interactable = true;
        }
    }

    public void StartGame()
    {
        this.gameObject.SetActive(false);
        playerController.StartGame();
    }

    //public void SelectPlayers()
    //{
    //    gameManager.SetGameState(gameManager.gameState.playerSelect);
    //}

    //public void SelectCardSets()
    //{
    //    gameManager.SetGameState(gameManager.gameState.cardSetSelect);
    //}

    //public void StartGame()
    //{
    //    gameManager.SetGameState(gameManager.gameState.question);
    //}
}
