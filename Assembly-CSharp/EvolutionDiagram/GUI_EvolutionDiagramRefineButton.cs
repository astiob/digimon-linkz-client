using Monster;
using System;
using UnityEngine;

namespace EvolutionDiagram
{
	public class GUI_EvolutionDiagramRefineButton : GUICollider
	{
		[SerializeField]
		private UI_EvolutionDiagramRefineButtonList buttonListRoot;

		[SerializeField]
		private UILabel label;

		[SerializeField]
		private UISprite buttonSprite;

		[SerializeField]
		private GrowStep[] growList;

		[SerializeField]
		private string grayOutSpriteName;

		private string selectedSpriteName;

		private Color grayOutTextColor;

		private BoxCollider buttonCollider;

		private void OnPushedButton()
		{
			if (0 < this.growList.Length)
			{
				this.buttonListRoot.SetSingleGrowData(this.growList[0]);
			}
			this.buttonListRoot.SetButtonState(this);
		}

		public void Initialize()
		{
			float num = 0.227450982f;
			this.grayOutTextColor = new Color(num, num, num);
			this.selectedSpriteName = this.buttonSprite.spriteName;
			this.buttonCollider = base.gameObject.GetComponent<BoxCollider>();
			if (0 < this.growList.Length && this.growList[0] != GrowStep.NONE)
			{
				this.label.text = string.Empty;
				for (int i = 0; i < this.growList.Length; i++)
				{
					int num2 = (int)this.growList[i];
					if (string.IsNullOrEmpty(this.label.text))
					{
						this.label.text = MonsterGrowStepData.GetGrowStepName(num2.ToString());
					}
					else
					{
						UILabel uilabel = this.label;
						uilabel.text = uilabel.text + "\n" + MonsterGrowStepData.GetGrowStepName(num2.ToString());
					}
				}
			}
		}

		public GrowStep GetGrowStep()
		{
			return this.growList[0];
		}

		public void SetGrayOut()
		{
			this.label.color = this.grayOutTextColor;
			this.buttonSprite.spriteName = this.grayOutSpriteName;
			this.buttonCollider.enabled = true;
		}

		public void SetSelected()
		{
			this.label.color = Color.white;
			this.buttonSprite.spriteName = this.selectedSpriteName;
			this.buttonCollider.enabled = false;
		}
	}
}
