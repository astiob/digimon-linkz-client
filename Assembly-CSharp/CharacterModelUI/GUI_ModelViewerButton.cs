using Master;
using System;
using UnityEngine;

namespace CharacterModelUI
{
	[RequireComponent(typeof(UISprite))]
	[RequireComponent(typeof(BoxCollider))]
	public sealed class GUI_ModelViewerButton : GUICollider
	{
		[SerializeField]
		private UILabel label;

		private UISprite background;

		private BoxCollider buttonCollider;

		protected override void Awake()
		{
			base.Awake();
			this.buttonCollider = base.GetComponent<BoxCollider>();
			this.background = base.GetComponent<UISprite>();
		}

		public void SetOpenButton()
		{
			this.label.text = StringMaster.GetString("CharaDetailsFullScreen");
		}

		public void SetCloseButton()
		{
			this.label.text = StringMaster.GetString("SystemButtonReturn");
		}

		public void SetEnable(bool enable)
		{
			this.buttonCollider.enabled = enable;
			if (enable)
			{
				this.background.spriteName = "Common02_Btn_BaseON";
			}
			else
			{
				this.background.spriteName = "Common02_Btn_BaseOFF";
			}
		}
	}
}
