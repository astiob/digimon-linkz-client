using Evolution;
using Monster;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class GUIMonsterIconList : ClassSingleton<GUIMonsterIconList>
{
	private List<GUIMonsterIcon> monsterIconList;

	public void Initialize()
	{
		if (this.monsterIconList == null)
		{
			this.monsterIconList = new List<GUIMonsterIcon>();
		}
		else
		{
			this.AllDisable();
			this.AllDelete();
		}
	}

	public void AddIcon(GUIMonsterIcon icon)
	{
		if (!this.monsterIconList.Contains(icon))
		{
			this.monsterIconList.Add(icon);
		}
	}

	public GUIMonsterIcon GetIcon(MonsterData monsterData)
	{
		GUIMonsterIcon result = null;
		for (int i = 0; i < this.monsterIconList.Count; i++)
		{
			if (this.monsterIconList[i].Data == monsterData)
			{
				result = this.monsterIconList[i];
				break;
			}
		}
		return result;
	}

	public void AllDisable()
	{
		for (int i = 0; i < this.monsterIconList.Count; i++)
		{
			this.monsterIconList[i].gameObject.SetActive(false);
		}
	}

	public void AllDelete()
	{
		for (int i = 0; i < this.monsterIconList.Count; i++)
		{
			this.monsterIconList[i].ClearMonsterData();
			UnityEngine.Object.Destroy(this.monsterIconList[i].gameObject);
		}
		this.monsterIconList.Clear();
	}

	public void ResetIconState()
	{
		for (int i = 0; i < this.monsterIconList.Count; i++)
		{
			this.monsterIconList[i].SetTouchAct_S(null);
			this.monsterIconList[i].SetTouchAct_L(null);
			this.monsterIconList[i].SortMess = string.Empty;
			this.monsterIconList[i].LevelMess = string.Empty;
			this.monsterIconList[i].SetGrayout(GUIMonsterIcon.DIMM_LEVEL.ACTIVE);
			this.monsterIconList[i].SelectNum = -1;
			this.monsterIconList[i].DimmMess = string.Empty;
		}
	}

	public void SetLockIcon()
	{
		for (int i = 0; i < this.monsterIconList.Count; i++)
		{
			this.monsterIconList[i].SetLock();
		}
	}

	public void UnnewMonserDataList()
	{
		for (int i = 0; i < this.monsterIconList.Count; i++)
		{
			this.monsterIconList[i].New = false;
		}
	}

	public void SetSortLSMessageEvoluve(bool onlyGrayOut)
	{
		for (int i = 0; i < this.monsterIconList.Count; i++)
		{
			this.monsterIconList[i].SetMessageLevel();
			this.monsterIconList[i].SetSortMessageColor(ConstValue.DIGIMON_GREEN);
			ClassSingleton<EvolutionData>.Instance.CheckEvolveable(this.monsterIconList[i], this.monsterIconList[i].Data, onlyGrayOut);
		}
	}

	public void SetSortLSMessage(MonsterSortType sortType)
	{
		for (int i = 0; i < this.monsterIconList.Count; i++)
		{
			this.monsterIconList[i].SetMessageLevel();
			this.monsterIconList[i].SetSortMessageColor(ConstValue.DIGIMON_GREEN);
			MonsterData data = this.monsterIconList[i].Data;
			switch (sortType)
			{
			case MonsterSortType.DATE:
			case MonsterSortType.AROUSAL:
			case MonsterSortType.LEVEL:
				this.monsterIconList[i].SortMess = string.Empty;
				break;
			case MonsterSortType.HP:
				this.monsterIconList[i].SetMonsterSortMessage(data.userMonster.hp);
				break;
			case MonsterSortType.ATK:
				this.monsterIconList[i].SetMonsterSortMessage(data.userMonster.attack);
				break;
			case MonsterSortType.DEF:
				this.monsterIconList[i].SetMonsterSortMessage(data.userMonster.defense);
				break;
			case MonsterSortType.S_ATK:
				this.monsterIconList[i].SetMonsterSortMessage(data.userMonster.spAttack);
				break;
			case MonsterSortType.S_DEF:
				this.monsterIconList[i].SetMonsterSortMessage(data.userMonster.spDefense);
				break;
			case MonsterSortType.SPD:
				this.monsterIconList[i].SetMonsterSortMessage(data.userMonster.speed);
				break;
			case MonsterSortType.LUCK:
				this.monsterIconList[i].SetMonsterSortMessage(data.userMonster.luck);
				break;
			case MonsterSortType.GROW_STEP:
				if (data.userMonster.IsEgg())
				{
					this.monsterIconList[i].SortMess = MonsterGrowStepData.GetGrowStepName(MonsterGrowStepData.ToGrowStepString(GrowStep.EGG));
				}
				else
				{
					this.monsterIconList[i].SortMess = MonsterGrowStepData.GetGrowStepName(data.monsterMG.growStep);
				}
				break;
			case MonsterSortType.TRIBE:
			{
				string tribeName = MonsterTribeData.GetTribeName(data.monsterMG.tribe);
				this.monsterIconList[i].SetMonsterSortMessage(tribeName);
				break;
			}
			}
		}
	}

	private GUIMonsterIcon GetIcon(string userMonsterId)
	{
		GUIMonsterIcon result = null;
		for (int i = 0; i < this.monsterIconList.Count; i++)
		{
			if (this.monsterIconList[i].Data.userMonster.userMonsterId == userMonsterId)
			{
				result = this.monsterIconList[i];
				break;
			}
		}
		return result;
	}

	public void RefreshList(List<MonsterData> monsterDataList)
	{
		this.AllDisable();
		List<GUIMonsterIcon> list = new List<GUIMonsterIcon>();
		for (int i = 0; i < this.monsterIconList.Count; i++)
		{
			MonsterData data = this.monsterIconList[i].Data;
			if (data != null)
			{
				string id = data.userMonster.userMonsterId;
				if (!monsterDataList.Any((MonsterData x) => x.userMonster.userMonsterId == id))
				{
					this.monsterIconList[i].ClearMonsterData();
					UnityEngine.Object.Destroy(this.monsterIconList[i].gameObject);
					list.Add(this.monsterIconList[i]);
				}
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			this.monsterIconList.Remove(list[j]);
		}
		list.Clear();
		List<GUIMonsterIcon> list2 = new List<GUIMonsterIcon>();
		Vector3 vPos = new Vector3(2000f, 2000f, 0f);
		for (int k = 0; k < monsterDataList.Count; k++)
		{
			GUIMonsterIcon icon = this.GetIcon(monsterDataList[k].userMonster.userMonsterId);
			if (null != icon)
			{
				icon.Data = monsterDataList[k];
			}
			else
			{
				GUIMonsterIcon guimonsterIcon = GUIMonsterIcon.MakePrefabByMonsterData(monsterDataList[k], Vector3.one, vPos, null, false, false);
				list2.Add(guimonsterIcon);
				guimonsterIcon.gameObject.SetActive(false);
			}
		}
		if (0 < list2.Count)
		{
			this.monsterIconList.AddRange(list2);
		}
	}

	public void PushBackAllMonsterPrefab()
	{
		for (int i = 0; i < this.monsterIconList.Count; i++)
		{
			float offsetX = 200f * (float)(i % 5) + 200f;
			float offsetY = 200f * (float)(i / 5) + -200f;
			this.monsterIconList[i].PushBack(offsetX, offsetY);
		}
	}
}
