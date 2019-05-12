using System;
using TMPro;
using UnityEngine;

public class AnimationColorForTextMeshPro : MonoBehaviour
{
	[Header("TextMeshPro")]
	public TextMeshPro textMeshPro;

	[Header("アニメーションさせるカラー")]
	public Color color = Color.white;

	private void Update()
	{
		this.textMeshPro.color = this.color;
	}
}
