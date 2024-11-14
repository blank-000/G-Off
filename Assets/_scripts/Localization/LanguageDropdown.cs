using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using TMPro;

public class LanguageDropdown : MonoBehaviour
{
    TMP_Dropdown _dropdown;

    void Start()
    {
        _dropdown = GetComponent<TMP_Dropdown>();

        // Populate dropdown with enum values
        PopulateDropdown();

        // Add listener for when the dropdown value changes
        _dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    private void PopulateDropdown()
    {
        // Get names of the Language enum and populate the dropdown
        var languages = Enum.GetNames(typeof(Language)).ToList();
        _dropdown.ClearOptions();
        _dropdown.AddOptions(languages);
    }

    private void OnDropdownValueChanged(int index)
    {
        // Parse the selected enum value and set the language
        Language selectedLanguage = (Language)Enum.Parse(typeof(Language), _dropdown.options[index].text);
        LocalizationManager.Instance.ChangeLanguage(selectedLanguage);
    }
}
