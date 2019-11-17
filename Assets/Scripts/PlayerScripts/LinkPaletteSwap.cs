using System.Collections;
using UnityEngine;

public enum ArmorType { Blue, Red }

[ExecuteInEditMode]
public class LinkPaletteSwap : MonoBehaviour
{
    [SerializeField] private Color _armorColorIn = default;
    [SerializeField] private Color _armorColorOut = default;
    [SerializeField] private Color _hairColorIn = default;
    [SerializeField] private Color _hairColorOut = default;
    [SerializeField] private Color _skinColorIn = default;
    [SerializeField] private Color _skinColorOut = default;
    private GameObject _link;
    private Material _linkMaterial;
    private readonly string _defaultArmorColor = "#80d010";
    private readonly string _defaultHairColor = "#c84c0c";
    private readonly string _defaultSkinColor = "#fc9838";
    private int _blinkingLoops = 5;


    void OnEnable()
	{
		Shader shader = Shader.Find("Hidden/PaletteSwapNaive");
		if (_linkMaterial == null)
			_linkMaterial = new Material(shader);
    }

	void OnRenderImage(RenderTexture source, RenderTexture dest)
	{
		_linkMaterial.SetColor("_In0", _armorColorIn);
		_linkMaterial.SetColor("_Out0", _armorColorOut);
        _linkMaterial.SetColor("_In1", _hairColorIn);
        _linkMaterial.SetColor("_Out1", _hairColorOut);
        _linkMaterial.SetColor("_In2", _skinColorIn);
        _linkMaterial.SetColor("_Out2", _skinColorOut);
        Graphics.Blit(source, dest,  _linkMaterial);
	}

    public void SetArmor(ArmorType armorType)
    {
        switch (armorType)
        {
            case ArmorType.Blue:
                ColorUtility.TryParseHtmlString("#c4d4fc", out _armorColorOut);
                break;
            case ArmorType.Red:
                ColorUtility.TryParseHtmlString("#d82800", out _armorColorOut);
                break;
        }
    }

    public void StartBlinking(GameObject link)
    {
        _link = link;
        StartCoroutine(Blinking());
    }

    IEnumerator Blinking()
    {
        if (_blinkingLoops > 0)
        {
            ColorUtility.TryParseHtmlString("#000000", out _armorColorOut);
            ColorUtility.TryParseHtmlString("#d82800", out _hairColorOut);
            ColorUtility.TryParseHtmlString("#008088", out _skinColorOut);
            yield return new WaitForSeconds(0.05f);
            ColorUtility.TryParseHtmlString("#d82800", out _armorColorOut);
            ColorUtility.TryParseHtmlString("#fcfcfc", out _hairColorOut);
            ColorUtility.TryParseHtmlString("#fc9838", out _skinColorOut);
            yield return new WaitForSeconds(0.05f);
            ColorUtility.TryParseHtmlString("#0000a8", out _armorColorOut);
            ColorUtility.TryParseHtmlString("#fcfcfc", out _hairColorOut);
            ColorUtility.TryParseHtmlString("#5c94fc", out _skinColorOut);
            yield return new WaitForSeconds(0.05f);
            ColorUtility.TryParseHtmlString(_defaultArmorColor, out _armorColorOut);
            ColorUtility.TryParseHtmlString(_defaultHairColor, out _hairColorOut);
            ColorUtility.TryParseHtmlString(_defaultSkinColor, out _skinColorOut);

            _blinkingLoops--;
            StartCoroutine(Blinking());
        }
        else
        {
            _link.GetComponent<LinkLOZ>().SetInvisibility(false);
            _blinkingLoops = 5;
        }
    }
}
