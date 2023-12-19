using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct MaterialComponent 
{
    public SpriteRenderer Renderer;
    public Stack<Color> Colors;
}
