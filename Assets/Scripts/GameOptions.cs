#pragma warning disable 649   // Disable [SerializeField] warnings CS0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOptions : MonoBehaviour
{
    public static GameOptions gameOptions;
    public float guessTime = 0;
    public bool showAnswer = false;
    public bool modCards = false;
    public bool sliderLock = false;
    public bool randomTurns = false;

    void Awake()
    {
        gameOptions = this;
    }

    void UpdateOptions (float newTime, bool newShowAnswer, bool newModCards, bool newSliderLock, bool newRandomTurns)
    {
        guessTime = newTime * 5.0f;
        showAnswer = newShowAnswer;
        modCards = newModCards;
        sliderLock = newSliderLock;
        randomTurns = newRandomTurns;
    }
}

