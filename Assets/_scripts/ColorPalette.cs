using UnityEngine;

[CreateAssetMenu(fileName = "Color Palette")]
public class ColorPalette : ScriptableObject
{
    public Color Light = new Color(251 / 255f, 238 / 255f, 191 / 255f, 255 / 255f);
    public Color Mid = new Color(108 / 255f, 132 / 255f, 149 / 255f, 255 / 255f);
    public Color Dark = new Color(70 / 255f, 92 / 255f, 115 / 255f, 255 / 255f);
}