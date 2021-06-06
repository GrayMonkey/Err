using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectCardSet : MonoBehaviour
{
    // mnu_NextQuestion should handle the points scored for the last player and also
    // the choice of the next CardSet quesiton

    public static SelectCardSet instance;
    
    [SerializeField] GameObject csIcon;
    [SerializeField] Transform csIconParent;

    QuestionManager questionManager;
    List<CardSetIcon> csIcons = new List<CardSetIcon>();
    List<CardSetIcon> csIconsActive = new List<CardSetIcon>();


    private void Awake()
    {
        instance = this;
        questionManager = QuestionManager.instance; 
        GenerateIcons();
    }

    private void OnEnable()
    {
        SetUpIcons();
    }

    void GenerateIcons()
    {
        foreach(CardSet cardSet in questionManager.csAll)
        {
            GameObject newIconObject = Instantiate(this.csIcon);
            newIconObject.SetActive(true);
            newIconObject.transform.SetParent(csIconParent);

            CardSetIcon csIcon = newIconObject.GetComponent<CardSetIcon>();
            csIcon.SetUp(cardSet);
            csIcons.Add(csIcon);
        }
    }

    void SetUpIcons()
    {
        // if there is only one cardset avaiable then proceed to the next questions
        if(questionManager.playableCardSets.Count ==1)
        {
            questionManager.SetNewQuestion(questionManager.playableCardSets[0]);
            return;
        }
        
        foreach(CardSetIcon csIcon in csIcons)
        {
            csIcon.gameObject.SetActive(false);
            if (questionManager.playableCardSets.Contains(csIcon.cardSet))
                csIcon.gameObject.SetActive(true);
        }
    }
}
