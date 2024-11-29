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


    Color _col;
    List<Transform> _children = new List<Transform>();
    List<Renderer> _availableRenderers = new List<Renderer>();


    void Start()
    {
        EvaluateColor(Choice);

        // currently only works with raw image components, and exits early if the bool is set
        if (WorkWithUI)
        {
            var img = GetComponent<RawImage>();
            if (img != null)
            {
                img.color = _col;
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



    private void EvaluateColor(ChooseColorFromPalette C)
    {
        switch (C)
        {
            case (ChooseColorFromPalette.Light):
                _col = Palette.Light;
                break;
            case (ChooseColorFromPalette.MidTone):
                _col = Palette.MidTone;
                break;
            case (ChooseColorFromPalette.Dark):
                _col = Palette.Dark;
                break;
            case (ChooseColorFromPalette.Accent):
                _col = Palette.Accent;
                break;

        }

    }
}
