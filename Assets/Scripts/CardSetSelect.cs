using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSetSelect : MonoBehaviour
{
    [SerializeField] private GameObject cardSetObject;
    [SerializeField] private Transform cardSetContent;

    public List<CardSet> cardSets = new List<CardSet>();


    private void OnEnable()
    {

        PopulateList();
    }

    public void PopulateList()
    {
        List<CardSet> playerCardSets = PlayerController.playerController.activePlayer.cardSets;

        // Clear the current CardSets and CardSetObjects
        for (int i = 0; i < cardSetContent.childCount; i++)
        {
            Destroy(cardSetContent.GetChild(i).gameObject);
        }

        foreach (CardSet cardset in CardSetManager.csManager.purchasedCardSets)
        {
            GameObject newCardSet = Instantiate(cardSetObject, cardSetContent);
            newCardSet.GetComponent<CardSetObject>().SetUp(cardset);
            bool toggleCardSet = playerCardSets.Contains(cardset);
            newCardSet.GetComponent<Toggle>().isOn = toggleCardSet;
        }
    }
}