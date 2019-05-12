using Quest;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GUISelectPanelS_DungeonR : GUISelectPanelBSPartsUD
{
	[SerializeField]
	private GameObject goTween;

	private AlphaTweener tweener;

	protected override void Awake()
	{
		base.Awake();
		this.tweener = this.goTween.GetComponent<AlphaTweener>();
	}

	protected override void Update()
	{
		base.Update();
		this.UpdateIcons();
	}

	public void AllBuild(List<QuestData.WorldDungeonData> dts)
	{
		base.InitBuild();
		this.partsCount = dts.Count;
		if (base.selectCollider != null)
		{
			GUISelectPanelBSPartsUD.PanelBuildData panelBuildData = base.CalcBuildData(1, this.partsCount, 1f, 1f);
			float num = panelBuildData.startY;
			float startX = panelBuildData.startX;
			int num2 = 0;
			int count = dts.Count;
			dts.Reverse();
			foreach (QuestData.WorldDungeonData worldDungeonData in dts)
			{
				GameObject gameObject = base.AddBuildPart();
				GUIListPartsS_DungeonR component = gameObject.GetComponent<GUIListPartsS_DungeonR>();
				if (component != null)
				{
					component.SetOriginalPos(new Vector3(startX, num, -5f));
					component.StageNum = (count - num2).ToString();
					component.WorldDungeonData = worldDungeonData;
					component.IsEventStage = ClassSingleton<QuestData>.Instance.ExistEvent(worldDungeonData.worldDungeonM.worldDungeonId);
					component.ShowGUI();
				}
				num -= panelBuildData.pitchH;
				num2++;
			}
			base.height = panelBuildData.lenH;
			base.InitMinMaxLocation(-1, 0f);
		}
	}

	private void UpdateIcons()
	{
		for (int i = 0; i < this.partObjs.Count; i++)
		{
			GUIListPartsS_DungeonR guilistPartsS_DungeonR = (GUIListPartsS_DungeonR)this.partObjs[i];
			if (guilistPartsS_DungeonR.gameObject.activeSelf && this.tweener != null)
			{
				float alphaValue = this.tweener.GetAlphaValue();
				if (guilistPartsS_DungeonR.ngTXT_TICKET_LEFT.text != string.Empty && guilistPartsS_DungeonR.ngTXT_NO_CONTINUE.text != string.Empty)
				{
					if (alphaValue > 0f)
					{
						guilistPartsS_DungeonR.ngTXT_TICKET_LEFT.alpha = alphaValue;
						guilistPartsS_DungeonR.ngTXT_NO_CONTINUE.alpha = 0f;
					}
					else
					{
						guilistPartsS_DungeonR.ngTXT_TICKET_LEFT.alpha = 0f;
						guilistPartsS_DungeonR.ngTXT_NO_CONTINUE.alpha = -alphaValue;
					}
				}
				else
				{
					guilistPartsS_DungeonR.ngTXT_TICKET_LEFT.alpha = 1f;
					guilistPartsS_DungeonR.ngTXT_NO_CONTINUE.alpha = 1f;
				}
			}
		}
	}
}
