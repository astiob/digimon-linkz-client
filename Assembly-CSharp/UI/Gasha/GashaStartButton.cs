using System;
using UI.Common;
using UnityEngine;

namespace UI.Gasha
{
	public sealed class GashaStartButton : MonoBehaviour
	{
		[SerializeField]
		private GameObject popObject;

		[SerializeField]
		private UILabel popLabel;

		private UIAssetsIcon assetsIcon;

		private UIAssetsNumber assetsNumber;

		private UISprite background;

		private string activeButtonSpriteName;

		private GUICollider guiCollider;

		private string playCount;

		private string playPrice;

		private Action<int, int> actionPushed;

		private void OnPushedStartButton()
		{
			if (this.actionPushed != null)
			{
				int num = 0;
				int.TryParse(this.playCount, out num);
				global::Debug.Assert(0 < num, "ガシャ実行回数が想定外の数値.");
				int arg = 0;
				if (!int.TryParse(this.playPrice, out arg))
				{
					global::Debug.LogError("ガシャ実行価格が想定外の数値");
				}
				this.actionPushed(arg, num);
			}
		}

		private void SetPopText(string remainingPlayCountText, string campaignText)
		{
			bool flag = !string.IsNullOrEmpty(remainingPlayCountText);
			bool flag2 = !string.IsNullOrEmpty(campaignText);
			string text = string.Empty;
			if (flag && flag2)
			{
				text = string.Format("{0}\n{1}", remainingPlayCountText, campaignText);
			}
			else if (flag)
			{
				text = remainingPlayCountText;
			}
			else if (flag2)
			{
				text = campaignText;
			}
			if (string.IsNullOrEmpty(text))
			{
				if (this.popObject.activeSelf)
				{
					this.popObject.SetActive(false);
				}
			}
			else
			{
				if (!this.popObject.activeSelf)
				{
					this.popObject.SetActive(true);
				}
				this.popLabel.text = text;
			}
		}

		public void Initialize(Action<int, int> action)
		{
			this.assetsIcon = base.GetComponent<UIAssetsIcon>();
			this.assetsNumber = base.GetComponent<UIAssetsNumber>();
			this.background = base.GetComponent<UISprite>();
			this.guiCollider = base.GetComponent<GUICollider>();
			global::Debug.Assert(null != this.assetsIcon, "実行ボタンのアセットアイコンがMissing");
			global::Debug.Assert(null != this.assetsNumber, "実行ボタンのコストラベルがMissing");
			global::Debug.Assert(null != this.background, "実行ボタンの背景がMissing");
			global::Debug.Assert(null != this.guiCollider, "実行ボタンのGUIがMissing");
			this.activeButtonSpriteName = this.background.spriteName;
			this.actionPushed = action;
		}

		public void SetAssets(MasterDataMng.AssetCategory category, string assetsValue, string price)
		{
			this.assetsIcon.SetAssetsCategory(category, assetsValue);
			this.assetsIcon.SetIcon();
			this.assetsNumber.SetNumber(price);
			this.playPrice = price;
		}

		public void SetPopText(GameWebAPI.RespDataGA_GetGachaInfo.Result gashaInfo, GameWebAPI.RespDataGA_GetGachaInfo.Detail gashaDetail, bool isTutorial)
		{
			this.playCount = gashaDetail.count;
			if (!isTutorial)
			{
				string remainingPlayCountText = string.Empty;
				if (gashaInfo.ExistLimitPlayCount())
				{
					int num = 0;
					if (int.TryParse(gashaDetail.count, out num))
					{
						remainingPlayCountText = gashaInfo.GetRemainingPlayCountText(num);
					}
				}
				this.SetPopText(remainingPlayCountText, gashaDetail.GetCampaignText());
			}
			else
			{
				this.popObject.SetActive(false);
				this.popLabel.text = string.Empty;
			}
		}

		public void SetButtonAppearance(MasterDataMng.AssetCategory category, string assetsValue, int number, string gashaPlayPrice)
		{
			int num = 0;
			int.TryParse(gashaPlayPrice, out num);
			if (num <= number || category == MasterDataMng.AssetCategory.DIGI_STONE)
			{
				this.background.spriteName = this.activeButtonSpriteName;
				this.guiCollider.activeCollider = true;
			}
			else
			{
				this.DisableButton();
			}
		}

		public void DisableButton()
		{
			this.background.spriteName = "Common02_Btn_Gray";
			this.guiCollider.activeCollider = false;
		}
	}
}
