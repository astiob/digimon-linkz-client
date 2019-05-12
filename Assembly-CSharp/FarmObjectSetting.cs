using FarmData;
using System;
using UnityEngine;

public class FarmObjectSetting : MonoBehaviour
{
	[NonSerialized]
	public FarmObject farmObject;

	[NonSerialized]
	public FarmObjectSetting.SettingMode settingMode;

	private Vector3 backupPosition;

	public bool SetFarmObject(FarmObject farmObject, Vector3 basePos3D)
	{
		FarmField field = FarmRoot.Instance.Field;
		this.farmObject = farmObject;
		this.settingMode = FarmObjectSetting.SettingMode.BUILD;
		this.farmObject.SetSettingMark(field, FarmRoot.Instance.SettingMark);
		FarmGrid.GridPosition gridPosition = field.Grid.GetGridPosition(basePos3D);
		bool flag = field.IsOutsideField(gridPosition);
		if (flag)
		{
			gridPosition = field.Grid.GetGridPosition(Vector3.zero);
		}
		int gridIndex = field.Grid.GetGridIndex(gridPosition);
		Vector3 positionGridCenter = field.Grid.GetPositionGridCenter(gridIndex, false);
		this.farmObject.SetPosition(field.gridHorizontal, field.gridVertical, positionGridCenter);
		this.farmObject.DisplayedInFront(true);
		FarmField.Field field2 = field.GetField();
		this.farmObject.SetMarkColor(field2, gridIndex);
		FarmRoot.Instance.Input.AddTouchDragEvent(new Func<InputControll, bool>(this.OnDrag));
		return flag;
	}

	public FarmObjectSetting.ExtendBuildPositionSearchResult SearchExtendBuildGrid(FarmObject oneBeforeFarmObject)
	{
		FarmScenery scenery = FarmRoot.Instance.Scenery;
		FarmObjectSetting.PriorityDirection priorityDirection = FarmObjectSetting.PriorityDirection.FRONT;
		FarmObject farmObject = null;
		if (2 <= scenery.farmObjects.Count)
		{
			farmObject = scenery.farmObjects[scenery.farmObjects.Count - 2];
		}
		UserFacility userFacility = Singleton<UserDataMng>.Instance.GetUserFacility(oneBeforeFarmObject.userFacilityID);
		if (null != farmObject && farmObject.facilityID == oneBeforeFarmObject.facilityID)
		{
			UserFacility userFacility2 = Singleton<UserDataMng>.Instance.GetUserFacility(farmObject.userFacilityID);
			int num = userFacility2.positionX - userFacility.positionX;
			int num2 = userFacility2.positionY - userFacility.positionY;
			bool flag = Mathf.Abs(num) > Mathf.Abs(num2);
			if (flag)
			{
				priorityDirection = ((0 <= num) ? FarmObjectSetting.PriorityDirection.LEFT : FarmObjectSetting.PriorityDirection.RIGHT);
			}
			else
			{
				priorityDirection = ((0 <= num2) ? FarmObjectSetting.PriorityDirection.REAR : FarmObjectSetting.PriorityDirection.FRONT);
			}
		}
		return this.GetExtendBuildGrid(oneBeforeFarmObject, userFacility, priorityDirection);
	}

	private FarmObjectSetting.ExtendBuildPositionSearchResult GetExtendBuildGrid(FarmObject oneBeforeFarmObject, UserFacility oneBeforeUserFacility, FarmObjectSetting.PriorityDirection priorityDirection)
	{
		FarmGrid.GridPosition gridPosition = new FarmGrid.GridPosition
		{
			x = oneBeforeUserFacility.positionX,
			y = oneBeforeUserFacility.positionY
		};
		FarmObjectSetting.ExtendBuildPositionSearchInfo extendBuildPositionSearchInfo = new FarmObjectSetting.ExtendBuildPositionSearchInfo
		{
			farmField = FarmRoot.Instance.Field,
			field = FarmRoot.Instance.Field.GetField(),
			oneBeforeFarmObject = oneBeforeFarmObject,
			aroundCount = 1,
			aroundX = oneBeforeFarmObject.sizeX,
			aroundY = oneBeforeFarmObject.sizeY,
			priorityDirection = priorityDirection,
			originGrid = gridPosition,
			resultGrid = gridPosition
		};
		while (!this.SearchExtendBuildGrid(extendBuildPositionSearchInfo))
		{
			if (extendBuildPositionSearchInfo.isOutsideMap)
			{
				break;
			}
			extendBuildPositionSearchInfo.aroundCount++;
			extendBuildPositionSearchInfo.aroundX = oneBeforeFarmObject.sizeX + extendBuildPositionSearchInfo.aroundCount;
			extendBuildPositionSearchInfo.aroundY = oneBeforeFarmObject.sizeY + extendBuildPositionSearchInfo.aroundCount;
		}
		return new FarmObjectSetting.ExtendBuildPositionSearchResult
		{
			grid = extendBuildPositionSearchInfo.resultGrid,
			isOutsideMap = extendBuildPositionSearchInfo.isOutsideMap
		};
	}

