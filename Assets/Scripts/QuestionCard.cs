using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Menus = MenuHandler.MenuOverlay;

public class QuestionCard : MonoBehaviour
{
    public static QuestionCard instance;
    public Button btnCorrect;

    [SerializeField] Text[] clueTextsEasyRead;
    [SerializeField] Text[] clueTextsAllAtOnce;
    [SerializeField] GameObject goCluesEasyRead;
    [SerializeField] GameObject goCluesAllAtOnce;
    [SerializeField] Text word;
    [SerializeField] Text hiddenWord;
    [SerializeField] Text letter;
    [SerializeField] Timer[] timers;               // 0-3 should be AllAtOnce timers, 4 should be EasyRead timer
    [SerializeField] Button[] buttonsEasyRead;
    [SerializeField] Button[] buttonsAllAtOnce;
    [SerializeField] Slider wordSlider;
    [SerializeField] Button btnFail;
    [SerializeField] Text cardSet;
    [SerializeField] Text credit;

    GameManager gameManager;
    QuestionManager questionManager;
    GameOptions gameOptions;
    MenuHandler uiMenus;
    Question activeQuestion;
    Vector3 scale;
    bool[] timerActivated = new bool[4];
    int lastClueID;

    Color unused = new Color(0.8f, 0.9f, 1.0f);
    Color unusedText = new Color(0.4f, 0.4f, 0.4f);
    Color used = new Color(1.0f, 1.0f, 0.7f);
    Color current = new Color(0.8f, 1.0f, 0.8f);

    // Use this for initialization
    void Awake()
    {
        instance = this;
        gameManager = GameManager.instance;
        gameOptions = GameOptions.instance;
        questionManager = QuestionManager.instance;
        uiMenus = MenuHandler.instance;
//        activeQuestion = questionManager.activeQuestion;
    }

    private void Start()
    {
        //SetCluePanel();
        //NextQuestion();
    }

    private void OnEnable()
    {
        for (int i = 0; i < timerActivated.Length; i++)
            timerActivated[i] = false;
        SetUpCard();
    }

/*    public void NextQuestion()
    {
        ClearCard();
        uiMenus.ShowMenu(Menus.NextQuestion);
        //questionManager.GetNewQuestion();
        //SetUpCard();
    }
*/
/*    private void ClearCard()
    {
        word.text = "";
        hiddenWord.text = "";
        letter.text = "";

        clueTextsEasyRead[0].text = "";

        clueTextsEasyRead[0].text = "";
        clueTextsEasyRead[1].text = "";
        clueTextsEasyRead[2].text = "";
        clueTextsEasyRead[3].text = "";

        clueTextsAllAtOnce[0].text = "";
        clueTextsAllAtOnce[1].text = "";
        clueTextsAllAtOnce[2].text = "";
        clueTextsAllAtOnce[3].text = "";
    }
*/
    public void SetUpCard()
    {
        activeQuestion = questionManager.activeQuestion;
        btnCorrect.interactable = false;
        btnFail.interactable = false;
        lastClueID = 0;
        cardSet.text = questionManager.activeCardSet.cardsetTitle.text;
        credit.text = questionManager.activeQuestion.credit;

        SetCluePanel();

        // Set the question word values
        word.text = activeQuestion.word;
        hiddenWord.text = new string('*', activeQuestion.word.Length);
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

        // Turn off all timers
        foreach (Timer timer in timers)
            timer.gameObject.SetActive(false);

        // Mark the clue as being activated


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
/*    private void ResetTimers()
    {
        foreach (Timer timer in timers)
            timer.ResetTimer();
    }

*/    public void StartTimer(int clueID)
    {
        int timerID = clueID;
        //timerID = 4 - questionManager.activeQuestion.maxPoints;

        if (gameOptions.easyRead)
            timerID = 4;
        
/*        if (timerID > 0)
            timers[timerID - 1].ResetTimer();
*/
        timers[timerID].gameObject.SetActive(true);
    }

    public void SetClue(int clueID)
    {
/*        // Diabsle the last clue timer
        timers[4].gameObject.SetActive(false);  // If easy read clues are on
        //if (lastClueID != null)
            timers[lastClueID].gameObject.SetActive(false);
        
*//*        // Rest all timers in case one is currently running when clue is activated
        foreach (Timer timer in timers)
            timer.ResetTimer();
*/        
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

        // If the clue has already been read then don't run the timer
        if (timerActivated[clueID] == false)
        {
            timerActivated[clueID] = true;
            // Update the score if the clue is easier and start the timer
            int maxPoints = 4 - clueID;
            if (questionManager.activeQuestion.maxPoints > maxPoints)
                questionManager.activeQuestion.maxPoints = maxPoints;
            if (gameOptions.easyRead)
                timers[4].gameObject.SetActive(true);
            else
                timers[clueID].gameObject.SetActive(true);
        }

        /*        // Change the Next Button if down to last clue
                if (questionManager.activeQuestion.maxPoints == 1)
                    ChangeNextFailButtons();
        */
        // Activate the fail and correct buttons
        btnCorrect.interactable = true;
        btnFail.interactable = true;

        lastClueID = clueID;
    }

    public void SetAnswer(bool correct)
    {
        if(!correct)
            questionManager.activeQuestion.maxPoints = 0;
        gameManager.SetGameState(gameManager.gameState.questionResult);
    }
/*    public void AnswerCorrect()
    {
        //ResetTimers();
//        uiMenus.ShowMenu(Menus.CorrectAnswer);
        gameObject.SetActive(false);
    }

    public void AnswerWrong()
    {
//        uiMenus.ShowMenu(Menus.FailAnswer);
        gameObject.SetActive(false);
    }
*/
    public void OptionsMenu()
    {
        uiMenus.ShowMenu(Menus.Options);
    }
}
