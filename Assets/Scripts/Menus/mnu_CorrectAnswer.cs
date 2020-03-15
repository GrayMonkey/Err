#pragma warning disable 649   // Disable [SerializeField] warnings CS0649

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Menus = MenuHandler.MenuOverlay;

public class mnu_CorrectAnswer : MonoBehaviour
{
    [SerializeField] Text playerName;
    [SerializeField] Text moves;
    [SerializeField] Stars stars;

    GameManager gameManager;
    PlayerController playerController;
    MenuHandler uiMenus;
    Player activePlayer;

    private void Awake()
    {
        gameManager = GameManager.gameManager;
        playerController = PlayerController.playerController;
        uiMenus = MenuHandler.uiMenus;
    }

    private void OnEnable()
    {
        int points = gameManager.activeQuestion.maxPoints;
        moves.text = points.ToString();
        activePlayer = playerController.activePlayer;
        playerName.text = activePlayer.playerName;
        stars.LaunchStars(points);
    }

    public void CloseMenu()
    {
        uiMenus.CloseMenu(Menus.CorrectAnswer);
    }

    public void NextPlayer()
    {
        activePlayer.questionsThisGame++;
        activePlayer.answersThisGame++;
        activePlayer.pointsThisGame += gameManager.activeQuestion.maxPoints;
        CloseMenu();
        playerController.NextPlayer();
    }

    public void Winner()
    {
        CloseMenu();
        uiMenus.ShowMenu(Menus.WinningPlayer);
    }
}
