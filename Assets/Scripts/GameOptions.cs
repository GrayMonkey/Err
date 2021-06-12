using UnityEngine;

public class GameOptions : MonoBehaviour
{
    public static GameOptions instance;
    public float guessTime = 0;
    public bool showAnswer = false;
    public bool easyRead = false;
    public bool sliderLock = false;
    public bool randomCardSets = false;
    public bool welcomeScreen = false;
    public SystemLanguage gameLang;

    LocManager locManager;

    void Awake()
    {
        instance = this;
        gameLang = Application.systemLanguage;
    }

    private void Start()
    {
        //PlayerPrefs.DeleteAll();
        GetPlayerPrefs();
        locManager = LocManager.instance;
        locManager.gameLang = gameLang;
    }

    public void UpdateOptions (float newGuessTime, bool newShowAnswer, bool newEasyRead,
                        bool newSliderLock, bool newRandomCardSets, bool newWelcomeScreen, SystemLanguage newGameLang)
    {
        guessTime = newGuessTime;
        showAnswer = newShowAnswer;
        easyRead = newEasyRead;
        sliderLock = newSliderLock;
        randomCardSets = newRandomCardSets;
        welcomeScreen = newWelcomeScreen;
        gameLang = newGameLang;
        locManager.gameLang = gameLang;

        SetPlayerPrefs();
    }

    void SetPlayerPrefs()
    {
        PlayerPrefs.SetFloat("timer", guessTime);
        PlayerPrefs.SetInt("showanswer", System.Convert.ToInt16(showAnswer));
        PlayerPrefs.SetInt("easyread", System.Convert.ToInt16(easyRead));
        PlayerPrefs.SetInt("sliderlock", System.Convert.ToInt16(sliderLock));
        PlayerPrefs.SetInt("randomcardsets", System.Convert.ToInt16(randomCardSets));
        PlayerPrefs.SetInt("welcomescreen", System.Convert.ToInt16(welcomeScreen));
        PlayerPrefs.SetInt("gamelang", (int)gameLang);
    }

    void GetPlayerPrefs()
    {
        if (PlayerPrefs.HasKey("timer"))
            guessTime = PlayerPrefs.GetFloat("timer");
        if (PlayerPrefs.HasKey("showanswer"))
            showAnswer = System.Convert.ToBoolean(PlayerPrefs.GetInt("showanswer"));
        if (PlayerPrefs.HasKey("easyread"))
            easyRead = System.Convert.ToBoolean(PlayerPrefs.GetInt("easyread"));
        if (PlayerPrefs.HasKey("sliderlock"))
            sliderLock = System.Convert.ToBoolean(PlayerPrefs.GetInt("sliderlock"));
        if (PlayerPrefs.HasKey("randomcardsets"))
            randomCardSets = System.Convert.ToBoolean(PlayerPrefs.GetInt("randomcardsets"));
        if (PlayerPrefs.HasKey("welcomescreen"))
            welcomeScreen = System.Convert.ToBoolean(PlayerPrefs.GetInt("welcomescreen"));
        if (PlayerPrefs.HasKey("gamelang"))
            gameLang = (SystemLanguage)PlayerPrefs.GetInt("gamelang");
        else
            gameLang = Application.systemLanguage;
    }
}

