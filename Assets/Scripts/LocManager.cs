using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameLanguage { en, fr, de, it, es };

public class LocManager : MonoBehaviour 
{
    public static LocManager locManager;
    public GameLanguage gameLanguage;

    private void Awake()
    {
        locManager = this;

        int sysLang = (int)Application.systemLanguage;

        // Uses standard web browser identification codes
        switch (sysLang)
        {
            case (int)SystemLanguage.French:
                gameLanguage = GameLanguage.fr;
                break;

            case (int)SystemLanguage.German:
                gameLanguage = GameLanguage.de;
                break;

            case (int)SystemLanguage.Italian:
                gameLanguage = GameLanguage.it;
                break;

            case (int)SystemLanguage.Spanish:
                gameLanguage = GameLanguage.es;
                break;

            default:
                gameLanguage = GameLanguage.en;
                break;
        }
    }

    // Use this for initialization
    private void Start () 
    {
		
	}
	
	// Update is called once per frame
	private void Update () 
    {
		
	}
}
