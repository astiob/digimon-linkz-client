using System;
using UnityEngine;

public class BattleMenu : MonoBehaviour
{
	[Header("DialogHelpのBG")]
	[SerializeField]
	private UIWidget helpBGWidget;

	[SerializeField]
	[Header("DialogMENUのBG")]
	private UIWidget menuBGWidget;

	[SerializeField]
	[Header("MenuDialog")]
	private MenuDialog menuDialog;

	[Header("DialogHelpのGameObject")]
	[SerializeField]
	public GameObject helpDialogGO;

	[SerializeField]
	[Header("Menu/PanelのTransform")]
	public Transform menuPanelTransform;

	public MenuDialog SetupMenu(Transform parent)
	{
		Transform transform = base.transform;
		transform.SetParent(parent);
		transform.localPosition = Vector3.right;
		transform.localScale = Vector3.one;
		this.SetupAnchor(this.helpBGWidget, parent);
		this.SetupAnchor(this.menuBGWidget, parent);
		return this.menuDialog;
	}

	private void SetupAnchor(UIWidget widget, Transform target)
	{
		widget.leftAnchor.target = target;
		widget.leftAnchor.relative = 0f;
		widget.leftAnchor.absolute = -1;
		widget.rightAnchor.target = target;
		widget.rightAnchor.relative = 1f;
		widget.rightAnchor.absolute = 1;
		widget.bottomAnchor.target = target;
		widget.bottomAnchor.relative = 0f;
		widget.bottomAnchor.absolute = -1;
		widget.topAnchor.target = target;
		widget.topAnchor.relative = 1f;
		widget.topAnchor.absolute = 1;
		widget.ResetAnchors();
		widget.UpdateAnchors();
	}
}
