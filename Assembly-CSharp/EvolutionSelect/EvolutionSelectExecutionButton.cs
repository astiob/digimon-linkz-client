using Master;
using System;
using UnityEngine;

namespace EvolutionSelect
{
	public class EvolutionSelectExecutionButton : GUICollider
	{
		[SerializeField]
		private GUIListPartsEvolution listPartsRoot;

		[SerializeField]
		private UILabel buttonText;

		private void ShowFailedLockMonster()
		{
			CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage") as CMD_ModalMessage;
			cmd_ModalMessage.Title = StringMaster.GetString("EvolutionTitle");
			string arg = string.Empty;
			if ("2" != this.listPartsRoot.GetEvotuionType())
			{
				arg = StringMaster.GetString("EvolutionTitle");
			}
			else
			{
				arg = StringMaster.GetString("EvolutionModeChange");
			}
			cmd_ModalMessage.Info = string.Format(StringMaster.GetString("EvolutionFailedLock"), arg);
		}

		private void ShowConfirmExecution(string title, string info)
		{
			CMD_Confirm cmd_Confirm = GUIMain.ShowCommonDialog(new Action<int>(this.listPartsRoot.OnCloseEvolveDo), "CMD_Confirm") as CMD_Confirm;
			cmd_Confirm.Title = title;
			cmd_Confirm.Info = info;
		}

		private void OnPushedButton()
		{
			MonsterData md = this.listPartsRoot.Data.md;
			if (md.userMonster.IsLocked)
			{
				this.ShowFailedLockMonster();
			}
			else if (!this.listPartsRoot.IsPossessCluster())
			{
				CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage") as CMD_ModalMessage;
				cmd_ModalMessage.Title = StringMaster.GetString("EvolutionTitle");
				cmd_ModalMessage.Info = StringMaster.GetString("EvolutionFailedTip");
			}
			else if ("2" == this.listPartsRoot.GetEvotuionType())
			{
				this.ShowConfirmExecution(StringMaster.GetString("EvolutionModeChange"), StringMaster.GetString("EvolutionConfirmModeChange"));
			}
			else
			{
				int num = int.Parse(md.userMonster.level);
				int num2 = int.Parse(md.monsterM.maxLevel);
				if (num >= num2)
				{
					this.ShowConfirmExecution(StringMaster.GetString("EvolutionConfirmTitle"), StringMaster.GetString("EvolutionConfirmInfo"));
				}
				else
				{
					CMD_ModalMessage cmd_ModalMessage2 = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage") as CMD_ModalMessage;
					cmd_ModalMessage2.Title = StringMaster.GetString("EvolutionTitle");
					cmd_ModalMessage2.Info = string.Format(StringMaster.GetString("EvolutionFailedLv"), StringMaster.GetString("EvolutionTitle"));
				}
			}
		}

		public void Initialize()
		{
			string evotuionType = this.listPartsRoot.GetEvotuionType();
			if ("2" == evotuionType)
			{
				this.buttonText.text = StringMaster.GetString("EvolutionModeChangeButton");
				this.buttonText.fontSize = 16;
			}
			else
			{
				this.buttonText.text = StringMaster.GetString("EvolutionTitle");
			}
		}

		public void SetImpossibleButton()
		{
			UISprite component = base.GetComponent<UISprite>();
			if (null != component)
			{
				component.spriteName = "Common02_Btn_Gray";
			}
			this.buttonText.color = new Color(0.6f, 0.6f, 0.6f, 1f);
			BoxCollider component2 = base.GetComponent<BoxCollider>();
			if (null != component2)
			{
				component2.enabled = false;
			}
		}
	}
}
