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
    
    Color proceed = new Color(1.0f, 1.0f, 1.0f);
    Color select = new Color(1.0f, 0.7f, 0.7f);

    private void OnEnable()
    {
        gameManager = GameManager.gameManager;
        playerController = PlayerController.playerController;
    }

    // Update is called once per frame
    void Update()
    {
        bool players = false;
        bool cardsets = false;
        startGame.interactable = false;

        playerSelect.GetComponent<Image>().color = select;
        cardSetSelect.GetComponent<Image>().color = select;


        if (playerController.playersActive.Count > 0)
        {
            playerSelect.GetComponent<Image>().color = proceed;
            players = true;
        }

        if (gameManager.defaultCardSets.Count > 0)
        {
            cardSetSelect.GetComponent<Image>().color = proceed;
            cardsets = true;
        }

        if (players && cardsets)
            startGame.interactable = true;
    }

    public void SelectPlayers()
    {
        gameManager.SetGameState(gameManager.gameState.playerSelect);
    }

    public void SelectCardSets()
    {
        gameManager.SetGameState(gameManager.gameState.cardSetSelect);
    }

    public void StarGame()
    {
        gameManager.SetGameState(gameManager.gameState.question);
    }
}
