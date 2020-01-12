using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// load loca file
//make singleton
[System.Serializable]
public class LocaEntry
{
    public string key;
    public string en;
    public string ru;
}

[System.Serializable]
public class LocaTexts
{
    public LocaEntry[] entries;
}

public class LocalizationManager : MonoBehaviour
{
    private string locale;
    private Dictionary<string, Dictionary<string, string>> dicts = new Dictionary<string, Dictionary<string, string>>();
    private Dictionary<string, string> activeDict;
    public TextAsset jsonFile;

    void LoadTexts()
    {
        //hardcoding locales looks kinda ugly, but with Unity Json handling it's simplest way
        dicts.Add("en", new Dictionary<string, string>());
        dicts.Add("ru", new Dictionary<string, string>());

        LocaTexts textsInJson = JsonUtility.FromJson<LocaTexts>(jsonFile.text);

        foreach (LocaEntry e in textsInJson.entries)
        {
            //Debug.Log("Found entry: " + e.key + " " + e.en);
            dicts["en"].Add(e.key, e.en);
            dicts["ru"].Add(e.key, e.ru);
        }
    }

    // Awake is always called before any Start function
    void Awake()
    {
        LoadTexts();
        SetLocale("en"); //get from OS
    }

    public void SetLocale(string newLocale)
    {
        if(dicts.TryGetValue(newLocale, out activeDict))
        {
            locale = newLocale;
        }
        else
        {
            Debug.Log("Locale not found: " + newLocale);
        }
    }

    public string GetText(string key)
    {
        string result = key + "_localization_not_found";
        if (activeDict != null)
        {
            activeDict.TryGetValue(key, out result);
        }

        return result;
    }
}
