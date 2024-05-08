using UnityEditor;
using UnityEngine;

public class RenderTest : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sourceSr;
    [SerializeField] private SpriteRenderer _thisSr;

    private Color[] GetPixels(Texture2D originalTexture, int x, int y, int width, int height)
    {
        // Get pixel data
        bool isReadable = originalTexture.isReadable;
        TextureImporter ti = null;
        try
        {
            // Ensure original texture is readable
            if (!isReadable)
            {
                var origTexPath = AssetDatabase.GetAssetPath(originalTexture);
                ti = (TextureImporter)AssetImporter.GetAtPath(origTexPath);
                ti.isReadable = true;
                ti.SaveAndReimport();
            }
    
            Color[] pixelData = originalTexture.GetPixels(x, y, width, height);
            return pixelData;
        }
        finally
        {
            // Revert
            if (!isReadable && ti != null)
            {
                ti.isReadable = false;
                ti.SaveAndReimport();
            }
        }
    }
    private void Start() 
    {
        Texture2D originalTexture = _sourceSr.sprite.texture;

        // Генерация случайных параметров
        int x = Random.Range(0, originalTexture.width);
        int y = Random.Range(0, originalTexture.height);
        int width = Random.Range(1, originalTexture.width - x);
        int height = Random.Range(1, originalTexture.height - y);
        Texture2D newTexture = new Texture2D(width, height);
        var sourcePixels = GetPixels(_sourceSr.sprite.texture, x, y, width, height);
        newTexture.SetPixels(sourcePixels);
        newTexture.Apply();
        
        // Создаем спрайт из новой текстуры
        Sprite fragmentSprite = Sprite.Create(
            newTexture, 
            new Rect(0, 0, width, height), 
            new Vector2(0.5f, 0.5f), 16);
        _thisSr.sprite = fragmentSprite;

    }
}
