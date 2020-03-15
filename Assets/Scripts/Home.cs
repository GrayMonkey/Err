using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Home : MonoBehaviour
{
    [SerializeField] Button playerSelect = default;
    [SerializeField] Button cardSetSelect = default;
    [SerializeField] Button startGame = default;

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
        playerBtnImage.color = select;
        playerBtnText.text = "0";
        cardSetBtnImage.color = select;
        cardSetBtnText.text = "0";

        if (playerController.playersActive.Count > 0)
        {
            if (playerController.playersActive.Count > 1) playerBtnImage.color = proceed;
            playerBtnText.text = playerController.playersActive.Count.ToString();
            players = true;
        }

        if (gameManager.defaultCardSets.Count > 0)
        {
            cardSetBtnImage.color = proceed;
            cardSetBtnText.text = gameManager.defaultCardSets.Count.ToString();
            cardsets = true;
        }

        if (players && cardsets)
            startGame.interactable = true;
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
