using System;
using UnityEngine;

[RequireComponent(typeof(UIWidget))]
public class UIAnchorOverride : MonoBehaviour
{
	[SerializeField]
	private bool useUIRoot;

	[SerializeField]
	private Transform targetObject;

	private static Transform uiRootPanel;

	private UIWidget widget;

	public static void SetUIRoot(UIRoot uiRoot)
	{
		UIAnchorOverride.uiRootPanel = uiRoot.transform;
	}

	private void OnEnable()
	{
		if (this.widget == null)
		{
			this.widget = base.GetComponent<UIWidget>();
		}
	}

	private void Update()
	{
		if (this.widget == null)
		{
			return;
		}
		if (this.useUIRoot && UIAnchorOverride.uiRootPanel == null)
		{
			return;
		}
		if (!this.useUIRoot && this.targetObject == null)
		{
			return;
		}
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		Transform target = (!this.useUIRoot) ? this.targetObject : UIAnchorOverride.uiRootPanel;
		this.widget.leftAnchor.target = target;
		this.widget.rightAnchor.target = target;
		this.widget.bottomAnchor.target = target;
		this.widget.topAnchor.target = target;
		this.widget.ResetAndUpdateAnchors();
	}
}
