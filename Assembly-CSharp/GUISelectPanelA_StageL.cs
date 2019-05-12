using Quest;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUISelectPanelA_StageL : GUISelectPanelBSPartsUD
{
	public bool animationMoving;

	[Header("バナーのセルのスケール")]
	[SerializeField]
	private Vector3 bannerScale;

	private int animIndexBK = -1;

	public void SetCellAnim(int selectedIndex)
	{
		if (selectedIndex == this.animIndexBK)
		{
			return;
		}
		this.animationMoving = true;
		GUICollider.DisableAllCollider("GUISelectPanelA_StageL::SetCellAnim");
		GameObject gameObject = this.partObjs[selectedIndex].gameObject;
		if (gameObject.activeSelf)
		{
			iTween.MoveTo(gameObject, iTween.Hash(new object[]
			{
				"x",
				25,
				"time",
				0.4,
				"islocal",
				true
			}));
		}
		else
		{
			gameObject.transform.localPosition = new Vector3(25f, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
		}
		GUIListPartsA_StageL component = gameObject.GetComponent<GUIListPartsA_StageL>();
		GUIListPartsA_StageL_Ticket component2 = gameObject.GetComponent<GUIListPartsA_StageL_Ticket>();
		if (component != null)
		{
			component.SetBGColor(true);
		}
		else if (component2 != null)
		{
			component2.SetBGColor(true);
		}
		else
		{
			GUIListPartsA_StageL_Banner component3 = gameObject.GetComponent<GUIListPartsA_StageL_Banner>();
			if (component3 != null)
			{
				component3.SetBGColor(true);
			}
		}
		if (this.animIndexBK != -1)
		{
			GameObject gameObject2 = this.partObjs[this.animIndexBK].gameObject;
			GUIListPartsA_StageL component4 = gameObject2.GetComponent<GUIListPartsA_StageL>();
			GUIListPartsA_StageL_Ticket component5 = gameObject2.GetComponent<GUIListPartsA_StageL_Ticket>();
			if (component4 != null)
			{
				component4.SetBGColor(false);
			}
			else if (component5 != null)
			{
				component5.SetBGColor(false);
			}
			else
			{
				GUIListPartsA_StageL_Banner component6 = gameObject2.GetComponent<GUIListPartsA_StageL_Banner>();
				if (component6 != null)
				{
					component6.SetBGColor(false);
				}
			}
			if (gameObject2.activeSelf)
			{
				iTween.MoveTo(gameObject2, iTween.Hash(new object[]
				{
					"x",
					0,
					"time",
					0.4,
					"islocal",
					true,
					"oncomplete",
					"complete",
					"oncompletetarget",
					base.gameObject
				}));
			}
			else
			{
				gameObject2.transform.localPosition = new Vector3(0f, gameObject2.transform.localPosition.y, gameObject2.transform.localPosition.z);
				base.Invoke("complete", 0.1f);
			}
		}
		else
		{
			base.Invoke("complete", 0.1f);
		}
		this.animIndexBK = selectedIndex;
	}

	private void complete()
	{
		base.Invoke("completeMoving", 0.1f);
	}

	private void completeMoving()
	{
		this.animationMoving = false;
		GUICollider.EnableAllCollider("GUISelectPanelA_StageL::SetCellAnim_completeMoving");
	}

	public int AllBuild(List<QuestData.WorldStageData> dts, bool fromResult, GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM nextDungeon)
	{
		this.animIndexBK = -1;
		base.InitBuild();
		this.partsCount = dts.Count;
		int viewIdx = 0;
		if (base.selectCollider != null)
		{
			GUISelectPanelBSPartsUD.PanelBuildData panelBuildData = base.CalcBuildData(1, this.partsCount, 1f, 1f);
			float num = panelBuildData.startY;
			int num2 = 0;
			dts.Reverse();
			if (!fromResult)
			{
				viewIdx = 0;
			}
			else
			{
				viewIdx = dts.Count - int.Parse(nextDungeon.worldStageId);
			}
			if (viewIdx < 0)
			{
				global::Debug.LogError("本来来ない筈だが,エラーを発見できる様に…");
			}
			foreach (QuestData.WorldStageData worldStageData in dts)
			{
				GameObject gameObject = base.AddBuildPart();
				GUIListPartsA_StageL component = gameObject.GetComponent<GUIListPartsA_StageL>();
				if (component != null)
				{
					component.SetOriginalPos(new Vector3(0f, num, -5f));
					component.Data = worldStageData;
					component.selectPanelA = this;
					component.AvoidDisableAllCollider = true;
					CampaignLabelQuest component2 = gameObject.GetComponent<CampaignLabelQuest>();
					component2.AreaId = worldStageData.worldStageM.worldStageId;
					component.SetProgress();
					if (num2 == viewIdx)
					{
						component.SetBGColor(true);
						component.SetFadeInEndCallBack(delegate
						{
							this.SetCellAnim(viewIdx);
						});
					}
				}
				num -= panelBuildData.pitchH;
				num2++;
			}
			base.height = panelBuildData.lenH;
			base.InitMinMaxLocation(viewIdx, 0f);
			if (!this.partObjs[viewIdx].IsFadeDo())
			{
				this.partObjs[viewIdx].gameObject.SetActive(false);
				this.SetCellAnim(viewIdx);
			}
		}
		return viewIdx;
	}

	public int AllBuild_Ticket(List<QuestData.WorldStageData> dts, bool fromResult, GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM nextDungeon)
	{
		this.animIndexBK = -1;
		base.InitBuild();
		this.partsCount = dts.Count;
		int viewIdx = 0;
		if (base.selectCollider != null)
		{
			GUISelectPanelBSPartsUD.PanelBuildData panelBuildData = base.CalcBuildData(1, this.partsCount, 1f, 1f);
			float num = panelBuildData.startY;
			int num2 = 0;
			if (!fromResult)
			{
				viewIdx = 0;
			}
			else
			{
				viewIdx = dts.Count - int.Parse(nextDungeon.worldStageId);
			}
			if (viewIdx < 0)
			{
				global::Debug.LogError("本来来ない筈だが,エラーを発見できる様に…");
			}
			foreach (QuestData.WorldStageData worldStageData in dts)
			{
				GameObject gameObject = base.AddBuildPart();
				GUIListPartsA_StageL_Ticket component = gameObject.GetComponent<GUIListPartsA_StageL_Ticket>();
				if (component != null)
				{
					component.SetOriginalPos(new Vector3(0f, num, -5f));
					component.Data = worldStageData;
					component.selectPanelA = this;
					component.AvoidDisableAllCollider = true;
					CampaignLabelQuest component2 = gameObject.GetComponent<CampaignLabelQuest>();
					component2.AreaId = worldStageData.worldStageM.worldStageId;
					component.SetProgress();
					if (num2 == viewIdx)
					{
						component.SetBGColor(true);
						component.SetFadeInEndCallBack(delegate
						{
							this.SetCellAnim(viewIdx);
						});
					}
				}
				num -= panelBuildData.pitchH;
				num2++;
			}
			base.height = panelBuildData.lenH;
			base.InitMinMaxLocation(viewIdx, 0f);
			if (!this.partObjs[viewIdx].IsFadeDo())
			{
				this.partObjs[viewIdx].gameObject.SetActive(false);
				this.SetCellAnim(viewIdx);
			}
		}
		return viewIdx;
	}

	public int AllBuildBanner(List<QuestData.WorldStageData> dts, bool fromResult, GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM nextDungeon)
	{
		this.animIndexBK = -1;
		RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_ON);
		base.InitBuild();
		this.partsCount = dts.Count;
		int viewIdx = 0;
		if (!fromResult)
		{
			viewIdx = 0;
		}
		else
		{
			viewIdx = dts.Count - int.Parse(nextDungeon.worldStageId);
		}
		int i;
		for (i = 0; i < dts.Count; i++)
		{
			if (dts[i].wdi.isOpen == 1)
			{
				break;
			}
		}
		if (i == dts.Count)
		{
			viewIdx = 0;
		}
		else
		{
			while (dts[viewIdx].wdi.isOpen != 1)
			{
				viewIdx++;
				if (viewIdx >= dts.Count)
				{
					viewIdx = 0;
				}
			}
		}
		if (base.selectCollider != null)
		{
			GUISelectPanelBSPartsUD.PanelBuildData panelBuildData = base.CalcBuildData(1, this.partsCount, 1f, 1f);
			float num = panelBuildData.startY;
			int num2 = 0;
			int num3 = 0;
			foreach (QuestData.WorldStageData worldStageData in dts)
			{
				GameObject gameObject = base.AddBuildPart();
				gameObject.transform.localScale = Vector3.zero;
				GUIListPartsA_StageL_Banner component = gameObject.GetComponent<GUIListPartsA_StageL_Banner>();
				component.SetOriginalPos(new Vector3(0f, num, -5f));
				if (component != null)
				{
					component.WorldStageData = worldStageData;
					component.selectPanelA = this;
					CampaignLabelQuest component2 = component.GetComponent<CampaignLabelQuest>();
					component2.AreaId = worldStageData.worldStageM.worldStageId;
					if (num2 == viewIdx)
					{
						component.SetBGColor(true);
						component.SetFadeInEndCallBack(delegate
						{
							this.SetCellAnim(viewIdx);
						});
					}
					this.SetBanner(component);
					num3++;
				}
				num -= panelBuildData.pitchH;
				num2++;
			}
			base.height = panelBuildData.lenH;
			base.InitMinMaxLocation(viewIdx, 0f);
			if (!this.partObjs[viewIdx].IsFadeDo())
			{
				this.partObjs[viewIdx].gameObject.SetActive(false);
				this.SetCellAnim(viewIdx);
			}
			foreach (GUIListPartBS guilistPartBS in this.partObjs)
			{
				guilistPartBS.transform.localScale = this.bannerScale;
			}
		}
		RestrictionInput.EndLoad();
		return viewIdx;
	}

	private void SetBanner(GUIListPartsA_StageL_Banner bannerParts)
	{
		Action<Texture2D> callback = delegate(Texture2D texture)
		{
			if (bannerParts != null)
			{
				this.SetTexture(bannerParts, texture);
				if (texture != null)
				{
					bannerParts.SetBannerErrorText(string.Empty, false);
				}
			}
		};
		bannerParts.SetBannerErrorText(bannerParts.WorldStageData.worldStageM.name, true);
		base.StartCoroutine(this.DownloadBannerTexture(bannerParts.WorldStageData, callback));
	}

	private void SetTexture(GUIListPartsA_StageL_Banner listParts, Texture eventTexture)
	{
		if (null != listParts)
		{
			UITexture bannerTex = listParts.bannerTex;
			if (null != eventTexture)
			{
				bannerTex.mainTexture = eventTexture;
			}
		}
	}

	private IEnumerator DownloadBannerTexture(QuestData.WorldStageData wsd, Action<Texture2D> callback)
	{
		bool isUnlock = ClassSingleton<QuestData>.Instance.IsUnlockWorldStage(wsd);
		if (wsd.worldStageM.worldAreaId == ConstValue.QUEST_AREA_ID_DEFAULT || wsd.worldStageM.worldAreaId == "4" || wsd.worldStageM.worldAreaId == "5" || !isUnlock || "null" == wsd.worldStageM.stageImage || string.IsNullOrEmpty(wsd.worldStageM.stageImage))
		{
			callback(null);
			yield break;
		}
		string path = ConstValue.APP_ASSET_DOMAIN + "/asset/img/events/" + wsd.worldStageM.stageImage;
		yield return TextureManager.instance.Load(path, callback, 30f, true);
		yield break;
	}
}
