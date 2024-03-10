using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
[DefaultExecutionOrder (310)]
public class LocalizationManager : MonoBehaviour
{
    private string currentLanguage;

    private Dictionary<string, string> localizedText;
    public static bool isReady = false;
    public LocalizationData loadedData = new LocalizationData();
    public delegate void ChangeLangText();
    public event ChangeLangText OnLanguageChanged;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("Language"))
        {
            //PlayerPrefs.SetString("Language", "en_US");
            //    if (Application.systemLanguage == SystemLanguage.Russian || Application.systemLanguage == SystemLanguage.Ukrainian || Application.systemLanguage == SystemLanguage.Belarusian)
            //        PlayerPrefs.SetString("Language", "ru_RU");
            //if (Application.systemLanguage == SystemLanguage.English)
            //    PlayerPrefs.SetString("Language", "en_US");
            //else if (Application.systemLanguage == SystemLanguage.Japanese)
            //    PlayerPrefs.SetString("Language", "jp_JP");
            if (Application.systemLanguage == SystemLanguage.Japanese)
                PlayerPrefs.SetString("Language", "jp_JP");
            else
                PlayerPrefs.SetString("Language", "en_US");
        }
        currentLanguage = PlayerPrefs.GetString("Language");


        LoadLocalizedText(currentLanguage);
    }

    public void LoadLocalizedText(string langName)
    {
        string path = Path.Combine(Application.streamingAssetsPath, langName + ".json");
        string dataAsJson;
        if (Application.platform == RuntimePlatform.Android)
        {
#pragma warning disable CS0618 // Тип или член устарел
            WWW reader = new WWW(path);
#pragma warning restore CS0618 // Тип или член устарел
            while (!reader.isDone) { }
            dataAsJson = System.Text.Encoding.UTF8.GetString(reader.bytes, 3, reader.bytes.Length - 3);

        }
        else
        {
              dataAsJson = File.ReadAllText(path);
        }

        loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);

        localizedText = new Dictionary<string, string>();
        for (int i = 0; i < loadedData.items.Length; i++)
        {
            localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
        }
        PlayerPrefs.SetString("Language", langName);
        currentLanguage = PlayerPrefs.GetString("Language");
        isReady = true;

        OnLanguageChanged?.Invoke();
    }

    public string GetLocalizedValue(string key)
    {
        if (localizedText.ContainsKey(key))
        {
            return localizedText[key];
        }
        else
        {
            throw new Exception("Localized text with key \"" + key + "\" not found");
        }
    }
    public string CurrentLanguage
    {
        get
        {
            return currentLanguage;
        }
        set
        {
            PlayerPrefs.SetString("Language", value);
            currentLanguage = PlayerPrefs.GetString("Language");
        }
    }
    

    public bool IsReady
    {
        get
        {
            return isReady;
        }
    }
}
