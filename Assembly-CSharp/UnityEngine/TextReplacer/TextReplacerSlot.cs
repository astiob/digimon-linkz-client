using System;
using UnityEngine.Serialization;

namespace UnityEngine.TextReplacer
{
	[Serializable]
	public struct TextReplacerSlot
	{
		public const string defaultOverflowJoinString = "...";

		public static int defaultOverflowLength = int.MaxValue;

		public static int defaulZeroPaddingLength = 1;

		[SerializeField]
		private TextReplacerSlotType _type;

		[SerializeField]
		private int _variableIndex;

		[SerializeField]
		private string _stringValue;

		[SerializeField]
		[FormerlySerializedAs("_lengthSwichingValue")]
		private int _overflowLength;

		[FormerlySerializedAs("_lengthSwichingValue")]
		[SerializeField]
		private int _zeroPaddingLength;

		[SerializeField]
		private string _overflowJoinString;

		public TextReplacerSlot(string pureText)
		{
			this._type = TextReplacerSlotType.PureString;
			this._stringValue = pureText;
			this._variableIndex = 0;
			this._overflowLength = TextReplacerSlot.defaultOverflowLength;
			this._zeroPaddingLength = TextReplacerSlot.defaulZeroPaddingLength;
			this._overflowJoinString = "...";
		}

		public TextReplacerSlot(int varIndex)
		{
			this._type = TextReplacerSlotType.Variable;
			this._stringValue = string.Empty;
			this._variableIndex = varIndex;
			this._overflowLength = TextReplacerSlot.defaultOverflowLength;
			this._zeroPaddingLength = TextReplacerSlot.defaulZeroPaddingLength;
			this._overflowJoinString = "...";
		}

		public TextReplacerSlot(int varIndex, int overflowLength)
		{
			this._type = TextReplacerSlotType.Variable;
			this._stringValue = string.Empty;
			this._variableIndex = varIndex;
			this._overflowLength = Mathf.Clamp(overflowLength, 0, int.MaxValue);
			this._zeroPaddingLength = TextReplacerSlot.defaulZeroPaddingLength;
			this._overflowJoinString = "...";
		}

		public TextReplacerSlot(int varIndex, int overflowLength, int zeroPaddingLength)
		{
			this._type = TextReplacerSlotType.Variable;
			this._stringValue = string.Empty;
			this._variableIndex = varIndex;
			this._overflowLength = Mathf.Clamp(overflowLength, 0, int.MaxValue);
			this._zeroPaddingLength = Mathf.Clamp(zeroPaddingLength, 1, int.MaxValue);
			this._overflowJoinString = "...";
		}

		public TextReplacerSlot(int varIndex, int overflowLength, string overflowJoinString)
		{
			this._type = TextReplacerSlotType.Variable;
			this._stringValue = string.Empty;
			this._variableIndex = varIndex;
			this._overflowLength = Mathf.Clamp(overflowLength, 0, int.MaxValue);
			this._zeroPaddingLength = TextReplacerSlot.defaulZeroPaddingLength;
			this._overflowJoinString = overflowJoinString;
		}

		public static TextReplacerSlot GetEmptySlot()
		{
			return new TextReplacerSlot(string.Empty);
		}

		public TextReplacerSlotType type
		{
			get
			{
				return this._type;
			}
		}

		public int variableIndex
		{
			get
			{
				return this._variableIndex;
			}
		}

		public string stringValue
		{
			get
			{
				return this._stringValue;
			}
		}

		public int zeroPaddingLength
		{
			get
			{
				return this._zeroPaddingLength;
			}
		}

		public int overflowLength
		{
			get
			{
				return this._overflowLength;
			}
		}

		public string overflowJoinString
		{
			get
			{
				return this._overflowJoinString;
			}
		}
	}
}
