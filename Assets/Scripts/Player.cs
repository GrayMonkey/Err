
using System;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Player
{
    public string playerName;
    public string playerID;
    public List<CardSet> cardSets = new List<CardSet>();
    public List<string> cardSetList = new List<string>();
    public SystemLanguage language;
    public int questionsThisGame = 0;
    public int answersThisGame = 0;
    public int pointsThisGame = 0;
    public int gamesTotal = 0;
    public int gamesWon = 0;
    public int questionsTotal = 0;
    public int answersTotal = 0;
    public int pointsTotal = 0;
    public int guessTime = -1;
}