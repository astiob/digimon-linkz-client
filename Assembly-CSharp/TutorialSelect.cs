using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class TutorialSelect : MonoBehaviour
{
	[SerializeField]
	private List<TutorialSelect.SelectItemInfo> selectItemInfo;

	public void SetSelectItem(int id, string selectText, Action onSelected)
	{
		if (this.selectItemInfo != null)
		{
			try
			{
				this.selectItemInfo.Single((TutorialSelect.SelectItemInfo x) => x.id == id).item.SetInfo(selectText, delegate
				{
					this.StartInvisible();
					if (onSelected != null)
					{
						onSelected();
					}
				});
			}
			catch
			{
				global::Debug.LogErrorFormat("SetSelectItem : ID NOT FOUND = {0}", new object[]
				{
					id
				});
			}
		}
	}

	public void StartDisplay()
	{
		for (int i = 0; i < this.selectItemInfo.Count; i++)
		{
			TutorialSelectItem item = this.selectItemInfo[i].item;
			if (item.IsSettings)
			{
				TweenAlpha component = item.GetComponent<TweenAlpha>();
				component.PlayForward();
			}
		}
	}

	private void StartInvisible()
	{
		for (int i = 0; i < this.selectItemInfo.Count; i++)
		{
			TutorialSelectItem item = this.selectItemInfo[i].item;
			if (item.IsSettings)
			{
				item.IsSettings = false;
				BoxCollider component = item.GetComponent<BoxCollider>();
				component.enabled = false;
				TweenAlpha component2 = item.GetComponent<TweenAlpha>();
				component2.PlayReverse();
			}
		}
	}

	[Serializable]
	public struct SelectItemInfo
	{
		[SerializeField]
		public int id;

		[SerializeField]
		public TutorialSelectItem item;
	}
}
