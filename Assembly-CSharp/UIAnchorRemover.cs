using System;
using System.Collections.Generic;
using UnityEngine;

public class UIAnchorRemover : MonoBehaviour
{
	[SerializeField]
	private bool _removeAutomatic;

	[SerializeField]
	private bool _isRootThisObject = true;

	[SerializeField]
	private bool _isIncludeRootObject;

	[SerializeField]
	private bool _isIncludeThisObject;

	private void Start()
	{
		if (this._removeAutomatic)
		{
			this.RemoveAnchors();
		}
	}

	public void RemoveAnchors(bool ignoreOnEnable, bool ignoreOnStart, bool ignoreUpdate)
	{
		foreach (Transform transform in this.GetHaveUIRectObjects<UIRect>(this._isRootThisObject, this._isIncludeRootObject, this._isIncludeThisObject))
		{
			UIRect component = transform.GetComponent<UIRect>();
			if (!ignoreOnEnable || component.updateAnchors != UIRect.AnchorUpdate.OnEnable)
			{
				if (!ignoreOnStart || component.updateAnchors != UIRect.AnchorUpdate.OnStart)
				{
					if (!ignoreUpdate || component.updateAnchors != UIRect.AnchorUpdate.OnUpdate)
					{
						component.topAnchor.target = null;
						component.bottomAnchor.target = null;
						component.leftAnchor.target = null;
						component.rightAnchor.target = null;
						component.ResetAnchors();
					}
				}
			}
		}
	}

	public void RemoveAnchors()
	{
		this.RemoveAnchors(false, false, false);
	}

	public void ActiveAndRemoveAnchors()
	{
		Transform[] haveUIRectObjects = this.GetHaveUIRectObjects<UIRect>(this._isRootThisObject, this._isIncludeRootObject, this._isIncludeThisObject);
		List<UIRect> list = new List<UIRect>();
		List<bool> list2 = new List<bool>();
		foreach (Transform transform in haveUIRectObjects)
		{
			list2.Add(transform.gameObject.activeSelf);
			transform.gameObject.SetActive(true);
			list.Add(transform.GetComponent<UIRect>());
		}
		base.transform.GetComponent<UIRect>().ResetAndUpdateAnchors();
		foreach (UIRect uirect in list)
		{
			uirect.topAnchor.target = null;
			uirect.bottomAnchor.target = null;
			uirect.leftAnchor.target = null;
			uirect.rightAnchor.target = null;
			uirect.ResetAnchors();
		}
		for (int j = 0; j < haveUIRectObjects.Length; j++)
		{
			haveUIRectObjects[j].gameObject.SetActive(list2[j]);
		}
	}

	private Transform[] GetHaveUIRectObjects<T>(bool isRootThisObject, bool isIncludeRootObject, bool isIncludeThisObject)
	{
		Transform transform = base.transform;
		Transform root = transform.root;
		List<Transform> list = new List<Transform>();
		Transform[] array = Resources.FindObjectsOfTypeAll<Transform>();
		foreach (Transform transform2 in array)
		{
			if (isIncludeRootObject || !(transform2 == root))
			{
				if (isIncludeThisObject || !(transform2 == transform))
				{
					if (transform2.gameObject.hideFlags == HideFlags.None)
					{
						if (!(transform2.root != root))
						{
							if (isRootThisObject)
							{
								Transform parent = transform2.parent;
								bool flag = false;
								while (!(parent == null))
								{
									if (!(parent == transform))
									{
										if (!(parent == root))
										{
											parent = parent.parent;
											continue;
										}
										flag = true;
									}
									IL_E5:
									if (flag)
									{
										goto IL_10C;
									}
									goto IL_F1;
								}
								flag = true;
								goto IL_E5;
							}
							IL_F1:
							if (transform2.GetComponents<T>().Length != 0)
							{
								list.Add(transform2);
							}
						}
					}
				}
			}
			IL_10C:;
		}
		return list.ToArray();
	}
}
