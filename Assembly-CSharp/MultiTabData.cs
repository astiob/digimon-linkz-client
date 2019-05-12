using System;
using UnityEngine;

[Serializable]
public class MultiTabData
{
	public int idx;

	public string name = string.Empty;

	public GameObject goTab;

	public UISprite spTab;

	public UISprite spLeftArrow;

	public UISprite spRightArrow;

	public UILabel tabLabel;

	public GameObject goAlertIcon;

	public GUICollider collider;

	public Action<int> callbackAction;

	public string labelText = string.Empty;
}
