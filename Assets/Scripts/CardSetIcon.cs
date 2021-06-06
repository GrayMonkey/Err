using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Menu = MenuHandler.MenuOverlay;

public class CardSetIcon : MonoBehaviour
{
    public CardSet cardSet;
    [SerializeField] GameObject selectCardSet;
    [SerializeField] Text title;
    [SerializeField] Image icon;
    [SerializeField] bool csRandom = false;

    QuestionManager questionManager;

    private void Start()
    {
        questionManager = QuestionManager.instance;

        if (csRandom)
            SetUp(null);
    }

    public void SetUp(CardSet cs)
    {
        if(csRandom)
        {
            title.text = LocManager.instance.GetLocText("UI_RandomCardSet");
        }
        else
        {
            cardSet = cs;
            title.text = cs.cardsetTitle.text;
            icon.sprite = cs.cardSetIcon.sprite;
        }
    }

    public void SetCardSet()
    {
        if (csRandom)
            cardSet = questionManager.playableCardSets[Random.Range(0,questionManager.playableCardSets.Count)];

        Debug.Log("Setting new question with " + cardSet.name);
        questionManager.SetNewQuestion(cardSet);
        selectCardSet.SetActive(false);
    }
}
