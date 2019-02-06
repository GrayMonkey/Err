using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LangToggle : MonoBehaviour, IPointerClickHandler
{
    public SystemLanguage lang;
    mnu_Options options;

    // Start is called before the first frame update
    void Start()
    {
        options = GetComponentInParent<mnu_Options>();
    }

    public void OnPointerClick (PointerEventData data)
    {
        //options.SetLanguage((int)lang);
        options.SetLanguage(lang);
    }
}
