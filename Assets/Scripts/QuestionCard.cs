using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Menus = MenuHandler.MenuOverlay;

public class QuestionCard : MonoBehaviour
{
    [SerializeField] Text[] clueTextsEasyRead;
    [SerializeField] Text[] clueTextsAllAtOnce;
    [SerializeField] GameObject goCluesEasyRead;
    [SerializeField] GameObject goCluesAllAtOnce;
    [SerializeField] Timer[] timers;               // 0-3 should be AllAtOnce timers, 4 should be EasyRead timer
    [SerializeField] GameObject nextBtn;
    [SerializeField] GameObject failBtn;
    [SerializeField] Button[] buttonsEasyRead;
    [SerializeField] Button[] buttonsAllAtOnce;
    [SerializeField] Button timerBtn;
    [SerializeField] Slider wordSlider;
    [SerializeField] Text word;
    [SerializeField] Text hiddenWord;
    [SerializeField] Text letter;
    [SerializeField] private Text credit;

    QuestionManager questionManager;
    GameOptions gameOptions;
    MenuHandler uiMenus;
    Question activeQuestion;
    Vector3 scale;
    int lastClueID;
    bool showAnswer = false;

    Color unused = new Color(0.8f, 0.9f, 1.0f);
    Color unusedText = new Color(0.4f, 0.4f, 0.4f);
    Color used = new Color(1.0f, 1.0f, 0.7f);
    Color current = new Color(0.8f, 1.0f, 0.8f);

    // Use this for initialization
    void Awake()
    {
        gameOptions = GameOptions.instance;
        questionManager = QuestionManager.instance;
        uiMenus = MenuHandler.instance;
        activeQuestion = questionManager.activeQuestion;
    }

    private void OnEnable()
    {
        showAnswer = gameOptions.showAnswer;
        SetUpCard();
    }

    // Set up the card according to the current question and game options
    public void SetUpCard()
    {
        activeQuestion = questionManager.activeQuestion;
        //nextBtn.SetActive(true);
        //failBtn.SetActive(false);
        lastClueID = 0;
        word.text = activeQuestion.word;
        hiddenWord.text = new string('*', activeQuestion.word.Length);
        credit.text = questionManager.activeQuestion.credit;

        SetCluePanel();

        // Set the question word values
        letter.text = activeQuestion.word.Substring(0, 1);

        clueTextsEasyRead[0].text = activeQuestion.clue4;

        clueTextsEasyRead[0].text = activeQuestion.clue4;
        clueTextsEasyRead[1].text = activeQuestion.clue3;
        clueTextsEasyRead[2].text = activeQuestion.clue2;
        clueTextsEasyRead[3].text = activeQuestion.clue1;

        clueTextsAllAtOnce[0].text = activeQuestion.clue4;
        clueTextsAllAtOnce[1].text = activeQuestion.clue3;
        clueTextsAllAtOnce[2].text = activeQuestion.clue2;
        clueTextsAllAtOnce[3].text = activeQuestion.clue1;

        // Hide the clues yet to be given
        clueTextsEasyRead[0].gameObject.SetActive(false);
        clueTextsEasyRead[1].gameObject.SetActive(false);
        clueTextsEasyRead[2].gameObject.SetActive(false);
        clueTextsEasyRead[3].gameObject.SetActive(false);

        clueTextsAllAtOnce[0].gameObject.SetActive(false);
        clueTextsAllAtOnce[1].gameObject.SetActive(false);
        clueTextsAllAtOnce[2].gameObject.SetActive(false);
        clueTextsAllAtOnce[3].gameObject.SetActive(false);

        // Reset the buttons
        foreach (Button button in buttonsEasyRead)
        {
            scale = button.transform.localScale;
            scale.Set(1.0f, 1.0f, 1.0f);
            button.transform.localScale = scale;
            button.transform.GetComponent<Image>().color = unused;
            button.transform.GetComponentInChildren<Text>().fontStyle = FontStyle.Italic;
            button.transform.GetComponentInChildren<Text>().color = unusedText;
        }

        foreach (Button button in buttonsAllAtOnce)
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
        // No longer called, clue buttons must be touched to activate clue. This has been added
        // to support One Shot questions
        // SetClue(0);
    }