	private bool SearchExtendBuildGrid(FarmObjectSetting.ExtendBuildPositionSearchInfo searchInfo)
	{
		bool result = false;
		for (int i = 0; i < Enum.GetNames(typeof(FarmObjectSetting.PriorityDirection)).Length; i++)
		{
			int num = (int)(i + searchInfo.priorityDirection);
			if (Enum.GetNames(typeof(FarmObjectSetting.PriorityDirection)).Length <= num)
			{
				num -= Enum.GetNames(typeof(FarmObjectSetting.PriorityDirection)).Length;
			}
			this.AddExtendBuildPositionSearchInfo(searchInfo, (FarmObjectSetting.PriorityDirection)num);
			if (this.SearchPutGrid(searchInfo))
			{
				result = true;
				break;
			}
		}
		return result;
	}

	private void AddExtendBuildPositionSearchInfo(FarmObjectSetting.ExtendBuildPositionSearchInfo searchInfo, FarmObjectSetting.PriorityDirection priorityDirection)
	{
		searchInfo.startGrid = searchInfo.originGrid;
		searchInfo.endGrid = searchInfo.originGrid;
		switch (priorityDirection)
		{
		case FarmObjectSetting.PriorityDirection.FRONT:
			searchInfo.startGrid.y = searchInfo.startGrid.y + searchInfo.aroundY;
			searchInfo.endGrid.x = searchInfo.endGrid.x - searchInfo.aroundX;
			searchInfo.changeValue.x = -searchInfo.aroundX;
			searchInfo.changeValue.y = -searchInfo.aroundY;
			break;
		case FarmObjectSetting.PriorityDirection.LEFT:
			searchInfo.startGrid.x = searchInfo.startGrid.x - searchInfo.aroundX;
			searchInfo.endGrid.y = searchInfo.endGrid.y - searchInfo.aroundY;
			searchInfo.changeValue.x = searchInfo.aroundX;
			searchInfo.changeValue.y = -searchInfo.aroundY;
			break;
		case FarmObjectSetting.PriorityDirection.REAR:
			searchInfo.startGrid.y = searchInfo.startGrid.y - searchInfo.aroundY;
			searchInfo.endGrid.x = searchInfo.endGrid.x + searchInfo.aroundX;
			searchInfo.changeValue.x = searchInfo.aroundX;
			searchInfo.changeValue.y = searchInfo.aroundY;
			break;
		case FarmObjectSetting.PriorityDirection.RIGHT:
			searchInfo.startGrid.x = searchInfo.startGrid.x + searchInfo.aroundX;
			searchInfo.endGrid.y = searchInfo.endGrid.y + searchInfo.aroundY;
			searchInfo.changeValue.x = -searchInfo.aroundX;
			searchInfo.changeValue.y = searchInfo.aroundY;
			break;
		}
	}

	private bool SearchPutGrid(FarmObjectSetting.ExtendBuildPositionSearchInfo searchInfo)
	{
		bool result = false;
		searchInfo.isOutsideMap = true;
		FarmGrid.GridPosition startGrid = searchInfo.startGrid;
		while (startGrid.x != searchInfo.endGrid.x && startGrid.y != searchInfo.endGrid.y)
		{
			if (!searchInfo.farmField.IsOutsideField(startGrid))
			{
				searchInfo.isOutsideMap = false;
				int gridIndex = searchInfo.farmField.Grid.GetGridIndex(startGrid);
				int[] gridIndexs = searchInfo.oneBeforeFarmObject.GetGridIndexs(searchInfo.field, gridIndex);
				if (!searchInfo.oneBeforeFarmObject.IsInvalidGrid(searchInfo.farmField, gridIndexs) && !searchInfo.oneBeforeFarmObject.IsPutGrid(searchInfo.field, gridIndexs))
				{
					result = true;
					searchInfo.resultGrid = startGrid;
					break;
				}
			}
			startGrid.x += searchInfo.changeValue.x;
			startGrid.y += searchInfo.changeValue.y;
		}
		return result;
	}

	public void SetMoveFarmObject(FarmObject farmObject)
	{
		FarmRoot instance = FarmRoot.Instance;
		FarmField field = instance.Field;
		this.farmObject = farmObject;
		this.settingMode = FarmObjectSetting.SettingMode.MOVE;
		this.farmObject.SetSettingMark(field, instance.SettingMark);
		this.farmObject.SetPlaceable(true);
		farmObject.BackupPosition();
		field.SetGridPutFlag(this.farmObject, false);
		this.farmObject.DisplayedInFront(true);
		instance.Input.AddTouchDragEvent(new Func<InputControll, bool>(this.OnDrag));
	}

