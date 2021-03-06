﻿using Monster;
using System;
using UnityEngine;

public class BattleMonsterButton : MonoBehaviour
{
	[SerializeField]
	public UIButton button;

	[Header("アイコン画像")]
	[SerializeField]
	public UITexture monsterIconTexture;

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

	[Header("プレイヤー番号アイコン")]
	[SerializeField]
	private UISprite playerNumberIcon;

	[Header("覚醒度アイコン")]
	[SerializeField]
	private UISprite arousalIcon;

	[Header("フレーム")]
	[SerializeField]
	private BattleMonsterButton.Frame frame;

	public void ApplyMonsterButtonIcon(CharacterStateControl characterStatus, bool isLeader, string resourcePath, string assetBundlePath)
	{
		GUIMonsterIcon.SetTextureMonsterParts(this.monsterIconTexture, resourcePath, assetBundlePath, true);
		this.frame.SetEvolutionStep(characterStatus.characterDatas.growStep);
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
		private UITexture charaTexture;

		[Header("マテリアル（通常、死亡時の順）")]
		[SerializeField]
		private Material[] materials;

		[Header("アトラス（通常、死亡時の順）")]
		[SerializeField]
		private UIAtlas[] uiAtlases;

		private BattleMonsterButton.Type iconType;

		public void SetEvolutionStep(GrowStep growStep)
		{
			switch (growStep)
			{
			case GrowStep.GROWING:
			case GrowStep.HYBRID_GROWING:
				this.baseFrame.spriteName = "Common02_Thumbnail_waku2";
				this.bgFrame.spriteName = "Common02_Thumbnail_bg2";
				this.flashFrame.spriteName = "Common02_Thumbnail_bg2";
				break;
			case GrowStep.RIPE:
			case GrowStep.ARMOR_1:
			case GrowStep.HYBRID_RIPE:
				this.baseFrame.spriteName = "Common02_Thumbnail_waku3";
				this.bgFrame.spriteName = "Common02_Thumbnail_bg3";
				this.flashFrame.spriteName = "Common02_Thumbnail_bg3";
				break;
			case GrowStep.PERFECT:
			case GrowStep.HYBRID_PERFECT:
				this.baseFrame.spriteName = "Common02_Thumbnail_waku4";
				this.bgFrame.spriteName = "Common02_Thumbnail_bg4";
				this.flashFrame.spriteName = "Common02_Thumbnail_bg4";
				break;
			case GrowStep.ULTIMATE:
			case GrowStep.ARMOR_2:
			case GrowStep.HYBRID_ULTIMATE:
				this.baseFrame.spriteName = "Common02_Thumbnail_waku5";
				this.bgFrame.spriteName = "Common02_Thumbnail_bg5";
				this.flashFrame.spriteName = "Common02_Thumbnail_bg5";
				break;
			}
		}

		public void SetType(BattleMonsterButton.Type type)
		{
			this.selectFrame.gameObject.SetActive(type == BattleMonsterButton.Type.Select);
			int num = (type != BattleMonsterButton.Type.Dead) ? 0 : 1;
			this.baseFrame.atlas = this.uiAtlases[num];
			this.bgFrame.atlas = this.uiAtlases[num];
			this.flashFrame.atlas = this.uiAtlases[num];
			if (this.charaTexture.material == null)
			{
				this.charaTexture.shader = this.materials[num].shader;
			}
			else if ((type == BattleMonsterButton.Type.Dead && this.iconType != BattleMonsterButton.Type.Dead) || (type != BattleMonsterButton.Type.Dead && this.iconType == BattleMonsterButton.Type.Dead))
			{
				this.iconType = type;
				Texture texture = this.charaTexture.material.GetTexture("_MaskTex");
				Texture texture2 = this.charaTexture.material.GetTexture("_MainTex");
				if (type == BattleMonsterButton.Type.Dead)
				{
					Shader iconShaderGray = GUIMonsterIcon.GetIconShaderGray();
					this.charaTexture.material = new Material(iconShaderGray);
					this.charaTexture.material.SetTexture("_MaskTex", texture);
					this.charaTexture.material.SetTexture("_MainTex", texture2);
				}
				else
				{
					Shader iconShader = GUIMonsterIcon.GetIconShader();
					this.charaTexture.material = new Material(iconShader);
					this.charaTexture.material.SetTexture("_MaskTex", texture);
					this.charaTexture.material.SetTexture("_MainTex", texture2);
				}
			}
		}
	}
}
