using Evolution;
using Master;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CMD_AlMightySelect : CMD
{
	[SerializeField]
	private GUISelectPanelViewPartsUD csSelectPanelViewPartsUD;

	private List<HaveSoulData> almSelectList;

	public int NeedNum { get; set; }

	public string CurSelectedSoulId { get; set; }

	public VersionUpItem SelectedVersionUpItem { get; set; }

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		base.PartsTitle.SetTitle(StringMaster.GetString("AlMightySelectTitle"));
		base.Show(f, sizeX, sizeY, aT);
	}

	protected override void Awake()
	{
		base.Awake();
		base.SetForceReturnValue(0);
	}

	public HaveSoulData GetSoulDataByIDX(int idx)
	{
		return this.almSelectList[idx];
	}

	public void MakeList(List<HaveSoulData> dtList, int needNum, string curSelectedSoulId)
	{
		this.NeedNum = needNum;
		this.CurSelectedSoulId = curSelectedSoulId;
		this.almSelectList = dtList;
		this.csSelectPanelViewPartsUD.AllBuild(this.almSelectList.Count, true, 1f, 1f, null, this);
	}

	public void SetSelected(string soulId)
	{
		List<GUISelectPanelViewControlUD.ListPartsData> partObjs = this.csSelectPanelViewPartsUD.partObjs;
		for (int i = 0; i < partObjs.Count; i++)
		{
			if (partObjs[i].csParts != null)
			{
				GUIListPartsAlMightySelect guilistPartsAlMightySelect = (GUIListPartsAlMightySelect)partObjs[i].csParts;
				if (guilistPartsAlMightySelect.Data.soulM.soulId == soulId)
				{
					guilistPartsAlMightySelect.spSelectIcon.gameObject.SetActive(true);
				}
				else
				{
					guilistPartsAlMightySelect.spSelectIcon.gameObject.SetActive(false);
				}
			}
		}
		this.CurSelectedSoulId = soulId;
	}

	private void OnTouchDecide()
	{
		base.SetForceReturnValue(1);
		base.ClosePanel(true);
	}

	public enum CLOSE_TYPE
	{
		CLOSE,
		DECIDE
	}
}
