
using System;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.EventSystems;
//using Menu = MenuHandler.MenuOverlay;

[System.Serializable]
public class Player //: MonoBehaviour
{
    // TODO Pass the player to the player button by reference (ref)
    // which should enable PlayerInfo (also pass by ref?) to update
    // the player info directly and then the playerRoster can be 
    // can be serialized correctly for file writing

    //public List<CardSet> cardSets = new List<CardSet>();
    public string playerName;
    public string language;
    //public bool saveData = false;
    public int questionsThisGame = 0;
    public int answersThisGame = 0;
    public int pointsThisGame = 0;
    public int gamesTotal = 0;
    public int gamesWon = 0;
    public int questionsTotal = 0;
    public int answersTotal = 0;
    public int pointsTotal = 0;
    //public int listID = -1;
    //public bool active = false;

    public void UpdateInfo(string newName, bool logData)
    {
        playerName = newName;
        //saveData = logData;
        //this.GetComponentInChildren<Text>().text = playerName;
        //this.transform.SetSiblingIndex(newturn);
    }

    public void ResetData()
    {    
        gamesTotal = 0;
        gamesWon = 0;
        questionsTotal = 0;
        questionsThisGame = 0;
        answersTotal = 0;
        answersThisGame = 0;
        pointsTotal = 0;
        pointsThisGame = 0;
    }

    public void EditPlayerInfo()
    {
        
    }
}