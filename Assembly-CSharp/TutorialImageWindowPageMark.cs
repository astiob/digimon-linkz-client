using System;
using System.Collections.Generic;
using UnityEngine;

public class TutorialImageWindowPageMark : MonoBehaviour
{
	[SerializeField]
	private Color color;

	[SerializeField]
	private Color glayColor;

	[SerializeField]
	private GameObject[] pageMarks;

	[SerializeField]
	private UIGrid grid;

	public void Initialize(int pageNum)
	{
		for (int i = 0; i < Mathf.Clamp(pageNum - this.pageMarks.Length, 0, 2147483647); i++)
		{
			this.AddPageMarks();
		}
		for (int j = 0; j < this.pageMarks.Length; j++)
		{
			GameObject gameObject = this.pageMarks[j];
			if (j < pageNum)
			{
				gameObject.SetActive(true);
			}
			else
			{
				gameObject.SetActive(false);
			}
		}
		this.grid.Reposition();
		this.SetActiveMark(0);
	}

	private void AddPageMarks()
	{
		GameObject gameObject = this.pageMarks[0];
		List<GameObject> list = new List<GameObject>(this.pageMarks);
		GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
		gameObject2.transform.SetParent(gameObject.transform.parent);
		gameObject2.transform.localScale = gameObject.transform.localScale;
		gameObject2.SetActive(false);
		gameObject2.name = "Mark" + (this.pageMarks.Length + 1);
		list.Add(gameObject2);
		this.pageMarks = list.ToArray();
	}

	public void StartDisplay()
	{
		UIPlayTween component = base.GetComponent<UIPlayTween>();
		component.Play(true);
	}

	public void StartInvisible()
	{
		UIPlayTween component = base.GetComponent<UIPlayTween>();
		component.Play(false);
	}

	public void SetActiveMark(int pageIndex)
	{
		for (int i = 0; i < this.pageMarks.Length; i++)
		{
			if (this.pageMarks[i].activeSelf)
			{
				UISprite component = this.pageMarks[i].GetComponent<UISprite>();
				component.color = this.glayColor;
			}
		}
		if (this.pageMarks[pageIndex].activeSelf)
		{
			UISprite component2 = this.pageMarks[pageIndex].GetComponent<UISprite>();
			component2.color = this.color;
		}
	}
}
