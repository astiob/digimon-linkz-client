using System;
using System.Collections.Generic;
using UnityEngine;

public class GUIFaceIndicator : MonoBehaviour
{
	public static GUIFaceIndicator instance;

	[SerializeField]
	private BoxCollider plusButtonCollider;

	[SerializeField]
	private List<GameObject> goHideLocatorShowList;

	[SerializeField]
	private GameObject meatIcon;

	private List<Vector3> locatorPosList;

	private List<Vector3> locatorLocalPosList;

	private float hideLocatorPosX = 10000f;

	protected void Awake()
	{
		GUIFaceIndicator.instance = this;
		this.InitLocator();
	}

	private void InitLocator()
	{
		if (this.goHideLocatorShowList != null)
		{
			this.locatorPosList = new List<Vector3>();
			this.locatorLocalPosList = new List<Vector3>();
			for (int i = 0; i < this.goHideLocatorShowList.Count; i++)
			{
				this.locatorPosList.Add(this.goHideLocatorShowList[i].transform.position);
				this.locatorLocalPosList.Add(this.goHideLocatorShowList[i].transform.localPosition);
			}
		}
	}

	public void ShowLocator()
	{
		Vector3 localPosition = base.gameObject.transform.localPosition;
		localPosition.x = 0f;
		base.gameObject.transform.localPosition = localPosition;
		if (this.goHideLocatorShowList != null)
		{
			for (int i = 0; i < this.goHideLocatorShowList.Count; i++)
			{
				this.goHideLocatorShowList[i].transform.localPosition = this.locatorLocalPosList[i];
			}
		}
	}

	public void HideLocator(bool showList)
	{
		Vector3 localPosition = base.gameObject.transform.localPosition;
		localPosition.x = this.hideLocatorPosX;
		base.gameObject.transform.localPosition = localPosition;
		if (showList && this.goHideLocatorShowList != null)
		{
			for (int i = 0; i < this.goHideLocatorShowList.Count; i++)
			{
				this.goHideLocatorShowList[i].transform.position = this.locatorPosList[i];
			}
		}
	}

	public void EnableCollider(bool isEnable)
	{
		this.plusButtonCollider.enabled = isEnable;
	}

	public void ShowGUI()
	{
		float z = base.transform.localPosition.z;
		Vector3 localPosition = base.transform.localPosition;
		localPosition.z = z;
		base.transform.localPosition = localPosition;
		this.ShowLocator();
	}

	public GameObject GetMeatIcon()
	{
		return this.meatIcon;
	}
}
