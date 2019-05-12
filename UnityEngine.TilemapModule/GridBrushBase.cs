using System;

namespace UnityEngine
{
	public abstract class GridBrushBase : ScriptableObject
	{
		public virtual void Paint(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
		{
		}

		public virtual void Erase(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
		{
		}

		public virtual void BoxFill(GridLayout gridLayout, GameObject brushTarget, BoundsInt position)
		{
			for (int i = position.zMin; i < position.zMax; i++)
			{
				for (int j = position.yMin; j < position.yMax; j++)
				{
					for (int k = position.xMin; k < position.xMax; k++)
					{
						this.Paint(gridLayout, brushTarget, new Vector3Int(k, j, i));
					}
				}
			}
		}

		public virtual void BoxErase(GridLayout gridLayout, GameObject brushTarget, BoundsInt position)
		{
			for (int i = position.zMin; i < position.zMax; i++)
			{
				for (int j = position.yMin; j < position.yMax; j++)
				{
					for (int k = position.xMin; k < position.xMax; k++)
					{
						this.Erase(gridLayout, brushTarget, new Vector3Int(k, j, i));
					}
				}
			}
		}

		public virtual void Select(GridLayout gridLayout, GameObject brushTarget, BoundsInt position)
		{
		}

		public virtual void FloodFill(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
		{
		}

		public virtual void Rotate(GridBrushBase.RotationDirection direction, GridLayout.CellLayout layout)
		{
		}

		public virtual void Flip(GridBrushBase.FlipAxis flip, GridLayout.CellLayout layout)
		{
		}

		public virtual void Pick(GridLayout gridLayout, GameObject brushTarget, BoundsInt position, Vector3Int pivot)
		{
		}

		public virtual void Move(GridLayout gridLayout, GameObject brushTarget, BoundsInt from, BoundsInt to)
		{
		}

		public virtual void MoveStart(GridLayout gridLayout, GameObject brushTarget, BoundsInt position)
		{
		}

		public virtual void MoveEnd(GridLayout gridLayout, GameObject brushTarget, BoundsInt position)
		{
		}

		public enum Tool
		{
			Select,
			Move,
			Paint,
			Box,
			Pick,
			Erase,
			FloodFill
		}

		public enum RotationDirection
		{
			Clockwise,
			CounterClockwise
		}

		public enum FlipAxis
		{
			X,
			Y
		}
	}
}
