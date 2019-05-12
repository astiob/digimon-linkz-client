using Master;
using System;

namespace CharacterDetailsUI
{
	public sealed class CharacterDetailsProtection
	{
		private string dialogErrorText;

		public void SetErrorText(CMD_CharacterDetailed.LockMode mode)
		{
			switch (mode)
			{
			case CMD_CharacterDetailed.LockMode.Laboratory:
				if (CMD_PairSelectBase.instance.baseDigimon != null && CMD_PairSelectBase.instance.baseDigimon == CMD_CharacterDetailed.DataChg)
				{
					this.SetErrorText(StringMaster.GetString("CharaDetailsNotLockBase"));
				}
				else if (CMD_PairSelectBase.instance.partnerDigimon != null && CMD_PairSelectBase.instance.partnerDigimon == CMD_CharacterDetailed.DataChg)
				{
					this.SetErrorText(StringMaster.GetString("CharaDetailsNotLockPartner"));
				}
				else if (CMD_BaseSelect.ElementType == CMD_BaseSelect.ELEMENT_TYPE.BASE)
				{
					this.SetErrorText(StringMaster.GetString("CharaDetailsNotLockBase"));
				}
				else if (CMD_BaseSelect.ElementType == CMD_BaseSelect.ELEMENT_TYPE.PARTNER)
				{
					this.SetErrorText(StringMaster.GetString("CharaDetailsNotLockPartner"));
				}
				break;
			case CMD_CharacterDetailed.LockMode.Farewell:
				this.SetErrorText(StringMaster.GetString("CharaDetailsNotLockSale"));
				break;
			case CMD_CharacterDetailed.LockMode.Reinforcement:
			case CMD_CharacterDetailed.LockMode.Succession:
			case CMD_CharacterDetailed.LockMode.Arousal:
				this.SetErrorText(StringMaster.GetString("CharaDetailsNotLockPartner"));
				break;
			case CMD_CharacterDetailed.LockMode.Evolution:
				this.SetErrorText(StringMaster.GetString("CharaDetailsNotLockBase"));
				break;
			}
		}

		public void SetErrorText(string text)
		{
			this.dialogErrorText = text;
		}

		public string GetErrorText()
		{
			return this.dialogErrorText;
		}
	}
}
