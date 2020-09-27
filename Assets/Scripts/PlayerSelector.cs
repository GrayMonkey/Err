#pragma warning disable 649   // Disable [SerializeField] warnings CS0649

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Menus = MenuHandler.MenuOverlay;

public class PlayerSelector : MonoBehaviour
{
    public static PlayerSelector playerSelector;
    public GameObject playerDragged;
    public GameObject dummyPlayer;
    public PlayerObject activePlayerObject;
    public int newPlayerCount = 0;
    
    [SerializeField] GameObject playersActiveContent;
    [SerializeField] GameObject playersRosterContent;
    [SerializeField] GameObject playerGameObject;
    [SerializeField] GameObject noActivePlayers;
    [SerializeField] GameObject noRosterPlayers;
    [SerializeField] Text playerCountLabel;
    [SerializeField] Button addPlayer;
    [SerializeField] Button startGame;

    PlayerController playerController;
    List<Player> playerData = new List<Player>();
    string updateCount;
    int maxPlayers = 10;

    private void Awake()
    {
        playerSelector = this;
    }

    // Use this for initialization
    private void Start()
    {
        playerController = PlayerController.playerController;
        AddRosterPlayers();
    }

    private void Update()
    {
        CheckPlayerCounts();
    }

    public void CheckPlayerCounts()
    {
        bool _addRosterPlayers = true;

        noActivePlayers.SetActive(false);
        noRosterPlayers.SetActive(false);
        addPlayer.interactable = true;
        startGame.interactable = false;

        //Check Active Players
        PlayerObject[] _playerCount = playersActiveContent.GetComponentsInChildren<PlayerObject>();

        // If players don't exist roster player holder then display the no players text
        if (_playerCount.Length == 0)
            noActivePlayers.SetActive(true);

        // If more than one player is active then activate the start button
        if (_playerCount.Length > 1)
            startGame.interactable = true;

        // If maxPlayers is reached then don't add any more players
        if (_playerCount.Length == maxPlayers)
        {
            addPlayer.interactable = false;
            _addRosterPlayers = false;
        }

        // Update the player count
        updateCount = LocManager.locManager.GetLocText("str_PlayerCount");
        updateCount += ": " + _playerCount.Length.ToString() + "/" + maxPlayers.ToString();
        playerCountLabel.text = updateCount;
        newPlayerCount = _playerCount.Length;

        //Check Roster Players
        _playerCount = playersRosterContent.GetComponentsInChildren<PlayerObject>();

        // If players don't exist roster player holder then display the no players text
        if (_playerCount.Length == 0)
            noRosterPlayers.SetActive(true);

        // If maxPlayers has been reached then don't allow any more players from the Roster
        // to be added
        foreach (PlayerObject _rosterPlayer in _playerCount)
            _rosterPlayer.SetAddPlayerFromRosterButton(_addRosterPlayers);
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
    }

    // Add a new player and activate a new PlayerObject
    public void AddNewPlayer()
    {
        Player newPlayer = playerController.AddNewPlayer();
        CreatePlayerObject(newPlayer, playersActiveContent.transform);
    }

    // Add a new Player Object
    public void CreatePlayerObject(Player player, Transform parent)
    {
        GameObject newPlayerObject = Instantiate(playerGameObject, parent);
        if (newPlayerObject.TryGetComponent(out PlayerObject newPlayer))
            newPlayer.SetPlayer(player);
        if (activePlayerObject != null)
            activePlayerObject.SubMenu(false);
        activePlayerObject = newPlayer;
    }

    public void UpdatePlayerList()
    {
        playerController.playersActive.Clear();

        for (int i = 0; i < playersActiveContent.transform.childCount; i++)
        {
            if (playersActiveContent.transform.GetChild(i).gameObject.activeInHierarchy)
            {
                if (playersActiveContent.transform.GetChild(i).gameObject.TryGetComponent(out PlayerObject playerObj))
                    playerController.playersActive.Add(playerObj.thisPlayer);
            }
        }


        // Deactivate the add player button if playersActive.count is 10
        addPlayer.interactable = true;
        if (playerController.playersActive.Count == playersActiveContent.transform.childCount)
        {
            addPlayer.interactable = false;
        }
    }
}