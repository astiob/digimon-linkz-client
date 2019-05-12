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
