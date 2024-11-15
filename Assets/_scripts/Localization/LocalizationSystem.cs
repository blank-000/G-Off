using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class LocalizationSystem
{
    // the currently active language
    public static Language SelectedLanguage = Language.english;

    // all available localizations
    static Dictionary<string, string> _localizationEN = new Dictionary<string, string>();
    static Dictionary<string, string> _localizationBG = new Dictionary<string, string>();
    static Dictionary<string, string> _localizationFR = new Dictionary<string, string>();
    static Dictionary<string, string> _localizationGR = new Dictionary<string, string>();

    public static bool isInitialized;
    public static CSVLoader loader;

    public static void Init()
    {
        loader = new CSVLoader();
        loader.LoadCSV();

        UpdateDictionaries();

        isInitialized = true;
    }

    public static Dictionary<string, string> GetCurrentDictionary(Language language)
    {
        if (!isInitialized) Init();
        UpdateDictionaries();

        switch (language)
        {
            case Language.english:
                return _localizationEN;
            case Language.french:
                return _localizationFR;
            case Language.german:
                return _localizationGR;
            case Language.bulgarian:
                return _localizationBG;
        }

        return _localizationEN;
    }

#if UNITY_EDITOR
    public static void Add(string key, string value)
    {
        // reformat quotation marks from the incoming value
        if (value.Contains("\"")) value = value.Replace('"', '\"');

        if (loader == null) loader = new CSVLoader();

        loader.LoadCSV();
        loader.AddEntry(key, value);
        loader.LoadCSV();

        UpdateDictionaries();
    }

    public static void Remove(string key)
    {
        if (loader == null) loader = new CSVLoader();

        loader.LoadCSV();
        loader.RemoveEntry(key);
        loader.LoadCSV();

        UpdateDictionaries();
    }


    public static void Replace(string key, string value)
    {

        // reformat quotation marks from the incoming value
        if (value.Contains("\"")) value.Replace('"', '\"');

        if (loader == null) loader = new CSVLoader();

        loader.LoadCSV();
        loader.EditEntry(key, value);
        loader.LoadCSV();

        UpdateDictionaries();
    }

#endif

    public static void UpdateDictionaries()
    {
        // it is probably overkill to load all dictionaries here 
        // I should think about only loading the current selected language, but these are future considerations
        _localizationEN = loader.GetLocalizedValues(Language.english);
        _localizationFR = loader.GetLocalizedValues(Language.french);
        _localizationGR = loader.GetLocalizedValues(Language.german);
        _localizationBG = loader.GetLocalizedValues(Language.bulgarian);

        // foreach (var key in _localizationFR)
        // {
        //     Debug.Log(key.Key + " - " + key.Value);
        // }
    }


    /// <summary>
    ///     Returns the translated value at key. If none is found returns the english value 
    /// </summary>
    public static string GetLocalizedText(string key)
    {
        if (!isInitialized) Init();

        string translatedValue = null;

        switch (SelectedLanguage)
        {
            case Language.english:
                _localizationEN.TryGetValue(key, out translatedValue);
                break;
            case Language.french:
                _localizationFR.TryGetValue(key, out translatedValue);
                break;
            case Language.german:
                _localizationGR.TryGetValue(key, out translatedValue);
                break;
            case Language.bulgarian:
                _localizationBG.TryGetValue(key, out translatedValue);
                break;
        }
        if (translatedValue == null) return key;
        return translatedValue;
    }

}
