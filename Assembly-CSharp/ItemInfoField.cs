using BattleStateMachineInternal;
using Master;
using System;
using UnityEngine;

public class ItemInfoField : MonoBehaviour
{
	[Header("Box画像(ノーマル)")]
	[SerializeField]
	private GameObject normalBoxImage;

	[Header("Box画像(レア)")]
	[SerializeField]
	private GameObject rareBoxImage;

	[SerializeField]
	[Header("Box(ノーマル)")]
	private GameObject normalBox;

	[SerializeField]
	[Header("Box(レア)")]
	private GameObject rareBox;

	[SerializeField]
	[Header("Wave & Roundのテキスト")]
	private UILabel waveAndRoundText;

	[Header("残りRoundのテキスト")]
	[SerializeField]
	private UILabel remainingRoundText;

	[SerializeField]
	[Header("Box数テキスト(ノーマル)")]
	private UILabel normalBoxNumberText;

	[Header("Box数テキスト(レア)")]
	[SerializeField]
	private UILabel rareBoxNumberText;

	[Header("BoxのTween(ノーマル)")]
	[SerializeField]
	private UITweener normalBoxTween;

	[Header("BoxのTween(レア)")]
	[SerializeField]
	private UITweener rareBoxTween;

	public void TwinkleNormalBox()
	{
		this.normalBoxTween.ResetToBeginning();
		this.normalBoxTween.PlayForward();
	}

	public void TwinkleRareBox()
	{
		this.rareBoxTween.ResetToBeginning();
		this.rareBoxTween.PlayForward();
	}

	public void ApplyDroppedItemIconHide()
	{
		NGUITools.SetActiveSelf(this.normalBox, false);
		NGUITools.SetActiveSelf(this.rareBox, false);
	}

	public Vector3 GetBoxImagePosition(bool isRare)
	{
		if (isRare)
		{
			return this.rareBoxImage.transform.position;
		}
		return this.normalBoxImage.transform.position;
	}

	public void ApplyDroppedItemNumber(int normalItems, int rareItems)
	{
		this.normalBoxNumberText.text = normalItems.ToString();
		this.rareBoxNumberText.text = rareItems.ToString();
	}

	public void ApplyWaveAndRound(int waveValue, int roundValue, int maxWave)
	{
		this.waveAndRoundText.text = string.Format(StringMaster.GetString("BattleUI-02"), waveValue, maxWave, roundValue);
	}

	public void ApplyWaveAndRound(BattleWave battleWave, int roundValue, int maxWave)
	{
		string key = "BattleUI-02";
		if (battleWave.floorType == 3)
		{
			key = "BattleUI-45";
		}
		this.waveAndRoundText.text = string.Format(StringMaster.GetString(key), battleWave.floorNum, maxWave, roundValue);
	}

	public void SetRemainingRoundText(int value)
	{
		if (this.remainingRoundText != null)
		{
			if (value > 0)
			{
				this.remainingRoundText.text = string.Format(StringMaster.GetString("BattleUI-51"), value);
			}
			else
			{
				this.remainingRoundText.text = string.Empty;
			}
		}
	}
}
