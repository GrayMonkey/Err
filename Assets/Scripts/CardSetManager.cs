using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// CardSets should be added to the Manager game object under the
// Card Set Manager component
public class CardSetManager : MonoBehaviour
{
    public static CardSetManager csManager;
    public List<CardSet> csAll = new List<CardSet>();
    public List<CardSet> csGame = new List<CardSet>();
    public List<CardSet> csShop = new List<CardSet>();
    public List<CardSet> csActive = new List<CardSet>();

    private void OnEnable()
    {
        csManager = this;
        string lang = Application.systemLanguage.ToString();
    }
}
