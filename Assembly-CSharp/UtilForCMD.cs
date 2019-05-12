using System;
using UnityEngine;

public class UtilForCMD : MonoBehaviour
{
	public bool UnderPrefabFolder;

	protected Transform myTransform;

	protected virtual void Awake()
	{
		this.myTransform = base.transform;
		this.SetParamToCMD();
	}

	public virtual void SetParamToCMD()
	{
	}

	protected void DisableCMD_CallBack(Transform tm)
	{
		GUICollider component = tm.gameObject.GetComponent<GUICollider>();
		if (component != null)
		{
			component.CallBackClass = base.gameObject;
		}
		foreach (object obj in tm)
		{
			Transform tm2 = (Transform)obj;
			this.DisableCMD_CallBack(tm2);
		}
	}

	protected CMD FindParentCMD()
	{
		CMD cmd = null;
		Transform parent = this.myTransform.parent;
		if (parent != null)
		{
			do
			{
				cmd = parent.gameObject.GetComponent<CMD>();
				if (cmd != null)
				{
					break;
				}
				parent = parent.parent;
			}
			while (!(parent == null));
		}
		return cmd;
	}
}
