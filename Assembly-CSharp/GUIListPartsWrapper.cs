using System;
using UI.MonsterInfoParts;
using UnityEngine;

public abstract class GUIListPartsWrapper : MonsterIconTouchEventUD
{
	[SerializeField]
	protected Vector2 partsSize;

	protected abstract void OnUpdatedParts(int listPartsIndex);

	public override void ShowGUI()
	{
		this.OnUpdatedParts(base.IDX);
	}

	public Vector2 GetPartsSize()
	{
		return this.partsSize;
	}
}
