using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Translate : MonoBehaviour 
{
    string key;

    private void Start()
    {
        Text text = GetComponent<Text>();
        key = text.text;
        text.text = LocManager.locManager.GetLocText(key);
    }
}
