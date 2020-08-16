    using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Is this ever used?

public class PlayerRosterSelect : MonoBehaviour
{

    public static PlayerRosterSelect playerRosterSelect;
    //public bool inUse = false;

    [SerializeField] private GameObject playerRosterObject;
    [SerializeField] private Transform playerRosterContent;

    PlayerController playerController;
    PlayerSelector playerSelector;
    List<Player> playersActive;
    List<Player> playerRoster;

    private void Awake()
    {
        playerRosterSelect = this;
    }

    private void Start()
    {
        playerSelector = PlayerSelector.playerSelector;
    }

    private void OnEnable()
    {
        playerController = PlayerController.playerController;
        playerRoster = playerController.playerRoster;
        PopulateList();
    }

    public void PopulateList()
    {
        playersActive = playerController.playersActive;

        // Clear the current content
        for (int i = 0; i < playerRosterContent.childCount; i++)
        {
            Debug.Log("Obj: " + playerRosterContent.GetChild(i).name);
            Destroy(playerRosterContent.GetChild(i).gameObject);
        }

        foreach (Player player in playerRoster)
        {
            GameObject newPlayer = Instantiate(playerRosterObject, playerRosterContent);
            newPlayer.GetComponent<PlayerRosterObject>().AttachPlayer(player);
            newPlayer.GetComponent<Button>().interactable = !playersActive.Contains(player);
        }
    }

    public void TrashPlayer(Player _player)
    {
        PlayerRosterObject[] _pros = gameObject.GetComponentsInChildren<PlayerRosterObject>();

        // Check to see if the player is in the roster
        foreach (PlayerRosterObject _pro in _pros)
        {
            if (_pro.refPlayer == _player)
            {
                playerRoster.Remove(_player);
                Destroy(_pro.gameObject);
                return;
            }
        }
    }
}