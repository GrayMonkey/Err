using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerRosterObject : MonoBehaviour
{
    public Player refPlayer;

    [SerializeField] Text playerName;

    PlayerController playerController;
    PlayerRosterSelect playerRosterSelect;

	// Use this for initialization
	private void Start () 
    {
        playerController = PlayerController.playerController;
        playerRosterSelect = PlayerRosterSelect.playerRosterSelect;
	}

    public void AttachPlayer (Player _player)
    {
        refPlayer = _player;
        playerName.text = refPlayer.playerName;
    }

    public void AddPlayerToGame()
    {
        PlayerObject _po = GetComponentInParent<PlayerObject>();
        PlayerObjectEvents _poe = _po.GetComponent<PlayerObjectEvents>();

        // Check to see if a player is already attached to the PlayerObject
        if (_po.refPlayer != null)
        {
            playerController.playersActive.Remove(_po.refPlayer);
        }

        // Set the player and object up
        playerController.playersActive.Add(refPlayer);
        _po.refPlayer = refPlayer;
        _poe.ResetButton();

        // Update the playerRoster
        //GetComponent<Button>().interactable = (false);
        playerRosterSelect.Hide();
    }



    //public void Show (Transform transform)
    //{
    //    transform.SetParent(transform);   
    //}
}
