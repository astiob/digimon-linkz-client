using Master;
using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshPro))]
public class TextMeshProStringKey : MonoBehaviour
{
	[SerializeField]
	[Header("文言マスタのキー")]
	private string stringMasterKey;

	[SerializeField]
	private TextMeshPro textMeshPro;

	private string masterString;

	protected void Awake()
	{
		this.masterString = StringMaster.GetString(this.stringMasterKey);
		if (this.textMeshPro != null)
		{
			this.textMeshPro.text = this.masterString;
			CountrySetting.ConvertTMProText(ref this.textMeshPro);
		}
	}

	protected void OnTransformChildrenChanged()
	{
		if (base.GetComponent<NGUIDepth>() != null)
		{
			base.GetComponent<NGUIDepth>().includeChildren = true;
			base.GetComponent<NGUIDepth>().Reset();
		}
	}

	protected void Reset()
	{
		this.textMeshPro = base.GetComponent<TextMeshPro>();
	}

	public string GetMasterString()
	{
		return this.masterString;
	}
}
