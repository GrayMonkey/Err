#pragma warning disable 649   // Disable [SerializeField] warnings CS0649

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Menus = MenuHandler.MenuOverlay;

public class mnu_CorrectAnswer : MonoBehaviour
{
    //ToDo: Is this class required anymore?
    
    [SerializeField] Text score;
    [SerializeField] Stars stars;
    [SerializeField] int points = 0;

    GameManager gameManager;
    CardSetCollection csCollection;
    QuestionManager questionManager;
    MenuHandler uiMenus;

    private void Awake()
    {
        gameManager = GameManager.instance;
        csCollection = CardSetCollection.instance;
        questionManager = QuestionManager.instance;
        uiMenus = MenuHandler.instance;
    }

    private void OnEnable()
    {
        points = questionManager.activeQuestion.maxPoints; //comment out for debug only
        score.text = points.ToString();
        stars.LaunchStars(points);
    }

        public void Home()
    {
        gameManager.SetGameState(gameManager.gameState.cardSetCollection);
//        uiMenus.CloseMenu(Menus.CorrectAnswer);
    }

    public void NextQuestion()
    {
//        csCollection.SelectCardSet(); ;
//        uiMenus.CloseMenu(Menus.CorrectAnswer);
    }
}
