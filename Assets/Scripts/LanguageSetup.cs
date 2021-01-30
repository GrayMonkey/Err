using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using UnityEngine.UI;

public class LanguageSetup : MonoBehaviour
{
    [SerializeField] Text title;
    [SerializeField] Text body1;
    [SerializeField] Image defLangFlag;
    [SerializeField] Text defLangName;
    [SerializeField] Text body2;
    [SerializeField] Text prefLangName;
    [SerializeField] Toggle[] langToggles;

    LocManager locManager;
    SystemLanguage prefLang;
    int toggleID;

    // Start is called before the first frame update
    void Start()
    {
        locManager = LocManager.instance;
        prefLang = locManager.GameLang;

        switch (prefLang)
        {
            case SystemLanguage.French:
                toggleID = 1;
                defLangFlag.sprite = langToggles[toggleID].GetComponentInChildren<Image>().sprite;
                defLangName.text = locManager.GetLocText("str_French");
                break;

            case SystemLanguage.German:
                toggleID = 2;
                defLangFlag.sprite = langToggles[toggleID].GetComponentInChildren<Image>().sprite;
                defLangName.text = locManager.GetLocText("str_German");
                break;

            case SystemLanguage.Italian:
                toggleID = 3;
                defLangFlag.sprite = langToggles[toggleID].GetComponentInChildren<Image>().sprite;
                defLangName.text = locManager.GetLocText("str_Italian");
                break;

            case SystemLanguage.Spanish:
                toggleID = 4;
                defLangFlag.sprite = langToggles[toggleID].GetComponentInChildren<Image>().sprite;
                defLangName.text = locManager.GetLocText("str_Spanish");
                break;

            default:
                toggleID = 0;
                defLangFlag.sprite = langToggles[toggleID].GetComponentInChildren<Image>().sprite;
                defLangName.text = locManager.GetLocText("str_English");
                break;
        }

        langToggles[toggleID].isOn = true;
    }

    public void SetPrefLangInfo(int toggle)
    {
        // Set the prefLang

        switch(toggle)
        {
            case 1:
                prefLang = SystemLanguage.French;
                break;

            case 2:
                prefLang = SystemLanguage.German;
                break;

            case 3:
                prefLang = SystemLanguage.Italian;
                break;

            case 4:
                prefLang = SystemLanguage.Spanish;
                break;

            default:
                prefLang = SystemLanguage.English;
                break;
        }

        locManager.GameLang = prefLang;

        /*        // Set the preferred language toggle
                foreach (Toggle _toggle in langToggles)
                    _toggle.isOn = false;
                langToggles[toggleID].isOn = true;
        */
        // Set up the preferred language string
        string newText1 = locManager.GetLocText("str_LanguageSetupPreferred");
        string newText2 = locManager.GetLocText("str_" + prefLang);
        prefLangName.text = newText1 + newText2;
        //prefLangName.GetComponent<Translate>().UpdateString();

        
    }
/*
    public void TogglePrefLang()
    {
        // Use toggles to set prefLang and toggleID
        string _lang = gameObject.name;

        switch(_lang)
        {
            case "French":
                toggleID = 1;
                prefLang = SystemLanguage.French;
                break;

            case "German":
                toggleID = 2;
                prefLang = SystemLanguage.German;
                break;

            case "Italian":
                toggleID = 3;
                prefLang = SystemLanguage.Italian;
                break;

            case "Spanish":
                toggleID = 4;
                prefLang = SystemLanguage.Spanish;
                break;

            default:
                toggleID = 0;
                prefLang = SystemLanguage.English;
                break;
        }
    }*/
}
