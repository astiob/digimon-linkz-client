using System;
using UnityEngine;

namespace CharacterMiniStatusUI
{
	public sealed class UI_SkillPanelSwitchButton : MonoBehaviour
	{
		[SerializeField]
		private StatusPanelViewControl switchPanel;

		private void OnEnable()
		{
			this.switchPanel.Initialize();
			UISprite component = base.GetComponent<UISprite>();
			component.flip = UIBasicSprite.Flip.Nothing;
		}

		private void OnPushSwitchButton()
		{
			this.switchPanel.SetNextPage();
			UISprite component = base.GetComponent<UISprite>();
			if (component.flip != UIBasicSprite.Flip.Vertically)
			{
				component.flip = UIBasicSprite.Flip.Vertically;
			}
			else
			{
				component.flip = UIBasicSprite.Flip.Nothing;
			}
		}
	}
}
