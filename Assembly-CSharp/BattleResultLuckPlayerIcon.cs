using System;
using UnityEngine;

public sealed class BattleResultLuckPlayerIcon : MonoBehaviour
{
	[SerializeField]
	private GameObject[] playerIcons;

	public void SetLuckPlayerIcon(int num)
	{
		if (this.playerIcons != null)
		{
			for (int i = 0; i < this.playerIcons.Length; i++)
			{
				NGUITools.SetActiveSelf(this.playerIcons[i].gameObject, i == num);
			}
		}
	}
}
