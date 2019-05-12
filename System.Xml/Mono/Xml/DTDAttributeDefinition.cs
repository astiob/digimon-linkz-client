using System;
using System.Collections;
using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace Mono.Xml
{
	internal class DTDAttributeDefinition : DTDNode
	{
		private string name;

		private XmlSchemaDatatype datatype;

		private ArrayList enumeratedLiterals;

		private string unresolvedDefault;

		private ArrayList enumeratedNotations;

		private DTDAttributeOccurenceType occurenceType;

		private string resolvedDefaultValue;

		private string resolvedNormalizedDefaultValue;

		internal DTDAttributeDefinition(DTDObjectModel root)
		{
			base.SetRoot(root);
		}

		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		public XmlSchemaDatatype Datatype
		{
			get
			{
				return this.datatype;
			}
			set
			{
				this.datatype = value;
			}
		}

		public DTDAttributeOccurenceType OccurenceType
		{
			get
			{
				return this.occurenceType;
			}
			set
			{
				this.occurenceType = value;
			}
		}

		public ArrayList EnumeratedAttributeDeclaration
		{
			get
			{
				if (this.enumeratedLiterals == null)
				{
					this.enumeratedLiterals = new ArrayList();
				}
				return this.enumeratedLiterals;
			}
		}

		public ArrayList EnumeratedNotations
		{
			get
			{
				if (this.enumeratedNotations == null)
				{
					this.enumeratedNotations = new ArrayList();
				}
				return this.enumeratedNotations;
			}
		}

		public string DefaultValue
		{
			get
			{
				if (this.resolvedDefaultValue == null)
				{
					this.resolvedDefaultValue = this.ComputeDefaultValue();
				}
				return this.resolvedDefaultValue;
			}
		}

		public string NormalizedDefaultValue
		{
			get
			{
				if (this.resolvedNormalizedDefaultValue == null)
				{
					string s = this.ComputeDefaultValue();
					try
					{
						object obj = this.Datatype.ParseValue(s, null, null);
						this.resolvedNormalizedDefaultValue = ((!(obj is string[])) ? ((!(obj is IFormattable)) ? obj.ToString() : ((IFormattable)obj).ToString(null, CultureInfo.InvariantCulture)) : string.Join(" ", (string[])obj));
					}
					catch (Exception)
					{
						this.resolvedNormalizedDefaultValue = this.Datatype.Normalize(s);
					}
				}
				return this.resolvedNormalizedDefaultValue;
			}
		}

		public string UnresolvedDefaultValue
		{
			get
			{
				return this.unresolvedDefault;
			}
			set
			{
				this.unresolvedDefault = value;
			}
		}

		public char QuoteChar
		{
			get
			{
				return (this.UnresolvedDefaultValue.Length <= 0) ? '"' : this.UnresolvedDefaultValue[0];
			}
		}

		internal string ComputeDefaultValue()
		{
			if (this.UnresolvedDefaultValue == null)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			int startIndex = 0;
			string unresolvedDefaultValue = this.UnresolvedDefaultValue;
			int num;
			while ((num = unresolvedDefaultValue.IndexOf('&', startIndex)) >= 0)
			{
				int num2 = unresolvedDefaultValue.IndexOf(';', num);
				if (unresolvedDefaultValue[num + 1] == '#')
				{
					char c = unresolvedDefaultValue[num + 2];
					NumberStyles numberStyles = NumberStyles.Integer;
					string s;
					if (c == 'x' || c == 'X')
					{
						s = unresolvedDefaultValue.Substring(num + 3, num2 - num - 3);
						numberStyles |= NumberStyles.HexNumber;
					}
					else
					{
						s = unresolvedDefaultValue.Substring(num + 2, num2 - num - 2);
					}
					stringBuilder.Append((char)int.Parse(s, numberStyles, CultureInfo.InvariantCulture));
				}
				else
				{
					stringBuilder.Append(unresolvedDefaultValue.Substring(startIndex, num - 1));
					string text = unresolvedDefaultValue.Substring(num + 1, num2 - 2);
					int predefinedEntity = XmlChar.GetPredefinedEntity(text);
					if (predefinedEntity >= 0)
					{
						stringBuilder.Append(predefinedEntity);
					}
					else
					{
						stringBuilder.Append(base.Root.ResolveEntity(text));
					}
				}
				startIndex = num2 + 1;
			}
			stringBuilder.Append(unresolvedDefaultValue.Substring(startIndex));
			string result = stringBuilder.ToString(1, stringBuilder.Length - 2);
			stringBuilder.Length = 0;
			return result;
		}
	}
}
