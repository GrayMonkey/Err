#pragma warning disable 649   // Disable [SerializeField] warnings CS0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Menus = MenuHandler.MenuOverlay;

public class CardTrad : MonoBehaviour
{
    [SerializeField] GameObject[] clues;
    [SerializeField] Text[] clueTexts;
    [SerializeField] Image[] numbers;
    [SerializeField] GameObject nextBtn;
    [SerializeField] GameObject failBtn;
    [SerializeField] Button timerBtn;
    [SerializeField] Timer[] timers;
    [SerializeField] Text word;
    [SerializeField] Text letter;
    [SerializeField] Text clue0;
    [SerializeField] Text clue1;
    [SerializeField] Text clue2;
    [SerializeField] Text clue3;
    [SerializeField] Text credit;

    GameManager gameManager;
    PlayerController playerController;
    //int currentClue;
    bool showAnswer = false;
    MenuHandler uiMenu;
    Question activeQuestion;

    Color current = new Color(0.8f, 1.0f, 0.8f);
    Color used = new Color(1.0f, 1.0f, 0.7f);
    Color hidden = new Color(1.0f, 0.7f, 0.7f);

    // Use this for initialization
    void Awake()
    {
        gameManager = GameManager.gameManager;
        playerController = PlayerController.playerController;
        uiMenu = MenuHandler.uiMenus;
    }

    private void OnEnable()
    {
        //activeQuestion = gameManager.activeQuestion;
        showAnswer = GameOptions.gameOptions.showAnswer;
        //currentClue = 4;
        SetUpCard();
    }

    // Set up the card according to the currnt question and game options
    public void SetUpCard()
    {
        activeQuestion = gameManager.activeQuestion;
        nextBtn.SetActive(true);
        failBtn.SetActive(false);
        timerBtn.interactable = true;
        credit.text = activeQuestion.credit;
        //        gameManager.activeQuestion.maxPoints = 4;


        /*        // Set the question word values
                letter.text = activeQuestion.word.Substring(0, 1);
                clue0.text = activeQuestion.clue4;
                clue1.text = activeQuestion.clue3;
                clue2.text = activeQuestion.clue2;
                clue3.text = activeQuestion.clue1;

                // Hide the clues yet to be given
                clues[0].SetActive(false);
                clues[1].SetActive(false);
                clues[2].SetActive(false);
                clues[3].SetActive(false);
        */
        // Set the clue texts
        clueTexts[0].text = activeQuestion.clue4;
        clueTexts[1].text = activeQuestion.clue3;
        clueTexts[2].text = activeQuestion.clue2;
        clueTexts[3].text = activeQuestion.clue1;

        //Hide the clues yet to be given
        clueTexts[0].gameObject.SetActive(false);
        clueTexts[1].gameObject.SetActive(false);
        clueTexts[2].gameObject.SetActive(false);
        clueTexts[3].gameObject.SetActive(false);


        // Recolour the number panels - they are current
        // colour because they are inactive
        numbers[0].color = current;
        numbers[1].color = current;
        numbers[2].color = current;
        numbers[3].color = current;

        // Set up the answer according to the options
        ToggleAnswer();

        // Change clues in case card style is changed halfway through
        // question using options
        for (int i = 0; i < 5 - gameManager.activeQuestion.maxPoints; i++)
        {
            ClueUpdate(i);
        }
    }

    void ClueUpdate(int clueID)
    {
        //clues[clueID].SetActive(true);
        clueTexts[clueID].gameObject.SetActive(true);

        if (clueID > 0)
        {
            numbers[clueID - 1].color = used;
        }

        if (gameManager.activeQuestion.maxPoints == 1)
        {
            ChangeNextFailButtons();
        }
    }

    public void NextClue()
    {
        gameManager.activeQuestion.maxPoints--;
        gameManager.activeQuestion.maxPoints = Mathf.Clamp(gameManager.activeQuestion.maxPoints--, 1, 4);
        ClueUpdate(4 - gameManager.activeQuestion.maxPoints);
        timerBtn.interactable = true;
 /*       foreach(Timer timer in timers)
        {
            timer.ResetTimer();
        }
 */   }

    private void ChangeNextFailButtons()
    {
        nextBtn.SetActive(false);
        failBtn.SetActive(true);
    }

    public void Answer()
    {
        uiMenu.ShowMenu(Menus.CorrectAnswer,this.gameObject);
        this.gameObject.SetActive(false);
    }

    public void Pass()
    {
        gameManager.activeQuestion.maxPoints = 0;
        // PlayerController.playerMoves = 0;
        uiMenu.ShowMenu(Menus.FailAnswer);
    }

    public void ToggleAnswer()
    {
        Color color = hidden;
        Image wordPanel = word.GetComponentInParent<Image>();
        string text = new string('*', activeQuestion.word.Length);
        int fontSize = 60;
        float rectBottom = -30f;

        if (showAnswer)
        {
            color = current;
            text = activeQuestion.word;
            fontSize = 40;
            rectBottom = 0f;
        }
       
        word.text = text;
        word.fontSize = fontSize;
        word.GetComponent<RectTransform>().offsetMin = new Vector2 (0f, rectBottom);
        wordPanel.color = color;

        // Toggle showWord for next call
        showAnswer = !showAnswer;
    }

    public void StartTimer()
    {
        int timerID = 4 - gameManager.activeQuestion.maxPoints;
        timers[timerID].StartTimer();
        timerBtn.interactable = false;
    }

    public void OptionsMenu()
    {
        uiMenu.ShowMenu(Menus.Options);
    }
}
