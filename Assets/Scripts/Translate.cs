using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Translate : MonoBehaviour
{
    Text text;
    string key;

    private void Awake()
    {
        text = gameObject.GetComponent<Text>();
        key = text.text;
    }

    private void OnEnable()
    {
        UpdateString();
    }

    public void UpdateString()
    {
        text.text = LocManager.locManager.GetLocText(key);
    }
}
