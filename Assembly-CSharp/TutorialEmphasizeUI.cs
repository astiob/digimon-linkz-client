using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TutorialEmphasizeUI : MonoBehaviour
{
	[SerializeField]
	private TutorialEmphasizeUI.UiNameType uiName;

	private Action actionTouchedEvent;

	private GUICollider guiCollider;

	private UIButton uiButton;

	private List<TutorialEmphasizeUI.SortOrderInfo> originalSortOrders = new List<TutorialEmphasizeUI.SortOrderInfo>();

	private List<TutorialEmphasizeUI.DepthInfo> originalDepths = new List<TutorialEmphasizeUI.DepthInfo>();

	private TutorialEmphasizeUI.TransformInfo originalTransform;

	public List<EventDelegate> onHoldWaitPress = new List<EventDelegate>();

	public List<EventDelegate> onDisengagePress = new List<EventDelegate>();

	public bool IsHoldPressOnly;

	public float WaitPressCall = 0.2f;

	private bool isHolded;

	public TutorialEmphasizeUI.UiNameType UiName
	{
		get
		{
			return this.uiName;
		}
		set
		{
			this.uiName = value;
		}
	}

	public bool IsParentChange { get; set; }

	public void SetEmphasizeAfterWindowOpened(Transform parent, Action actionWinOpened)
	{
		CommonDialog componentInParent = base.GetComponentInParent<CommonDialog>();
		if (null != componentInParent)
		{
			componentInParent.SetOnOpened(delegate(int x)
			{
				this.SetEmphasize(parent);
				actionWinOpened();
			});
		}
		else
		{
			this.SetEmphasize(parent);
			actionWinOpened();
		}
	}

	public void SetEmphasize(Transform parent)
	{
		this.SetOriginalTransformInfo();
		if (this.IsParentChange)
		{
			this.ChangeParent(parent);
			this.ChangePosition(new Vector3(base.transform.localPosition.x, base.transform.localPosition.y, 0f));
		}
		else
		{
			this.ChangePosition(new Vector3(base.transform.localPosition.x, base.transform.localPosition.y, parent.localPosition.z));
		}
		this.SetDepth(base.transform, 8200, 200);
		UIPanel componentInParent = base.transform.GetComponentInParent<UIPanel>();
		if (null != componentInParent)
		{
			this.SetSortOrder(base.transform, componentInParent.sortingOrder + 1);
		}
	}

	public void ResetEmphasize()
	{
		this.ResetSortOrder();
		if (this.IsParentChange)
		{
			this.ChangeParent(this.originalTransform.parent);
			this.ChangePosition(this.originalTransform.localPosition);
			this.ChangeScale(this.originalTransform.localScale);
		}
		else
		{
			this.ChangePosition(this.originalTransform.localPosition);
		}
		this.ResetDepth(base.transform);
		this.IsParentChange = false;
	}

	private void SetOriginalTransformInfo()
	{
		Transform transform = base.transform;
		this.originalTransform.parent = transform.parent;
		this.originalTransform.localPosition = transform.localPosition;
		this.originalTransform.localScale = transform.localScale;
	}

	private void ChangeParent(Transform newParent)
	{
		base.transform.parent = newParent;
	}

	private void ChangePosition(Vector3 position)
	{
		base.transform.localPosition = position;
	}

	private void ChangeScale(Vector3 scale)
	{
		base.transform.localScale = scale;
	}

	private void SetDepth(Transform t, int newWidgetDepth, int newPanelDepth)
	{
		UIWidget component = t.GetComponent<UIWidget>();
		UIPanel component2 = t.GetComponent<UIPanel>();
		TutorialEmphasizeUI.DepthInfo depthInfo = new TutorialEmphasizeUI.DepthInfo
		{
			uiTransform = t,
			widget = ((!(null != component)) ? 0 : component.depth),
			panel = ((!(null != component2)) ? 0 : component2.depth)
		};
		this.originalDepths.Add(depthInfo);
		int newWidgetDepth2 = newWidgetDepth + ((depthInfo.widget >= 0) ? depthInfo.widget : 0);
		int newPanelDepth2 = newPanelDepth + ((depthInfo.panel >= 0) ? depthInfo.panel : 0);
		this.ChangeDepth(component, component2, newWidgetDepth2, newPanelDepth2);
		for (int i = 0; i < t.childCount; i++)
		{
			this.SetDepth(t.GetChild(i), newWidgetDepth, newPanelDepth);
		}
	}

	private void ResetDepth(Transform t)
	{
		TutorialEmphasizeUI.DepthInfo depthInfo = this.originalDepths.SingleOrDefault((TutorialEmphasizeUI.DepthInfo x) => x.uiTransform == t);
		if (depthInfo != null)
		{
			this.ChangeDepth(t.GetComponent<UIWidget>(), t.GetComponent<UIPanel>(), depthInfo.widget, depthInfo.panel);
			for (int i = 0; i < t.childCount; i++)
			{
				this.ResetDepth(t.GetChild(i));
			}
		}
		this.originalDepths.Remove(depthInfo);
	}

	private void ChangeDepth(UIWidget widget, UIPanel panel, int newWidgetDepth, int newPanelDepth)
	{
		if (null != widget)
		{
			widget.ParentHasChanged();
			widget.depth = newWidgetDepth;
		}
		if (null != panel)
		{
			panel.depth = newPanelDepth;
		}
	}

	private void SetSortOrder(Transform t, int newSortOrder)
	{
		UIPanel component = t.GetComponent<UIPanel>();
		if (null != component)
		{
			TutorialEmphasizeUI.SortOrderInfo sortOrderInfo = new TutorialEmphasizeUI.SortOrderInfo
			{
				uiPanel = component,
				sortOrder = component.sortingOrder
			};
			this.originalSortOrders.Add(sortOrderInfo);
			int sortingOrder = newSortOrder + ((sortOrderInfo.sortOrder >= 0) ? sortOrderInfo.sortOrder : 0);
			component.sortingOrder = sortingOrder;
		}
		for (int i = 0; i < t.childCount; i++)
		{
			this.SetSortOrder(t.GetChild(i), newSortOrder);
		}
	}

	private void ResetSortOrder()
	{
		for (int i = 0; i < this.originalSortOrders.Count; i++)
		{
			TutorialEmphasizeUI.SortOrderInfo sortOrderInfo = this.originalSortOrders[i];
			sortOrderInfo.uiPanel.sortingOrder = sortOrderInfo.sortOrder;
		}
	}

	public void SetPushedEvent(Action actionPushed)
	{
		this.actionTouchedEvent = actionPushed;
		this.guiCollider = this.FindChildComponent<GUICollider>(base.transform);
		if (null != this.guiCollider)
		{
			if (this.guiCollider.callBackState == GUICollider.CallBackState.OnTouchBegan)
			{
				this.guiCollider.onTouchBegan += this.OnTouchBeginEvent;
			}
			else
			{
				this.guiCollider.onTouchEnded += this.OnTouchEndEvent;
			}
		}
		else
		{
			this.uiButton = this.FindChildComponent<UIButton>(base.transform);
			if (null != this.uiButton)
			{
				EventDelegate.Add(this.uiButton.onClick, new EventDelegate.Callback(this.OnPushedButton));
			}
		}
	}

	public void SetActiveCollider(bool active)
	{
		BoxCollider boxCollider = this.FindChildComponent<BoxCollider>(base.transform);
		if (null != boxCollider)
		{
			boxCollider.enabled = active;
		}
	}

	private void OnTouchEndEvent(Touch arg1, Vector2 arg2, bool touchUpOver)
	{
		if (touchUpOver)
		{
			this.guiCollider.onTouchEnded -= this.OnTouchEndEvent;
			this.guiCollider = null;
			if (this.actionTouchedEvent != null)
			{
				this.actionTouchedEvent();
				this.actionTouchedEvent = null;
			}
		}
	}

	private void OnTouchBeginEvent(Touch arg1, Vector2 arg2)
	{
		this.guiCollider.onTouchBegan -= this.OnTouchBeginEvent;
		this.guiCollider = null;
		if (this.actionTouchedEvent != null)
		{
			this.actionTouchedEvent();
			this.actionTouchedEvent = null;
		}
	}

	private void OnPushedButton()
	{
		if (!this.IsHoldPressOnly)
		{
			EventDelegate.Remove(this.uiButton.onClick, new EventDelegate.Callback(this.OnPushedButton));
			this.uiButton = null;
			if (this.actionTouchedEvent != null)
			{
				this.actionTouchedEvent();
				this.actionTouchedEvent = null;
			}
		}
	}

	private T FindChildComponent<T>(Transform t) where T : class
	{
		T t2 = t.GetComponent<T>();
		if (t2 == null)
		{
			for (int i = 0; i < t.childCount; i++)
			{
				t2 = this.FindChildComponent<T>(t.GetChild(i));
				if (t2 != null)
				{
					break;
				}
			}
		}
		return t2;
	}

	private void OnPress(bool IsDown)
	{
		if (!this.IsHoldPressOnly)
		{
			return;
		}
		if (IsDown)
		{
			this.InvokeWaitPressCallback();
		}
		else if (!this.isHolded)
		{
			this.CancelInvokeWaitPressCallback();
		}
		else
		{
			if (this.actionTouchedEvent != null)
			{
				this.actionTouchedEvent();
				this.actionTouchedEvent = null;
			}
			EventDelegate.Execute(this.onDisengagePress);
			this.isHolded = false;
			this.IsHoldPressOnly = false;
		}
	}

	private void OnDragOut(GameObject DraggedObj)
	{
		if (!this.IsHoldPressOnly)
		{
			return;
		}
		if (!this.isHolded)
		{
			this.CancelInvokeWaitPressCallback();
		}
		else
		{
			if (this.actionTouchedEvent != null)
			{
				this.actionTouchedEvent();
				this.actionTouchedEvent = null;
			}
			this.isHolded = false;
			this.IsHoldPressOnly = false;
		}
	}

	private void InvokeWaitPressCallback()
	{
		base.Invoke("WaitPressCallback", this.WaitPressCall);
	}

	private void CancelInvokeWaitPressCallback()
	{
		base.CancelInvoke("WaitPressCallback");
	}

	private void WaitPressCallback()
	{
		EventDelegate.Execute(this.onHoldWaitPress);
		this.isHolded = true;
	}

	public enum UiNameType
	{
		NAME,
		DIGIMON,
		SKILL,
		OPERATION,
		SHORTCUT,
		YES_BUTTON,
		DIGIVICE,
		MEAL,
		MEAL_DIGI,
		MEAL_OK,
		MEAL_GIVE,
		MEAL_PLUS,
		PARTY,
		RECOVERY,
		RECOVERY_NO,
		GASHA,
		GASHA_START,
		ATTACK,
		DIGISTONE,
		RECOVERY_YES,
		UI_CLOSE_BUTTON,
		QUEST,
		EVOLUTION,
		DIGI_GARDEN,
		SKILL_HOLD,
		TAB2_RIGHT,
		GASHA_LINK_POINT,
		GARDEN_LIST,
		GARDEN_EVOLUTION,
		GARDEN_DIGI,
		TRAINING,
		FACILITY,
		FACILITY_SHOP,
		MISSION,
		CHIP_INSTALLING
	}

	private sealed class SortOrderInfo
	{
		public UIPanel uiPanel;

		public int sortOrder;
	}

	private sealed class DepthInfo
	{
		public Transform uiTransform;

		public int widget;

		public int panel;
	}

	private struct TransformInfo
	{
		public Transform parent;

		public Vector3 localPosition;

		public Vector3 localScale;
	}
}
