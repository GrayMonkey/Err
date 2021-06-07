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

    [SerializeField] QuestionCard questionCard;
    [SerializeField] Slider timeSlider;
    [SerializeField] Text guessTime;
    [SerializeField] Slider showAnswer;
    [SerializeField] Slider easyRead;
    [SerializeField] Slider sliderLock;
    [SerializeField] Slider randomCardSets;
    [SerializeField] Slider welcomeScreen;
    [SerializeField] GameObject[] langs;
    [SerializeField] Toggle[] langsGroup;

    private void Awake()
    {
        gameManager = GameManager.instance;
        gameOptions = GameOptions.instance;
        locManager = LocManager.instance;

        uiMenus = MenuHandler.instance;
    }

    private void OnEnable()
    {
        timeSlider.value = gameOptions.guessTime / 5.0f;
        showAnswer.value = System.Convert.ToSingle(gameOptions.showAnswer);
        sliderLock.value = System.Convert.ToSingle(gameOptions.sliderLock);
        randomCardSets.value = System.Convert.ToSingle(gameOptions.randomCardSets);
        easyRead.value = System.Convert.ToSingle(gameOptions.easyRead);
        welcomeScreen.value = System.Convert.ToSingle(gameOptions.welcomeScreen);
        tempGameLang = locManager.GameLang;
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

    public void Btn_Credits()
    {
        //uiMenus.ShowMenu(Menus.Credits);
        uiMenus.CloseMenu(Menus.Options);
    }

    public void Btn_Home()
    {
        if (gameManager.gameInProgress)
        {
//            uiMenus.ShowMenu(Menus.QuitGame);
            return;
        }
        uiMenus.CloseMenu(Menus.Options);
    }

    public void CloseMenu(bool updateOptions)
    {
        if (updateOptions)
        {
            gameOptions.guessTime = timeSlider.value * 5.0f;
            gameOptions.showAnswer = System.Convert.ToBoolean(showAnswer.value);
            gameOptions.easyRead = System.Convert.ToBoolean(easyRead.value);
            gameOptions.sliderLock = System.Convert.ToBoolean(sliderLock.value);
            gameOptions.randomCardSets = System.Convert.ToBoolean(randomCardSets.value);
            gameOptions.welcomeScreen = System.Convert.ToBoolean(welcomeScreen.value);
            locManager.GameLang = tempGameLang;

            // Change the question style to match modCards
            questionCard.SetCluePanel();

            // Bug Fix: If the card type is changed during a question and 
            // answers correctly, the card would not update correctly to the
            // next question, but would use the old question. This forces an
            // update mid game.
            //if (questionManager.gameInProgress)
            //{
            //    questionManager.ForceCardTypeChange();
            //}
        }

        //locManager.SetDefaultLang(locManager.GameLang);
        uiMenus.CloseMenu(Menus.Options);
    }

    public void SetLanguage (SystemLanguage newLang)
    {
        // note: ToggleGroups appear to have stopped working, they lose their
        // members every disable
        //langsGroup.SetAllTogglesOff();

        foreach (Toggle toggle in langsGroup)
            toggle.isOn = false;

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
        locManager.GameLang = tempGameLang;

        // Update just in case timer is off
        GuessTime();
    }
}
