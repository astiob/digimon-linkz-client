﻿using Master;
using System;
using UnityEngine;

public class BattleMonsterButton : MonoBehaviour
{
	[SerializeField]
	public UIButton button;

	[SerializeField]
	[Header("アイコン画像")]
	public UI2DSprite monsterIcon;

	[SerializeField]
	public HoldPressButton playerMonsterDescriptionSwitch;

	[Header("リーダーアイコン")]
	[SerializeField]
	private UISprite leaderIcon;

	[Header("プレイヤー名ラベル")]
	[SerializeField]
	private UILabel playerNameLabel;

	[Header("プレイヤー名板")]
	[SerializeField]
	private UISprite playerNamePlate;

	[SerializeField]
	[Header("プレイヤー番号アイコン")]
	private UISprite playerNumberIcon;

	[SerializeField]
	[Header("覚醒度アイコン")]
	private UISprite arousalIcon;

	[Header("フレーム")]
	[SerializeField]
	private BattleMonsterButton.Frame frame;

	public void ApplyMonsterButtonIcon(Sprite image, CharacterStateControl characterStatus, bool isLeader)
	{
		this.monsterIcon.sprite2D = image;
		this.frame.SetEvolutionStep(characterStatus.evolutionStep);
		this.SetArousal(characterStatus.arousal);
		this.ApplyLeaderIcon(isLeader);
	}

	public void ApplyLeaderIcon(bool isLeader)
	{
		this.leaderIcon.gameObject.SetActive(isLeader);
	}

	public void SetPlayerNameActive(bool value)
	{
		this.playerNameLabel.gameObject.SetActive(value);
		this.playerNamePlate.gameObject.SetActive(value);
	}

	public void SetPlayerNameColor(Color color)
	{
		this.playerNameLabel.color = color;
	}

	public void SetPlayerName(string name)
	{
		this.playerNameLabel.text = name;
	}

	public void SetPlayerNumber(int number)
	{
		this.playerNumberIcon.spriteName = "MultiBattle_P" + number;
	}

	private void SetArousal(int arousal)
	{
		this.arousalIcon.spriteName = "Common02_Arousal_" + arousal;
	}

	public void SetType(BattleMonsterButton.Type Type)
	{
		this.frame.SetType(Type);
		this.leaderIcon.spriteName = ((Type != BattleMonsterButton.Type.Dead) ? "Battle_Leader" : "Battle_Leader_g");
		base.transform.localPosition = ((Type != BattleMonsterButton.Type.Select) ? Vector3.zero : new Vector3(0f, 10f, 0f));
		if (BattleStateManager.current.battleMode == BattleMode.PvP)
		{
			base.transform.localPosition += new Vector3(0f, 20f, 0f);
		}
	}

	public enum Type
	{
		None,
		Select,
		Dead
	}

	[Serializable]
	private class Frame
	{
		[SerializeField]
		private UISprite baseFrame;

		[SerializeField]
		private UISprite bgFrame;

		[SerializeField]
		private UISprite flashFrame;

		[SerializeField]
		private UISprite selectFrame;

		[SerializeField]
		private UI2DSprite materialFrame;

		[Header("マテリアル（通常、死亡時の順）")]
		[SerializeField]
		private Material[] materials;

		[SerializeField]
		[Header("アトラス（通常、死亡時の順）")]
		private UIAtlas[] uiAtlases;

		public void SetEvolutionStep(EvolutionStep evolutionStep)
		{
			if (GrowStep.GROWING.ConverBattleInt() == (int)evolutionStep)
			{
				this.baseFrame.spriteName = "Common02_Thumbnail_waku2";
				this.bgFrame.spriteName = "Common02_Thumbnail_bg2";
				this.flashFrame.spriteName = "Common02_Thumbnail_bg2";
			}
			else if (GrowStep.RIPE.ConverBattleInt() == (int)evolutionStep || GrowStep.ARMOR_1.ConverBattleInt() == (int)evolutionStep)
			{
				this.baseFrame.spriteName = "Common02_Thumbnail_waku3";
				this.bgFrame.spriteName = "Common02_Thumbnail_bg3";
				this.flashFrame.spriteName = "Common02_Thumbnail_bg3";
			}
			else if (GrowStep.PERFECT.ConverBattleInt() == (int)evolutionStep)
			{
				this.baseFrame.spriteName = "Common02_Thumbnail_waku4";
				this.bgFrame.spriteName = "Common02_Thumbnail_bg4";
				this.flashFrame.spriteName = "Common02_Thumbnail_bg4";
			}
			else if (GrowStep.ULTIMATE.ConverBattleInt() == (int)evolutionStep || GrowStep.ARMOR_2.ConverBattleInt() == (int)evolutionStep)
			{
				this.baseFrame.spriteName = "Common02_Thumbnail_waku5";
				this.bgFrame.spriteName = "Common02_Thumbnail_bg5";
				this.flashFrame.spriteName = "Common02_Thumbnail_bg5";
			}
		}

		public void SetType(BattleMonsterButton.Type type)
		{
			this.selectFrame.gameObject.SetActive(type == BattleMonsterButton.Type.Select);
			int num = (type != BattleMonsterButton.Type.Dead) ? 0 : 1;
			this.materialFrame.material = this.materials[num];
			this.baseFrame.atlas = this.uiAtlases[num];
			this.bgFrame.atlas = this.uiAtlases[num];
			this.flashFrame.atlas = this.uiAtlases[num];
		}
	}
}
