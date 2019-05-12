using System;
using UnityEngine;

public sealed class SortieLimitListItem : MonoBehaviour
{
	[SerializeField]
	private UILabel info;

	public void SetText(string text)
	{
		this.info.text = text;
	}
}