    public void SetCluePanel()
    {
        if (gameOptions == null)
            return;

        goCluesEasyRead.SetActive(gameOptions.easyRead);
        goCluesAllAtOnce.SetActive(!gameOptions.easyRead);
    }

/*    // Removed as clues now manually started
 *    public void NextClue()
    {
        if(lastClueID == 3)
        {
            Fail();
            return;
        }

        int clueID = lastClueID + 1;
        SetClue(clueID);
        StartTimer();
    }
*/
    private void ResetTimers()
    {
        foreach (Timer timer in timers)
            timer.ResetTimer();
    }

    public void StartTimer(int clueID)
    {
        int timerID = clueID;
        //timerID = 4 - questionManager.activeQuestion.maxPoints;

        if (gameOptions.easyRead)
            timerID = 4;
        
        if (timerID > 0)
            timers[timerID - 1].ResetTimer();

        timers[timerID].SetTimer();
    }

    public void SetClue(int clueID)
    {
        // Rest all timers in case one is currently running when clue is activated
        foreach (Timer timer in timers)
            timer.ResetTimer();
        
        // Reset up the last clue to used
        clueTextsEasyRead[lastClueID].gameObject.SetActive(false);
        scale = buttonsEasyRead[lastClueID].transform.localScale;
        scale.Set(1.0f, 1.0f, 1.0f);
        buttonsEasyRead[lastClueID].transform.localScale = scale;
        buttonsEasyRead[lastClueID].transform.GetComponent<Image>().color = used;

        //clueTextsAllAtOnce[lastClueID].gameObject.SetActive(false);
        scale = buttonsAllAtOnce[lastClueID].transform.localScale;
        scale.Set(1.0f, 1.0f, 1.0f);
        buttonsAllAtOnce[lastClueID].transform.localScale = scale;
        buttonsAllAtOnce[lastClueID].transform.GetComponent<Image>().color = used;

        // Set up the new clue
        clueTextsEasyRead[clueID].gameObject.SetActive(true);
        scale.Set(1.2f, 1.2f, 1.0f);
        buttonsEasyRead[clueID].transform.localScale = scale;
        buttonsEasyRead[clueID].transform.localScale.Set(2.0f, 2.0f, 1.0f);
        buttonsEasyRead[clueID].transform.GetComponent<Image>().color = current;
        buttonsEasyRead[clueID].transform.GetComponentInChildren<Text>().color = Color.black;
        buttonsEasyRead[clueID].transform.GetComponentInChildren<Text>().fontStyle = FontStyle.Normal;

        clueTextsAllAtOnce[clueID].gameObject.SetActive(true);
        scale.Set(1.2f, 1.2f, 1.0f);
        buttonsAllAtOnce[clueID].transform.localScale = scale;
        buttonsAllAtOnce[clueID].transform.localScale.Set(2.0f, 2.0f, 1.0f);
        buttonsAllAtOnce[clueID].transform.GetComponent<Image>().color = current;
        buttonsAllAtOnce[clueID].transform.GetComponentInChildren<Text>().color = Color.black;
        buttonsAllAtOnce[clueID].transform.GetComponentInChildren<Text>().fontStyle = FontStyle.Normal;

        // Update moves if the clue is easier
        if (questionManager.activeQuestion.maxPoints > 4 - clueID)
        {
            questionManager.activeQuestion.maxPoints = 4 - clueID;
        }

/*        // Change the Next Button if down to last clue
        if (questionManager.activeQuestion.maxPoints == 1)
            ChangeNextFailButtons();
*/
        lastClueID = clueID;
        StartTimer(clueID);
    }

    public void Fail()
    {
        ResetTimers();
        questionManager.activeQuestion.maxPoints = 0;
        uiMenus.ShowMenu(Menus.FailAnswer);
        this.gameObject.SetActive(false);
    }

    public void Answer()
    {
        ResetTimers();
        uiMenus.ShowMenu(Menus.CorrectAnswer);
        this.gameObject.SetActive(false);
    }

/*    private void ChangeNextFailButtons()
    {
        nextBtn.SetActive(false);
        failBtn.SetActive(true);
    }
*/
    public void OptionsMenu()
    {
        uiMenus.ShowMenu(Menus.Options);
    }
}
