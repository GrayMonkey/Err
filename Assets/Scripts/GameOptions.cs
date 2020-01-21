using UnityEngine;

public class GameOptions : MonoBehaviour
{
    public static GameOptions gameOptions;
    public float guessTime = 0;
    public bool showAnswer = false;
    public bool modCards = false;
    public bool sliderLock = false;
    public bool randomTurns = false;
    public bool firstLaunch = true;

    void Awake()
    {
        gameOptions = this;
    }

    void UpdateOptions (float newTime, bool newShowAnswer, bool newModCards,
                        bool newSliderLock, bool newRandomTurns)
    {
        guessTime = newTime * 5.0f;
        showAnswer = newShowAnswer;
        modCards = newModCards;
        sliderLock = newSliderLock;
        randomTurns = newRandomTurns;
    }
}

