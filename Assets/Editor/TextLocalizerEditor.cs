using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using System.Collections.Generic;

public class TextLocalizerEditorWindow : EditorWindow
{
    public string Key;
    public string Value;

    public static void Open(string key)
    {
        TextLocalizerEditorWindow window = new TextLocalizerEditorWindow();
        window.titleContent = new GUIContent("Localization Window");
        window.ShowUtility();
        window.Key = key;
    }

    public void OnGUI()
    {
        Key = EditorGUILayout.TextField("Key :", Key);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Value :", GUILayout.MaxWidth(50));

        EditorStyles.textArea.wordWrap = true;
        Value = EditorGUILayout.TextArea(Value, EditorStyles.textArea, GUILayout.Height(100), GUILayout.Width(400));
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Add"))
        {
            if (LocalizationSystem.GetLocalizedText(Key) != string.Empty)
            {
                LocalizationSystem.Replace(Key, Value);
            }
            else
            {
                LocalizationSystem.Add(Key, Value);
            }
        }

        minSize = new Vector2(460, 250);
        maxSize = minSize;
    }
}

public class TextLocalizerSearchWindow : EditorWindow
{
    public static void Open()
    {
        TextLocalizerSearchWindow window = new TextLocalizerSearchWindow();
        window.titleContent = new GUIContent("Localization Search");

        Vector2 mouse = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
        Rect r = new Rect(mouse.x - 450, mouse.y + 10, 10, 10);
        window.ShowAsDropDown(r, new Vector2(500, 300));
    }

    public string Value;
    public Vector2 Scroll;
    public Dictionary<string, string> Dict;

    private void OnEnable()
    {
        Dict = LocalizationSystem.GetEditorDictionary();
    }

    public void OnGUI()
    {
        EditorGUILayout.BeginHorizontal("Box");
        EditorGUILayout.LabelField("Search:", EditorStyles.boldLabel);
        Value = EditorGUILayout.TextField(Value);
        EditorGUILayout.EndHorizontal();

        GetSearchResults();
    }

    private void GetSearchResults()
    {
        if (Value == null) return;

        EditorGUILayout.BeginVertical();
        Scroll = EditorGUILayout.BeginScrollView(Scroll);
        foreach (KeyValuePair<string, string> element in Dict)
        {
            if (element.Key.ToLower().Contains(Value.ToLower()) || element.Value.ToLower().Contains(Value.ToLower()))
            {
                EditorGUILayout.BeginHorizontal("box");
                Texture closeIcon = Resources.Load<Texture>("remove");

                GUIContent content = new GUIContent(closeIcon);

                if (GUILayout.Button(content, GUILayout.MaxWidth(20), GUILayout.MaxHeight(20)))
                {
                    if (EditorUtility.DisplayDialog("Remove Key " + element.Key + "?", "This will remove the element from the .csv localization file, are you sure?", "Confirm"))
                    {
                        LocalizationSystem.Remove(element.Key);
                        AssetDatabase.Refresh();
                        LocalizationSystem.Init();
                        Dict = LocalizationSystem.GetEditorDictionary();
                    }
                }

                EditorGUILayout.TextField(element.Key);
                EditorGUILayout.LabelField(element.Value);
                EditorGUILayout.EndHorizontal();
            }
        }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }


}
