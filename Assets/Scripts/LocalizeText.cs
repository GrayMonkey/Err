using UnityEngine;
using System;
using UnityEngine.UI;

public class LocalizeText : MonoBehaviour
{
    [SerializeField] Text transText;

    private SystemLanguage language;
    private string textKey;

    public void Start()
    {
        language = GameManager.gameManager.defaultLanguage;
        textKey = transText.text;
    }

    //private string TranslateText (string key)
    //{
        
    //}
}
