using System;
using UnityEngine.TextReplacer;

namespace UnityEngine
{
	[Serializable]
	public struct TextReplacerValue
	{
		[SerializeField]
		private TextReplacerValueType _valueType;

		[SerializeField]
		private int _intValue;

		[SerializeField]
		private float _floatValue;

		[SerializeField]
		private string _stringValue;

		public TextReplacerValue(int value)
		{
			this._valueType = TextReplacerValueType.Int;
			this._intValue = value;
			this._floatValue = 0f;
			this._stringValue = string.Empty;
		}

		public TextReplacerValue(float value)
		{
			this._valueType = TextReplacerValueType.Float;
			this._intValue = 0;
			this._floatValue = value;
			this._stringValue = string.Empty;
		}

		public TextReplacerValue(string value)
		{
			this._valueType = TextReplacerValueType.String;
			this._intValue = 0;
			this._floatValue = 0f;
			this._stringValue = value;
		}

		public TextReplacerValueType valueType
		{
			get
			{
				return this._valueType;
			}
		}

		public int intValue
		{
			get
			{
				return this._intValue;
			}
			set
			{
				this._intValue = value;
			}
		}

		public float floatValue
		{
			get
			{
				return this._floatValue;
			}
			set
			{
				this._floatValue = value;
			}
		}

		public string stringValue
		{
			get
			{
				return this._stringValue;
			}
			set
			{
				this._stringValue = value;
			}
		}

		public string GetValueString(int overflowLength, int zeroPadding, string overflowJoinString)
		{
			TextReplacerValueType valueType = this._valueType;
			if (valueType == TextReplacerValueType.Int)
			{
				return this.GetOverflowClampString(this.GetZeroPaddingString(this.intValue.ToString(), zeroPadding), overflowLength, overflowJoinString);
			}
			if (valueType != TextReplacerValueType.Float)
			{
				return this.GetOverflowClampString(this.stringValue, overflowLength, overflowJoinString);
			}
			return this.GetOverflowClampString(this.GetZeroPaddingString(this.floatValue.ToString(), zeroPadding), overflowLength, overflowJoinString);
		}

		private string GetZeroPaddingString(string baseText, int zeroPadding)
		{
			string text = baseText;
			int num = Mathf.Clamp(baseText.Split(new char[]
			{
				'.'
			})[0].Length - zeroPadding, int.MinValue, 0) * -1;
			for (int i = 0; i < num; i++)
			{
				text = "0" + text;
			}
			return text;
		}

		private string GetOverflowClampString(string baseText, int overflowLength, string overflowJoinString)
		{
			if (overflowLength == -1)
			{
				return baseText;
			}
			if (baseText.Length <= overflowLength)
			{
				return baseText;
			}
			string str = baseText.Remove(overflowLength, baseText.Length - overflowLength);
			return str + overflowJoinString;
		}
	}
}
