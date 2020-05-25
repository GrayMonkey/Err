using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Translate : MonoBehaviour
{
    public string key;
    public Text text;

    private void Awake()
    {
        text = gameObject.GetComponent<Text>();
        key = text.text;
        if (key == "")
        {
            Debug.Log("No text component found for: " + gameObject.name);
        }
    }

    private void OnEnable()
    {
        if (key != "") UpdateString();
    }

    public void UpdateString()
    {
        try
        {
            text.text = LocManager.locManager.GetLocText(key);
        }
        catch (Exception e)
        {
            Debug.Log(this.gameObject.name);
            Debug.LogException(e, this);
        }
    }
}