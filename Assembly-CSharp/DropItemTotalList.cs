using System;
using System.Collections.Generic;
using UnityEngine;

public class DropItemTotalList
{
	private const int MAX_WIDTH = 5;

	private const int MAX_HEIGHT = 2;

	private GameObject gameObject;

	private List<DropItemTotalParts> partsList;

	public DropItemTotalList(GameObject parent, DropItemTotalParts.Data[] datas)
	{
		this.gameObject = new GameObject();
		this.gameObject.name = "DropItemTotalListBase";
		this.gameObject.transform.SetParent(parent.transform);
		this.gameObject.transform.localPosition = Vector3.zero;
		this.gameObject.transform.localScale = Vector3.one;
		this.partsList = new List<DropItemTotalParts>();
		for (int i = 0; i < 2; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				int num = 5 * i + j;
				if (num >= datas.Length)
				{
					return;
				}
				GameObject gameObject = GUIManager.LoadCommonGUI("ListParts/ListParts_dropTotal", this.gameObject);
				gameObject.transform.SetParent(this.gameObject.transform);
				Vector3 a = new Vector3(-464f, -160f, 0f);
				Vector3 b = new Vector3(200f * (float)j, -60f * (float)i, 0f);
				gameObject.transform.localPosition = a + b;
				gameObject.transform.localScale = Vector3.one;
				DropItemTotalParts component = gameObject.GetComponent<DropItemTotalParts>();
				component.SetData(datas[num]);
				this.partsList.Add(component);
			}
		}
	}

	public void SetActive(bool value)
	{
		this.gameObject.SetActive(value);
	}

	public void SetPosition(Vector3 position)
	{
		this.gameObject.transform.localPosition = position;
	}
}
