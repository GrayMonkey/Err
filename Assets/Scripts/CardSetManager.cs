using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSetManager : MonoBehaviour
{
    public static CardSetManager csManager;
    public List<CardSet> availableCardSets;
    public List<CardSet> purchasedCardSets;
    private void OnEnable()
    {
        csManager = this;
        purchasedCardSets.Clear();

        foreach (CardSet cardset in availableCardSets)
        {
            if (cardset.purchased)
                purchasedCardSets.Add(cardset);
        }
    }
}
