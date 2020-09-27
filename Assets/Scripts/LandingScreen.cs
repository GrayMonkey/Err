using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LandingScreen : MonoBehaviour
{
    [SerializeField] GameObject mainScreen;
    [SerializeField] GameObject welcomeScreen;
    [SerializeField] GameObject touchPanel;
    [SerializeField] GameObject tapText;
    [SerializeField] Toggle dontShowAgain;
    [SerializeField] string url = "https://sites.google.com/errgame.com/err/home";
    [SerializeField] RectTransform textPanel;

    GameManager gameManager;
    GameOptions gameOptions;

    void Start()
    {
        gameOptions = GameOptions.gameOptions;
        gameManager = GameManager.gameManager;
        welcomeScreen.SetActive(false);
        mainScreen.SetActive(true);
        touchPanel.SetActive(true);
        CanvasGroup canvasGroup = GetComponentInChildren<CanvasGroup>();
        StartCoroutine(CanvasFade(canvasGroup, Time.time, 5.0f));
        WelcomeScreen(true);
    }

    public void WelcomeScreen(bool show)
    {
        if (!show)
        {
            mainScreen.SetActive(true);
            welcomeScreen.SetActive(false);
            return;
        }
        else if(gameOptions.welcomeScreen)
        {
            welcomeScreen.SetActive(true);
            mainScreen.SetActive(false);
            //LayoutRebuilder.ForceRebuildLayoutImmediate(textPanel);
        }
    }

    public void SetGameOptionWelcome()
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

    IEnumerator CanvasFade(CanvasGroup canvasGroup, float startTime, float fadeTime)
    {
        while (canvasGroup.alpha > 0f)
        {
            float deltaTime = (Time.time - startTime) / fadeTime;
            canvasGroup.alpha = Mathf.SmoothStep(1.0f, 0.0f, deltaTime);
            yield return null;
        }

        canvasGroup.gameObject.SetActive(false);
        tapText.SetActive(true);
    }
}
