using System;
using System.Collections;
using System.Xml.XPath;

namespace System.Xml.Schema
{
	/// <summary>Represents the typed value of a validated XML element or attribute. The <see cref="T:System.Xml.Schema.XmlAtomicValue" /> class cannot be inherited.</summary>
	[MonoTODO]
	public sealed class XmlAtomicValue : XPathItem, ICloneable
	{
		private bool booleanValue;

		private DateTime dateTimeValue;

		private decimal decimalValue;

		private double doubleValue;

		private int intValue;

		private long longValue;

		private object objectValue;

		private float floatValue;

		private string stringValue;

		private XmlSchemaType schemaType;

		private XmlTypeCode xmlTypeCode;

		internal XmlAtomicValue(bool value, XmlSchemaType xmlType)
		{
			this.Init(value, xmlType);
		}

		internal XmlAtomicValue(DateTime value, XmlSchemaType xmlType)
		{
			this.Init(value, xmlType);
		}

		internal XmlAtomicValue(decimal value, XmlSchemaType xmlType)
		{
			this.Init(value, xmlType);
		}

		internal XmlAtomicValue(double value, XmlSchemaType xmlType)
		{
			this.Init(value, xmlType);
		}

		internal XmlAtomicValue(int value, XmlSchemaType xmlType)
		{
			this.Init(value, xmlType);
		}

		internal XmlAtomicValue(long value, XmlSchemaType xmlType)
		{
			this.Init(value, xmlType);
		}

		internal XmlAtomicValue(float value, XmlSchemaType xmlType)
		{
			this.Init(value, xmlType);
		}

		internal XmlAtomicValue(string value, XmlSchemaType xmlType)
		{
			this.Init(value, xmlType);
		}

		internal XmlAtomicValue(object value, XmlSchemaType xmlType)
		{
			this.Init(value, xmlType);
		}

		/// <summary>For a description of this member, see <see cref="M:System.Xml.Schema.XmlAtomicValue.Clone" />.</summary>
		object ICloneable.Clone()
		{
			return this.Clone();
		}

		private void Init(bool value, XmlSchemaType xmlType)
		{
			if (xmlType == null)
			{
				throw new ArgumentNullException("xmlType");
			}
			this.xmlTypeCode = XmlTypeCode.Boolean;
			this.booleanValue = value;
			this.schemaType = xmlType;
		}

		private void Init(DateTime value, XmlSchemaType xmlType)
		{
			if (xmlType == null)
			{
				throw new ArgumentNullException("xmlType");
			}
			this.xmlTypeCode = XmlTypeCode.DateTime;
			this.dateTimeValue = value;
			this.schemaType = xmlType;
		}

		private void Init(decimal value, XmlSchemaType xmlType)
		{
			if (xmlType == null)
			{
				throw new ArgumentNullException("xmlType");
			}
			this.xmlTypeCode = XmlTypeCode.Decimal;
			this.decimalValue = value;
			this.schemaType = xmlType;
		}

		private void Init(double value, XmlSchemaType xmlType)
		{
			if (xmlType == null)
			{
				throw new ArgumentNullException("xmlType");
			}
			this.xmlTypeCode = XmlTypeCode.Double;
			this.doubleValue = value;
			this.schemaType = xmlType;
		}

		private void Init(int value, XmlSchemaType xmlType)
		{
			if (xmlType == null)
			{
				throw new ArgumentNullException("xmlType");
			}
			this.xmlTypeCode = XmlTypeCode.Int;
			this.intValue = value;
			this.schemaType = xmlType;
		}

		private void Init(long value, XmlSchemaType xmlType)
		{
			if (xmlType == null)
			{
				throw new ArgumentNullException("xmlType");
			}
			this.xmlTypeCode = XmlTypeCode.Long;
			this.longValue = value;
			this.schemaType = xmlType;
		}

		private void Init(float value, XmlSchemaType xmlType)
		{
			if (xmlType == null)
			{
				throw new ArgumentNullException("xmlType");
			}
			this.xmlTypeCode = XmlTypeCode.Float;
			this.floatValue = value;
			this.schemaType = xmlType;
		}

