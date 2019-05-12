using System;
using System.Collections.Generic;

namespace UnityEngine.Csv.Internal
{
	[Serializable]
	public class CsvColumnList
	{
		[SerializeField]
		private List<CsvValue> _value = new List<CsvValue>();

		public List<CsvValue> value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
			}
		}
	}
}
