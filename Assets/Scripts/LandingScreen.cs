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

    GameManager gameManager;
    GameOptions gameOptions;

    void Start()
    {
        gameOptions = GameOptions.instance;
        gameManager = GameManager.instance;
        welcomeScreen.SetActive(false);
        mainScreen.SetActive(true);
        touchPanel.SetActive(true);
        CanvasGroup canvasGroup = GetComponentInChildren<CanvasGroup>();
        StartCoroutine(CanvasFade(canvasGroup, Time.time, 5.0f));
        WelcomeScreen(gameOptions.welcomeScreen);
    }

    public void WelcomeScreen(bool show)
    {
        mainScreen.SetActive(!show);
        welcomeScreen.SetActive(show);
    }

    public void SetGameOptionWelcome()
    {
        gameOptions.welcomeScreen = !dontShowAgain.isOn;
        PlayerPrefs.SetInt("welcomescreen", System.Convert.ToInt32(gameOptions.welcomeScreen));
    }

    public void BuyGame()
    {
        Application.OpenURL(url);
    }

    public void InitiateFreePurchase()
    {
        // ToDo - Link this in to the correct purchase from the store
        // questionManager.defaultCardSets.Add(csStarter);
        WelcomeScreen(false);
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

    public void StartGame(GameObject gameObject)
    {
        gameManager.SetGameState(gameObject);
        gameManager.bgParticles.SetActive(false);
    }
}
