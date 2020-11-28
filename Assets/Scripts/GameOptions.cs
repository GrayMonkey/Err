using UnityEngine;

public class GameOptions : MonoBehaviour
{
    public static GameOptions gameOptions;
    public float guessTime = 0;
    public bool showAnswer = false;
    public bool easyRead = false;
    public bool sliderLock = false;
    public bool randomTurns = false;
    public bool welcomeScreen = false;

    void Awake()
    {
        gameOptions = this;
    }

    void UpdateOptions (float newTime, bool newShowAnswer, bool newEasyRead,
                        bool newSliderLock, bool newRandomTurns)
    {
        guessTime = newTime * 5.0f;
        showAnswer = newShowAnswer;
        easyRead = newEasyRead;
        sliderLock = newSliderLock;
        randomTurns = newRandomTurns;
    }
}

