using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LandingScreen : MonoBehaviour
{
    [SerializeField] GameObject welcomeScreen = default;
    [SerializeField] Toggle dontShowAgain = default;
    [SerializeField] string url;
    [SerializeField] Button tapAnywhere;
    [SerializeField] RectTransform textPanel;

    GameManager gameManager;
    GameOptions gameOptions;
    RectTransform rect;

    void Start()
    {
        gameManager = GameManager.gameManager;
        gameOptions = GameOptions.gameOptions;
        rect = GetComponent<RectTransform>();
        gameOptions.welcomeScreen = !dontShowAgain.isOn;

        if (gameOptions.welcomeScreen)
        {
            ShowWelcomeScreen(true);
        }
    }

    public void ShowWelcomeScreen(bool show)
    {
        welcomeScreen.SetActive(show);
        tapAnywhere.interactable = !show;
        LayoutRebuilder.ForceRebuildLayoutImmediate(textPanel);
    }

    public void DisableWelcomeScreen()
    {
        gameOptions.welcomeScreen = !dontShowAgain.isOn;
    }

    public void ShowHomeScreen()
    {
        //gameManager.UpdateGameState(GameManager.GameState.Home);
        gameManager.SetGameState(gameManager.gameState.home);
    }

    public void BuyGame()
    {
        Application.OpenURL(url);
    }
}
