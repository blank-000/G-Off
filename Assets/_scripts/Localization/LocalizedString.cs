using UnityEngine;

[System.Serializable]
public struct LocalizedString
{
    public string Key;
    public LocalizedString(string key)
    {
        this.Key = key;
    }

    public string value
    {
        get
        {
            return LocalizationSystem.GetLocalizedText(Key);
        }
    }

    public static implicit operator LocalizedString(string key)
    {
        return new LocalizedString(key);
    }
}
