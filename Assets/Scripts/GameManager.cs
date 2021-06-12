using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Menu = MenuHandler.MenuOverlay;

public class GameManager : MonoBehaviour
{
    // TODO Tidy this file up and remove all old
    // commented code!!!!

    [System.Serializable]
    public struct GameState
    {
        public GameObject landingScreen;
        public GameObject cardSetCollection;
        public GameObject selectCardSet;
        public GameObject questionCard;
        public GameObject questionResult;
    }

    public static GameManager instance;
    public GameOptions gameOptions;

    public GameObject currGameState;
    public GameState gameState;
    public bool gameInProgress = false;
    public GameObject bgParticles;
    
    MenuHandler uiMenus;

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start()
    {
        gameOptions = GameOptions.instance;
        uiMenus = MenuHandler.instance;

        // Turn off all relevant game states
        gameState.landingScreen.SetActive(false);
        gameState.questionCard.SetActive(false);
        gameState.cardSetCollection.SetActive(false);
        gameState.selectCardSet.SetActive(false);

        // Turn off all relevant menus
        uiMenus.DisableMenus();

        currGameState = gameState.landingScreen;
        SetGameState(currGameState);
    }

    public void SetGameState(GameObject newgameState)
    {
        currGameState.SetActive(false);
        Debug.Log(currGameState + " deactivated");
        newgameState.SetActive(true);
        Debug.Log(newgameState + " activated");
        currGameState = newgameState;
    }

    public void ShowInstructions()
    {
        uiMenus.ShowMenu(Menu.Instructions);
    }

    public void ShowOptions()
    {
        uiMenus.ShowMenu(Menu.Options);
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }
}