		private void Init(string value, XmlSchemaType xmlType)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (xmlType == null)
			{
				throw new ArgumentNullException("xmlType");
			}
			this.xmlTypeCode = XmlTypeCode.String;
			this.stringValue = value;
			this.schemaType = xmlType;
		}

		private void Init(object value, XmlSchemaType xmlType)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (xmlType == null)
			{
				throw new ArgumentNullException("xmlType");
			}
			switch (Type.GetTypeCode(value.GetType()))
			{
			case TypeCode.Boolean:
				this.Init((bool)value, xmlType);
				return;
			case TypeCode.Int16:
			case TypeCode.UInt16:
			case TypeCode.Int32:
				this.Init((int)value, xmlType);
				return;
			case TypeCode.UInt32:
			case TypeCode.Int64:
				this.Init((long)value, xmlType);
				return;
			case TypeCode.Single:
				this.Init((float)value, xmlType);
				return;
			case TypeCode.Double:
				this.Init((double)value, xmlType);
				return;
			case TypeCode.Decimal:
				this.Init((decimal)value, xmlType);
				return;
			case TypeCode.DateTime:
				this.Init((DateTime)value, xmlType);
				return;
			case TypeCode.String:
				this.Init((string)value, xmlType);
				return;
			}
			ICollection collection = value as ICollection;
			if (collection != null && collection.Count == 1)
			{
				if (collection is IList)
				{
					this.Init(((IList)collection)[0], xmlType);
				}
				else
				{
					IEnumerator enumerator = collection.GetEnumerator();
					if (!enumerator.MoveNext())
					{
						return;
					}
					if (enumerator.Current is DictionaryEntry)
					{
						DictionaryEntry dictionaryEntry = (DictionaryEntry)enumerator.Current;
						this.Init(dictionaryEntry.Value, xmlType);
					}
					else
					{
						this.Init(enumerator.Current, xmlType);
					}
				}
				return;
			}
			XmlAtomicValue xmlAtomicValue = value as XmlAtomicValue;
			if (xmlAtomicValue != null)
			{
				XmlTypeCode xmlTypeCode = xmlAtomicValue.xmlTypeCode;
				switch (xmlTypeCode)
				{
				case XmlTypeCode.String:
					this.Init(xmlAtomicValue.stringValue, xmlType);
					return;
				case XmlTypeCode.Boolean:
					this.Init(xmlAtomicValue.booleanValue, xmlType);
					return;
				case XmlTypeCode.Decimal:
					this.Init(xmlAtomicValue.decimalValue, xmlType);
					return;
				case XmlTypeCode.Float:
					this.Init(xmlAtomicValue.floatValue, xmlType);
					return;
				case XmlTypeCode.Double:
					this.Init(xmlAtomicValue.doubleValue, xmlType);
					return;
				default:
					if (xmlTypeCode == XmlTypeCode.Long)
					{
						this.Init(xmlAtomicValue.longValue, xmlType);
						return;
					}
					if (xmlTypeCode == XmlTypeCode.Int)
					{
						this.Init(xmlAtomicValue.intValue, xmlType);
						return;
					}
					this.objectValue = xmlAtomicValue.objectValue;
					break;
				case XmlTypeCode.DateTime:
					this.Init(xmlAtomicValue.dateTimeValue, xmlType);
					return;
				}
			}
			this.objectValue = value;
			this.schemaType = xmlType;
		}

		/// <summary>Returns a copy of this <see cref="T:System.Xml.Schema.XmlAtomicValue" /> object.</summary>
		/// <returns>An <see cref="T:System.Xml.Schema.XmlAtomicValue" /> object copy of this <see cref="T:System.Xml.Schema.XmlAtomicValue" /> object.</returns>
		public XmlAtomicValue Clone()
		{
			return new XmlAtomicValue(this, this.schemaType);
		}

		/// <summary>Returns the validated XML element or attribute's value as the type specified using the <see cref="T:System.Xml.IXmlNamespaceResolver" /> object specified to resolve namespace prefixes.</summary>
		/// <returns>The value of the validated XML element or attribute as the type requested.</returns>
		/// <param name="type">The type to return the validated XML element or attribute's value as.</param>
		/// <param name="nsResolver">The <see cref="T:System.Xml.IXmlNamespaceResolver" /> object used to resolve namespace prefixes.</param>
		/// <exception cref="T:System.FormatException">The validated XML element or attribute's value is not in the correct format for the target type.</exception>
		/// <exception cref="T:System.InvalidCastException">The attempted cast is not valid.</exception>
		/// <exception cref="T:System.OverflowException">The attempted cast resulted in an overflow.</exception>
		public override object ValueAs(Type type, IXmlNamespaceResolver nsResolver)
		{
			XmlTypeCode xmlTypeCode = XmlAtomicValue.XmlTypeCodeFromRuntimeType(type, false);
			switch (xmlTypeCode)
			{
			case XmlTypeCode.Long:
			case XmlTypeCode.UnsignedInt:
				return this.ValueAsLong;
			case XmlTypeCode.Int:
			case XmlTypeCode.Short:
			case XmlTypeCode.UnsignedShort:
				return this.ValueAsInt;
			default:
				switch (xmlTypeCode)
				{
				case XmlTypeCode.String:
					return this.Value;
				case XmlTypeCode.Boolean:
					return this.ValueAsBoolean;
				default:
					if (xmlTypeCode == XmlTypeCode.Item)
					{
						return this.TypedValue;
					}
					if (xmlTypeCode != XmlTypeCode.QName)
					{
						throw new NotImplementedException();
					}
					return XmlQualifiedName.Parse(this.Value, nsResolver, true);
				case XmlTypeCode.Float:
				case XmlTypeCode.Double:
					return this.ValueAsDouble;
				case XmlTypeCode.DateTime:
					return this.ValueAsDateTime;
				}
				break;
			}
		}

		/// <summary>Gets the string value of the validated XML element or attribute.</summary>
		/// <returns>The string value of the validated XML element or attribute.</returns>
		public override string ToString()
		{
			return this.Value;
		}

		/// <summary>Gets a value indicating whether the validated XML element or attribute is an XPath node or an atomic value.</summary>
		/// <returns>true if the validated XML element or attribute is an XPath node; false if the validated XML element or attribute is an atomic value.</returns>
		public override bool IsNode
		{
			get
			{
				return false;
			}
		}

		internal XmlTypeCode ResolvedTypeCode
		{
			get
			{
				if (this.schemaType != XmlSchemaComplexType.AnyType)
				{
					return this.schemaType.TypeCode;
				}
				return this.xmlTypeCode;
			}
		}

		/// <summary>Gets the current validated XML element or attribute as a boxed object of the most appropriate Microsoft .NET Framework type according to its schema type.</summary>
		/// <returns>The current validated XML element or attribute as a boxed object of the most appropriate .NET Framework type.</returns>
		public override object TypedValue
		{
			get
			{
				XmlTypeCode resolvedTypeCode = this.ResolvedTypeCode;
				switch (resolvedTypeCode)
				{
				case XmlTypeCode.String:
					return this.Value;
				case XmlTypeCode.Boolean:
					return this.ValueAsBoolean;
				default:
					if (resolvedTypeCode == XmlTypeCode.Long)
					{
						return this.ValueAsLong;
					}
					if (resolvedTypeCode != XmlTypeCode.Int)
					{
						return this.objectValue;
					}
					return this.ValueAsInt;
				case XmlTypeCode.Float:
				case XmlTypeCode.Double:
					return this.ValueAsDouble;
				case XmlTypeCode.DateTime:
					return this.ValueAsDateTime;
				}
			}
		}

		/// <summary>Gets the string value of the validated XML element or attribute.</summary>
		/// <returns>The string value of the validated XML element or attribute.</returns>
		public override string Value
		{
			get
			{
				XmlTypeCode resolvedTypeCode = this.ResolvedTypeCode;
				switch (resolvedTypeCode)
				{
				case XmlTypeCode.NonPositiveInteger:
				case XmlTypeCode.NegativeInteger:
				case XmlTypeCode.Long:
				case XmlTypeCode.NonNegativeInteger:
				case XmlTypeCode.UnsignedLong:
				case XmlTypeCode.PositiveInteger:
					this.stringValue = XQueryConvert.IntegerToString(this.ValueAsLong);
					break;
				case XmlTypeCode.Int:
				case XmlTypeCode.Short:
				case XmlTypeCode.Byte:
				case XmlTypeCode.UnsignedInt:
				case XmlTypeCode.UnsignedShort:
				case XmlTypeCode.UnsignedByte:
					this.stringValue = XQueryConvert.IntToString(this.ValueAsInt);
					break;
				default:
				{
					switch (resolvedTypeCode)
					{
					case XmlTypeCode.AnyAtomicType:
						break;
					default:
						if (resolvedTypeCode != XmlTypeCode.None && resolvedTypeCode != XmlTypeCode.Item)
						{
							goto IL_218;
						}
						break;
					case XmlTypeCode.String:
						return this.stringValue;
					case XmlTypeCode.Boolean:
						this.stringValue = XQueryConvert.BooleanToString(this.ValueAsBoolean);
						goto IL_218;
					case XmlTypeCode.Float:
					case XmlTypeCode.Double:
						this.stringValue = XQueryConvert.DoubleToString(this.ValueAsDouble);
						goto IL_218;
					case XmlTypeCode.DateTime:
						this.stringValue = XQueryConvert.DateTimeToString(this.ValueAsDateTime);
						goto IL_218;
					}
					XmlTypeCode xmlTypeCode = XmlAtomicValue.XmlTypeCodeFromRuntimeType(this.objectValue.GetType(), false);
					switch (xmlTypeCode)
					{
					case XmlTypeCode.String:
						this.stringValue = (string)this.objectValue;
						break;
					case XmlTypeCode.Boolean:
						this.stringValue = XQueryConvert.BooleanToString((bool)this.objectValue);
						break;
					case XmlTypeCode.Decimal:
						this.stringValue = XQueryConvert.DecimalToString((decimal)this.objectValue);
						break;
					case XmlTypeCode.Float:
						this.stringValue = XQueryConvert.FloatToString((float)this.objectValue);
						break;
					case XmlTypeCode.Double:
						this.stringValue = XQueryConvert.DoubleToString((double)this.objectValue);
						break;
					default:
						if (xmlTypeCode != XmlTypeCode.Long)
						{
							if (xmlTypeCode == XmlTypeCode.Int)
							{
								this.stringValue = XQueryConvert.IntToString((int)this.objectValue);
							}
						}
						else
						{
							this.stringValue = XQueryConvert.IntegerToString((long)this.objectValue);
						}
						break;
					case XmlTypeCode.DateTime:
						this.stringValue = XQueryConvert.DateTimeToString((DateTime)this.objectValue);
						break;
					}
					break;
				}
				}
				IL_218:
				if (this.stringValue != null)
				{
					return this.stringValue;
				}
				if (this.objectValue != null)
				{
					throw new InvalidCastException(string.Format("Conversion from runtime type {0} to {1} is not supported", this.objectValue.GetType(), XmlTypeCode.String));
				}
				throw new InvalidCastException(string.Format("Conversion from schema type {0} (type code {1}, resolved type code {2}) to {3} is not supported.", new object[]
				{
					this.schemaType.QualifiedName,
					this.xmlTypeCode,
					this.ResolvedTypeCode,
					XmlTypeCode.String
				}));
			}
		}

		/// <summary>Gets the validated XML element or attribute's value as a <see cref="T:System.Boolean" />.</summary>
		/// <returns>The validated XML element or attribute's value as a <see cref="T:System.Boolean" />.</returns>
		/// <exception cref="T:System.FormatException">The validated XML element or attribute's value is not in the correct format for the <see cref="T:System.Boolean" /> type.</exception>
		/// <exception cref="T:System.InvalidCastException">The attempted cast to <see cref="T:System.Boolean" /> is not valid.</exception>
		public override bool ValueAsBoolean
		{
			get
			{
				XmlTypeCode xmlTypeCode = this.xmlTypeCode;
				switch (xmlTypeCode)
				{
				case XmlTypeCode.AnyAtomicType:
					break;
				default:
					if (xmlTypeCode != XmlTypeCode.None && xmlTypeCode != XmlTypeCode.Item)
					{
						if (xmlTypeCode == XmlTypeCode.Long)
						{
							return XQueryConvert.IntegerToBoolean(this.longValue);
						}
						if (xmlTypeCode != XmlTypeCode.Int)
						{
							goto IL_BE;
						}
						return XQueryConvert.IntToBoolean(this.intValue);
					}
					break;
				case XmlTypeCode.String:
					return XQueryConvert.StringToBoolean(this.stringValue);
				case XmlTypeCode.Boolean:
					return this.booleanValue;
				case XmlTypeCode.Decimal:
					return XQueryConvert.DecimalToBoolean(this.decimalValue);
				case XmlTypeCode.Float:
					return XQueryConvert.FloatToBoolean(this.floatValue);
				case XmlTypeCode.Double:
					return XQueryConvert.DoubleToBoolean(this.doubleValue);
				}
				if (this.objectValue is bool)
				{
					return (bool)this.objectValue;
				}
				IL_BE:
				throw new InvalidCastException(string.Format("Conversion from {0} to {1} is not supported", this.schemaType.QualifiedName, XmlSchemaSimpleType.XsBoolean.QualifiedName));
			}
		}

		/// <summary>Gets the validated XML element or attribute's value as a <see cref="T:System.DateTime" />.</summary>
		/// <returns>The validated XML element or attribute's value as a <see cref="T:System.DateTime" />.</returns>
		/// <exception cref="T:System.FormatException">The validated XML element or attribute's value is not in the correct format for the <see cref="T:System.DateTime" /> type.</exception>
		/// <exception cref="T:System.InvalidCastException">The attempted cast to <see cref="T:System.DateTime" /> is not valid.</exception>
		public override DateTime ValueAsDateTime
		{
			get
			{
				XmlTypeCode xmlTypeCode = this.xmlTypeCode;
				switch (xmlTypeCode)
				{
				case XmlTypeCode.AnyAtomicType:
					break;
				default:
					if (xmlTypeCode != XmlTypeCode.None && xmlTypeCode != XmlTypeCode.Item)
					{
						if (xmlTypeCode != XmlTypeCode.DateTime)
						{
							goto IL_6A;
						}
						return this.dateTimeValue;
					}
					break;
				case XmlTypeCode.String:
					return XQueryConvert.StringToDateTime(this.stringValue);
				}
				if (this.objectValue is DateTime)
				{
					return (DateTime)this.objectValue;
				}
				IL_6A:
				throw new InvalidCastException(string.Format("Conversion from {0} to {1} is not supported", this.schemaType.QualifiedName, XmlSchemaSimpleType.XsDateTime.QualifiedName));
			}
		}

		/// <summary>Gets the validated XML element or attribute's value as a <see cref="T:System.Double" />.</summary>
		/// <returns>The validated XML element or attribute's value as a <see cref="T:System.Double" />.</returns>
		/// <exception cref="T:System.FormatException">The validated XML element or attribute's value is not in the correct format for the <see cref="T:System.Double" /> type.</exception>
		/// <exception cref="T:System.InvalidCastException">The attempted cast to <see cref="T:System.Double" /> is not valid.</exception>
		/// <exception cref="T:System.OverflowException">The attempted cast resulted in an overflow.</exception>
		public override double ValueAsDouble
		{
			get
			{
				XmlTypeCode xmlTypeCode = this.xmlTypeCode;
				switch (xmlTypeCode)
				{
				case XmlTypeCode.AnyAtomicType:
					break;
				default:
					if (xmlTypeCode != XmlTypeCode.None && xmlTypeCode != XmlTypeCode.Item)
					{
						if (xmlTypeCode == XmlTypeCode.Long)
						{
							return XQueryConvert.IntegerToDouble(this.longValue);
						}
						if (xmlTypeCode != XmlTypeCode.Int)
						{
							goto IL_BE;
						}
						return XQueryConvert.IntToDouble(this.intValue);
					}
					break;
				case XmlTypeCode.String:
					return XQueryConvert.StringToDouble(this.stringValue);
				case XmlTypeCode.Boolean:
					return XQueryConvert.BooleanToDouble(this.booleanValue);
				case XmlTypeCode.Decimal:
					return XQueryConvert.DecimalToDouble(this.decimalValue);
				case XmlTypeCode.Float:
					return XQueryConvert.FloatToDouble(this.floatValue);
				case XmlTypeCode.Double:
					return this.doubleValue;
				}
				if (this.objectValue is double)
				{
					return (double)this.objectValue;
				}
				IL_BE:
				throw new InvalidCastException(string.Format("Conversion from {0} to {1} is not supported", this.schemaType.QualifiedName, XmlSchemaSimpleType.XsDouble.QualifiedName));
			}
		}

		/// <summary>Gets the validated XML element or attribute's value as an <see cref="T:System.Int32" />.</summary>
		/// <returns>The validated XML element or attribute's value as an <see cref="T:System.Int32" />.</returns>
		/// <exception cref="T:System.FormatException">The validated XML element or attribute's value is not in the correct format for the <see cref="T:System.Int32" /> type.</exception>
		/// <exception cref="T:System.InvalidCastException">The attempted cast to <see cref="T:System.Int32" /> is not valid.</exception>
		/// <exception cref="T:System.OverflowException">The attempted cast resulted in an overflow.</exception>
		public override int ValueAsInt
		{
			get
			{
				XmlTypeCode xmlTypeCode = this.xmlTypeCode;
				switch (xmlTypeCode)
				{
				case XmlTypeCode.AnyAtomicType:
					break;
				default:
					if (xmlTypeCode != XmlTypeCode.None && xmlTypeCode != XmlTypeCode.Item)
					{
						if (xmlTypeCode == XmlTypeCode.Long)
						{
							return XQueryConvert.IntegerToInt(this.longValue);
						}
						if (xmlTypeCode != XmlTypeCode.Int)
						{
							goto IL_BE;
						}
						return this.intValue;
					}
					break;
				case XmlTypeCode.String:
					return XQueryConvert.StringToInt(this.stringValue);
				case XmlTypeCode.Boolean:
					return XQueryConvert.BooleanToInt(this.booleanValue);
				case XmlTypeCode.Decimal:
					return XQueryConvert.DecimalToInt(this.decimalValue);
				case XmlTypeCode.Float:
					return XQueryConvert.FloatToInt(this.floatValue);
				case XmlTypeCode.Double:
					return XQueryConvert.DoubleToInt(this.doubleValue);
				}
				if (this.objectValue is int)
				{
					return (int)this.objectValue;
				}
				IL_BE:
				throw new InvalidCastException(string.Format("Conversion from {0} to {1} is not supported", this.schemaType.QualifiedName, XmlSchemaSimpleType.XsInt.QualifiedName));
			}
		}

		/// <summary>Gets the validated XML element or attribute's value as an <see cref="T:System.Int64" />.</summary>
		/// <returns>The validated XML element or attribute's value as an <see cref="T:System.Int64" />.</returns>
		/// <exception cref="T:System.FormatException">The validated XML element or attribute's value is not in the correct format for the <see cref="T:System.Int64" /> type.</exception>
		/// <exception cref="T:System.InvalidCastException">The attempted cast to <see cref="T:System.Int64" /> is not valid.</exception>
		/// <exception cref="T:System.OverflowException">The attempted cast resulted in an overflow.</exception>
		public override long ValueAsLong
		{
			get
			{
				XmlTypeCode xmlTypeCode = this.xmlTypeCode;
				switch (xmlTypeCode)
				{
				case XmlTypeCode.AnyAtomicType:
					break;
				default:
					if (xmlTypeCode != XmlTypeCode.None && xmlTypeCode != XmlTypeCode.Item)
					{
						if (xmlTypeCode == XmlTypeCode.Long)
						{
							return this.longValue;
						}
						if (xmlTypeCode != XmlTypeCode.Int)
						{
							goto IL_C0;
						}
						return (long)XQueryConvert.IntegerToInt((long)this.intValue);
					}
					break;
				case XmlTypeCode.String:
					return XQueryConvert.StringToInteger(this.stringValue);
				case XmlTypeCode.Boolean:
					return XQueryConvert.BooleanToInteger(this.booleanValue);
				case XmlTypeCode.Decimal:
					return XQueryConvert.DecimalToInteger(this.decimalValue);
				case XmlTypeCode.Float:
					return XQueryConvert.FloatToInteger(this.floatValue);
				case XmlTypeCode.Double:
					return XQueryConvert.DoubleToInteger(this.doubleValue);
				}
				if (this.objectValue is long)
				{
					return (long)this.objectValue;
				}
				IL_C0:
				throw new InvalidCastException(string.Format("Conversion from {0} to {1} is not supported", this.schemaType.QualifiedName, XmlSchemaSimpleType.XsLong.QualifiedName));
			}
		}

		/// <summary>Gets the Microsoft .NET Framework type of the validated XML element or attribute.</summary>
		/// <returns>The .NET Framework type of the validated XML element or attribute. The default value is <see cref="T:System.String" />.</returns>
		public override Type ValueType
		{
			get
			{
				return this.schemaType.Datatype.ValueType;
			}
		}

		/// <summary>Gets the <see cref="T:System.Xml.Schema.XmlSchemaType" /> for the validated XML element or attribute.</summary>
		/// <returns>The <see cref="T:System.Xml.Schema.XmlSchemaType" /> for the validated XML element or attribute.</returns>
		public override XmlSchemaType XmlType
		{
			get
			{
				return this.schemaType;
			}
		}

		internal static Type RuntimeTypeFromXmlTypeCode(XmlTypeCode typeCode)
		{
			switch (typeCode)
			{
			case XmlTypeCode.Long:
				return typeof(long);
			case XmlTypeCode.Int:
				return typeof(int);
			case XmlTypeCode.Short:
				return typeof(short);
			default:
				switch (typeCode)
				{
				case XmlTypeCode.String:
					return typeof(string);
				case XmlTypeCode.Boolean:
					return typeof(bool);
				case XmlTypeCode.Decimal:
					return typeof(decimal);
				case XmlTypeCode.Float:
					return typeof(float);
				case XmlTypeCode.Double:
					return typeof(double);
				default:
					if (typeCode != XmlTypeCode.Item)
					{
						throw new NotSupportedException(string.Format("XQuery internal error: Cannot infer Runtime Type from XmlTypeCode {0}.", typeCode));
					}
					return typeof(object);
				case XmlTypeCode.DateTime:
					return typeof(DateTime);
				}
				break;
			case XmlTypeCode.UnsignedInt:
				return typeof(uint);
			case XmlTypeCode.UnsignedShort:
				return typeof(ushort);
			}
		}

		internal static XmlTypeCode XmlTypeCodeFromRuntimeType(Type cliType, bool raiseError)
		{
			switch (Type.GetTypeCode(cliType))
			{
			case TypeCode.Object:
				return XmlTypeCode.Item;
			case TypeCode.Boolean:
				return XmlTypeCode.Boolean;
			case TypeCode.Int16:
				return XmlTypeCode.Short;
			case TypeCode.UInt16:
				return XmlTypeCode.UnsignedShort;
			case TypeCode.Int32:
				return XmlTypeCode.Int;
			case TypeCode.UInt32:
				return XmlTypeCode.UnsignedInt;
			case TypeCode.Int64:
				return XmlTypeCode.Long;
			case TypeCode.Single:
				return XmlTypeCode.Float;
			case TypeCode.Double:
				return XmlTypeCode.Double;
			case TypeCode.Decimal:
				return XmlTypeCode.Decimal;
			case TypeCode.DateTime:
				return XmlTypeCode.DateTime;
			case TypeCode.String:
				return XmlTypeCode.String;
			}
			if (raiseError)
			{
				throw new NotSupportedException(string.Format("XQuery internal error: Cannot infer XmlTypeCode from Runtime Type {0}", cliType));
			}
			return XmlTypeCode.None;
		}
	}
}
