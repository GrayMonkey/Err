using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// CardSets should be added to the Manager game object under the
// Card Set Manager component
public class QuestionManager : MonoBehaviour
{
    public static QuestionManager instance;

    // Only list support Languages with active cardsets
    [Header("Defaults Language CardSets")]
    [SerializeField] CardSet csDefault_EN;
    [SerializeField] CardSet csDefault_FR;
    [Space(10)]

    public CardSet csDefault;
    public List<CardSet> csAll = new List<CardSet>();
/*    public List<CardSet> csGame = new List<CardSet>();
    public List<CardSet> csShop = new List<CardSet>();
*/    public Question activeQuestion;
    public CardSet activeCardSet;
    public List<CardSet> playableCardSets = new List<CardSet>();

    PlayerController playerController;


    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        SetDefaultCardSet();
    }

    private void Start()
    {
        playerController = PlayerController.instance;
    }


    public void GetNewQuestion()
    {
        CardSet questionSet;
        Player activePlayer = playerController.activePlayer;

        int i = UnityEngine.Random.Range(0, activePlayer.cardSets.Count-1);
        questionSet = activePlayer.cardSets[i];
        questionSet.GetQuestion();
    }

    public void SetDefaultCardSet()
    {
        switch (LocManager.instance.GameLang)
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

}
