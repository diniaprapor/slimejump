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
    //private string locale;
    private Dictionary<UnityEngine.SystemLanguage, string> languages = new Dictionary<UnityEngine.SystemLanguage, string>
    {
        { SystemLanguage.English, "en" },
        { SystemLanguage.Russian, "ru" },
    };
    private Dictionary<string, Dictionary<string, string>> dicts = new Dictionary<string, Dictionary<string, string>>();
    private Dictionary<string, string> activeDict;
    private UnityEngine.SystemLanguage defaultLanguage = SystemLanguage.English;
    public TextAsset jsonFile;

    public string LoadLanguage()
    {
        string language = languages[defaultLanguage];
        if (languages.ContainsKey(Application.systemLanguage))
        {
            language = languages[Application.systemLanguage];
        }
        if (PlayerPrefs.HasKey("language") && languages.ContainsValue(PlayerPrefs.GetString("language")))
        {
            language = PlayerPrefs.GetString("language");
        }

        return language;
    }

    public void SaveLanguage(string language)
    {
        PlayerPrefs.SetString("language", language);
    }

    void LoadTexts()
    {
        foreach (KeyValuePair<UnityEngine.SystemLanguage, string> entry in languages)
        {
            dicts.Add(entry.Value, new Dictionary<string, string>());
        }

        LocaTexts textsInJson = JsonUtility.FromJson<LocaTexts>(jsonFile.text);

        foreach (LocaEntry e in textsInJson.entries)
        {
            //hack due to shitty json deserializer
            //did not want to use third party lib or change file format because of only this one place
            //Debug.Log("Found entry: " + e.key + " " + e.en);
            dicts["en"].Add(e.key, e.en);
            dicts["ru"].Add(e.key, e.ru);
        }
    }

    // Awake is always called before any Start function
    void Awake()
    {
        LoadTexts();
        SetActiveDictionary(LoadLanguage());
    }

    public void SetActiveDictionary(string newLlanguage)
    {
        if(!dicts.TryGetValue(newLlanguage, out activeDict))
        {
            Debug.Log("Localization not found: " + newLlanguage);
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
