using System;
using UnityEngine;
using UnityEngine.Serialization;

[AddComponentMenu("GUI/GameStrings Font")]
[ExecuteInEditMode]
public sealed class GameStringsFont : MonoBehaviour
{
	private TextMesh textMesh;

	private UILabel NGFont;

	[FormerlySerializedAs("color_")]
	[SerializeField]
	private Color _color = new Color(1f, 1f, 1f, 1f);

	private bool first = true;

	private string text_;

	private void GetFontComp()
	{
		if (this.first)
		{
			this.first = false;
			this.textMesh = base.GetComponent<TextMesh>();
			this.NGFont = base.GetComponent<UILabel>();
		}
	}

	public Color color
	{
		get
		{
			return this._color;
		}
		set
		{
			this.GetFontComp();
			this._color = value;
			if (this.textMesh != null)
			{
				this.textMesh.color = this._color;
			}
			if (this.NGFont != null)
			{
				this.NGFont.color = this._color;
			}
		}
	}

	public string text
	{
		get
		{
			return this.text_;
		}
		set
		{
			this.GetFontComp();
			this.text_ = value;
			if (this.textMesh != null)
			{
				this.textMesh.text = this.text_;
			}
			else if (this.NGFont != null)
			{
				this.NGFont.text = this.text_;
			}
		}
	}

	public string FocusText
	{
		get
		{
			string result = string.Empty;
			if (this.textMesh != null)
			{
				result = this.textMesh.text;
			}
			else if (this.NGFont != null)
			{
				result = this.NGFont.text;
			}
			return result;
		}
	}

	private void Awake()
	{
		this.GetFontComp();
	}
}
