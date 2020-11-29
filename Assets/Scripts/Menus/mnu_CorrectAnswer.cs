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
    [SerializeField] int points = 0;

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
        points = gameManager.activeQuestion.maxPoints; //comment out for debug only
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
        playerController.NextPlayer();

        CloseMenu();
        uiMenus.ShowMenu(Menus.NewQuestion);
    }

    public void Winner()
    {
        CloseMenu();
        uiMenus.ShowMenu(Menus.WinningPlayer);
    }
}
