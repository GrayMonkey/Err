using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mnu_GameResults : MonoBehaviour
{
    [SerializeField] Text playerWinner;
    [SerializeField] Text labelPoints;
    [SerializeField] Text playerPoints;
    [SerializeField] Text labelAnswers;
    [SerializeField] Text playerAnswers;

    PlayerController playerController;
    GameManager gameManager;
    LocManager locManager;
    string origLabelPoints;
    string origLabelAnswers;

    private void Awake()
    {
        gameManager = GameManager.instance;
        playerController = PlayerController.instance;
        locManager = LocManager.instance;
        origLabelPoints = labelPoints.text;
        origLabelAnswers = labelAnswers.text;
    }

    private void OnEnable()
    {
        // set the winning player
        playerWinner.text = playerController.activePlayer.playerName;

        // find the player(s) with the highest number of points and answers
        int maxPoints = 0;
        float maxAnswers = 0;
        string pointNames = "";
        string answerNames = "";

        foreach (Player player in playerController.playersActive)
        {
            if (player.pointsThisGame == maxPoints)
                pointNames += "\n" + player.playerName;

            if (player.pointsThisGame > maxPoints)
            {
                maxPoints = player.pointsThisGame;
                pointNames = player.playerName;
            }

            float questions = player.questionsThisGame;
            float answers = player.answersThisGame;
            float percentage = answers / questions;

            if (percentage == maxAnswers)
                answerNames += "\n" + player.playerName;

            if (percentage > maxAnswers)
            {
                maxAnswers = percentage;
                answerNames = player.playerName;
            }

            // Update the text fields
            playerPoints.text = pointNames;
            labelPoints.text = locManager.GetLocText(origLabelPoints) + maxPoints.ToString() + "):";

            playerAnswers.text = answerNames;
            labelAnswers.text = locManager.GetLocText(origLabelAnswers) + percentage.ToString("p0") + "):";

        }
    }

    public void ReturnHome()
    {
        labelPoints.text = origLabelPoints;
        labelAnswers.text = origLabelAnswers;
        gameManager.SetGameState(gameManager.gameState.cardSetSelect);
        MenuHandler.instance.CloseMenu(MenuHandler.MenuOverlay.GameResults);
    }
}