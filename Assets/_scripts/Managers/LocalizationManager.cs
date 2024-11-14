using System.Collections.Generic;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance;
    List<TextLocalizer> _listeners = new List<TextLocalizer>();

    void Awake()
    {
        // singleton init
        if (Instance != null) Destroy(this.gameObject);
        else Instance = this;
    }

    public void RegisterLocalizedText(TextLocalizer listener)
    {
        if (_listeners.Contains(listener)) return;

        _listeners.Add(listener);
    }

    public void DeregisterLocalizedText(TextLocalizer listener)
    {
        if (!_listeners.Contains(listener)) return;

        _listeners.Remove(listener);
    }

    public void ChangeLanguage(Language newLanguage)
    {
        LocalizationSystem.SelectedLanguage = newLanguage;
        foreach (var listener in _listeners)
        {
            listener.UpdateLanguage(newLanguage);
        }
    }
}
