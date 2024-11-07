using UnityEngine;

public class ShaderParam
{
    public static int OutlineColor = Shader.PropertyToID("_OutlineColor");
    public static int OutlineThickness = Shader.PropertyToID("_Thickness");
    public static int OutlineSampleCorners = Shader.PropertyToID("CORNERS_ON");
    public static int AlphaSlider = Shader.PropertyToID("Alpha_Slider");
}
