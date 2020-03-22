using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// CardSets should be added to the Manager game object under the
// Card Set Manager component
public class CardSetManager : MonoBehaviour
{
    public static CardSetManager csManager;

    [SerializeField] List<GameObject> allCardSets;
    List<CardSet> activeCardsets;

    private void OnEnable()
    {
        csManager = this;
        allCardSets.Clear();
        string lang = Application.systemLanguage.ToString();
        UpdateCardSets();
        FilterCardSets(lang, true);
    }

    public void UpdateCardSets()
    {
    }

    public void FilterCardSets (string lang, bool active)
    {
    }
}
