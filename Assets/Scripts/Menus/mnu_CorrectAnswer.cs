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

    QuestionManager questionManager;
    PlayerController playerController;
    MenuHandler uiMenus;
    Player activePlayer;

    private void Awake()
    {
        playerController = PlayerController.instance;
        questionManager = QuestionManager.instance;
        uiMenus = MenuHandler.instance;
    }

    private void OnEnable()
    {
        points = questionManager.activeQuestion.maxPoints; //comment out for debug only
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
        activePlayer.pointsThisGame += questionManager.activeQuestion.maxPoints;
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
