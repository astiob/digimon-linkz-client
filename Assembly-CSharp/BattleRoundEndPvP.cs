﻿using Master;
using System;
using UnityEngine;

public class BattleRoundEndPvP : MonoBehaviour
{
	[SerializeField]
	[Header("あと○ラウンド")]
	private UILabel remainingTime;

	[SerializeField]
	[Header("rootのオブジェクトラウンド")]
	private GameObject rootObject;

	public void ShowLeftRoundUI(int leftRound)
	{
		NGUITools.SetActiveSelf(this.rootObject, true);
		string @string = StringMaster.GetString("BattleUI-41");
		this.remainingTime.text = string.Format(@string, leftRound);
	}

	public void HideLeftRoundUI()
	{
		NGUITools.SetActiveSelf(this.rootObject, false);
	}
}
