using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerRosterObject : MonoBehaviour
{
    public Player refPlayer;

    [SerializeField] private Text playerName;
    [SerializeField] private Text playerID;

    PlayerController playerController;
    PlayerRosterSelect playerRosterSelect;

    // Use this for initialization
    private void Start()
    {
        playerController = PlayerController.playerController;
        playerRosterSelect = PlayerRosterSelect.playerRosterSelect;
    }

    public void AttachPlayer(Player _player)
    {
        refPlayer = _player;
        playerName.text = refPlayer.playerName;
        playerID.text = refPlayer.playerID;
    }

    public void AddPlayerToGame()
    {
        PlayerObject playerObject = GetComponentInParent<PlayerObject>();
        Animator animator = GetComponentInParent<Animator>();

        playerObject.UpdateToRosterPlayer(refPlayer);
        animator.SetBool("openMenu", false);
        PlayerSelector.playerSelector.selectedPlayer = null;
    }
}
