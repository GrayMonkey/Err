using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LandingScreen : MonoBehaviour
{
    [SerializeField] GameObject welcomeScreen = default;
    [SerializeField]  Toggle dontShowAgain = default;

    GameManager gameManager;
    GameOptions gameOptions;

    void Start()
    {
        gameManager = GameManager.gameManager;
        gameOptions = GameOptions.gameOptions;

        if (gameOptions.firstLaunch)
            ShowWelcomeScreen(true);
    }

    public void ShowWelcomeScreen(bool show)
    {
        welcomeScreen.SetActive(show);
    }

    public void DisableWelcomeScreen()
    {
        gameOptions.firstLaunch = false;
    }

    public void ShowHomeScreen()
    {
        //gameManager.UpdateGameState(GameManager.GameState.Home);
        gameManager.SetGameState(gameManager.gameState.home);
    }
}
