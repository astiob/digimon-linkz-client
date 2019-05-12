using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class MonsterInfoList : MonoBehaviour
{
	[SerializeField]
	private PartsMultiRecruitMonsInfo monsterInfoUI;

	[SerializeField]
	private GameObject[] infoAnchorList;

	public PartsMultiRecruitMonsInfo[] CreateMonsterInfo(List<GameWebAPI.RespDataMA_WorldDungeonSortieLimit.WorldDungeonSortieLimit> limitList)
	{
		PartsMultiRecruitMonsInfo[] array = new PartsMultiRecruitMonsInfo[this.infoAnchorList.Length];
		Transform transform = this.infoAnchorList[0].transform;
		this.monsterInfoUI.transform.localPosition = transform.localPosition;
		array[0] = this.monsterInfoUI;
		array[0].SetSortieLimit(limitList);
		for (int i = 1; i < this.infoAnchorList.Length; i++)
		{
			array[i] = UnityEngine.Object.Instantiate<PartsMultiRecruitMonsInfo>(this.monsterInfoUI);
			transform = this.infoAnchorList[i].transform;
			array[i].transform.parent = this.monsterInfoUI.transform.parent;
			array[i].transform.localScale = this.monsterInfoUI.transform.localScale;
			array[i].transform.localPosition = transform.localPosition;
			array[i].transform.localRotation = this.monsterInfoUI.transform.localRotation;
			array[i].SetOriginalPos(transform.localPosition);
			array[i].SetLeaderMark(false);
			array[i].SetSortieLimit(limitList);
		}
		for (int j = 0; j < this.infoAnchorList.Length; j++)
		{
			UnityEngine.Object.Destroy(this.infoAnchorList[j].gameObject);
			this.infoAnchorList[j] = null;
		}
		this.infoAnchorList = null;
		return array;
	}
}
