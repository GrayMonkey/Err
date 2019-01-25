using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LocManager : MonoBehaviour
{
    public static LocManager locManager;
    public List<LocTextData> locText = new List<LocTextData>();

    private SystemLanguage gameLang;

    private void Awake()
    {
        locManager = this;
        GameLang = Application.systemLanguage;
        GameLang = SystemLanguage.Spanish;
        SetLang();
    }

    public SystemLanguage GameLang
    {
        get
        {
            return gameLang;
        }

        set
        {
            gameLang = value;
        }
    }

    private void SetLang()
    {
        string m_lang = GameLang.ToString();
        string file = "LocText - " + m_lang;

        TextAsset textData = Resources.Load<TextAsset>(file);

        string[] rowData = textData.text.Split(new char[] { '\n' });

        for (int i = 0; i < rowData.Length; i++)
        {
            string[] cellData = rowData[i].Split(new char[] { ',' });
            LocTextData data = new LocTextData();
            data.key = cellData[0];
            data.text = cellData[1];
            locText.Add(data);
        }

        Debug.Log(locText[32].key + ": " + locText[32].text);

        string text = GetLocText("UI_Trash");
        Debug.Log(text);
    }

    public string GetLocText(string key)
    {
        LocTextData text = locText.Find(x => x.key == key);
        if (text == null)
        {
            return ("Key for text not found!");
        }
        else
        {
            return text.text;
        }
    }
}