using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Menus = MenuHandler.MenuOverlay;

public class mnu_Instructions : MonoBehaviour 
{
    MenuHandler uiMenus;

	// Use this for initialization
	void Start () 
    {
        uiMenus = MenuHandler.uiMenus;
	}

    public void CloseInstructions()
    {
        //uiMenu.CloseMenu(Menus.HowToPlay);
    }
}
