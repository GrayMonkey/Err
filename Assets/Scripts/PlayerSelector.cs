#pragma warning disable 649   // Disable [SerializeField] warnings CS0649

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;
using Menus = MenuHandler.MenuOverlay;

public class PlayerSelector : MonoBehaviour
{
    public static PlayerSelector playerSelector;
    public List<Player> playersActive;
    public GameObject playerDragged;
    public GameObject dummyPlayer;
    public int newPlayerCount = 0;
    public bool loadPlayerActive = false;
    public PlayerObject selectedPlayer;

    [SerializeField] GameObject homeObject;
    [SerializeField] GameObject playersActiveContent;
    [SerializeField] GameObject playersRosterContent;
    [SerializeField] GameObject playerObject;
    [SerializeField] GameObject noActivePlayers;
    [SerializeField] GameObject noRosterPlayers;
    [SerializeField] RectTransform rectPlayerContent;
    [SerializeField] Text playerCountLabel;
    [SerializeField] Button addPlayer;
    [SerializeField] Button startGame;

    PlayerController playerController;
    GameOptions gameOptions;
    MenuHandler uiMenus;
    List<Player> playerData = new List<Player>();
    string updateCount;
    int lastActivePlayerCount = 0;

    private void Awake()
    {
        playerSelector = this;
    }

    // Use this for initialization
    private void Start()
    {
        playerController = PlayerController.playerController;
        playersActive = playerController.playersActive;
        gameOptions = GameOptions.gameOptions;
        uiMenus = MenuHandler.uiMenus;
        updateCount = LocManager.locManager.GetLocText("str_PlayerCount");
        CheckActivePlayers();
        AddRosterPlayers();
    }

    private void CheckActivePlayers()
    {
        // If players don't exist in the active players then display the no players text
        noActivePlayers.SetActive(false);
        if (playerController.playersActive.Count == 0)
            noActivePlayers.SetActive(true);
    }

    private void AddRosterPlayers()
    {
        // If no players are in the player roster then don't show the player roster
        GameObject playersRoster = playersRosterContent.transform.parent.gameObject;
        List<Player> playerRoster = playerController.playerRoster;
        playersRoster.SetActive(true);
        if (playerRoster.Count == 0)
        {
            playersRoster.SetActive(false);
        }
        else
        {
            foreach (Player rosterPlayer in playerRoster)
            {
                CreatePlayerObject(rosterPlayer, playersRosterContent.transform);
            }
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(playersRosterContent.GetComponent<RectTransform>());            
    }

    private void Update()
    {
        //int currentActivePlayerCount = playersActiveContent.transform.childCount-1;
        //if(!lastActivePlayerCount.Equals(currentActivePlayerCount))
        //{
        //    lastActivePlayerCount = currentActivePlayerCount;
        //    playerController.playersActive.Clear();

        //    for (int i = 0; i<lastActivePlayerCount-1;i++)
        //    {
        //        if (playersActiveContent.transform.GetChild(i).TryGetComponent<Player>
        //            (out Player player))
        //            playerController.playersActive.Add(player);
        //    }

        //    string players = currentActivePlayerCount.ToString() + "/10";
        //    playerCountLabel.text = updateCount + ": " + players;

        //    ResizePlayerHolders();
        //}

        //switch (playersActive.Count)
        //{
        //    case 0:
        //    case 1:
        //        startGame.interactable = false;
        //        break;
        //    case 10:
        //        addPlayer.interactable = false;
        //        break;
        //    default:
        //        startGame.interactable = true;
        //        addPlayer.interactable = true;
        //        break;
        //}
    }

    public void ResizePlayerHolders()
    {
        // Resize the height of the Collider box to accomodate new players
        //RectTransform rectActivePlayers = playersActiveContent.GetComponent<RectTransform>();
        //RectTransform rectRosterPlayers = playersRosterContent.GetComponent<RectTransform>();
        //BoxCollider2D collider2D = playersActiveContent.GetComponent<BoxCollider2D>();
        //LayoutRebuilder.ForceRebuildLayoutImmediate(rectActivePlayers);
        //LayoutRebuilder.ForceRebuildLayoutImmediate(rectRosterPlayers);
        //LayoutRebuilder.ForceRebuildLayoutImmediate(rectPlayerContent);
        //float height = rectActivePlayers.sizeDelta.y;
        //Vector2 size = new Vector2(rectActivePlayers.sizeDelta.x, height);
        //collider2D.size = size;
    }

    // Add a new player and activate a new PlayerObject
    public void AddNewPlayer()
    {
        newPlayerCount++;
        Player newPlayer = playerController.AddNewPlayer();
        CreatePlayerObject(newPlayer, playersActiveContent.transform);
        selectedPlayer = null;
        CheckActivePlayers();
        ResizePlayerHolders();
    }

    // Add a new Player Object
    private void CreatePlayerObject(Player player, Transform parent)
    {
        GameObject newPlayerObject = Instantiate(playerObject, parent);
        if (newPlayerObject.TryGetComponent(out PlayerObject newPlayer))
            newPlayer.SetPlayer(player);
    }
   
    //// Activate a new player button object
    //public static CreatePlayerObject(Player player, Transform newPlayerParent)
    //{
    //    //playerController.activePlayer = player;
    //    //GameObject newPlayerObject = Instantiate(playerObject, newPlayerParent);
    //    //newPlayerObject.SetActive(true);
    //    //newPlayerObject.GetComponent<PlayerObject>().refPlayer = player;
    //    //if (!playersActive.Contains(player))
    //    //    playersActive.Add(player);
    //    //return ;
    //    return null;
    //}

    public void UpdatePlayerList()
    {
        playersActive.Clear();

        for (int i = 0; i < playersActiveContent.transform.childCount; i++)
        {
            if(playersActiveContent.transform.GetChild(i).gameObject.activeInHierarchy)
            {
                if (playersActiveContent.transform.GetChild(i).gameObject.TryGetComponent(out PlayerObject playerObj))
                    playersActive.Add(playerObj.refPlayer);
            }
        }

 
        // Deactivate the add player button if playersActive.count is 10
        addPlayer.interactable = true;
        if (playersActive.Count == playersActiveContent.transform.childCount)
        {
            addPlayer.interactable = false;
        }

        CheckActivePlayers();
    }

    public void AddPlayerFromRoster()
    {
        // If a player is clicked on and is in the roster then add them to the active players

        CheckActivePlayers();
    }

    //public void ProceedWithPlayers()
    //{
    //    this.gameObject.SetActive(false);
    //    homeObject.SetActive(true);
    //}
}
