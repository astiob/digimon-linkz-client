using System;
using UnityEngine;

public class TitleListItem : GUIListPartBS
{
	private int titleId;

	private bool equiped;

	private bool owned;

	[SerializeField]
	private GameObject equipedIcon;

	[SerializeField]
	private GameObject selectedEffect;

	private Action<TitleListItem> onSelectTitle;

	public void SetDetail(GameWebAPI.RespDataMA_TitleMaster.TitleM titleMaster, bool owned, bool equiped, Action<TitleListItem> touchEvent)
	{
		this.titleId = int.Parse(titleMaster.titleId);
		this.owned = owned;
		this.equiped = equiped;
		this.onSelectTitle = touchEvent;
		this.equipedIcon.SetActive(this.equiped);
		TitleDataMng.SetTitleIcon(titleMaster.titleId, base.GetComponent<UITexture>());
		if (!this.owned)
		{
			base.GetComponent<UITexture>().color = Util.convertColor(109f, 109f, 109f, 255f);
		}
	}

	public void OnSelectTitle()
	{
		this.selectedEffect.SetActive(false);
		this.selectedEffect.transform.localPosition = base.transform.localPosition;
		this.selectedEffect.SetActive(true);
		if (this.onSelectTitle != null)
		{
			this.onSelectTitle(this);
		}
	}

	public void equip()
	{
		this.equiped = true;
		this.equipedIcon.SetActive(this.equiped);
	}

	public void unequip()
	{
		this.equiped = false;
		this.equipedIcon.SetActive(this.equiped);
	}

	public int GetTitleId()
	{
		return this.titleId;
	}

	public bool GetOwned()
	{
		return this.owned;
	}

	public bool GetEquiped()
	{
		return this.equiped;
	}
}
