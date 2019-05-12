using System;
using UnityEngine;

public class GUISelectPresentBoxPanel : GUISelectPanelBSPartsUD
{
	private float panelUpdateTime;

	private bool isLock;

	private bool isDispAll;

	private float beforeMaxLocate;

	protected override void Awake()
	{
		base.Awake();
	}

	protected override void Update()
	{
		base.Update();
		if (CMD_ModalPresentBox.Instance != null)
		{
			this.panelUpdateTime += Time.deltaTime;
			if (CMD_ModalPresentBox.Instance.displayBoxType == CMD_ModalPresentBox.DISPLAY_BOX_TYPE.PRESENT && base.gameObject.transform.localPosition.y > base.maxLocate + 150f && this.panelUpdateTime > 1f && !this.isDispAll && !this.isLock)
			{
				this.isLock = true;
				CMD_ModalPresentBox.Instance.nowPage++;
				CMD_ModalPresentBox.Instance.GetPresentBoxData(CMD_ModalPresentBox.Instance.nowPage, false, delegate
				{
					CMD_ModalPresentBox.Instance.DispPresentBox(true);
					this.isLock = false;
				});
				this.panelUpdateTime = 0f;
			}
		}
	}

	public void AllBuild(GameWebAPI.RespDataPR_PrizeData data)
	{
		base.InitBuild();
		this.isDispAll = false;
		this.partsCount = data.prizeData.Length;
		GUISelectPanelBSPartsUD.PanelBuildData panelBuildData = base.CalcBuildData(1, this.partsCount, 1f, 1f);
		float startX = panelBuildData.startX;
		float num = panelBuildData.startY;
		if (data != null)
		{
			foreach (GameWebAPI.RespDataPR_PrizeData.PrizeData data2 in data.prizeData)
			{
				GameObject gameObject = base.AddBuildPart();
				GUIListPresentBoxParts component = gameObject.GetComponent<GUIListPresentBoxParts>();
				if (component != null)
				{
					component.SetOriginalPos(new Vector3(startX, num, -5f));
					component.Data = data2;
				}
				num -= panelBuildData.pitchH;
			}
		}
		base.height = panelBuildData.lenH;
		base.InitMinMaxLocation(-1, 0f);
		if (this.beforeMaxLocate > 0f && CMD_ModalPresentBox.Instance.nowPage > 1)
		{
			this.selectLoc = base.minLocate + this.beforeMaxLocate + panelBuildData.pitchH;
		}
		if (this.partsCount >= int.Parse(data.prizeTotalCount))
		{
			this.isDispAll = true;
		}
		this.beforeMaxLocate = base.maxLocate * 2f;
	}

	public void AllBuildHistory(GameWebAPI.RespDataPR_PrizeReceiveHistory data)
	{
		base.InitBuild();
		this.partsCount = data.prizeReceiveHistory.Length;
		GUISelectPanelBSPartsUD.PanelBuildData panelBuildData = base.CalcBuildData(1, this.partsCount, 1f, 1f);
		float startX = panelBuildData.startX;
		float num = panelBuildData.startY;
		if (data != null)
		{
			foreach (GameWebAPI.RespDataPR_PrizeReceiveHistory.PrizeReceiveHistory data2 in data.prizeReceiveHistory)
			{
				GameObject gameObject = base.AddBuildPart();
				GUIListPresentHistoryParts component = gameObject.GetComponent<GUIListPresentHistoryParts>();
				if (component != null)
				{
					component.SetOriginalPos(new Vector3(startX, num, -5f));
					component.Data = data2;
				}
				num -= panelBuildData.pitchH;
			}
		}
		base.height = panelBuildData.lenH;
		base.InitMinMaxLocation(-1, 0f);
	}
}
