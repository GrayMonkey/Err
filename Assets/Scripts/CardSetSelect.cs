using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSetSelect : MonoBehaviour 
{
    [SerializeField] GameObject cardSetObject;
    [SerializeField] Transform cardSetContent;

    public CardSet[] cardSets;

    private void Start()
    {
        PopulateList();
    }

    private void PopulateList () 
    {
        cardSets = FindObjectsOfType<CardSet>();
        foreach (CardSet cardSet in cardSets)
        {
            GameObject newCardSet = Instantiate(cardSetObject, cardSetContent);
        }
	}
}
