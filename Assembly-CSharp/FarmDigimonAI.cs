using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class FarmDigimonAI : MonoBehaviour
{
	private FarmDigimonAI.ActionParam actionParam = new FarmDigimonAI.ActionParam
	{
		aiID = FarmDigimonAI.AI_ID.NORMAL,
		actionID = FarmDigimonAI.ActionID.NONE,
		targetGridIndex = -1
	};

	private IEnumerator enumerator;

	private FarmDigimonAction Action
	{
		get
		{
			return base.GetComponent<FarmDigimonAction>();
		}
	}

	public IEnumerator Appearance(GameObject digimon)
	{
		if (this.ChooseAppearanceType() == FarmDigimonAction.AppearanceType.NORMAL)
		{
			this.enumerator = this.Action.AppearanceNormal(digimon, delegate
			{
				this.enumerator = null;
			});
			yield return base.StartCoroutine(this.enumerator);
		}
		yield break;
	}

	private FarmDigimonAction.AppearanceType ChooseAppearanceType()
	{
		return FarmDigimonAction.AppearanceType.NORMAL;
	}

	public void SetAI_ID(FarmDigimonAI.AI_ID id)
	{
		this.actionParam.aiID = id;
	}

	public FarmDigimonAI.ActionID ChooseAction()
	{
		FarmRoot instance = FarmRoot.Instance;
		FarmScenery scenery = instance.Scenery;
		List<FarmDigimonAI.ActionID> list = new List<FarmDigimonAI.ActionID>();
		for (int i = 1; i < Enum.GetNames(typeof(FarmDigimonAI.ActionID)).Length; i++)
		{
			if (i != 6)
			{
				list.Add((FarmDigimonAI.ActionID)i);
			}
		}
		if (scenery.farmObjects.Any((FarmObject x) => x.IsConstruction()))
		{
			list.Remove(FarmDigimonAI.ActionID.CONSTRUCTION);
		}
		if (!scenery.farmObjects.Any((FarmObject x) => x.facilityID == 1))
		{
			list.Remove(FarmDigimonAI.ActionID.MEAT_FARM);
		}
		int index = UnityEngine.Random.Range(0, list.Count);
		return list[index];
	}

	public void CreateActionParam(FarmDigimonAI.ActionID actionID)
	{
		switch (actionID)
		{
		case FarmDigimonAI.ActionID.MEAT_FARM:
			this.actionParam.targetGridIndex = this.GetMeatFarmAroundGridIndex();
			break;
		case FarmDigimonAI.ActionID.STROLL:
		case FarmDigimonAI.ActionID.STROLL_FAST:
			this.actionParam.targetGridIndex = FarmDigimonUtility.GetPassableGridIndex();
			break;
		case FarmDigimonAI.ActionID.CONSTRUCTION:
			this.actionParam.targetGridIndex = this.GetConstructionAroundGridIndex();
			break;
		}
		this.actionParam.actionID = actionID;
		if (this.actionParam.targetGridIndex != -1)
		{
			this.actionParam.pathGridIndexs = this.GetPassGridIndexs(this.actionParam.targetGridIndex);
		}
	}

	public FarmDigimonAI.ActionParam GetActionParam()
	{
		return this.actionParam;
	}

	public void ClearActionParam()
	{
		if (this.enumerator != null)
		{
			base.StopCoroutine(this.enumerator);
			this.enumerator = null;
		}
		this.actionParam.aiID = FarmDigimonAI.AI_ID.NORMAL;
		this.actionParam.actionID = FarmDigimonAI.ActionID.NONE;
		this.actionParam.targetGridIndex = -1;
		this.actionParam.pathGridIndexs = null;
	}

	private int GetMeatFarmAroundGridIndex()
	{
		FarmRoot instance = FarmRoot.Instance;
		FarmScenery scenery = instance.Scenery;
		FarmObject[] array = scenery.farmObjects.Where((FarmObject x) => x.facilityID == 1).ToArray<FarmObject>();
		if (array != null && 0 < array.Length)
		{
			int num = UnityEngine.Random.Range(0, array.Length);
			FarmObject farmObject = array[num];
			return FarmDigimonUtility.ChooseAroundGridIndex(farmObject);
		}
		return -1;
	}

	private int GetConstructionAroundGridIndex()
	{
		FarmRoot instance = FarmRoot.Instance;
		FarmScenery scenery = instance.Scenery;
		FarmObject[] array = scenery.farmObjects.Where((FarmObject x) => x.IsConstruction()).ToArray<FarmObject>();
		if (array != null && 0 < array.Length)
		{
			int num = UnityEngine.Random.Range(0, array.Length);
			FarmObject farmObject = array[num];
			return FarmDigimonUtility.ChooseAroundGridIndex(farmObject);
		}
		return -1;
	}

	private int[] GetPassGridIndexs(int targetGridIndex)
	{
		FarmRoot instance = FarmRoot.Instance;
		FarmField field = instance.Field;
		FarmGrid.GridPosition gridPosition = field.Grid.GetGridPosition(base.transform.localPosition);
		int gridIndex = field.Grid.GetGridIndex(gridPosition);
		List<FarmDigimonUtility.PathInfo> path = FarmDigimonUtility.GetPath(gridIndex, targetGridIndex);
		if (path != null)
		{
			FarmDigimonUtility.PathInfo pathInfo = path.SingleOrDefault((FarmDigimonUtility.PathInfo x) => x.gridIndex == targetGridIndex);
			int startGridIndex = pathInfo.gridIndex;
			int num = pathInfo.checkPoint - 1;
			List<int> list = new List<int>();
			while (0 <= num && this.ChoosePath(field.Grid, path, startGridIndex, num, list))
			{
				num--;
				startGridIndex = list.Last<int>();
			}
			if (0 < list.Count)
			{
				return list.ToArray();
			}
		}
		return null;
	}

	private bool ChoosePath(FarmGrid farmGrid, List<FarmDigimonUtility.PathInfo> path, int startGridIndex, int targetPathCount, List<int> choosePath)
	{
		FarmDigimonUtility.PathInfo[] array = path.Where((FarmDigimonUtility.PathInfo x) => x.checkPoint == targetPathCount).ToArray<FarmDigimonUtility.PathInfo>();
		if (array != null)
		{
			List<int> list = new List<int>();
			if (FarmDigimonUtility.GetAroundGridIndexsForPath(farmGrid, startGridIndex, array, list))
			{
				choosePath.Add(list[0]);
				return true;
			}
		}
		return false;
	}

	public enum AI_ID
	{
		NORMAL,
		MOOD_GOOD,
		MOOD_BAD
	}

	public enum ActionID
	{
		NONE,
		WAIT,
		MEAT_FARM,
		STROLL,
		STROLL_FAST,
		CONSTRUCTION,
		TOUCH_ACTION
	}

	public struct ActionParam
	{
		public FarmDigimonAI.AI_ID aiID;

		public FarmDigimonAI.ActionID actionID;

		public int targetGridIndex;

		public int[] pathGridIndexs;
	}
}
