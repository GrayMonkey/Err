using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class LocManager : MonoBehaviour
{
    public static LocManager instance;
    public List<LocTextData> locText = new List<LocTextData>();

    GameOptions gameOptions;
    SystemLanguage lang;
    Translate[] trans;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        trans = Resources.FindObjectsOfTypeAll<Translate>();
    }

    public SystemLanguage gameLang
    {
        get
        {
            return lang;
        }

        set
        {
            lang = value;
            SetDefaultLang(lang);
        }
    }

    private void SetDefaultLang(SystemLanguage lang)
    {
        string m_lang = lang.ToString();
        string file = "LocText - " + m_lang;
        locText.Clear();

        TextAsset textData = Resources.Load<TextAsset>(file);

        //Remove any hard returns, new lines and double quotes
        //string[] rowData = textData.text.Split(new char[] { '\n' });
        string[] rowData = textData.text.Split(new char[] { '\r' , '\n' },System.StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < rowData.Length; i++)
        {
            //Split on the first comma
            int firstComma = rowData[i].IndexOf(",");
            string dataKey = rowData[i].Substring(0, firstComma);
            string dataValue = rowData[i].Substring(firstComma + 1);
            dataValue = dataValue.Replace("\\n", "\n");

            //Remove double quotes from beginning and end of string
            if (dataValue.StartsWith("\""))
            {
                dataValue = dataValue.Substring(1, dataValue.Length - 2);
                if(dataValue.Contains("\\n"))
                {
                    Debug.Log(dataValue);
                }
            }

            //Add data to locText
            LocTextData data = new LocTextData();
            data.key = dataKey;
            data.text = dataValue;
            locText.Add(data);
        }

        // Update the activate Text assests with Translate
        foreach (Translate _trans in trans)
        {
            if(_trans.gameObject.activeInHierarchy)
                _trans.UpdateString();
        }   
    }

    public string GetLocText(string key)
    {
        if(key == "")
        {
            Debug.LogWarning("GetLocText is trying to process a blank key");
            return null;
        }
        
        LocTextData transText = locText.Find(x => x.key == key);
        if (transText == null)
        {
            Debug.LogWarning(key + ": Key component for translation not found");
            return "Missing key : " + lang;
        }
        else if (transText.text == null)
        {
            Debug.LogWarning(key + ": Key found but translation missing");
            return "Missing translation";
        }
        else
        {
            string returnString = transText.text;
            if(returnString.Contains("%%PlayerName"))
                {
                string playerName = PlayerController.instance.activePlayer.playerName;
                returnString = returnString.Replace("%%PlayerName", playerName);
                }
            return returnString;
        }
    }
}
[System.Serializable]
public class Langs
{
    public bool english;
    public bool french;
    public bool german;
    public bool italian;
    public bool spanish;
}