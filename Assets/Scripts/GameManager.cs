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
        //public GameObject endGame;
    }

    public static GameManager instance;
    public GameOptions gameOptions;
    public GameObject currGameState;
    public GameState gameState;
/*    public Question activeQuestion;
    public CardSet activeCardSet;
*/    //public List<CardSet> defaultCardSets;
    public bool gameInProgress = false;
    public GameObject bgParticles;

//    [SerializeField] GameObject qCard;
    [SerializeField] GameObject prevGameState;
    
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

        // Turn off all relevant game states
        gameState.landingScreen.SetActive(false);
        gameState.questionCard.SetActive(false);
        gameState.cardSetCollection.SetActive(false);
        gameState.selectCardSet.SetActive(false);
        //        gameState.endGame.SetActive(false);

        // Turn off all relevant menus
        uiMenus.DisableMenus();

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
        SetGameState(gameState.questionCard);
    }

/*    public void SetGameState(GameObject newGameState)
    {
        //Debug.Log(currGameState.name + " > " + newGameState.name);
        if (currGameState != newGameState)
        {
            currGameState.SetActive(false);
            Debug.Log(currGameState.name + " deactivated");
            prevGameState = currGameState;
        }
        else
        {
            prevGameState = null;
        }

        currGameState = newGameState;
        newGameState.SetActive(true);
        Debug.Log(newGameState.name + " activated");
    }*/

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
}