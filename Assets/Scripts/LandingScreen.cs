using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LandingScreen : MonoBehaviour
{
    [SerializeField] GameObject welcomeScreen;
    [SerializeField] GameObject mainScreen;
    [SerializeField] GameObject tapText;
    [SerializeField] Toggle dontShowAgain;
    [SerializeField] string url;
    [SerializeField] RectTransform textPanel;

    GameManager gameManager;
    GameOptions gameOptions;

    void Start()
    {
        gameOptions = GameOptions.gameOptions;
        gameManager = GameManager.gameManager;
        CanvasGroup canvasGroup = GetComponentInChildren<CanvasGroup>();
        StartCoroutine(CanvasFade(canvasGroup, Time.time, 5.0f));
    }

    public void ShowWelcomeScreen()
    {
        if(gameOptions.welcomeScreen)
        {
            welcomeScreen.SetActive(true);
            mainScreen.SetActive(false);
            LayoutRebuilder.ForceRebuildLayoutImmediate(textPanel);
        }
        else
        {
            gameManager.SetGameState(gameManager.gameState.home);
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
        yield return null;
    }
}
