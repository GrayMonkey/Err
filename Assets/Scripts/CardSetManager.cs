using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// CardSets should be added to the Manager game object under the
// Card Set Manager component
public class CardSetManager : MonoBehaviour
{
    public static CardSetManager instance;

    // Only list support Languages with active cardsets
    [Header("Defaults Language CardSets")]
    [SerializeField] CardSet csDefault_EN;
    [SerializeField] CardSet csDefault_FR;
    [Space(10)]

    public CardSet csDefault;
    public List<CardSet> csAll = new List<CardSet>();
    public List<CardSet> csGame = new List<CardSet>();
    public List<CardSet> csShop = new List<CardSet>();
    public List<CardSet> activeCardSets = new List<CardSet>();

    private void OnEnable()
    {
        instance = this;
        SetDefaultCardSet();
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

        if(!activeCardSets.Contains(csDefault))
            activeCardSets.Add(csDefault);
    }
}
