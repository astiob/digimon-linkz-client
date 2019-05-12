using Quest;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GUISelectPanelPartyEdit : GUISelectPanelBSPartsLR
{
	public List<GUIListPartsPartyEdit> fastSetPartObjs;

	public Action popupCallback { get; set; }

	public void AllBuild(List<GameWebAPI.RespDataMN_GetDeckList.DeckList> dts, CMD_PartyEdit partyEdit, QuestBonusPack questBonus, QuestBonusTargetCheck checker)
	{
		base.InitBuild();
		this.fastSetPartObjs.Clear();
		this.partsCount = dts.Count;
		if (base.selectCollider != null)
		{
			float num = base.selectCollider.width + this.horizontalMargin;
			float num2 = (float)this.partsCount * num - this.horizontalMargin + this.horizontalBorder * 2f;
			float num3 = num2 / 2f - this.horizontalBorder - base.selectCollider.width / 2f;
			float y = 0f;
			for (int i = dts.Count - 1; i >= 0; i--)
			{
				GameObject gameObject = base.AddBuildPart();
				gameObject.name += i.ToString();
				GUIListPartsPartyEdit component = gameObject.GetComponent<GUIListPartsPartyEdit>();
				if (component != null)
				{
					component.SetOriginalPos(new Vector3(num3, y, -5f));
					component.SetDeck(dts[i]);
					component.partyEdit = partyEdit;
					component.selectPanelParty = this;
					component.SetUI(questBonus, checker);
					component.ShowGUI();
					int partyNumber = int.Parse(dts[i].deckNum);
					component.partyNumber = partyNumber;
					this.fastSetPartObjs.Add(component);
				}
				num3 -= num;
			}
			base.width = num2;
			base.InitMinMaxLocation(true);
			base.FreeScrollMode = false;
		}
		this.arrowOffestX = 504f;
		base.EnableEternalScroll = true;
	}

	public void ReloadAllCharacters(bool flg)
	{
		for (int i = 0; i < this.partObjs.Count; i++)
		{
			GUIListPartsPartyEdit guilistPartsPartyEdit = (GUIListPartsPartyEdit)this.partObjs[i];
			guilistPartsPartyEdit.ReloadAllCharacters(flg);
		}
	}
}
