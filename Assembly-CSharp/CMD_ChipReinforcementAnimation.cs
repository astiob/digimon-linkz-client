using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CMD_ChipReinforcementAnimation : CMD
{
	[SerializeField]
	private UITexture[] icons;

	[SerializeField]
	private Animation animation;

	[SerializeField]
	private UILabel label;

	private List<ChipIcon> chipIconList = new List<ChipIcon>();

	public static CMD_ChipReinforcementAnimation Create(GameObject parent, GameWebAPI.RespDataMA_ChipM.Chip data, Action<int> callback = null)
	{
		CMD_ChipReinforcementAnimation cmd_ChipReinforcementAnimation = GUIMain.ShowCommonDialog(callback, "CMD_Chip_LvUP") as CMD_ChipReinforcementAnimation;
		cmd_ChipReinforcementAnimation.SetParam(data);
		return cmd_ChipReinforcementAnimation;
	}

	public void SetParam(GameWebAPI.RespDataMA_ChipM.Chip data)
	{
		base.StartCoroutine(this.SetParamProcess(data));
	}

	private IEnumerator SetParamProcess(GameWebAPI.RespDataMA_ChipM.Chip data)
	{
		IEnumerator loadWait = this.Load(data);
		while (loadWait.MoveNext())
		{
			yield return null;
		}
		this.label.text = data.name;
		IEnumerator animationWait = this.Animation();
		while (animationWait.MoveNext())
		{
			yield return null;
		}
		yield break;
	}

	private IEnumerator Load(GameWebAPI.RespDataMA_ChipM.Chip data)
	{
		foreach (UITexture icon in this.icons)
		{
			ChipTools.CreateChipIcon(data, icon, delegate(ChipIcon chipIcon)
			{
				this.chipIconList.Add(chipIcon);
			});
		}
		float maxTime = 30f;
		float currentTime = 0f;
		while (this.chipIconList.Count < this.icons.Length)
		{
			currentTime += Time.deltaTime;
			if (currentTime >= maxTime)
			{
				yield break;
			}
			yield return null;
		}
		yield break;
	}

	private IEnumerator Animation()
	{
		SoundMng.Instance().TryPlaySE("SEInternal/Common/se_111", 0f, false, true, null, -1);
		while (this.animation.isPlaying)
		{
			for (int i = 0; i < this.icons.Length; i++)
			{
				UIWidget uiWidget = this.chipIconList[i].GetComponent<UIWidget>();
				uiWidget.alpha = this.icons[i].alpha;
			}
			yield return null;
		}
		while (!Input.GetMouseButton(0))
		{
			yield return null;
		}
		SoundMng.Instance().PlaySE("SEInternal/Common/se_105", 0f, false, true, null, -1, 1f);
		this.ClosePanel(false);
		yield break;
	}
}
