using System;
using System.Collections.Generic;
using UI.Gasha;
using UnityEngine;

public sealed class GUISelectPanelGashaMain : GUISelectPanelBSPartsUD
{
	public bool animationMoving;

	[SerializeField]
	[Header("バナーのセルのスケール")]
	private Vector3 bannerScale;

	[Header("選択されたパーツのアニメ量 X")]
	[SerializeField]
	private float selectPartsAnimX = 25f;

	private int animIndexBK = -1;

	private Action<int> actionPushedButton;

	public void Create(GameObject selectPartsResouce)
	{
		base.selectParts = selectPartsResouce;
		Vector3 localPosition = base.transform.localPosition;
		base.ListWindowViewRect = new Rect
		{
			xMin = -280f + localPosition.x,
			xMax = 280f + localPosition.x,
			yMin = -240f - GUIMain.VerticalSpaceSize,
			yMax = 250f + GUIMain.VerticalSpaceSize
		};
		base.initLocation = true;
	}

	public bool SetCellAnim(int selectedIndex)
	{
		if (selectedIndex == this.animIndexBK)
		{
			return false;
		}
		GUIListPartsGashaMain guilistPartsGashaMain = (GUIListPartsGashaMain)this.partObjs[selectedIndex];
		if (null != guilistPartsGashaMain)
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
		if (null != component)
		{
			component.SetBGColor(true);
		}
		if (this.animIndexBK != -1)
		{
			GameObject gameObject2 = this.partObjs[this.animIndexBK].gameObject;
			GUIListPartsGashaMain component2 = gameObject2.GetComponent<GUIListPartsGashaMain>();
			if (null != component2)
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

	private void OnPushedGashaButton(int buttonIndex)
	{
		if (this.SetCellAnim(buttonIndex) && this.actionPushedButton != null)
		{
			this.actionPushedButton(buttonIndex);
		}
	}

	public void AllBuild(List<GameWebAPI.RespDataGA_GetGachaInfo.Result> gashaInfoList, Texture[] textureList, Action<int> pushedAction, int selectedButtonIndex, bool isTutorial)
	{
		this.animIndexBK = -1;
		base.InitBuild();
		this.partsCount = gashaInfoList.Count;
		if (null != base.selectCollider)
		{
			this.actionPushedButton = pushedAction;
			GUISelectPanelBSPartsUD.PanelBuildData panelBuildData = base.CalcBuildData(1, this.partsCount, 1f, 1f);
			float num = panelBuildData.startY;
			for (int i = 0; i < gashaInfoList.Count; i++)
			{
				GameWebAPI.RespDataGA_GetGachaInfo.Result result = gashaInfoList[i];
				GameObject gameObject = base.AddBuildPart();
				GUIListPartsGashaMain component = gameObject.GetComponent<GUIListPartsGashaMain>();
				if (null != component)
				{
					component.SetOriginalPos(new Vector3(0f, num, -5f));
					component.GashaInfo = result;
					component.ShowGUI(textureList[i]);
					component.selectPanelA = this;
					component.AvoidDisableAllCollider = true;
					component.SetPushedAction(new Action<int>(this.OnPushedGashaButton));
					if (isTutorial && result.priceType.GetCostAssetsCategory() == MasterDataMng.AssetCategory.LINK_POINT)
					{
						component.gameObject.AddComponent<TutorialEmphasizeUI>();
						TutorialEmphasizeUI component2 = component.gameObject.GetComponent<TutorialEmphasizeUI>();
						component2.UiName = TutorialEmphasizeUI.UiNameType.TAB2_RIGHT;
					}
					if (i == selectedButtonIndex)
					{
						component.SetBGColor(true);
						component.SetFadeInEndCallBack(delegate
						{
							this.SetCellAnim(selectedButtonIndex);
						});
					}
				}
				num -= panelBuildData.pitchH;
			}
			base.height = panelBuildData.lenH;
			base.InitMinMaxLocation(selectedButtonIndex, 0f);
			if (!this.partObjs[selectedButtonIndex].IsFadeDo())
			{
				this.partObjs[selectedButtonIndex].gameObject.SetActive(false);
				this.SetCellAnim(selectedButtonIndex);
			}
		}
	}

	public void RefreshAbleCount()
	{
		for (int i = 0; i < this.partObjs.Count; i++)
		{
			GUIListPartsGashaMain guilistPartsGashaMain = this.partObjs[i] as GUIListPartsGashaMain;
			if (null != guilistPartsGashaMain)
			{
				guilistPartsGashaMain.ShowAbleCount();
			}
		}
	}
}
