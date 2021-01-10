using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LangToggle : MonoBehaviour, IPointerClickHandler
{
    public SystemLanguage lang;

    [SerializeField] mnu_Options options;

    LocManager locManager;

    private void Start()
    {
        locManager = LocManager.locManager;
    }

    public void OnPointerClick (PointerEventData data)
    {
        // Probably better to use listeners for this but can't be bothered to learn!
        //options.SetLanguage((int)lang);
        if (options.gameObject.activeInHierarchy)
            options.SetLanguage(lang);
        else
            locManager.GameLang = lang;
    }
}
