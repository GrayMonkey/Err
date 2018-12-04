#pragma warning disable 649   // Disable [SerializeField] warnings CS0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Menus = MenuHandler.MenuOverlay;

public class CardMod : MonoBehaviour
{
    [SerializeField] GameObject[] clues;
    [SerializeField] Button[] buttons;
    [SerializeField] Slider wordSlider;
    [SerializeField] GameObject nextBtn;
    [SerializeField] GameObject failBtn;
    [SerializeField] Text word;
    [SerializeField] Text hiddenWord;
    [SerializeField] Text letter;
    [SerializeField] Text clue4;
    [SerializeField] Text clue3;
    [SerializeField] Text clue2;
    [SerializeField] Text clue1;

    GameManager gameManager;
    GameOptions gameOptions;
    PlayerController playerController;
    MenuHandler uiMenus;
    Question activeQuestion;
    Vector3 scale;
    int lastClueID;
    int cluePoints;
    bool showAnswer = false;

    Color unused = new Color(1.0f, 0.7f, 0.7f);
    Color unusedText = new Color(0.4f, 0.4f, 0.4f);
    Color used = new Color(1.0f, 1.0f, 0.7f);
    Color current = new Color(0.8f, 1.0f, 0.8f);

    // Use this for initialization
    void Awake()
    {
        gameOptions = GameOptions.gameOptions;
        gameManager = GameManager.gameManager;
        uiMenus = MenuHandler.uiMenus;
        playerController = PlayerController.playerController;
        activeQuestion = gameManager.activeQuestion;
    }

    private void OnEnable()
    {
        //activeQuestion = gameManager.activeQuestion;
        showAnswer = gameOptions.showAnswer;
        SetUpCard();
    }

    // Set up the card according to the currnt question and game options
    public void SetUpCard()
    {
        activeQuestion = gameManager.activeQuestion;
        nextBtn.SetActive(true);
        failBtn.SetActive(false);
        cluePoints = 4;
        lastClueID = 0;
        word.text = activeQuestion.word;
        hiddenWord.text = new string('*', activeQuestion.word.Length);

        // Set the question word values
        letter.text = activeQuestion.word.Substring(0, 1);
        clues[0].GetComponent<Text>().text = activeQuestion.clue4;
        clues[1].GetComponent<Text>().text = activeQuestion.clue3;
        clues[2].GetComponent<Text>().text = activeQuestion.clue2;
        clues[3].GetComponent<Text>().text = activeQuestion.clue1;
 
        // Hide the clues yet to be given
        foreach (GameObject clue in clues)
        {
            clue.SetActive(false);
        }

        // Reset the buttons
        foreach (Button button in buttons)
        {
            scale = button.transform.localScale;
            scale.Set(1.0f, 1.0f, 1.0f);
            button.transform.localScale = scale;
            button.transform.GetComponent<Image>().color = unused;
            button.transform.GetComponentInChildren<Text>().fontStyle = FontStyle.Italic;
            button.transform.GetComponentInChildren<Text>().color = unusedText;
        }

        // Set up the answer so that it's hidden
        wordSlider.value = 0.0f;
        if(gameOptions.showAnswer)
        {
            wordSlider.value = 1.0f;
        }

        // Set up the relevant button and clue
        SetClue(0);

        // Iterate through SetClue() if card is changed in options and
        // activePlayer has already seen clues
        if (activeQuestion.maxPoints < 4)
        {
            for (int i = 0; i < 4 - activeQuestion.maxPoints; i++)
            {
                NextClue();
            }
        }
    }

    public void NextClue()
    {
        int clueID = lastClueID + 1;
        SetClue(clueID);
    }

    public void SetClue(int clueID)
    {
        // Reset up the last clue to used
        clues[lastClueID].SetActive(false);
        scale = buttons[lastClueID].transform.localScale;
        scale.Set(1.0f, 1.0f, 1.0f);
        buttons[lastClueID].transform.localScale = scale;
        buttons[lastClueID].transform.GetComponent<Image>().color = used;

        // Set up the new clue
        clues[clueID].SetActive(true);
        scale.Set(1.2f, 1.2f, 1.0f);
        buttons[clueID].transform.localScale = scale;
        buttons[clueID].transform.localScale.Set(2.0f, 2.0f, 1.0f);
        buttons[clueID].transform.GetComponent<Image>().color = current;
        buttons[clueID].transform.GetComponentInChildren<Text>().color = Color.black;
        buttons[clueID].transform.GetComponentInChildren<Text>().fontStyle = FontStyle.Normal;

        // Update moves if the clue is easier
        if (gameManager.activeQuestion.maxPoints > 4 - clueID)
        {
            gameManager.activeQuestion.maxPoints = 4 - clueID;
        }

        // Change the Next Button if down to last clue
        if (gameManager.activeQuestion.maxPoints == 1)
        {
            ChangeNextFailButtons();
        }

        lastClueID = clueID;
    }

    public void Pass()
    {
        gameManager.activeQuestion.maxPoints = 0;
        uiMenus.ShowMenu(Menus.FailAnswer);
    }

    public void Answer()
    {
        //playerController.activePlayer.currentMoves = cluePoints;
        uiMenus.ShowMenu(Menus.CorrectAnswer);
    }

    private void ChangeNextFailButtons()
    {
        nextBtn.SetActive(false);
        failBtn.SetActive(true);
    }

    public void OptionsMenu()
    {
        uiMenus.ShowMenu(Menus.Options);
    }
}
