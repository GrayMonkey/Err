
using UnityEngine;
using UnityEngine.UI;
using Menus = MenuHandler.MenuOverlay;

public class QuestionResult : MonoBehaviour
{
    [SerializeField] GameObject correctAnswer;
    [SerializeField] GameObject failAnswer;
    [SerializeField] Text score;
    [SerializeField] Stars stars;
    [SerializeField] int points = 0;

    GameManager gameManager;
    QuestionManager questionManager;
    CardSetCollection csCollection;
    MenuHandler uiMenus;

    private void Awake()
    {
        gameManager = GameManager.instance;
        questionManager = QuestionManager.instance;
        csCollection = CardSetCollection.instance;
        uiMenus = MenuHandler.instance;
    }
    private void OnEnable()
    {
        correctAnswer.SetActive(false);
        failAnswer.SetActive(false);
        ValidateAnswer();
    }

    void ValidateAnswer()
    {
        if (questionManager.activeQuestion.maxPoints>0)
        {
            correctAnswer.SetActive(true);
            points = questionManager.activeQuestion.maxPoints; //comment out for debug only
            score.text = points.ToString();
            stars.LaunchStars(points);
        }
        else
        {
            failAnswer.SetActive(true);
        }
    }

    public void Home()
    {
        gameManager.SetGameState(gameManager.gameState.cardSetCollection);
    }

    public void NextQuestion()
    {
        questionManager.GetNewQuestion();
    }
}