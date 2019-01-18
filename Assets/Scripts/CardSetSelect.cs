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

    //public void PopulateList()
    //{
    //    playersActive = playerController.playersActive;

    //    // Clear the current content
    //    for (int i = 0; i < playerRosterContent.childCount; i++)
    //    {
    //        Debug.Log("Obj: " + playerRosterContent.GetChild(i).name);
    //        Destroy(playerRosterContent.GetChild(i).gameObject);
    //    }

    //    foreach (Player player in playerRoster)
    //    {
    //        GameObject newPlayer = Instantiate(playerRosterObject, transform);
    //        newPlayer.transform.SetParent(playerRosterContent);
    //        newPlayer.GetComponent<PlayerRosterObject>().AttachPlayer(player);
    //        newPlayer.GetComponent<Button>().interactable = !playersActive.Contains(player);
    //    }
    //}

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
        // Populate the list and check if the cardset is currently selected for the player


        //CardSet[] objs = Object.FindObjectsOfType<CardSet>();
        //Player activePlayer = PlayerController.playerController.activePlayer;

        //// Check that the cardset doesn't already exist
        //foreach (CardSet cardSet in objs)
        //{
        //    if (!cardSets.Contains(cardSet))
        //        cardSets.Add(cardSet);
        //}

        //cardSets.Sort(delegate (CardSet x, CardSet y)
        //{
        //    if (x.cardSetName == null && y.cardSetName == null) return 0;     // The null values should never
        //    else if (x.cardSetName == null) return -1;                       // be evaluated as the playerName
        //    else if (y.cardSetName == null) return 1;                        // will always have a value
        //    else return x.cardSetName.CompareTo(y.cardSetName);
        //});

        //foreach (CardSet findCardSet in cardSets)
        //{
        //    bool found = false;
        //    CardSet[] foundCardSets = cardSetContent.GetComponentsInChildren<CardSet>();
        //    foreach (CardSet cardSet in foundCardSets)
        //    {
        //        if (findCardSet == cardSet)
        //        {
        //            found = true;
        //            bool playerCardSet = activePlayer.cardSets.Contains(findCardSet);
        //            cardSet.gameObject.GetComponent<Toggle>().isOn = playerCardSet;
        //            break;
        //        }
        //    }

        //    if (!found)
        //    {
        //        GameObject newCardSet = Instantiate(cardSetObject, cardSetContent);
        //        newCardSet.GetComponent<CardSetObject>().SetUp(findCardSet);
        //        newCardSet.GetComponent<Toggle>().isOn = activePlayer.cardSets.Contains(findCardSet);
        //    }
        //}
