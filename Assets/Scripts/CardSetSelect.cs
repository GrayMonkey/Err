using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSetSelect : MonoBehaviour 
{
    [SerializeField] GameObject cardSetObject;
    [SerializeField] Transform cardSetContent;

    public List<CardSet> cardSets = new List<CardSet>();

    private void Awake()
    {
        PopulateList();
    }

    //private void Start()
    //{
    //    PopulateList();
    //}

    private void PopulateList () 
    {
        CardSet[] objs = Object.FindObjectsOfType<CardSet>();
        foreach (CardSet cardSet in objs)
        {
            GameObject newCardSet = Instantiate(cardSetObject, cardSetContent);
            newCardSet.GetComponent<CardSetObject>().SetUp(cardSet);
        }
	}
}
