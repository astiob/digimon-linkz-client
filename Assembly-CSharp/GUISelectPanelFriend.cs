using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUISelectPanelFriend : GUISelectPanelBSPartsUD
{
	private bool isAllBuild;

	private bool isLoading;

	protected override void Awake()
	{
		base.Awake();
	}

	protected override void Update()
	{
		if (!this.isAllBuild || this.isLoading)
		{
			return;
		}
		base.Update();
	}

	public IEnumerator AllBuild(List<GameWebAPI.FriendList> dts, float x = 0f, float afterWaitTime = 0f, Action cb = null)
	{
		this.isAllBuild = true;
		this.isLoading = true;
		base.InitBuild();
		this.partsCount = dts.Count;
		if (base.selectCollider != null)
		{
			GUISelectPanelBSPartsUD.PanelBuildData pbd = base.CalcBuildData(1, this.partsCount, 1f, 1f);
			float ypos = pbd.startY;
			int index = 0;
			foreach (GameWebAPI.FriendList dt in dts)
			{
				GameObject part = base.AddBuildPart();
				GUIListPartsFriend cpart = part.GetComponent<GUIListPartsFriend>();
				if (cpart != null)
				{
					cpart.SetOriginalPos(new Vector3(x, ypos, -5f));
					cpart.Data = dt;
				}
				part.SetActive(false);
				index++;
				ypos -= pbd.pitchH;
				if (index % 10 == 0)
				{
					yield return null;
				}
			}
			base.height = pbd.lenH;
			base.InitMinMaxLocation(-1, 0f);
		}
		if (cb != null)
		{
			cb();
		}
		if (afterWaitTime > 0f)
		{
			yield return new WaitForSeconds(afterWaitTime);
		}
		this.isLoading = false;
		yield break;
	}
}
