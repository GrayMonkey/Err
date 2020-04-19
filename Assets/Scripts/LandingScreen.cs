using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LandingScreen : MonoBehaviour
{
    [SerializeField] GameObject welcomeScreen;
    [SerializeField] GameObject mainScreen;
    [SerializeField] Image mainLogo;
    [SerializeField] Text mainText;
    [SerializeField] Text mainPressKey;
    [SerializeField] Toggle dontShowAgain;
    [SerializeField] string url;
    [SerializeField] Button tapAnywhere;
    [SerializeField] RectTransform textPanel;

    GameOptions gameOptions;

    void Start()
    {
        gameOptions = GameOptions.gameOptions;

        LayoutRebuilder.ForceRebuildLayoutImmediate(textPanel);
        welcomeScreen.SetActive(false);
        mainScreen.SetActive(false);

        WelcomeScreenSelect(gameOptions.welcomeScreen);
    }

    public void WelcomeScreenSelect(bool show)
    {
        welcomeScreen.SetActive(show);

        if(!show)
        {
            mainScreen.SetActive(!show);
            CanvasGroup canvasGroup = GetComponentInChildren<CanvasGroup>();
            StartCoroutine(CanvasFade(canvasGroup, 5.0f));
            tapAnywhere.interactable = !show;
            
        }
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

    IEnumerator CanvasFade(CanvasGroup canvasGroup, float fadeTime)
    {
        while (canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha -= Time.deltaTime / fadeTime;
            yield return null;
        }

        canvasGroup.interactable = true;
        yield return null;
    }
}
