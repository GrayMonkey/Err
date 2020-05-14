using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Menus = MenuHandler.MenuOverlay;

public class mnu_Instructions : MonoBehaviour 
{
    [SerializeField] GameObject[] subMenus;
 
    MenuHandler uiMenus;
    int lastMenuID;

    // Use this for initialization
    void Start () 
    {
        uiMenus = MenuHandler.uiMenus;
    }

    private void OnEnable()
    {
        for (int i = 0; i < subMenus.Length; i++)
        {
            subMenus[i].SetActive(false);
        }
        subMenus[0].SetActive(true);
        lastMenuID = 0;
    }

    public void SetMenuActive(int id)
    {
        //Close if returning from the Instructions menu
        if (lastMenuID == id) 
        {
            uiMenus.CloseMenu(Menus.Instructions);
            return;
        }

        subMenus[lastMenuID].SetActive(false);
        subMenus[id].SetActive(true);
        lastMenuID = id;
    }
}
