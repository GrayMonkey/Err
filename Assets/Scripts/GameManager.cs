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
        public GameObject home;
        public GameObject cardSetSelect;
        public GameObject playerSelect;
        public GameObject question;
        public GameObject endGame;
    }

    public static GameManager instance;
    public GameOptions gameOptions;
    public GameObject currGameState;
    public GameState gameState;
    public Question activeQuestion;
    public CardSet activeCardSet;
    public List<CardSet> defaultCardSets;
    public bool gameInProgress = false;

    [SerializeField] GameObject qCard;

    GameObject prevGameState;
    MenuHandler uiMenus;

    private void Awake()
    {
        instance = this;
    }

    private void OnApplicationQuit()
    {
        // TODO Save Game and/or update player info
    }

    // Use this for initialization
    // TODO Read in save game data
    void Start()
    {
        gameOptions = GameOptions.instance;
        uiMenus = MenuHandler.instance;

        // Turn off all game states
        gameState.landingScreen.SetActive(false);
        gameState.home.SetActive(false);
        gameState.endGame.SetActive(false);
        gameState.playerSelect.SetActive(false);
        gameState.question.SetActive(false);
        gameState.cardSetSelect.SetActive(false);

        currGameState = gameState.landingScreen;
        SetGameState(currGameState);
    }

    // Bug Fix: If the card type is changed during a question and 
    // answers correctly, the card would not update correctly to the
    // next question, but would use the old question. This forces an
    // update mid game. See mnu_Options.CloseMenu()
    // Called when card type is changed via options during a game
    public void ForceCardTypeChange()
    {
        SetGameState(gameState.question);
    }

    public void SetGameState(GameObject newGameState)
    {
        //Debug.Log(currGameState.name + " > " + newGameState.name);
        if (currGameState != newGameState)
        {
            prevGameState = currGameState;
            prevGameState.SetActive(false);
        }
        else
        {
            prevGameState = null;
        }

        currGameState = newGameState;
        currGameState.SetActive(true);
    }

    public void ShowInstructions()
    {
        uiMenus.ShowMenu(Menu.Instructions);
    }

    public void ShowOptions()
    {
        uiMenus.ShowMenu(Menu.Options);
    }
}