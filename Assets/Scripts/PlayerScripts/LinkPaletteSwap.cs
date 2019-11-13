using UnityEngine;

public enum ArmorType { Blue, Red }

[ExecuteInEditMode]
public class LinkPaletteSwap : MonoBehaviour
{
	[SerializeField] private Color in0;
    [SerializeField] private Color out0;

    Material _linkMaterial;

	void OnEnable()
	{
		Shader shader = Shader.Find("Hidden/PaletteSwapNaive");
		if (_linkMaterial == null)
			_linkMaterial = new Material(shader);
    }

	void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
		_linkMaterial.SetColor("_In0", in0);
		_linkMaterial.SetColor("_Out0", out0);

        Graphics.Blit(src, dst,  _linkMaterial);
	}

    public void SetArmor(ArmorType armorType)
    {
        switch (armorType)
        {
            case ArmorType.Blue:
                ColorUtility.TryParseHtmlString("#5c94fc", out out0);
                break;
            case ArmorType.Red:
                ColorUtility.TryParseHtmlString("#d82800", out out0);
                break;
        }
    }
}
