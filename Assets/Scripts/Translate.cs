using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Translate : MonoBehaviour
{
    [SerializeField] string key;
    [SerializeField] Text text;

    private void Awake()
    {
        text = gameObject.GetComponent<Text>();
        if (text == null)
            Debug.Log("No text component found for: " + gameObject.name);

        key = text.text;
        if (key == "")
            Debug.Log("No key component found for: " + gameObject.name);
    }

    private void Start()
    {
        if (key != "") 
            UpdateString();
    }

    public void UpdateString()
    {
        try
        {
            text.text = LocManager.instance.GetLocText(key);
        }
        catch (Exception e)
        {
            Debug.Log(this.gameObject.name + " (Unknown Key)");
            Debug.LogException(e, this);
        }
    }
}