using System;
using System.Collections.Generic;
using UnityEngine.TextReplacer;

namespace UnityEngine
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(UILabel))]
	public class UITextReplacer : MonoBehaviour
	{
		private UILabel _uiLabel;

		[SerializeField]
		private int _value;

		[SerializeField]
		private TextReplacerSlotPattern[] _patterns = new TextReplacerSlotPattern[]
		{
			new TextReplacerSlotPattern()
		};

		[SerializeField]
		private TextReplacerValue[] _values = new TextReplacerValue[0];

		private void Awake()
		{
			if (this != null && this._uiLabel == null)
			{
				this._uiLabel = base.GetComponent<UILabel>();
			}
		}

		private void OnEnable()
		{
			this.Apply();
		}

		public int value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
				this.ApplyString();
			}
		}

		public int PatternLength
		{
			get
			{
				return this._patterns.Length;
			}
		}

		public int ValuesLength
		{
			get
			{
				return this._values.Length;
			}
		}

		public TextReplacerValue GetValue(int index)
		{
			if (index >= this._values.Length)
			{
				throw new IndexOutOfRangeException();
			}
			return this._values[index];
		}

		public bool TryGetValue(int index, out TextReplacerValue result)
		{
			if (index >= this._values.Length)
			{
				result = default(TextReplacerValue);
				return false;
			}
			result = this._values[index];
			return true;
		}

		public void SetValue(int index, TextReplacerValue replacerValue)
		{
			if (index < 0)
			{
				return;
			}
			if (index < this._values.Length)
			{
				this._values[index] = replacerValue;
			}
			if (index >= this._values.Length)
			{
				List<TextReplacerValue> list = new List<TextReplacerValue>(this._values);
				for (int i = this._values.Length; i < index; i++)
				{
					if (i == index)
					{
						list.Add(replacerValue);
					}
					else
					{
						list.Add(default(TextReplacerValue));
					}
				}
			}
			this.Apply();
		}

		public void SetPatterns(params TextReplacerSlotPattern[] patterns)
		{
			this._patterns = patterns;
			this.Apply();
		}

		private string ApplyString()
		{
			return this._patterns[this._value].GetApplyString(this._values);
		}

		public void Apply()
		{
			this.Awake();
			if (this._patterns.Length <= 0)
			{
				return;
			}
			this._uiLabel.text = this.ApplyString();
			NGUITools.SetDirty(this._uiLabel);
		}
	}
}
