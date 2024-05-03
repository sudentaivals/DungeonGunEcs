using UnityEditor;
using UnityEngine;

public class RenderTest : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sourceSr;
    [SerializeField] private SpriteRenderer _thisSr;

    private Color[] GetPixels(Texture2D originalTexture)
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
    
            Color[] pixelData = originalTexture.GetPixels();
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
        var width = _sourceSr.sprite.texture.width;
        var height = _sourceSr.sprite.texture.height;

        int fragmentWidth = Random.Range(width / 2, width / 3);
        int fragmentHeight = Random.Range(height / 2, height / 3);
        int fragmentX = Random.Range(0, width - fragmentWidth);
        int fragmentY = Random.Range(0, height - fragmentHeight);

        var sourcePixels = GetPixels(_sourceSr.sprite.texture);
        var fragmentPixels = new Color[fragmentWidth * fragmentHeight];
        
        Texture2D fragmentTexture = new Texture2D(fragmentWidth, fragmentHeight);
        int sourceIndex = 0;
    
        // Циклы для копирования пикселей
        for (int y = 0; y < fragmentHeight; y++)
        {
            for (int x = 0; x < fragmentWidth; x++)
            {
                // Вычисляем индекс пикселя в исходной текстуре
                sourceIndex = (fragmentY + y) * width + (fragmentX + x);

                // Копируем пиксель
                fragmentPixels[y * fragmentWidth + x] = sourcePixels[sourceIndex];
            }
        }
        // Создаем спрайт из новой текстуры
        Sprite fragmentSprite = Sprite.Create(
            fragmentTexture, 
            new Rect(0, 0, fragmentWidth, fragmentHeight), 
            new Vector2(0.5f, 0.5f));
        _thisSr.sprite = fragmentSprite;

    }
}
