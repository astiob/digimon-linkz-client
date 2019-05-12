using System;
using System.Collections.Generic;
using UnityEngine;

public class GUISelectPanelGashaMain : GUISelectPanelBSPartsUD
{
	public bool animationMoving;

	[SerializeField]
	[Header("バナーのセルのスケール")]
	private Vector3 bannerScale;

	[SerializeField]
	[Header("選択されたパーツのアニメ量 X")]
	private float selectPartsAnimX = 25f;

	private int animIndexBK = -1;

	public bool SetCellAnim(int selectedIndex)
	{
		if (selectedIndex == this.animIndexBK)
		{
			return false;
		}
		GUIListPartsGashaMain guilistPartsGashaMain = (GUIListPartsGashaMain)this.partObjs[selectedIndex];
		if (guilistPartsGashaMain != null)
		{
			guilistPartsGashaMain.ResetNew();
		}
		this.animationMoving = true;
		GUICollider.DisableAllCollider("GUISelectPanelGasha::SetCellAnim");
		GameObject gameObject = this.partObjs[selectedIndex].gameObject;
		if (gameObject.activeSelf)
		{
			iTween.MoveTo(gameObject, iTween.Hash(new object[]
			{
				"x",
				this.selectPartsAnimX,
				"time",
				0.4,
				"islocal",
				true
			}));
		}
		else
		{
			gameObject.transform.localPosition = new Vector3(this.selectPartsAnimX, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
		}
		GUIListPartsGashaMain component = gameObject.GetComponent<GUIListPartsGashaMain>();
		if (component != null)
		{
			component.SetBGColor(true);
		}
		if (this.animIndexBK != -1)
		{
			GameObject gameObject2 = this.partObjs[this.animIndexBK].gameObject;
			GUIListPartsGashaMain component2 = gameObject2.GetComponent<GUIListPartsGashaMain>();
			if (component2 != null)
			{
				component2.SetBGColor(false);
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
		return true;
	}

	private void complete()
	{
		base.Invoke("completeMoving", 0.1f);
	}

	private void completeMoving()
	{
		this.animationMoving = false;
		GUICollider.EnableAllCollider("GUISelectPanelGasha::SetCellAnim_completeMoving");
	}

	public void AllBuild(List<GameWebAPI.RespDataGA_GetGachaInfo.Result> dts)
	{
		this.animIndexBK = -1;
		base.InitBuild();
		this.partsCount = dts.Count;
		if (base.selectCollider != null)
		{
			GUISelectPanelBSPartsUD.PanelBuildData panelBuildData = base.CalcBuildData(1, this.partsCount, 1f, 1f);
			float num = panelBuildData.startY;
			int num2 = 0;
			int viewIdx = 0;
			foreach (GameWebAPI.RespDataGA_GetGachaInfo.Result result in dts)
			{
				GameObject gameObject = base.AddBuildPart();
				GUIListPartsGashaMain component = gameObject.GetComponent<GUIListPartsGashaMain>();
				if (component != null)
				{
					component.SetOriginalPos(new Vector3(0f, num, -5f));
					component.Data = result;
					component.selectPanelA = this;
					component.AvoidDisableAllCollider = true;
					if (result.IsLink())
					{
						component.gameObject.AddComponent<TutorialEmphasizeUI>();
						TutorialEmphasizeUI component2 = component.gameObject.GetComponent<TutorialEmphasizeUI>();
						component2.UiName = TutorialEmphasizeUI.UiNameType.TAB2_RIGHT;
					}
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
	}

	public void RefreshAbleCount()
	{
		for (int i = 0; i < this.partObjs.Count; i++)
		{
			GUIListPartsGashaMain guilistPartsGashaMain = (GUIListPartsGashaMain)this.partObjs[i];
			guilistPartsGashaMain.ShowAbleCount();
		}
	}
}
