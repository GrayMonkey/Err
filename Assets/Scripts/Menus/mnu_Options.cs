#pragma warning disable 649   // Disable [SerializeField] warnings CS0649

using UnityEngine;
using UnityEngine.UI;
using Menus = MenuHandler.MenuOverlay;

public class mnu_Options : MonoBehaviour
{
    GameManager gameManager;
    GameOptions gameOptions;
    LocManager locManager;
    MenuHandler uiMenus;
    SystemLanguage tempGameLang;

    [SerializeField] ScrollRect scrollRect;
    [SerializeField] Slider timeSlider;
    [SerializeField] Text guessTime;
    [SerializeField] Toggle showAnswer;
    [SerializeField] Toggle modCards;
    [SerializeField] Toggle sliderLock;
    [SerializeField] Toggle randomTurns;
    [SerializeField] GameObject modCard;
    [SerializeField] GameObject tradCard;
    [SerializeField] GameObject[] langs;
    [SerializeField] ToggleGroup langsGroup;

    private void Awake()
    {
        gameManager = GameManager.gameManager;
        gameOptions = GameOptions.gameOptions;
        locManager = LocManager.locManager;

        uiMenus = MenuHandler.uiMenus;
    }

    void OnEnable()
    {
        timeSlider.value = gameOptions.guessTime / 5.0f;
        showAnswer.isOn = gameOptions.showAnswer;
        modCards.isOn = gameOptions.modCards;
        sliderLock.isOn = gameOptions.sliderLock;
        randomTurns.isOn = gameOptions.randomTurns;
        scrollRect.verticalNormalizedPosition = 1.0f;
        SetLanguage(locManager.GameLang);
    }

    public void GuessTime()
    {
        string xTime = locManager.GetLocText("UI_Off");
        if (timeSlider.value > 0)
        {
            float timeLength = timeSlider.value * 5.0f;
            xTime = timeLength.ToString() + "s";
        }
        guessTime.text = xTime;
    }

    //public void Btn_Instruction()
    //{
    //    //uiMenu.ShowMenu(Menus.HowToPlay);
    //    uiMenus.CloseMenu(Menus.Options);
    //}

    public void Btn_Credits()
    {
        uiMenus.ShowMenu(Menus.Credits);
        uiMenus.CloseMenu(Menus.Options);
    }

    public void Btn_Home()
    {
        if (gameManager.gameInProgress)
        {
            uiMenus.ShowMenu(Menus.QuitGame);
            return;
        }
        uiMenus.CloseMenu(Menus.Options);
    }

    public void CloseMenu(bool updateOptions)
    {
        if (updateOptions)
        {
            // Update the game options
            gameOptions.guessTime = timeSlider.value * 5.0f;
            gameOptions.showAnswer = showAnswer.isOn;
            gameOptions.modCards = modCards.isOn;
            gameOptions.sliderLock = sliderLock.isOn;
            gameOptions.randomTurns = randomTurns.isOn;
            locManager.GameLang = tempGameLang;

            // Bug Fix: If the card type is changed during a question and 
            // answers correctly, the card would not update correctly to the
            // next question, but would use the old question. This forces an
            // update mid game.
            if (gameManager.gameInProgress)
            {
                gameManager.ChangeCardType();
            }
        }

        locManager.SetLang(locManager.GameLang);
        uiMenus.CloseMenu(Menus.Options);
    }

    public void SetLanguage (SystemLanguage newLang)
    {
        langsGroup.SetAllTogglesOff();
        switch (newLang)
        {
            case SystemLanguage.French:
                tempGameLang = SystemLanguage.French;
                langs[1].GetComponent<Toggle>().isOn = true;
                break;
            case SystemLanguage.Italian:
                tempGameLang = SystemLanguage.Italian;
                langs[2].GetComponent<Toggle>().isOn = true;
                break;
            case SystemLanguage.Spanish:
                tempGameLang = SystemLanguage.Spanish;
                langs[3].GetComponent<Toggle>().isOn = true;
                break;
            default:
                tempGameLang = SystemLanguage.English;
                langs[0].GetComponent<Toggle>().isOn = true;
                break;
        }

        // Reset all text in scene
        locManager.SetLang(tempGameLang);

        // Update just in case timer is off
        GuessTime();
    }
}