	public bool SetEditFarmObject(FarmObject farmObject, Vector3 basePos3D)
	{
		FarmRoot instance = FarmRoot.Instance;
		FarmField field = instance.Field;
		FarmField.Field field2 = field.GetField();
		this.farmObject = farmObject;
		this.settingMode = FarmObjectSetting.SettingMode.EDIT;
		FarmGrid.GridPosition gridPosition = field.Grid.GetGridPosition(basePos3D);
		bool flag = field.IsOutsideField(gridPosition);
		if (flag)
		{
			gridPosition = field.Grid.GetGridPosition(Vector3.zero);
		}
		int gridIndex = field.Grid.GetGridIndex(gridPosition);
		Vector3 positionGridCenter = field.Grid.GetPositionGridCenter(gridIndex, false);
		this.farmObject.SetPosition(field.gridHorizontal, field.gridVertical, positionGridCenter);
		this.farmObject.DisplayedInFront(true);
		int[] gridIndexs = this.farmObject.GetGridIndexs(field2, gridIndex);
		this.farmObject.isPlaceable = (!this.farmObject.IsInvalidGrid(field, gridIndexs) && !this.farmObject.IsPutGrid(field2, gridIndexs));
		return flag;
	}

	public void CancelBuild()
	{
		if (this.farmObject != null)
		{
			this.farmObject.DisplayedInFront(false);
		}
		bool deleteFarmObject = FarmObjectSetting.SettingMode.BUILD == this.settingMode;
		this.EndSetting(deleteFarmObject);
	}

	public FarmGrid.GridPosition ComplatedSetting()
	{
		FarmRoot instance = FarmRoot.Instance;
		FarmField field = instance.Field;
		this.farmObject.DisplayedInFront(false);
		FarmGrid.GridPosition result = field.SetGridPutFlag(this.farmObject, true);
		if (this.farmObject.facilityID == 11)
		{
			FarmRoot.Instance.ResetSetteingFence();
		}
		this.EndSetting(false);
		return result;
	}

	public void CancelMove()
	{
		FarmRoot instance = FarmRoot.Instance;
		FarmField field = instance.Field;
		this.farmObject.DisplayedInFront(false);
		this.farmObject.ResetPosition();
		field.SetGridPutFlag(this.farmObject, true);
		this.EndSetting(false);
	}

	public void EndSetting(bool deleteFarmObject)
	{
		if (deleteFarmObject)
		{
			UnityEngine.Object.Destroy(this.farmObject.gameObject);
		}
		this.farmObject = null;
		this.settingMode = FarmObjectSetting.SettingMode.NONE;
		FarmRoot instance = FarmRoot.Instance;
		instance.Input.RemoveTouchDragEvent(new Func<InputControll, bool>(this.OnDrag));
	}

	public bool OnDrag(InputControll inputControll)
	{
		if (this.farmObject.gameObject == inputControll.rayHitColliderObject)
		{
			FarmRoot instance = FarmRoot.Instance;
			FarmField field = instance.Field;
			if (field.Grid.isSelectedGrid)
			{
				FarmField.Field field2 = field.GetField();
				int[] gridIndexs = this.farmObject.GetGridIndexs(field2, field.Grid.selectedGridIndex);
				if (!this.farmObject.IsInvalidGrid(field, gridIndexs))
				{
					this.farmObject.SetPosition(field.gridHorizontal, field.gridVertical, field.Grid.GetPositionGridCenter(field.Grid.selectedGridIndex, false));
					this.farmObject.DisplayedInFront(true);
					bool placeable = false == this.farmObject.IsPutGrid(field2, gridIndexs);
					this.farmObject.SetPlaceable(placeable);
				}
			}
			return true;
		}
		return false;
	}

	public enum SettingMode
	{
		NONE,
		BUILD,
		MOVE,
		EDIT
	}

	private enum PriorityDirection
	{
		FRONT,
		LEFT,
		REAR,
		RIGHT
	}

	private sealed class ExtendBuildPositionSearchInfo
	{
		public FarmField farmField;

		public FarmField.Field field;

		public FarmObject oneBeforeFarmObject;

		public FarmObjectSetting.PriorityDirection priorityDirection;

		public int aroundCount;

		public int aroundX;

		public int aroundY;

		public FarmGrid.GridPosition startGrid;

		public FarmGrid.GridPosition endGrid;

		public FarmGrid.GridPosition changeValue;

		public FarmGrid.GridPosition originGrid;

		public FarmGrid.GridPosition resultGrid;

		public bool isOutsideMap;
	}

	public sealed class ExtendBuildPositionSearchResult
	{
		public FarmGrid.GridPosition grid;

		public bool isOutsideMap;
	}
}
