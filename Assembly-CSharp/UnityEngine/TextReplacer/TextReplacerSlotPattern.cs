using System;

namespace UnityEngine.TextReplacer
{
	[Serializable]
	public class TextReplacerSlotPattern
	{
		[SerializeField]
		private TextReplacerSlot[] _slots = new TextReplacerSlot[]
		{
			default(TextReplacerSlot)
		};

		public TextReplacerSlotPattern()
		{
			this._slots = new TextReplacerSlot[]
			{
				TextReplacerSlot.GetEmptySlot()
			};
		}

		public TextReplacerSlotPattern(params TextReplacerSlot[] slots)
		{
			this._slots = slots;
		}

		public TextReplacerSlotPattern(params string[] pureStrings)
		{
			this._slots = new TextReplacerSlot[pureStrings.Length];
			for (int i = 0; i < pureStrings.Length; i++)
			{
				this._slots[i] = new TextReplacerSlot(pureStrings[i]);
			}
		}

		public string GetApplyString(TextReplacerValue[] valueList)
		{
			string text = string.Empty;
			foreach (TextReplacerSlot textReplacerSlot in this._slots)
			{
				TextReplacerSlotType type = textReplacerSlot.type;
				if (type != TextReplacerSlotType.PureString)
				{
					if (type == TextReplacerSlotType.Variable)
					{
						if (valueList != null && valueList.Length > textReplacerSlot.variableIndex)
						{
							text += valueList[textReplacerSlot.variableIndex].GetValueString(textReplacerSlot.overflowLength, textReplacerSlot.zeroPaddingLength, textReplacerSlot.overflowJoinString);
						}
						else
						{
							string text2 = text;
							text = string.Concat(new object[]
							{
								text2,
								"[",
								textReplacerSlot.variableIndex,
								"]"
							});
						}
					}
				}
				else
				{
					text += textReplacerSlot.stringValue;
				}
			}
			return text;
		}

		public int GetVariableLength()
		{
			int num = 0;
			foreach (TextReplacerSlot textReplacerSlot in this._slots)
			{
				if (textReplacerSlot.type == TextReplacerSlotType.Variable)
				{
					num++;
				}
			}
			return num;
		}
	}
}
