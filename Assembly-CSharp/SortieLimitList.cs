using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class SortieLimitList : MonoBehaviour
{
	[SerializeField]
	private SortieLimitListItem[] tribeList;

	[SerializeField]
	private UITable growStepRoot;

	[SerializeField]
	private GameObject srcGrowStepListItem;

	private List<GameWebAPI.RespDataMA_WorldDungeonSortieLimit.WorldDungeonSortieLimit> sortieLimitList;

	private SortieLimitListItem GetGrowStepListItem()
	{
		SortieLimitListItem sortieLimitListItem = null;
		Transform transform = this.growStepRoot.transform;
		for (int i = 0; i < transform.childCount; i++)
		{
			SortieLimitListItem component = transform.GetChild(i).GetComponent<SortieLimitListItem>();
			if (!component.gameObject.activeSelf)
			{
				sortieLimitListItem = component;
				break;
			}
		}
		if (null == sortieLimitListItem)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.srcGrowStepListItem);
			sortieLimitListItem = gameObject.GetComponent<SortieLimitListItem>();
			sortieLimitListItem.name = "GrowStep";
			Transform transform2 = sortieLimitListItem.transform;
			transform2.parent = transform;
			transform2.localScale = Vector3.one;
			transform2.localRotation = Quaternion.identity;
		}
		return sortieLimitListItem;
	}

	private void SetTribeList(List<GameWebAPI.RespDataMA_WorldDungeonSortieLimit.WorldDungeonSortieLimit> limitList)
	{
		List<string> list = new List<string>();
		int num = 0;
		for (int i = 0; i < limitList.Count; i++)
		{
			if (num < this.tribeList.Length)
			{
				string tribe = CommonSentenceData.GetTribe(limitList[i].tribe);
				if (!list.Contains(tribe))
				{
					list.Add(tribe);
					this.tribeList[num].gameObject.SetActive(true);
					this.tribeList[num].SetText(tribe);
					num++;
				}
			}
		}
	}

	private void SetGrowStepList(List<GameWebAPI.RespDataMA_WorldDungeonSortieLimit.WorldDungeonSortieLimit> limitList)
	{
		List<string> list = new List<string>();
		int num = 0;
		for (int i = 0; i < limitList.Count; i++)
		{
			string grade = CommonSentenceData.GetGrade(limitList[i].growStep);
			if (!list.Contains(grade))
			{
				SortieLimitListItem growStepListItem = this.GetGrowStepListItem();
				if (null != growStepListItem)
				{
					list.Add(grade);
					growStepListItem.gameObject.SetActive(true);
					growStepListItem.SetText(grade);
					num++;
				}
			}
		}
		this.growStepRoot.columns = num;
		this.growStepRoot.Reposition();
	}

	public void Initialize()
	{
		this.sortieLimitList = new List<GameWebAPI.RespDataMA_WorldDungeonSortieLimit.WorldDungeonSortieLimit>();
	}

	public void SetSortieLimit(int worldDungeonId)
	{
		GameWebAPI.RespDataMA_WorldDungeonSortieLimit.WorldDungeonSortieLimit[] worldDungeonSortieLimitM = MasterDataMng.Instance().WorldDungeonSortieLimitMaster.worldDungeonSortieLimitM;
		string b = worldDungeonId.ToString();
		for (int i = 0; i < worldDungeonSortieLimitM.Length; i++)
		{
			if (worldDungeonSortieLimitM[i].worldDungeonId == b)
			{
				this.sortieLimitList.Add(worldDungeonSortieLimitM[i]);
			}
		}
		if (0 < this.sortieLimitList.Count)
		{
			base.gameObject.SetActive(true);
			this.SetTribeList(this.sortieLimitList);
			this.SetGrowStepList(this.sortieLimitList);
		}
	}

	public List<GameWebAPI.RespDataMA_WorldDungeonSortieLimit.WorldDungeonSortieLimit> GetSortieLimitList()
	{
		return this.sortieLimitList;
	}
}
