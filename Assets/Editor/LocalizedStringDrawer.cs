using UnityEditor;
using UnityEngine;


[CustomPropertyDrawer(typeof(LocalizedString))]
public class LocalizedStringDrawer : PropertyDrawer
{
    bool _dropdown;
    float _height;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (_dropdown)
        {
            return _height + 25;
        }
        return 20;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        position.width -= 34;
        position.height = 18;

        Rect valueRect = new Rect(position);
        valueRect.x += 15;
        valueRect.width -= 15;

        Rect foldButtonRect = new Rect(position);
        foldButtonRect.width = 15;

        _dropdown = EditorGUI.Foldout(foldButtonRect, _dropdown, "");

        position.x += 15;
        position.width -= 15;

        SerializedProperty key = property.FindPropertyRelative("Key");
        key.stringValue = EditorGUI.TextField(position, key.stringValue);

        position.x += position.width + 2;
        position.width = 17;
        position.height = 17;

        Texture searchIcon = Resources.Load<Texture>("search");
        GUIContent searchContent = new GUIContent(searchIcon);

        if (GUI.Button(position, searchContent))
        {
            TextLocalizerSearchWindow.Open();
        }

        position.x += position.width + 2;

        Texture addIcon = Resources.Load<Texture>("add");
        GUIContent addContent = new GUIContent(addIcon);

        if (GUI.Button(position, addContent))
        {
            TextLocalizerEditorWindow.Open(key.stringValue);
        }

        if (_dropdown)
        {
            var value = LocalizationSystem.GetLocalizedText(key.stringValue);
            GUIStyle style = GUI.skin.box;
            _height = style.CalcHeight(new GUIContent(value), valueRect.width);

            valueRect.height = _height;
            valueRect.y += 21;
            EditorGUI.LabelField(valueRect, value, EditorStyles.wordWrappedLabel);
        }

        EditorGUI.EndProperty();
    }
}
