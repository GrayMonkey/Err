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
        text = GetComponent<Text>();
        try
        {
            key = text.text;
        }
        catch (Exception)
        {
            Debug.Log("No text object found for: " + gameObject.name);
            throw;
        }
    }

    private void Start()
    {
        /*        if (text == null)
                    Debug.Log("No text component found for: " + gameObject.name);

                key = text.text;
                if (key == "")
                    Debug.Log("No key component found for: " + gameObject.name + GetPath(transform));
                else
                    UpdateString();
        */
        UpdateString();
    }

    public void UpdateKey()
    {
        key = text.text;
        UpdateString();
    }

    public void UpdateString()
    {
        if (key == "") return;
        try
        {
            text.text = LocManager.instance.GetLocText(key);
            if(text.text == "")
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