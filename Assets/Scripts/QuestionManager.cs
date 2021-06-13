using System.Collections.Generic;
using UnityEngine;
using Menu = MenuHandler.MenuOverlay;

// CardSets should be added to the Manager game object under the
// Card Set Manager component
public class QuestionManager : MonoBehaviour
{
    public static QuestionManager instance;

/*    // Only list support Languages with active cardsets
    [Header("Defaults Language CardSets")]
    [SerializeField] CardSet csDefault_EN;
    [SerializeField] CardSet csDefault_FR;
    [Space(10)]
*/
//    public CardSet csDefault;
    public List<CardSet> csAll = new List<CardSet>();
    public Question activeQuestion;
    public CardSet activeCardSet;
    public List<CardSet> playableCardSets = new List<CardSet>();
    public bool currentQuestionCorrect = false;
    public bool randomCardSets = false;

    private MenuHandler uiMenus;
    private GameManager gameManager;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        gameManager = GameManager.instance;
        uiMenus = MenuHandler.instance;
        CheckCardSets();
//        SetDefaultCardSet();
    }

    void CheckCardSets()
    {
        for (int i = 0; i < csAll.Count; i++)
            if (csAll[i] == null)
                Debug.LogError("Missing CardSet reference from QuestionManager.csAll");
    }

/*    public void SetDefaultCardSet()
    {
        // Only two languages with CardSets at the moment is French and English
        switch (GameOptions.instance.gameLang)
        {
            case SystemLanguage.French:
                csDefault = csDefault_FR;
                break;

            default:
                csDefault = csDefault_EN;
                break;
        }

        if (!playableCardSets.Contains(csDefault))
            playableCardSets.Add(csDefault);
    }
*/
    /*    public void GetNewQuestion()
        {
            *//*        CardSet questionSet;
                    Player activePlayer = playerController.activePlayer;

                    int i = UnityEngine.Random.Range(0, activePlayer.cardSets.Count-1);
                    questionSet = activePlayer.cardSets[i];
                    questionSet.GetQuestion();
            *//*
    //        uiMenus.ShowMenu(Menu.NextQuestion);
        }
    */
    public void GetNewQuestion()
    {
        if (playableCardSets.Count == 1)
        {
            SetNewQuestion(playableCardSets[0]);
        }
        else if (randomCardSets)
        {
            int i = Random.Range(0, playableCardSets.Count);
            SetNewQuestion(playableCardSets[i]);
        }
        else
        {
            gameManager.SetGameState(gameManager.gameState.selectCardSet);
        }
    }
    
    public void SetNewQuestion(CardSet cardSet)
    {
        cardSet.GetQuestion();
        gameManager.SetGameState(gameManager.gameState.questionCard);
    }
}

