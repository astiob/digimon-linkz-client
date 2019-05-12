using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class FarmDigimonUtility
{
	public const int NOT_WALK = -1;

	public static int GetPassableGridIndex()
	{
		FarmRoot instance = FarmRoot.Instance;
		FarmField field = instance.Field;
		int[] passableGridIndexs = field.Grid.GetPassableGridIndexs();
		if (0 < passableGridIndexs.Length)
		{
			int num = UnityEngine.Random.Range(0, passableGridIndexs.Length);
			return passableGridIndexs[num];
		}
		return -1;
	}

	public static int ChooseAroundGridIndex(FarmObject farmObject)
	{
		FarmRoot instance = FarmRoot.Instance;
		FarmField field = instance.Field;
		FarmGrid.GridPosition gridPosition = field.Grid.GetGridPosition(farmObject.transform.localPosition);
		int[] gridIndexs = farmObject.GetGridIndexs(field.GetField(), field.Grid.GetGridIndex(gridPosition));
		int[] aroundGridIndexs = FarmDigimonUtility.GetAroundGridIndexs(gridIndexs);
		int[] passableGridIndexs = field.Grid.GetPassableGridIndexs(aroundGridIndexs);
		if (0 < passableGridIndexs.Length)
		{
			int num = UnityEngine.Random.Range(0, passableGridIndexs.Length);
			return passableGridIndexs[num];
		}
		return -1;
	}

	public static int[] GetAroundGridIndexs(int[] gridIndexs)
	{
		FarmRoot instance = FarmRoot.Instance;
		FarmField field = instance.Field;
		List<FarmGrid.GridPosition> list = new List<FarmGrid.GridPosition>();
		List<int> list2 = new List<int>();
		for (int i = 0; i < gridIndexs.Length; i++)
		{
			FarmGrid.GridPosition item = field.Grid.IndexToPosition(gridIndexs[i]);
			list.Add(item);
		}
		for (int j = 0; j < list.Count; j++)
		{
			FarmGrid.GridPosition gridPosition = list[j];
			int index = field.Grid.GetGridIndex(gridPosition.x - 1, gridPosition.y);
			if (index != -1 && !gridIndexs.Any((int x) => x == index) && !list2.Any((int x) => x == index))
			{
				list2.Add(index);
			}
			index = field.Grid.GetGridIndex(gridPosition.x + 1, gridPosition.y);
			if (index != -1 && !gridIndexs.Any((int x) => x == index) && !list2.Any((int x) => x == index))
			{
				list2.Add(index);
			}
			index = field.Grid.GetGridIndex(gridPosition.x, gridPosition.y - 1);
			if (index != -1 && !gridIndexs.Any((int x) => x == index) && !list2.Any((int x) => x == index))
			{
				list2.Add(index);
			}
			index = field.Grid.GetGridIndex(gridPosition.x, gridPosition.y + 1);
			if (index != -1 && !gridIndexs.Any((int x) => x == index) && !list2.Any((int x) => x == index))
			{
				list2.Add(index);
			}
		}
		return list2.ToArray();
	}

	public static List<FarmDigimonUtility.PathInfo> GetPath(int startGridIndex, int endGridIndex)
	{
		FarmRoot instance = FarmRoot.Instance;
		FarmField field = instance.Field;
		List<FarmGrid.Grid> grids = instance.Field.GetField().grids;
		bool[] array = new bool[grids.Count];
		List<FarmDigimonUtility.PathInfo> list = new List<FarmDigimonUtility.PathInfo>();
		int num = 0;
		List<int> list2 = new List<int>();
		List<int> list3 = new List<int>();
		list2.Add(startGridIndex);
		list.Add(new FarmDigimonUtility.PathInfo
		{
			gridIndex = startGridIndex
		});
		if (startGridIndex == endGridIndex)
		{
			return list;
		}
		bool flag = false;
		FarmGrid.GridPosition gridPosition = field.Grid.IndexToPosition(startGridIndex);
		if (FarmDigimonUtility.CheckGrid(field.Grid, grids, gridPosition.x, gridPosition.y, false) == -1)
		{
			flag = true;
		}
		while (0 < list2.Count)
		{
			num++;
			for (int i = 0; i < list2.Count; i++)
			{
				FarmDigimonUtility.CheckAroundGrids(field.Grid, grids, list2[i], list3, false);
			}
			if (flag)
			{
				if (0 >= list3.Count)
				{
					for (int j = 0; j < list2.Count; j++)
					{
						FarmDigimonUtility.CheckAroundGrids(field.Grid, grids, list2[j], list3, true);
					}
				}
				else
				{
					flag = false;
				}
			}
			list2.Clear();
			for (int k = 0; k < list3.Count; k++)
			{
				int num2 = list3[k];
				if (!array[num2])
				{
					array[num2] = true;
					list2.Add(num2);
					list.Add(new FarmDigimonUtility.PathInfo
					{
						gridIndex = num2,
						checkPoint = num
					});
					if (endGridIndex == num2)
					{
						return list;
					}
				}
			}
			list3.Clear();
		}
		return null;
	}

	private static void CheckAroundGrids(FarmGrid farmGrid, List<FarmGrid.Grid> fieldGrids, int centerGridIndex, List<int> aroundGridIndexs, bool isIgnorePutedFlag)
	{
		FarmGrid.GridPosition gridPosition = farmGrid.IndexToPosition(centerGridIndex);
		int num = FarmDigimonUtility.CheckGrid(farmGrid, fieldGrids, gridPosition.x, gridPosition.y - 1, isIgnorePutedFlag);
		if (num != -1)
		{
			aroundGridIndexs.Add(num);
		}
		num = FarmDigimonUtility.CheckGrid(farmGrid, fieldGrids, gridPosition.x, gridPosition.y + 1, isIgnorePutedFlag);
		if (num != -1)
		{
			aroundGridIndexs.Add(num);
		}
		num = FarmDigimonUtility.CheckGrid(farmGrid, fieldGrids, gridPosition.x - 1, gridPosition.y, isIgnorePutedFlag);
		if (num != -1)
		{
			aroundGridIndexs.Add(num);
		}
		num = FarmDigimonUtility.CheckGrid(farmGrid, fieldGrids, gridPosition.x + 1, gridPosition.y, isIgnorePutedFlag);
		if (num != -1)
		{
			aroundGridIndexs.Add(num);
		}
	}

	private static int CheckGrid(FarmGrid farmGrid, List<FarmGrid.Grid> fieldGrids, int checkGridX, int checkGridY, bool isIgnorePutedFlag)
	{
		int gridIndex = farmGrid.GetGridIndex(checkGridX, checkGridY);
		if (gridIndex != -1 && FarmUtility.IsPassableGrid(fieldGrids, gridIndex, isIgnorePutedFlag))
		{
			return gridIndex;
		}
		return -1;
	}

	public static bool GetAroundGridIndexsForPath(FarmGrid farmGrid, int centerGridIndex, FarmDigimonUtility.PathInfo[] path, List<int> aroundGridIndexs)
	{
		FarmGrid.GridPosition gridPosition = farmGrid.IndexToPosition(centerGridIndex);
		for (int i = 0; i < path.Length; i++)
		{
			FarmGrid.GridPosition gridPosition2 = farmGrid.IndexToPosition(path[i].gridIndex);
			if ((gridPosition2.x == gridPosition.x - 1 && gridPosition2.y == gridPosition.y) || (gridPosition2.x == gridPosition.x + 1 && gridPosition2.y == gridPosition.y) || (gridPosition2.x == gridPosition.x && gridPosition2.y == gridPosition.y - 1) || (gridPosition2.x == gridPosition.x && gridPosition2.y == gridPosition.y + 1))
			{
				aroundGridIndexs.Add(path[i].gridIndex);
			}
		}
		return 0 < aroundGridIndexs.Count;
	}

	public sealed class PathInfo
	{
		public FarmGrid.GridPosition gridPos;

		public int gridIndex;

		public int checkPoint;
	}
}
