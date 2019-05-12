using System;
using UnityEngine;

public sealed class GUISelectPanelItemList : GUISelectPanelViewPartsUD
{
	[SerializeField]
	private GameObject goUClip;

	[SerializeField]
	private GameObject goDClip;

	[SerializeField]
	private int ITEMLIST_SHOW_INTERVAL = 6;

	[SerializeField]
	private int ITEMLIST_FAST_SHOW_INTERVAL = 1;

	public int ItemListShowInterval { get; set; }

	protected override void Awake()
	{
		base.Awake();
		this.ItemListShowInterval = this.ITEMLIST_SHOW_INTERVAL;
	}

	public void UpdateBarrier()
	{
		if (base.EnableScroll)
		{
			if (this.goUClip != null && !this.goUClip.activeSelf)
			{
				this.goUClip.SetActive(true);
			}
			if (this.goDClip != null && !this.goDClip.activeSelf)
			{
				this.goDClip.SetActive(true);
			}
		}
		else
		{
			if (this.goUClip != null && this.goUClip.activeSelf)
			{
				this.goUClip.SetActive(false);
			}
			if (this.goDClip != null && this.goDClip.activeSelf)
			{
				this.goDClip.SetActive(false);
			}
		}
	}

	private void OnClickedPanel()
	{
		this.ItemListShowInterval = this.ITEMLIST_FAST_SHOW_INTERVAL;
	}
}
