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
    }

    private void Start()
    {
        SetLang(GameLang);
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

    public void SetLang(SystemLanguage lang)
    {
        string m_lang = lang.ToString();
        string file = "LocText - " + m_lang;
        locText.Clear();

        TextAsset textData = Resources.Load<TextAsset>(file);

        //string[] rowData = textData.text.Split(new char[] { '\n' });
        string[] rowData = textData.text.Split(new char[] { '\r' , '\n' },System.StringSplitOptions.RemoveEmptyEntries);


        for (int i = 0; i < rowData.Length; i++)
        {
            string[] cellData = rowData[i].Split(new char[] { ',' });
            LocTextData data = new LocTextData();
            data.key = cellData[0];
            data.text = cellData[1];
            locText.Add(data);
        }


        // Update all the strings in the scene 
        Translate[] updateStrings = FindObjectsOfType<Translate>();

        foreach (Translate _string in updateStrings)
        {
            _string.UpdateString();
            //_string.text.text = GetLocText(_string.key);

        }
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