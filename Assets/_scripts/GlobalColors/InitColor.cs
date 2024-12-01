using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitColor : MonoBehaviour
{
    public bool SearchInChildren;
    public bool WorkWithUI;
    public enum ChooseColorFromPalette
    {
        Light,
        MidTone,
        Dark,
        Accent
    }
    public ChooseColorFromPalette Choice;
    public ChooseColorFromPalette InvertedChoice;


    Color _col, _invCol;
    List<Transform> _children = new List<Transform>();
    List<Renderer> _availableRenderers = new List<Renderer>();

    public void InvertImageColors(object data)
    {
        if (data is WorldState state)
        {
            var rimg = GetComponent<Image>();
            rimg.color = (state == WorldState.Dark) ? _col : _invCol;

        }
    }


    void Start()
    {
        _col = EvaluateColor(Choice);
        _invCol = EvaluateColor(InvertedChoice);



        if (WorkWithUI)
        {
            var img = GetComponent<RawImage>();
            var rimg = GetComponent<Image>();
            if (img != null)
            {
                img.color = _col;
            }
            if (rimg != null)
            {
                rimg.color = _col;
            }
            return;
        }

        // go trough and find all renderers
        if (SearchInChildren)
        {
            GetChildRecursive(transform);
            foreach (Transform child in _children)
            {
                child.TryGetComponent<Renderer>(out Renderer rend);
                if (rend == null) continue;

                _availableRenderers.Add(rend);
            }
        }
        else
        {
            TryGetComponent<Renderer>(out Renderer rend);
            if (rend == null) return;

            _availableRenderers.Add(rend);
        }

        // apply the chosen color to every material found;
        foreach (Renderer rend in _availableRenderers)
        {
            Material mat = rend.sharedMaterial;
            mat.color = _col;
        }
    }



    private void GetChildRecursive(Transform parent)
    {
        if (null == parent)
            return;

        foreach (Transform child in parent)
        {
            if (null == child)
                continue;
            _children.Add(child);
            GetChildRecursive(child);
        }
    }



    private Color EvaluateColor(ChooseColorFromPalette C)
    {
        Color col = Color.black;
        switch (C)
        {
            case (ChooseColorFromPalette.Light):
                col = Palette.Light;
                break;
            case (ChooseColorFromPalette.MidTone):
                col = Palette.MidTone;
                break;
            case (ChooseColorFromPalette.Dark):
                col = Palette.Dark;
                break;
            case (ChooseColorFromPalette.Accent):
                col = Palette.Accent;
                break;

        }
        return col;

    }
}
