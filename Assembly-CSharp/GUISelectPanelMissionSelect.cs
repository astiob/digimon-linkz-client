using System;
using UnityEngine;

public class GUISelectPanelMissionSelect : GUISelectPanelViewPartsUD
{
	public bool animationMoving;

	[Header("選択されたパーツのアニメ量 X")]
	[SerializeField]
	private float selectPartsAnimX = 25f;

	private int animIndexBK = -1;

	public void ResetAnimIDX()
	{
		this.animIndexBK = -1;
	}

	public void ResetColorCurIDX()
	{
		GameObject gameObject = this.partObjs[this.animIndexBK].csParts.gameObject;
		GUIListPartsMissionSelect component = gameObject.GetComponent<GUIListPartsMissionSelect>();
		if (component != null)
		{
			component.SetBGColor(false);
		}
	}

	public bool SetCellAnim(int selectedIndex, bool resetNew = true)
	{
		if (selectedIndex == this.animIndexBK)
		{
			return false;
		}
		if (this.partObjs[selectedIndex].csParts == null)
		{
			return false;
		}
		GUIListPartsMissionSelect guilistPartsMissionSelect = (GUIListPartsMissionSelect)this.partObjs[selectedIndex].csParts;
		if (guilistPartsMissionSelect != null)
		{
			if (resetNew)
			{
				guilistPartsMissionSelect.ResetNew();
			}
			this.animationMoving = true;
			GUICollider.DisableAllCollider("GUISelectPanelMissionSelect::SetCellAnim");
			GameObject gameObject = this.partObjs[selectedIndex].csParts.gameObject;
			if (gameObject.activeSelf)
			{
				iTween.MoveTo(gameObject, iTween.Hash(new object[]
				{
					"x",
					this.selectPartsAnimX,
					"time",
					0.4,
					"islocal",
					true
				}));
			}
			else
			{
				gameObject.transform.localPosition = new Vector3(this.selectPartsAnimX, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
			}
			guilistPartsMissionSelect.SetBGColor(true);
			if (this.animIndexBK != -1 && this.partObjs[this.animIndexBK].csParts != null)
			{
				GameObject gameObject2 = this.partObjs[this.animIndexBK].csParts.gameObject;
				GUIListPartsMissionSelect component = gameObject2.GetComponent<GUIListPartsMissionSelect>();
				if (component != null)
				{
					component.SetBGColor(false);
				}
				if (gameObject2.activeSelf)
				{
					iTween.MoveTo(gameObject2, iTween.Hash(new object[]
					{
						"x",
						0,
						"time",
						0.4,
						"islocal",
						true,
						"oncomplete",
						"complete",
						"oncompletetarget",
						base.gameObject
					}));
				}
				else
				{
					gameObject2.transform.localPosition = new Vector3(0f, gameObject2.transform.localPosition.y, gameObject2.transform.localPosition.z);
					base.Invoke("complete", 0.1f);
				}
			}
			else
			{
				base.Invoke("complete", 0.1f);
			}
			this.animIndexBK = selectedIndex;
			return true;
		}
		return false;
	}

	public void SetCellAnimReserve(int selectedIndex, bool resetNew = true)
	{
		if (this.partObjs[selectedIndex].csParts != null)
		{
			GUIListPartBS csParts = this.partObjs[selectedIndex].csParts;
			csParts.SetFadeInEndCallBack(delegate
			{
				this.SetCellAnim(selectedIndex, resetNew);
			});
		}
	}

	public void RefreshBadge()
	{
		for (int i = 0; i < this.partObjs.Count; i++)
		{
			if (this.partObjs[i].csParts != null)
			{
				GUIListPartsMissionSelect guilistPartsMissionSelect = (GUIListPartsMissionSelect)this.partObjs[i].csParts;
				if (guilistPartsMissionSelect != null)
				{
					guilistPartsMissionSelect.RefreshBadge();
				}
			}
		}
	}

	private void complete()
	{
		base.Invoke("completeMoving", 0.1f);
	}

	private void completeMoving()
	{
		this.animationMoving = false;
		GUICollider.EnableAllCollider("GUISelectPanelMissionSelect::SetCellAnim_completeMoving");
	}
}
