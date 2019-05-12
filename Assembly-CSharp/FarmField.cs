using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class FarmField : MonoBehaviour
{
	private FarmGrid farmGrid;

	[NonSerialized]
	public float gridHorizontal;

	[NonSerialized]
	public float gridVertical;

	private List<FarmField.Field> fields = new List<FarmField.Field>();

	public FarmGrid Grid
	{
		get
		{
			return this.farmGrid;
		}
	}

	private void Awake()
	{
		base.gameObject.tag = "Farm.Field";
		this.farmGrid = new FarmGrid(this);
	}

	public void SetFieldData(FarmFieldData fileData)
	{
		this.gridHorizontal = fileData.fieldBaseData.gridHorizontal;
		this.gridVertical = fileData.fieldBaseData.gridVertical;
		this.fields.Clear();
		foreach (FarmFieldData.FieldData fieldData in fileData.fieldData)
		{
			this.fields.Add(new FarmField.Field
			{
				originPosition = fieldData.originPosition,
				fieldHorizontal = fieldData.fieldHorizontal,
				fieldVertical = fieldData.fieldVertical,
				grids = this.farmGrid.CreateGridList(fieldData.grids)
			});
		}
	}

	public FarmField.Field GetField()
	{
		if (0 < this.fields.Count)
		{
			return this.fields[0];
		}
		return null;
	}

	public FarmGrid.GridPosition SetGridPutFlag(FarmObject farmObject, bool putFlag)
	{
		FarmGrid.GridPosition gridPosition = this.farmGrid.GetGridPosition(farmObject.GetBaseGridPosition3D());
		this.SetPutFlag(farmObject, gridPosition.x, gridPosition.y, putFlag);
		return gridPosition;
	}

	public void SetPutFlag(FarmObject farmObject, int gridX, int gridY, bool putFlag)
	{
		FarmField.Field field = this.GetField();
		FarmGrid.GridPosition gridPosition = farmObject.AdjustGridPosition(gridX, gridY);
		bool impassable = !FarmUtility.IsWalkBuild(farmObject.facilityID);
		for (int i = 0; i < farmObject.sizeY; i++)
		{
			int num = (gridPosition.y + i) * field.fieldHorizontal;
			for (int j = 0; j < farmObject.sizeX; j++)
			{
				int num2 = num + gridPosition.x + j;
				if (num2 >= 0 && field.grids.Count > num2)
				{
					FarmGrid.Grid value = field.grids[num2];
					value.put = putFlag;
					value.impassable = impassable;
					field.grids[num2] = value;
				}
			}
		}
	}

	public void ClearPutFlag()
	{
		List<FarmGrid.Grid> grids = this.GetField().grids;
		for (int i = 0; i < grids.Count; i++)
		{
			FarmGrid.Grid value = grids[i];
			value.put = false;
			value.impassable = false;
			grids[i] = value;
		}
	}

	public bool IsOutsideField(FarmGrid.GridPosition gridPosition)
	{
		FarmField.Field field = this.GetField();
		return 0 > gridPosition.x || 0 > gridPosition.y || field.fieldHorizontal <= gridPosition.x || field.fieldVertical <= gridPosition.y;
	}

	public class Field
	{
		public int fieldHorizontal;

		public int fieldVertical;

		public Vector2 originPosition;

		public List<FarmGrid.Grid> grids;
	}
}
