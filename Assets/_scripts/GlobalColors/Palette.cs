// using UnityEngine;

// public static class Palette
// {
//     public static ColorPalette current;

//     public static readonly Color Light = current.Light;
//     public static readonly Color MidTone = current.Mid;
//     public static readonly Color Dark = current.Dark;
// }

using UnityEngine;

public static class Palette
{
    private static ColorPalette _currentPalette;

    public static ColorPalette CurrentPalette
    {
        get
        {
            if (_currentPalette == null)
            {
                Debug.LogError("Palette: CurrentPalette is not assigned!");
            }
            return _currentPalette;
        }
    }

    // Static accessors for the colors
    public static Color Light => CurrentPalette.Light;
    public static Color MidTone => CurrentPalette.Mid;
    public static Color Dark => CurrentPalette.Dark;
    public static Color Accent => CurrentPalette.Accent;

    // Initialize the palette (typically you would assign this in the editor)
    [RuntimeInitializeOnLoadMethod]
    private static void InitializePalette()
    {
        _currentPalette = Resources.Load<ColorPalette>("ColorPalette");
        if (_currentPalette == null)
        {
            Debug.LogError("Palette: ColorPalette asset not found in Resources folder.");
        }
    }
}