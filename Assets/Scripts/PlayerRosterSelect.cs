using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRosterSelect : MonoBehaviour
{

    public static PlayerRosterSelect playerRosterSelect;
    public bool inUse = false;

    [SerializeField] GameObject playerRosterObject;
    [SerializeField] Transform PlayerRosterContent;
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
        playerController = PlayerController.playerController;
        playerSelector = PlayerSelector.playerSelector;
        playerRoster = playerController.playerRoster;
        PopulateList();
    }

    private void PopulateList()
    {
        playersActive = playerController.playersActive;

        foreach (Player player in playerRoster)
        {
            if (!playersActive.Contains(player))
            {
                GameObject newPlayer = Instantiate(playerRosterObject, transform);
                newPlayer.transform.SetParent(PlayerRosterContent);
                newPlayer.GetComponent<PlayerRosterObject>().AttachPlayer(player);
            }
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

    public void Hide()
    {
        PlayerRosterObject[] _pros = gameObject.GetComponentsInChildren<PlayerRosterObject>();

        // Check to see if the players on the roster are already active
        foreach (PlayerRosterObject _pro in _pros)
        {
            Button button = _pro.GetComponent<Button>();
            Player player = _pro.refPlayer;

            button.interactable = true;
            if (playersActive.Contains(player))
            {
                button.interactable = false;
            }
        }


        transform.SetParent(playerSelector.transform);
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.localPosition = new Vector3(700.0f, 0.0f, 0.0f);
        inUse = false;
    }

    //public void Reset()
    //{
    //    playerRosterSelect.transform.SetParent(playerSelector.transform);
    //    //playerRosterSelect.transform.parent = playerSelector.transform;
    //    playerRosterSelect.transform.localPosition = new Vector3(700.0f, 0f, 0f);
    //}
}