#pragma warning disable 649   // Disable [SerializeField] warnings CS0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Menus = MenuHandler.MenuOverlay;

public class mnu_Options : MonoBehaviour
{
    GameManager gameManager;
    GameOptions gameOptions;
    MenuHandler uiMenus;

    [SerializeField] Slider timeSlider;
    [SerializeField] Text guessTime;
    [SerializeField] Toggle showAnswer;
    [SerializeField] Toggle modCards;
    [SerializeField] Toggle sliderLock;
    [SerializeField] Toggle randomTurns;
    [SerializeField] GameObject modCard;
    [SerializeField] GameObject tradCard;


    private void Awake()
    {
        gameManager = GameManager.gameManager;
        gameOptions = GameOptions.gameOptions;
        uiMenus = MenuHandler.uiMenus;
    }

    void OnEnable()
    {
        timeSlider.value = gameOptions.guessTime / 5.0f;
        showAnswer.isOn = gameOptions.showAnswer;
        modCards.isOn = gameOptions.modCards;
        sliderLock.isOn = gameOptions.sliderLock;
        randomTurns.isOn = gameOptions.randomTurns;
    }

    public void GuessTime()
    {
        string xTime = "Off";
        if (timeSlider.value > 0)
        {
            float timeLength = timeSlider.value * 5.0f;
            xTime = timeLength.ToString() + "s";
        }
        guessTime.text = xTime;
    }

    public void Btn_Instruction()
    {
        //uiMenu.ShowMenu(Menus.HowToPlay);
        uiMenus.CloseMenu(Menus.Options);
    }

    public void Btn_Credits()
    {
        uiMenus.ShowMenu(Menus.Credits);
        uiMenus.CloseMenu(Menus.Options);
    }

    public void Btn_Home()
    {
        if (GameManager.gameManager.gameInProgress)
        {
            uiMenus.ShowMenu(Menus.QuitGame);
            return;
        }
        uiMenus.CloseMenu(Menus.Options);
    }

    public void CloseMenu()
    {
        // Update the game options
        gameOptions.guessTime = timeSlider.value * 5.0f;
        gameOptions.showAnswer = showAnswer.isOn;
        gameOptions.modCards = modCards.isOn;
        gameOptions.sliderLock = sliderLock.isOn;
        gameOptions.randomTurns = randomTurns.isOn;

        // Bug Fix: If the card type is changed during a question and 
        // answers correctly, the card would not update correctly to the
        // next question, but would use the old question. This forces an
        // update mid game.
        if (gameManager.gameInProgress)
        {
            gameManager.ChangeCardType();
        }

        uiMenus.CloseMenu(Menus.Options);
    }
}
