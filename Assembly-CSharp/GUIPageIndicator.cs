using System;
using System.Collections.Generic;
using UnityEngine;

public class GUIPageIndicator : MonoBehaviour
{
	private const float PARTS_PITCH = 30f;

	[Header("●のゲームオブジェクトのNGUIスプライトの指定")]
	[SerializeField]
	private UISprite spParts;

	[Header("オンの時のスプライト名")]
	[SerializeField]
	private string onSpriteName;

	[Header("オフの時のスプライト名")]
	[SerializeField]
	private string offSpriteName;

	private int nowIdx;

	private List<UISprite> NGSprPartsList;

	protected void Awake()
	{
		this.NGSprPartsList = new List<UISprite>();
	}

	public void SetUp(int max, int idx, bool isLeftStart = true, float pitch = 30f)
	{
		float num = pitch * (float)(max - 1);
		float num2;
		float num3;
		if (isLeftStart)
		{
			num2 = -num / 2f;
			num3 = pitch;
		}
		else
		{
			num2 = num / 2f;
			num3 = -pitch;
		}
		for (int i = 0; i < max; i++)
		{
			if (this.spParts != null)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.spParts.gameObject);
				if (gameObject != null)
				{
					gameObject.transform.parent = base.gameObject.transform;
					gameObject.transform.localPosition = new Vector3(num2, 0f, 0f);
					UISprite component = gameObject.GetComponent<UISprite>();
					if (component != null)
					{
						this.NGSprPartsList.Add(component);
					}
				}
			}
			num2 += num3;
		}
		this.spParts.gameObject.SetActive(false);
		this.nowIdx = -1;
		this.Refresh(idx);
	}

	public void Refresh(int idx)
	{
		if (idx != this.nowIdx)
		{
			this.nowIdx = idx;
			string empty = string.Empty;
			for (int i = 0; i < this.NGSprPartsList.Count; i++)
			{
				if (i == this.nowIdx)
				{
					empty = this.onSpriteName;
				}
				else
				{
					empty = this.offSpriteName;
				}
				this.NGSprPartsList[i].spriteName = empty;
			}
		}
	}
}
