using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Translate : MonoBehaviour
{
    LocManager locManager;
    string key;
    Text text;

    private void Awake()
    {
        locManager = LocManager.instance;
        text = GetComponent<Text>();
        key = text.text;
    }

    private void OnEnable()
    {
        UpdateString();
    }

    public void UpdateKey(string newKey)
    {
        key = newKey;
        GetTranslation();
    }

    public void UpdateString()
    {
        if (text == null)
            return;

        if (key == "")
        {
            if (text.text == "")
                return;
            else
                key = text.text;
        }

        GetTranslation();
    }

    private void GetTranslation()
    {
        try
        {
            text.text = locManager.GetLocText(key);
            if (text.text == "")
                Debug.Log(this.gameObject.name + " (blank translation) >" + GetPath(transform));
        }
        catch (Exception e)
        {
            Debug.Log(this.gameObject.name + " (Unknown Key) >" + GetPath(transform));
            Debug.LogException(e, this);
        }
    }

    private string GetPath(Transform current)
    {
        string path = "";
        string newPath;
        while (current.parent != null)
        {
            newPath = "." + current.name +  path;
            path = newPath;
            current = current.parent;
        }
        path = path.Remove(0, 1);
        return " (" + path +")";
    }
}