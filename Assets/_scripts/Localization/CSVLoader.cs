using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;

public enum Language
{
    english,
    french,
    german,
    bulgarian
}

public class CSVLoader
{
    // translations file reference
    private TextAsset _csvFile;
    // variables pointing to file
    private static string _fileName = "LocalizationFromSheets";
    private static string _filePath = "Assets/Resources/" + _fileName + ".csv";
    // delimiters
    private char _newLine = '\n';
    private char _doubleQuotes = '"';
    private char _commas = ',';

    public void LoadCSV()
    {
        // loads a csv file that has headers of : key,en,languge to be translated into 1, languge to be translated into 2
        _csvFile = Resources.Load<TextAsset>(_fileName);
    }


    public Dictionary<string, string> GetLocalizedValues(Language code)
    {
        int languageIndex = -1;
        string languageId = languageCodeToString(code);
        Dictionary<string, string> dict = new Dictionary<string, string>();

        string[] lines = _csvFile.text.Split(_newLine);
        string[] headers = lines[0].Split(_commas, System.StringSplitOptions.None);

        // regular expression to match commas that are not inside double quotes
        // hopefully won't summon cthulhu when executing..
        Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

        // find the correct language header
        for (int i = 0; i < headers.Length; i++)
        {
            if (headers[i].Contains(languageId))
            {
                languageIndex = i;
                break;
            }
        }

        // populate the dictionary with all the key value pairs from the corresponding language
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i];
            string[] entries = CSVParser.Split(line);

            for (int j = 0; j < entries.Length; j++)
            {
                entries[j] = entries[j].TrimStart(' ', _doubleQuotes);
                entries[j] = entries[j].TrimEnd(_doubleQuotes);
            }

            if (entries.Length > languageIndex)
            {
                var key = entries[0];
                if (dict.ContainsKey(key)) continue;

                var value = entries[languageIndex];
                dict.Add(key, value);
            }
        }


        return dict;
    }

    public string languageCodeToString(Language code)
    {
        switch (code)
        {
            case (Language.english):
                return "en";
            case (Language.french):
                return "fr";
            case (Language.german):
                return "gr";
            case (Language.bulgarian):
                return "bg";
        }
        Debug.LogError("Language code to string conversion failure, defaulting to english!");
        return "en";
    }

#if UNITY_EDITOR

    public void AddEntry(string key, string value)
    {
        string appended = string.Format("\n{0},{1}", key, value);
        File.AppendAllText(_filePath, appended);

        UnityEditor.AssetDatabase.Refresh();
    }

    public void RemoveEntry(string key)
    {
        string[] lines = _csvFile.text.Split(_newLine);
        string[] keys = new string[lines.Length];

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            keys[i] = line.Split(_commas, System.StringSplitOptions.None)[0].Trim();
        }

        int index = -1;

        for (int i = 0; i < keys.Length; i++)
        {
            if (keys[i].Contains(key))
            {
                index = i;
                break;
            }
        }

        if (index > -1)
        {
            string[] newLines;
            newLines = lines.Where(w => w != lines[index]).ToArray();

            string replaced = string.Join(_newLine.ToString(), newLines);
            File.WriteAllText(_filePath, replaced);
        }
    }

    public void EditEntry(string key, string value)
    {
        RemoveEntry(key);
        AddEntry(key, value);
    }

#endif

}
