using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LandingScreen : MonoBehaviour
{
    [SerializeField] GameObject welcomeScreen;
    [SerializeField] GameObject mainScreen;
    [SerializeField] Toggle dontShowAgain;
    [SerializeField] string url;
    [SerializeField] Button tapAnywhere;
    [SerializeField] RectTransform textPanel;
    [SerializeField] AudioSource welcome;

    GameManager gameManager;
    GameOptions gameOptions;
    RectTransform rect;

    void Start()
    {
        gameManager = GameManager.gameManager;
        gameOptions = GameOptions.gameOptions;
        rect = GetComponent<RectTransform>();

        LayoutRebuilder.ForceRebuildLayoutImmediate(textPanel);
        welcomeScreen.SetActive(false);
        mainScreen.SetActive(false);

        WelcomeScreenSelect(gameOptions.welcomeScreen);
    }

    public void WelcomeScreenSelect(bool show)
    {
        welcomeScreen.SetActive(show);
        mainScreen.SetActive(!show);
        tapAnywhere.interactable = !show;
        if (!show) welcome.Play();
    }

    public void DisableWelcomeScreen()
    {
        gameOptions.welcomeScreen = !dontShowAgain.isOn;
    }

    //public void ShowHomeScreen()
    //{
    //    //gameManager.UpdateGameState(GameManager.GameState.Home);
    //    gameManager.SetGameState(gameManager.gameState.home);
    //}

    public void BuyGame()
    {
        Application.OpenURL(url);
    }
}
