using System.Threading.Tasks;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextLocalizer : MonoBehaviour
{

    TextMeshProUGUI _textField;
    string _key;


    async void OnEnable()
    {
        _textField = GetComponent<TextMeshProUGUI>();
        _key = _textField.text;
#if UNITY_EDITOR
        if (!LocalizationSystem.GetCurrentDictionary(LocalizationSystem.SelectedLanguage).ContainsKey(_key)) LocalizationSystem.Replace(_key, _textField.text);
#endif
        _textField.text = LocalizationSystem.GetLocalizedText(_key);

        await RegisterWhenReadyAsync();
    }


    void OnDisable()
    {
        LocalizationManager.Instance.DeregisterLocalizedText(this);



    }

    public void UpdateLanguage(Language newLanguage)
    {
        _textField.text = LocalizationSystem.GetLocalizedText(_key);
    }

    private async Task RegisterWhenReadyAsync()
    {

        while (LocalizationManager.Instance == null)
        {
            await Task.Delay(100);  // Small delay to avoid busy-waiting
        }

        LocalizationManager.Instance.RegisterLocalizedText(this);
    }
}
