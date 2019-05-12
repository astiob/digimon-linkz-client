using System;
using System.Globalization;
using System.Text;

namespace System.Xml.Serialization
{
	internal class EnumMap : ObjectMap
	{
		private readonly EnumMap.EnumMapMember[] _members;

		private readonly bool _isFlags;

		private readonly string[] _enumNames;

		private readonly string[] _xmlNames;

		private readonly long[] _values;

		public EnumMap(EnumMap.EnumMapMember[] members, bool isFlags)
		{
			this._members = members;
			this._isFlags = isFlags;
			this._enumNames = new string[this._members.Length];
			this._xmlNames = new string[this._members.Length];
			this._values = new long[this._members.Length];
			for (int i = 0; i < this._members.Length; i++)
			{
				EnumMap.EnumMapMember enumMapMember = this._members[i];
				this._enumNames[i] = enumMapMember.EnumName;
				this._xmlNames[i] = enumMapMember.XmlName;
				this._values[i] = enumMapMember.Value;
			}
		}

		public bool IsFlags
		{
			get
			{
				return this._isFlags;
			}
		}

		public EnumMap.EnumMapMember[] Members
		{
			get
			{
				return this._members;
			}
		}

		public string[] EnumNames
		{
			get
			{
				return this._enumNames;
			}
		}

		public string[] XmlNames
		{
			get
			{
				return this._xmlNames;
			}
		}

		public long[] Values
		{
			get
			{
				return this._values;
			}
		}

		public string GetXmlName(string typeName, object enumValue)
		{
			if (enumValue is string)
			{
				throw new InvalidCastException();
			}
			long num = 0L;
			try
			{
				num = ((IConvertible)enumValue).ToInt64(CultureInfo.CurrentCulture);
			}
			catch (FormatException)
			{
				throw new InvalidCastException();
			}
			for (int i = 0; i < this.Values.Length; i++)
			{
				if (this.Values[i] == num)
				{
					return this.XmlNames[i];
				}
			}
			if (this.IsFlags && num == 0L)
			{
				return string.Empty;
			}
			string text = string.Empty;
			if (this.IsFlags)
			{
				text = XmlCustomFormatter.FromEnum(num, this.XmlNames, this.Values, typeName);
			}
			if (text.Length == 0)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "'{0}' is not a valid value for {1}.", new object[]
				{
					num,
					typeName
				}));
			}
			return text;
		}

		public string GetEnumName(string typeName, string xmlName)
		{
			if (!this._isFlags)
			{
				foreach (EnumMap.EnumMapMember enumMapMember in this._members)
				{
					if (enumMapMember.XmlName == xmlName)
					{
						return enumMapMember.EnumName;
					}
				}
				return null;
			}
			xmlName = xmlName.Trim();
			if (xmlName.Length == 0)
			{
				return "0";
			}
			StringBuilder stringBuilder = new StringBuilder();
			string[] array = xmlName.Split(null);
			foreach (string text in array)
			{
				if (!(text == string.Empty))
				{
					string text2 = null;
					for (int k = 0; k < this.XmlNames.Length; k++)
					{
						if (this.XmlNames[k] == text)
						{
							text2 = this.EnumNames[k];
							break;
						}
					}
					if (text2 == null)
					{
						throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "'{0}' is not a valid value for {1}.", new object[]
						{
							text,
							typeName
						}));
					}
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(',');
					}
					stringBuilder.Append(text2);
				}
			}
			return stringBuilder.ToString();
		}

		public class EnumMapMember
		{
			private readonly string _xmlName;

			private readonly string _enumName;

			private readonly long _value;

			private string _documentation;

			public EnumMapMember(string xmlName, string enumName) : this(xmlName, enumName, 0L)
			{
			}

			public EnumMapMember(string xmlName, string enumName, long value)
			{
				this._xmlName = xmlName;
				this._enumName = enumName;
				this._value = value;
			}

			public string XmlName
			{
				get
				{
					return this._xmlName;
				}
			}

			public string EnumName
			{
				get
				{
					return this._enumName;
				}
			}

			public long Value
			{
				get
				{
					return this._value;
				}
			}

			public string Documentation
			{
				get
				{
					return this._documentation;
				}
				set
				{
					this._documentation = value;
				}
			}
		}
	}
}
