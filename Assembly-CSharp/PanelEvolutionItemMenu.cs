using System;
using System.Collections.Generic;
using UnityEngine;

public class PanelEvolutionItemMenu : MonoBehaviour
{
	[SerializeField]
	private List<PartsEvolutionItemMenuBtn> btnList;

	public void OnTouchedMenuBtn(PartsEvolutionItemMenuBtn.TYPE type)
	{
		foreach (PartsEvolutionItemMenuBtn partsEvolutionItemMenuBtn in this.btnList)
		{
			if (partsEvolutionItemMenuBtn.type != type)
			{
				partsEvolutionItemMenuBtn.ChangeSelected(false);
			}
		}
	}
}
