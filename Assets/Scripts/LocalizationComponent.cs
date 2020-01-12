using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class LocalizationComponent : MonoBehaviour
{
    public string key;
    // Start is called before the first frame update
    void Start()
    {
        // find localization manager in globals
        // scan current for text field
        // replace by provided value
        GameObject globals = GameObject.Find("Globals");
        Assert.IsNotNull(globals, "Globals GameObject not found!");
        LocalizationManager locaMgr = globals.GetComponent<LocalizationManager>();
        Assert.IsNotNull(locaMgr, "LocalizationManager not found!");
        Text goText = GetComponent<Text>();
        goText.text = locaMgr.GetText(key);
    }
}
