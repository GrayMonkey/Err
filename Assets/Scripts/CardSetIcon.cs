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
//    [SerializeField] bool csRandom = false;

    QuestionManager questionManager;

    private void Start()
    {
        questionManager = QuestionManager.instance;
    }

    public void UpdateCardSet(CardSet newCardSet)
    {
        cardSet = newCardSet;
        title.text = cardSet.cardSetTitleKey;
        icon.sprite = cardSet.cardSetIcon.sprite;

        Translate _trans = title.GetComponent<Translate>();
        _trans.UpdateKey();
    }

    public void ChooseCardSet()
    {
        Debug.Log("Setting new question with " + cardSet.name);
        questionManager.SetNewQuestion(cardSet);
        selectCardSet.SetActive(false);
    }
}
