using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translate : MonoBehaviour 
{
    public enum GameLangs { English, French, Italian, Spanish };
    public static GameLangs gameLang;

    // TODO Set up Google Translation Sheet
	// Use this for initialization
	void Start () 
    {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

/*    // Translate string function to local system language
    // Use GSFU functions to connect to Google Sheet
    public string TranslateText(string transText)
    {
        switch (Application.systemLanguage())
        {
            case (SystemLanguage.French):
                break;


            case (SystemLanguage.Italian):
                break;
                break;

            case (SystemLanguage.Spanish):
                break;

            default:
                break;
        }
    }
*/}
