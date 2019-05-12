using System;
using System.Collections.Generic;
using UnityEngine;

public class FarmFieldData : ScriptableObject
{
	public FarmFieldData.FarmBaseData fieldBaseData;

	public List<FarmFieldData.FieldData> fieldData;

	[Serializable]
	public struct FarmBaseData
	{
		public float gridHorizontal;

		public float gridVertical;
	}

	[Serializable]
	public struct GridData
	{
		public bool invalid;
	}

	[Serializable]
	public struct FieldData
	{
		public Vector2 originPosition;

		public int fieldHorizontal;

		public int fieldVertical;

		public List<FarmFieldData.GridData> grids;
	}
}
