using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class FarmGrid
{
	public const int OUTSIDE = -1;

	private FarmField farmField;

	[NonSerialized]
	public bool isSelectedGrid;

	[NonSerialized]
	public int selectedGridIndex;

	public FarmGrid(FarmField farmField)
	{
		this.farmField = farmField;
	}

	public List<FarmGrid.Grid> CreateGridList(List<FarmFieldData.GridData> gridData)
	{
		List<FarmGrid.Grid> list = new List<FarmGrid.Grid>();
		foreach (FarmFieldData.GridData gridData2 in gridData)
		{
			FarmGrid.Grid item = new FarmGrid.Grid
			{
				invalid = gridData2.invalid
			};
			list.Add(item);
		}
		return list;
	}

	public void UpdateSelectGrid(Vector3 touchPos)
	{
		this.isSelectedGrid = false;
		FarmField.Field field = this.farmField.GetField();
		if (field == null)
		{
			return;
		}
		Rect rect = new Rect(field.originPosition.x, field.originPosition.y, (float)field.fieldHorizontal * this.farmField.gridHorizontal, (float)field.fieldVertical * this.farmField.gridVertical);
		if (rect.x <= touchPos.x && touchPos.x <= rect.x + rect.width && rect.y >= touchPos.z && touchPos.z >= rect.y - rect.height)
		{
			FarmGrid.GridPosition gridPosition = this.GetGridPosition(touchPos);
			int gridIndex = this.GetGridIndex(gridPosition);
			if (0 <= gridIndex && gridIndex <= field.grids.Count)
			{
				this.isSelectedGrid = true;
				this.selectedGridIndex = gridIndex;
			}
		}
	}

	public FarmGrid.GridPosition GetGridPosition(Vector3 position)
	{
		int x = 0;
		int y = 0;
		FarmField.Field field = this.farmField.GetField();
		if (field != null)
		{
			x = Mathf.FloorToInt((position.x - field.originPosition.x) / this.farmField.gridHorizontal);
			y = Mathf.CeilToInt((position.z - field.originPosition.y) / this.farmField.gridVertical) * -1;
		}
		return new FarmGrid.GridPosition
		{
			x = x,
			y = y
		};
	}

	public Vector3 GetPositionGridCenter(int gridIndex, bool localPosition = false)
	{
		Vector3 zero = Vector3.zero;
		FarmField.Field field = this.farmField.GetField();
		if (field != null)
		{
			int num = gridIndex % field.fieldHorizontal;
			int num2 = gridIndex / field.fieldHorizontal;
			float new_x = (float)num * this.farmField.gridHorizontal + this.farmField.gridHorizontal * 0.5f + field.originPosition.x;
			float new_z = -((float)num2 * this.farmField.gridVertical + this.farmField.gridVertical * 0.5f) + field.originPosition.y;
			if (!localPosition)
			{
				zero.Set(new_x, this.farmField.transform.parent.localPosition.y, new_z);
			}
			else
			{
				zero.Set(new_x, 0f, new_z);
			}
		}
		return zero;
	}

	public int GetGridIndex(FarmGrid.GridPosition gridPosition)
	{
		FarmField.Field field = this.farmField.GetField();
		return gridPosition.y * field.fieldHorizontal + gridPosition.x;
	}

	public int GetGridIndex(int x, int y)
	{
		FarmField.Field field = this.farmField.GetField();
		if (0 <= x && x <= field.fieldHorizontal && 0 <= y && y <= field.fieldVertical)
		{
			return y * field.fieldHorizontal + x;
		}
		return -1;
	}

	public FarmGrid.GridPosition IndexToPosition(int gridIndex)
	{
		FarmField.Field field = this.farmField.GetField();
		int num = gridIndex / field.fieldHorizontal;
		int x = gridIndex - num * field.fieldHorizontal;
		return new FarmGrid.GridPosition
		{
			x = x,
			y = num
		};
	}

	public int[] GetPassableGridIndexs()
	{
		List<int> list = new List<int>();
		List<FarmGrid.Grid> grids = this.farmField.GetField().grids;
		for (int i = 0; i < grids.Count; i++)
		{
			if (FarmUtility.IsPassableGrid(grids, i, false))
			{
				list.Add(i);
			}
		}
		return list.ToArray();
	}

	public int[] GetPassableGridIndexs(int[] gridIndexs)
	{
		List<int> list = new List<int>();
		List<FarmGrid.Grid> grids = this.farmField.GetField().grids;
		foreach (int num in gridIndexs)
		{
			if (0 <= num && num < grids.Count && FarmUtility.IsPassableGrid(grids, num, false))
			{
				list.Add(num);
			}
		}
		return list.ToArray();
	}

	public struct GridPosition
	{
		public int x;

		public int y;
	}

	public struct Grid
	{
		public bool invalid;

		public bool put;

		public bool impassable;
	}
}
