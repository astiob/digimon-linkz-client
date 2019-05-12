using System;
using TMPro;
using UnityEngine;

public class TweenAlphaForTextMeshPro : TweenAlpha
{
	private TextMeshPro[] textMeshPros;

	protected override void Start()
	{
		base.Start();
		this.textMeshPros = base.GetComponentsInChildren<TextMeshPro>(true);
	}

	protected override void OnUpdate(float factor, bool isFinished)
	{
		base.OnUpdate(factor, isFinished);
		if (this.textMeshPros != null)
		{
			foreach (TextMeshPro textMeshPro in this.textMeshPros)
			{
				Color color = textMeshPro.color;
				color.a = base.value;
				textMeshPro.color = color;
			}
		}
	}
}
