using UnityEngine;

public class ShaderTest : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    private MaterialPropertyBlock _mPropertyBlock;
    private void Start()
    {
        _mPropertyBlock = new MaterialPropertyBlock();
        //_mPropertyBlock.SetFloat("_Thickness", _thickness);
        //_renderer.SetPropertyBlock(_mPropertyBlock);
    }

    private void Update()
    {
        var color = Random.insideUnitSphere;
        _renderer.material.SetColor(ShaderParam.OutlineColor, new Color(color.x, color.y, color.z, 1));
        _renderer.material.SetFloat(ShaderParam.OutlineThickness, Random.Range(0.0f, 2f));
    }
}
