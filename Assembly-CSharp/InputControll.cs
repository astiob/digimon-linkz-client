using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class InputControll : MonoBehaviour
{
	private Camera uiCamera;

	private Camera farmCamera;

	[NonSerialized]
	public bool disabledCameraOperation;

	private bool bTouchOne;

	private bool bTouchTwo;

	private InputControll.ControllValues controllValues;

	public float fXmove;

	public float fYmove;

	public float fRot;

	public float fScl;

	private Dictionary<int, Touch> prevTouch = new Dictionary<int, Touch>();

	private InputControll.TouchState touchState;

	public List<Func<InputControll, bool>> touchDragEvents = new List<Func<InputControll, bool>>();

	public List<Action<InputControll, bool>> touchEndEvents = new List<Action<InputControll, bool>>();

	[NonSerialized]
	public InputControll.TouchObjectType rayHitObjectType;

	[NonSerialized]
	public GameObject rayHitColliderObject;

	private int touchBeginGrid = -1;

	private float touchStartRealTime;

	private bool enableControl = true;

	public bool EnableControl
	{
		set
		{
			this.enableControl = value;
		}
	}

	private void Start()
	{
		this.uiCamera = Singleton<GUIMain>.Instance.GetComponent<Camera>();
		this.farmCamera = FarmRoot.Instance.Camera;
		this.touchState.isDraged = false;
		this.touchState.state = InputControll.TouchStateType.NONE;
	}

	private void Update()
	{
		if (GUIManager.IsTouching())
		{
			this.ResetCameraOperationParameter();
			if (this.touchState.state != InputControll.TouchStateType.NONE)
			{
				this.ClearFarmTouchInfo();
			}
		}
		else if (this.enableControl)
		{
			List<Touch> touch = this.GetTouch();
			this.UpdateControllValues(touch);
			this.UpdateTouchState(touch);
			this.UpdateRaycastHit(touch);
			this.TouchEventNotification();
		}
	}

	private List<Touch> GetTouch()
	{
		List<Touch> list = new List<Touch>();
		if (Input.touchCount > 0)
		{
			for (int i = 0; i < Input.touchCount; i++)
			{
				Touch touch = Input.GetTouch(i);
				list.Add(touch);
			}
		}
		return list;
	}

	private void UpdateControllValues(List<Touch> touchList)
	{
		if (this.disabledCameraOperation)
		{
			this.ResetCameraOperationParameter();
			return;
		}
		if (touchList.Count == 1)
		{
			if (this.bTouchTwo)
			{
				this.ResetCameraOperationParameter();
			}
			else if (!this.bTouchOne && !this.bTouchTwo)
			{
				this.bTouchOne = true;
				this.prevTouch.Add(touchList[0].fingerId, touchList[0]);
			}
			else if (this.bTouchOne)
			{
				int fingerId = touchList[0].fingerId;
				if (this.prevTouch.ContainsKey(fingerId))
				{
					this.ResetPos();
					Touch touch = this.prevTouch[fingerId];
					Vector3 vector = new Vector3(touch.position.x, touch.position.y, 0f);
					vector = this.uiCamera.ScreenToWorldPoint(vector);
					touch = touchList[0];
					Vector3 vector2 = new Vector3(touch.position.x, touch.position.y, 0f);
					vector2 = this.uiCamera.ScreenToWorldPoint(vector2);
					this.controllValues.move.x = vector2.x - vector.x;
					this.controllValues.move.y = vector2.y - vector.y;
					this.prevTouch[fingerId] = touchList[0];
				}
				else
				{
					this.ResetPos();
				}
			}
		}
		else if (touchList.Count >= 2)
		{
			if (this.bTouchOne)
			{
				this.ResetCameraOperationParameter();
			}
			else if (!this.bTouchOne && !this.bTouchTwo)
			{
				this.bTouchTwo = true;
				this.prevTouch.Add(touchList[0].fingerId, touchList[0]);
				this.prevTouch.Add(touchList[1].fingerId, touchList[1]);
			}
			else if (this.bTouchTwo)
			{
				this.ResetPos();
				List<Vector3> list = new List<Vector3>();
				List<Vector3> list2 = new List<Vector3>();
				for (int i = 0; i < touchList.Count; i++)
				{
					int fingerId = touchList[i].fingerId;
					if (this.prevTouch.ContainsKey(fingerId))
					{
						Touch touch = this.prevTouch[fingerId];
						Vector3 vector = new Vector3(touch.position.x, touch.position.y, 0f);
						vector = this.uiCamera.ScreenToWorldPoint(vector);
						list.Add(vector);
						touch = touchList[i];
						Vector3 vector2 = new Vector3(touch.position.x, touch.position.y, 0f);
						vector2 = this.uiCamera.ScreenToWorldPoint(vector2);
						list2.Add(vector2);
						this.prevTouch[fingerId] = touchList[i];
					}
				}
				if (list.Count == 2 && list2.Count == 2)
				{
					this.controllValues.scale = this.CalcScl(list2, list);
				}
			}
		}
		else
		{
			this.ResetCameraOperationParameter();
		}
	}

	private void ResetCameraOperationParameter()
	{
		this.bTouchOne = false;
		this.bTouchTwo = false;
		this.ResetPos();
		this.prevTouch = new Dictionary<int, Touch>();
	}

	private void ResetPos()
	{
		this.fXmove = 0f;
		this.fYmove = 0f;
		this.fRot = 0f;
		this.fScl = 0f;
		this.controllValues.move = Vector2.zero;
		this.controllValues.scale = 0f;
	}

	private float CalcRot(List<Vector3> vNowList, List<Vector3> vPrvList)
	{
		Vector3 vector = vPrvList[1] - vPrvList[0];
		Vector3 vector2 = vNowList[1] - vNowList[0];
		float num = vector2.x * vector2.x + vector2.y * vector2.y + vector2.z * vector2.z;
		float num2 = vector.x * vector.x + vector.y * vector.y + vector.z * vector.z;
		if (num == 0f || num2 == 0f)
		{
			return 0f;
		}
		float num3 = Mathf.Sqrt(num);
		float num4 = Mathf.Sqrt(num2);
		float num5 = vector2.x * vector.x + vector2.y * vector.y + vector2.z * vector.z;
		float num6 = vector2.y * vector.z - vector2.z * vector.y + (vector2.z * vector.x - vector2.x * vector.z) + (vector2.x * vector.y - vector2.y * vector.x);
		if (num3 * num4 == 0f)
		{
			return 0f;
		}
		float num7 = num5 / (num3 * num4);
		if (num7 <= 1f && num7 >= -1f)
		{
			float num8 = Mathf.Acos(num7);
			if (num6 > 0f)
			{
				num8 = -num8;
			}
			return num8 * 57.29578f;
		}
		return 0f;
	}

	private float CalcScl(List<Vector3> vNowList, List<Vector3> vPrvList)
	{
		Vector3 vector = vPrvList[1] - vPrvList[0];
		Vector3 vector2 = vNowList[1] - vNowList[0];
		float num = vector2.x * vector2.x + vector2.y * vector2.y + vector2.z * vector2.z;
		float num2 = vector.x * vector.x + vector.y * vector.y + vector.z * vector.z;
		if (num == 0f || num2 == 0f)
		{
			return 0f;
		}
		float num3 = Mathf.Sqrt(num);
		float num4 = Mathf.Sqrt(num2);
		return num3 - num4;
	}

	private void UpdateTouchState(List<Touch> touchList)
	{
		if (0 < touchList.Count)
		{
			if (this.touchState.state == InputControll.TouchStateType.NONE)
			{
				this.touchState.state = InputControll.TouchStateType.BEGIN;
				this.touchBeginGrid = this.GetGridIndexOfScreenCenter();
				this.touchStartRealTime = RealTime.time;
			}
			else
			{
				this.touchState.state = InputControll.TouchStateType.PUSH;
				Vector2 vector = new Vector2(this.controllValues.move.x, this.controllValues.move.y);
				if (0f < vector.magnitude)
				{
					this.touchState.isDraged = true;
					this.touchState.state = InputControll.TouchStateType.DRAG;
				}
			}
		}
		else if (this.touchState.state != InputControll.TouchStateType.NONE)
		{
			this.touchState.state = InputControll.TouchStateType.END;
		}
	}

	private void UpdateRaycastHit(List<Touch> touchList)
	{
		if (0 < touchList.Count)
		{
			Vector3 position = new Vector3(touchList[0].position.x, touchList[0].position.y, 0f);
			Ray ray = this.farmCamera.ScreenPointToRay(position);
			RaycastHit[] raycasts = Physics.RaycastAll(ray);
			InputControll.TouchObjectInfo touchObject = this.GetTouchObject(raycasts);
			if (this.rayHitObjectType == InputControll.TouchObjectType.NONE && touchObject.type != InputControll.TouchObjectType.NONE)
			{
				this.rayHitObjectType = touchObject.type;
				this.rayHitColliderObject = touchObject.transform.gameObject;
			}
		}
	}

	private InputControll.TouchObjectInfo GetTouchObject(RaycastHit[] raycasts)
	{
		InputControll.TouchObjectInfo result = new InputControll.TouchObjectInfo
		{
			type = InputControll.TouchObjectType.NONE,
			transform = null
		};
		FarmRoot instance = FarmRoot.Instance;
		FarmField field = instance.Field;
		Vector3 b = new Vector3(-0.5f, 0f, -0.5f);
		bool isInvalidCharaTap = false;
		if (instance.SettingObject.settingMode == FarmObjectSetting.SettingMode.BUILD || instance.farmMode == FarmRoot.FarmControlMode.EDIT)
		{
			isInvalidCharaTap = true;
		}
		int i = 0;
		while (i < raycasts.Length)
		{
			string tag = raycasts[i].transform.tag;
			if (tag != null)
			{
				if (InputControll.<>f__switch$map3B == null)
				{
					InputControll.<>f__switch$map3B = new Dictionary<string, int>(4)
					{
						{
							"Farm.Facility",
							0
						},
						{
							"Farm.Chara",
							1
						},
						{
							"Farm.Other",
							2
						},
						{
							"Farm.Field",
							3
						}
					};
				}
				int num;
				if (InputControll.<>f__switch$map3B.TryGetValue(tag, out num))
				{
					InputControll.TouchObjectType type;
					switch (num)
					{
					case 0:
						type = InputControll.TouchObjectType.FACILITY;
						break;
					case 1:
						type = InputControll.TouchObjectType.CHARA;
						break;
					case 2:
						type = InputControll.TouchObjectType.OTHER;
						break;
					case 3:
						field.Grid.UpdateSelectGrid(raycasts[i].point);
						goto IL_1FD;
					default:
						goto IL_13E;
					}
					if (null == result.transform)
					{
						result.Set(type, raycasts[i].transform, isInvalidCharaTap);
					}
					else
					{
						Vector3 vector = result.transform.position;
						vector = ((result.type == InputControll.TouchObjectType.CHARA) ? (vector - b) : vector);
						Vector3 vector2 = raycasts[i].transform.position;
						vector2 = ((result.type == InputControll.TouchObjectType.CHARA) ? (vector2 - b) : vector2);
						if (this.GetCameraDistanceXZ(vector) > this.GetCameraDistanceXZ(vector2))
						{
							result.Set(type, raycasts[i].transform, isInvalidCharaTap);
						}
					}
				}
			}
			IL_1FD:
			i++;
			continue;
			IL_13E:
			goto IL_1FD;
		}
		return result;
	}

	private float GetCameraDistanceXZ(Vector3 hitObjectPos)
	{
		Vector3 position = this.farmCamera.transform.position;
		position.y = 0f;
		Vector3 a = hitObjectPos;
		a.y = 0f;
		Vector3 vector = a - position;
		return Mathf.Abs(vector.x * vector.x) + Mathf.Abs(vector.z * vector.z);
	}

	private void TouchEventNotification()
	{
		bool flag = false;
		InputControll.TouchStateType state = this.touchState.state;
		if (state != InputControll.TouchStateType.DRAG)
		{
			if (state == InputControll.TouchStateType.END)
			{
				if (this.IsClick())
				{
					this.touchState.isDraged = false;
				}
				for (int i = 0; i < this.touchEndEvents.Count; i++)
				{
					this.touchEndEvents[i](this, this.touchState.isDraged);
				}
				this.ClearFarmTouchInfo();
			}
		}
		else
		{
			for (int j = 0; j < this.touchDragEvents.Count; j++)
			{
				if (this.touchDragEvents[j](this))
				{
					flag = true;
				}
			}
		}
		if (!flag)
		{
			this.fXmove = this.controllValues.move.x;
			this.fYmove = this.controllValues.move.y;
			this.fScl = this.controllValues.scale;
		}
	}

	private int GetGridIndexOfScreenCenter()
	{
		FarmRoot instance = FarmRoot.Instance;
		FarmField field = instance.Field;
		Vector3 position = this.farmCamera.transform.localPosition + FarmUtility.GetDistanceToGround();
		FarmGrid.GridPosition gridPosition = field.Grid.GetGridPosition(position);
		return field.Grid.GetGridIndex(gridPosition);
	}

	private bool IsClick()
	{
		float num = RealTime.time - this.touchStartRealTime;
		return this.touchBeginGrid == this.GetGridIndexOfScreenCenter() && 0.2f > num;
	}

	private void ClearFarmTouchInfo()
	{
		this.touchState.isDraged = false;
		this.touchState.state = InputControll.TouchStateType.NONE;
		this.rayHitObjectType = InputControll.TouchObjectType.NONE;
		this.rayHitColliderObject = null;
		this.touchBeginGrid = -1;
		this.touchStartRealTime = 0f;
	}

	public void AddTouchDragEvent(Func<InputControll, bool> callback)
	{
		if (this.touchDragEvents.Find((Func<InputControll, bool> x) => x == callback) == null)
		{
			this.touchDragEvents.Add(callback);
		}
	}

	public void RemoveTouchDragEvent(Func<InputControll, bool> callback)
	{
		this.touchDragEvents.Remove(callback);
	}

	public void AddTouchEndEvent(Action<InputControll, bool> callback)
	{
		if (this.touchEndEvents.Find((Action<InputControll, bool> x) => x == callback) == null)
		{
			this.touchEndEvents.Add(callback);
		}
	}

	public void RemoveTouchEndEvent(Action<InputControll, bool> callback)
	{
		this.touchEndEvents.Remove(callback);
	}

	public void RemoveAllTouchEndEvent()
	{
		this.touchEndEvents.Clear();
	}

	private struct ControllValues
	{
		public Vector2 move;

		public float scale;
	}

	private enum TouchStateType
	{
		NONE,
		BEGIN,
		PUSH,
		DRAG,
		END
	}

	private struct TouchState
	{
		public InputControll.TouchStateType state;

		public bool isDraged;
	}

	public enum TouchObjectType
	{
		NONE,
		FIELD,
		FACILITY,
		CHARA,
		OTHER
	}

	private struct TouchObjectInfo
	{
		public InputControll.TouchObjectType type;

		public Transform transform;

		public void Set(InputControll.TouchObjectType type, Transform transform, bool isInvalidCharaTap)
		{
			if (type == InputControll.TouchObjectType.CHARA)
			{
				if (!isInvalidCharaTap)
				{
					this.type = type;
					this.transform = transform;
				}
			}
			else
			{
				this.type = type;
				this.transform = transform;
			}
		}
	}
}
