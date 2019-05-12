using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;

namespace JsonFx.Json
{
	public class JsonWriter : IDisposable
	{
		public const string JsonMimeType = "application/json";

		public const string JsonFileExtension = ".json";

		private const string AnonymousTypePrefix = "<>f__AnonymousType";

		private const string ErrorMaxDepth = "The maxiumum depth of {0} was exceeded. Check for cycles in object graph.";

		private const string ErrorIDictionaryEnumerator = "Types which implement Generic IDictionary<TKey, TValue> must have an IEnumerator which implements IDictionaryEnumerator. ({0})";

		private readonly TextWriter Writer;

		private JsonWriterSettings settings;

		private int depth;

		public JsonWriter(TextWriter output) : this(output, new JsonWriterSettings())
		{
		}

		public JsonWriter(TextWriter output, JsonWriterSettings settings)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			if (settings == null)
			{
				throw new ArgumentNullException("settings");
			}
			this.Writer = output;
			this.settings = settings;
			this.Writer.NewLine = this.settings.NewLine;
		}

		public JsonWriter(Stream output) : this(output, new JsonWriterSettings())
		{
		}

		public JsonWriter(Stream output, JsonWriterSettings settings)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			if (settings == null)
			{
				throw new ArgumentNullException("settings");
			}
			this.Writer = new StreamWriter(output, Encoding.UTF8);
			this.settings = settings;
			this.Writer.NewLine = this.settings.NewLine;
		}

		public JsonWriter(string outputFileName) : this(outputFileName, new JsonWriterSettings())
		{
		}

		public JsonWriter(string outputFileName, JsonWriterSettings settings)
		{
			if (outputFileName == null)
			{
				throw new ArgumentNullException("outputFileName");
			}
			if (settings == null)
			{
				throw new ArgumentNullException("settings");
			}
			Stream stream = new FileStream(outputFileName, FileMode.Create, FileAccess.Write, FileShare.Read);
			this.Writer = new StreamWriter(stream, Encoding.UTF8);
			this.settings = settings;
			this.Writer.NewLine = this.settings.NewLine;
		}

		public JsonWriter(StringBuilder output) : this(output, new JsonWriterSettings())
		{
		}

		public JsonWriter(StringBuilder output, JsonWriterSettings settings)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			if (settings == null)
			{
				throw new ArgumentNullException("settings");
			}
			this.Writer = new StringWriter(output, CultureInfo.InvariantCulture);
			this.settings = settings;
			this.Writer.NewLine = this.settings.NewLine;
		}

		void IDisposable.Dispose()
		{
			if (this.Writer != null)
			{
				this.Writer.Dispose();
			}
		}

		[Obsolete("This has been deprecated in favor of JsonWriterSettings object")]
		public string TypeHintName
		{
			get
			{
				return this.settings.TypeHintName;
			}
			set
			{
				this.settings.TypeHintName = value;
			}
		}

		[Obsolete("This has been deprecated in favor of JsonWriterSettings object")]
		public bool PrettyPrint
		{
			get
			{
				return this.settings.PrettyPrint;
			}
			set
			{
				this.settings.PrettyPrint = value;
			}
		}

		[Obsolete("This has been deprecated in favor of JsonWriterSettings object")]
		public string Tab
		{
			get
			{
				return this.settings.Tab;
			}
			set
			{
				this.settings.Tab = value;
			}
		}

		[Obsolete("This has been deprecated in favor of JsonWriterSettings object")]
		public string NewLine
		{
			get
			{
				return this.settings.NewLine;
			}
			set
			{
				TextWriter writer = this.Writer;
				this.settings.NewLine = value;
				writer.NewLine = value;
			}
		}

		protected int Depth
		{
			get
			{
				return this.depth;
			}
		}

		[Obsolete("This has been deprecated in favor of JsonWriterSettings object")]
		public int MaxDepth
		{
			get
			{
				return this.settings.MaxDepth;
			}
			set
			{
				this.settings.MaxDepth = value;
			}
		}

		[Obsolete("This has been deprecated in favor of JsonWriterSettings object")]
		public bool UseXmlSerializationAttributes
		{
			get
			{
				return this.settings.UseXmlSerializationAttributes;
			}
			set
			{
				this.settings.UseXmlSerializationAttributes = value;
			}
		}

		[Obsolete("This has been deprecated in favor of JsonWriterSettings object")]
		public WriteDelegate<DateTime> DateTimeSerializer
		{
			get
			{
				return this.settings.DateTimeSerializer;
			}
			set
			{
				this.settings.DateTimeSerializer = value;
			}
		}

		public TextWriter TextWriter
		{
			get
			{
				return this.Writer;
			}
		}

		public JsonWriterSettings Settings
		{
			get
			{
				return this.settings;
			}
			set
			{
				if (value == null)
				{
					value = new JsonWriterSettings();
				}
				this.settings = value;
			}
		}

		public static string Serialize(object value)
		{
			StringBuilder stringBuilder = new StringBuilder();
			using (JsonWriter jsonWriter = new JsonWriter(stringBuilder))
			{
				jsonWriter.Write(value);
			}
			return stringBuilder.ToString();
		}

		public void Write(object value)
		{
			this.Write(value, false);
		}

		protected virtual void Write(object value, bool isProperty)
		{
			if (isProperty && this.settings.PrettyPrint)
			{
				this.Writer.Write(' ');
			}
			if (value == null)
			{
				this.Writer.Write("null");
				return;
			}
			if (value is IJsonSerializable)
			{
				try
				{
					if (isProperty)
					{
						this.depth++;
						if (this.depth > this.settings.MaxDepth)
						{
							throw new JsonSerializationException(string.Format("The maxiumum depth of {0} was exceeded. Check for cycles in object graph.", this.settings.MaxDepth));
						}
						this.WriteLine();
					}
					((IJsonSerializable)value).WriteJson(this);
				}
				finally
				{
					if (isProperty)
					{
						this.depth--;
					}
				}
				return;
			}
			if (value is Enum)
			{
				this.Write((Enum)value);
				return;
			}
			Type type = value.GetType();
			switch (Type.GetTypeCode(type))
			{
			case TypeCode.Empty:
			case TypeCode.DBNull:
				this.Writer.Write("null");
				return;
			case TypeCode.Boolean:
				this.Write((bool)value);
				return;
			case TypeCode.Char:
				this.Write((char)value);
				return;
			case TypeCode.SByte:
				this.Write((sbyte)value);
				return;
			case TypeCode.Byte:
				this.Write((byte)value);
				return;
			case TypeCode.Int16:
				this.Write((short)value);
				return;
			case TypeCode.UInt16:
				this.Write((ushort)value);
				return;
			case TypeCode.Int32:
				this.Write((int)value);
				return;
			case TypeCode.UInt32:
				this.Write((uint)value);
				return;
			case TypeCode.Int64:
				this.Write((long)value);
				return;
			case TypeCode.UInt64:
				this.Write((ulong)value);
				return;
			case TypeCode.Single:
				this.Write((float)value);
				return;
			case TypeCode.Double:
				this.Write((double)value);
				return;
			case TypeCode.Decimal:
				this.Write((decimal)value);
				return;
			case TypeCode.DateTime:
				this.Write((DateTime)value);
				return;
			case TypeCode.String:
				this.Write((string)value);
				return;
			}
			if (value is Guid)
			{
				this.Write((Guid)value);
				return;
			}
			if (value is Uri)
			{
				this.Write((Uri)value);
				return;
			}
			if (value is TimeSpan)
			{
				this.Write((TimeSpan)value);
				return;
			}
			if (value is Version)
			{
				this.Write((Version)value);
				return;
			}
			if (value is IDictionary)
			{
				try
				{
					if (isProperty)
					{
						this.depth++;
						if (this.depth > this.settings.MaxDepth)
						{
							throw new JsonSerializationException(string.Format("The maxiumum depth of {0} was exceeded. Check for cycles in object graph.", this.settings.MaxDepth));
						}
						this.WriteLine();
					}
					this.WriteObject((IDictionary)value);
				}
				finally
				{
					if (isProperty)
					{
						this.depth--;
					}
				}
				return;
			}
			if (type.GetInterface("System.Collections.Generic.IDictionary`2") != null)
			{
				try
				{
					if (isProperty)
					{
						this.depth++;
						if (this.depth > this.settings.MaxDepth)
						{
							throw new JsonSerializationException(string.Format("The maxiumum depth of {0} was exceeded. Check for cycles in object graph.", this.settings.MaxDepth));
						}
						this.WriteLine();
					}
					this.WriteDictionary((IEnumerable)value);
				}
				finally
				{
					if (isProperty)
					{
						this.depth--;
					}
				}
				return;
			}
			if (value is IEnumerable)
			{
				try
				{
					if (isProperty)
					{
						this.depth++;
						if (this.depth > this.settings.MaxDepth)
						{
							throw new JsonSerializationException(string.Format("The maxiumum depth of {0} was exceeded. Check for cycles in object graph.", this.settings.MaxDepth));
						}
						this.WriteLine();
					}
					this.WriteArray((IEnumerable)value);
				}
				finally
				{
					if (isProperty)
					{
						this.depth--;
					}
				}
				return;
			}
			try
			{
				if (isProperty)
				{
					this.depth++;
					if (this.depth > this.settings.MaxDepth)
					{
						throw new JsonSerializationException(string.Format("The maxiumum depth of {0} was exceeded. Check for cycles in object graph.", this.settings.MaxDepth));
					}
					this.WriteLine();
				}
				this.WriteObject(value, type);
			}
			finally
			{
				if (isProperty)
				{
					this.depth--;
				}
			}
		}

		public virtual void WriteBase64(byte[] value)
		{
			this.Write(Convert.ToBase64String(value));
		}

		public virtual void WriteHexString(byte[] value)
		{
			if (value == null || value.Length == 0)
			{
				this.Write(string.Empty);
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < value.Length; i++)
			{
				stringBuilder.Append(value[i].ToString("x2"));
			}
			this.Write(stringBuilder.ToString());
		}

		public virtual void Write(DateTime value)
		{
			if (this.settings.DateTimeSerializer != null)
			{
				this.settings.DateTimeSerializer(this, value);
				return;
			}
			DateTimeKind kind = value.Kind;
			if (kind != DateTimeKind.Utc)
			{
				if (kind != DateTimeKind.Local)
				{
					this.Write(string.Format("{0:s}", value));
					return;
				}
				value = value.ToUniversalTime();
			}
			this.Write(string.Format("{0:s}Z", value));
		}

		public virtual void Write(Guid value)
		{
			this.Write(value.ToString("D"));
		}

		public virtual void Write(Enum value)
		{
			Type type = value.GetType();
			string value2;
			if (type.IsDefined(typeof(FlagsAttribute), true) && !Enum.IsDefined(type, value))
			{
				Enum[] flagList = JsonWriter.GetFlagList(type, value);
				string[] array = new string[flagList.Length];
				for (int i = 0; i < flagList.Length; i++)
				{
					array[i] = JsonNameAttribute.GetJsonName(flagList[i]);
					if (string.IsNullOrEmpty(array[i]))
					{
						array[i] = flagList[i].ToString("f");
					}
				}
				value2 = string.Join(", ", array);
			}
			else
			{
				value2 = JsonNameAttribute.GetJsonName(value);
				if (string.IsNullOrEmpty(value2))
				{
					value2 = value.ToString("f");
				}
			}
			this.Write(value2);
		}

		public virtual void Write(string value)
		{
			if (value == null)
			{
				this.Writer.Write("null");
				return;
			}
			int num = 0;
			int length = value.Length;
			this.Writer.Write('"');
			for (int i = num; i < length; i++)
			{
				char c = value[i];
				if (c <= '\u001f' || c >= '\u007f' || c == '<' || c == '"' || c == '\\')
				{
					if (i > num)
					{
						this.Writer.Write(value.Substring(num, i - num));
					}
					num = i + 1;
					char c2 = c;
					switch (c2)
					{
					case '\b':
						this.Writer.Write("\\b");
						break;
					case '\t':
						this.Writer.Write("\\t");
						break;
					case '\n':
						this.Writer.Write("\\n");
						break;
					default:
						if (c2 != '"' && c2 != '\\')
						{
							this.Writer.Write("\\u");
							this.Writer.Write(char.ConvertToUtf32(value, i).ToString("X4"));
						}
						else
						{
							this.Writer.Write('\\');
							this.Writer.Write(c);
						}
						break;
					case '\f':
						this.Writer.Write("\\f");
						break;
					case '\r':
						this.Writer.Write("\\r");
						break;
					}
				}
			}
			if (length > num)
			{
				this.Writer.Write(value.Substring(num, length - num));
			}
			this.Writer.Write('"');
		}

		public virtual void Write(bool value)
		{
			this.Writer.Write((!value) ? "false" : "true");
		}

		public virtual void Write(byte value)
		{
			this.Writer.Write(value.ToString("g", CultureInfo.InvariantCulture));
		}

		public virtual void Write(sbyte value)
		{
			this.Writer.Write(value.ToString("g", CultureInfo.InvariantCulture));
		}

		public virtual void Write(short value)
		{
			this.Writer.Write(value.ToString("g", CultureInfo.InvariantCulture));
		}

		public virtual void Write(ushort value)
		{
			this.Writer.Write(value.ToString("g", CultureInfo.InvariantCulture));
		}

		public virtual void Write(int value)
		{
			this.Writer.Write(value.ToString("g", CultureInfo.InvariantCulture));
		}

		public virtual void Write(uint value)
		{
			if (this.InvalidIeee754(value))
			{
				this.Write(value.ToString("g", CultureInfo.InvariantCulture));
				return;
			}
			this.Writer.Write(value.ToString("g", CultureInfo.InvariantCulture));
		}

		public virtual void Write(long value)
		{
			if (this.InvalidIeee754(value))
			{
				this.Write(value.ToString("g", CultureInfo.InvariantCulture));
				return;
			}
			this.Writer.Write(value.ToString("g", CultureInfo.InvariantCulture));
		}

		public virtual void Write(ulong value)
		{
			if (this.InvalidIeee754(value))
			{
				this.Write(value.ToString("g", CultureInfo.InvariantCulture));
				return;
			}
			this.Writer.Write(value.ToString("g", CultureInfo.InvariantCulture));
		}

		public virtual void Write(float value)
		{
			if (float.IsNaN(value) || float.IsInfinity(value))
			{
				this.Writer.Write("null");
			}
			else
			{
				this.Writer.Write(value.ToString("r", CultureInfo.InvariantCulture));
			}
		}

		public virtual void Write(double value)
		{
			if (double.IsNaN(value) || double.IsInfinity(value))
			{
				this.Writer.Write("null");
			}
			else
			{
				this.Writer.Write(value.ToString("r", CultureInfo.InvariantCulture));
			}
		}

		public virtual void Write(decimal value)
		{
			if (this.InvalidIeee754(value))
			{
				this.Write(value.ToString("g", CultureInfo.InvariantCulture));
				return;
			}
			this.Writer.Write(value.ToString("g", CultureInfo.InvariantCulture));
		}

		public virtual void Write(char value)
		{
			this.Write(new string(value, 1));
		}

		public virtual void Write(TimeSpan value)
		{
			this.Write(value.Ticks);
		}

		public virtual void Write(Uri value)
		{
			this.Write(value.ToString());
		}

		public virtual void Write(Version value)
		{
			this.Write(value.ToString());
		}

		protected internal virtual void WriteArray(IEnumerable value)
		{
			bool flag = false;
			this.Writer.Write('[');
			this.depth++;
			if (this.depth > this.settings.MaxDepth)
			{
				throw new JsonSerializationException(string.Format("The maxiumum depth of {0} was exceeded. Check for cycles in object graph.", this.settings.MaxDepth));
			}
			try
			{
				foreach (object item in value)
				{
					if (flag)
					{
						this.WriteArrayItemDelim();
					}
					else
					{
						flag = true;
					}
					this.WriteLine();
					this.WriteArrayItem(item);
				}
			}
			finally
			{
				this.depth--;
			}
			if (flag)
			{
				this.WriteLine();
			}
			this.Writer.Write(']');
		}

		protected virtual void WriteArrayItem(object item)
		{
			this.Write(item, false);
		}

		protected virtual void WriteObject(IDictionary value)
		{
			this.WriteDictionary(value);
		}

		protected virtual void WriteDictionary(IEnumerable value)
		{
			IDictionaryEnumerator dictionaryEnumerator = value.GetEnumerator() as IDictionaryEnumerator;
			if (dictionaryEnumerator == null)
			{
				throw new JsonSerializationException(string.Format("Types which implement Generic IDictionary<TKey, TValue> must have an IEnumerator which implements IDictionaryEnumerator. ({0})", value.GetType()));
			}
			bool flag = false;
			this.Writer.Write('{');
			this.depth++;
			if (this.depth > this.settings.MaxDepth)
			{
				throw new JsonSerializationException(string.Format("The maxiumum depth of {0} was exceeded. Check for cycles in object graph.", this.settings.MaxDepth));
			}
			try
			{
				while (dictionaryEnumerator.MoveNext())
				{
					if (flag)
					{
						this.WriteObjectPropertyDelim();
					}
					else
					{
						flag = true;
					}
					this.WriteObjectProperty(Convert.ToString(dictionaryEnumerator.Entry.Key), dictionaryEnumerator.Entry.Value);
				}
			}
			finally
			{
				this.depth--;
			}
			if (flag)
			{
				this.WriteLine();
			}
			this.Writer.Write('}');
		}

		private void WriteObjectProperty(string key, object value)
		{
			this.WriteLine();
			this.WriteObjectPropertyName(key);
			this.Writer.Write(':');
			this.WriteObjectPropertyValue(value);
		}

		protected virtual void WriteObjectPropertyName(string name)
		{
			this.Write(name);
		}

		protected virtual void WriteObjectPropertyValue(object value)
		{
			this.Write(value, true);
		}

		protected virtual void WriteObject(object value, Type type)
		{
			bool flag = false;
			this.Writer.Write('{');
			this.depth++;
			if (this.depth > this.settings.MaxDepth)
			{
				throw new JsonSerializationException(string.Format("The maxiumum depth of {0} was exceeded. Check for cycles in object graph.", this.settings.MaxDepth));
			}
			try
			{
				if (!string.IsNullOrEmpty(this.settings.TypeHintName))
				{
					if (flag)
					{
						this.WriteObjectPropertyDelim();
					}
					else
					{
						flag = true;
					}
					this.WriteObjectProperty(this.settings.TypeHintName, type.FullName + ", " + type.Assembly.GetName().Name);
				}
				bool flag2 = type.IsGenericType && type.Name.StartsWith("<>f__AnonymousType");
				PropertyInfo[] properties = type.GetProperties();
				foreach (PropertyInfo propertyInfo in properties)
				{
					if (propertyInfo.CanRead)
					{
						if (propertyInfo.CanWrite || flag2)
						{
							if (!this.IsIgnored(type, propertyInfo, value))
							{
								object value2 = propertyInfo.GetValue(value, null);
								if (!this.IsDefaultValue(propertyInfo, value2))
								{
									if (flag)
									{
										this.WriteObjectPropertyDelim();
									}
									else
									{
										flag = true;
									}
									string text = JsonNameAttribute.GetJsonName(propertyInfo);
									if (string.IsNullOrEmpty(text))
									{
										text = propertyInfo.Name;
									}
									this.WriteObjectProperty(text, value2);
								}
							}
						}
					}
				}
				FieldInfo[] fields = type.GetFields();
				foreach (FieldInfo fieldInfo in fields)
				{
					if (fieldInfo.IsPublic && !fieldInfo.IsStatic)
					{
						if (!this.IsIgnored(type, fieldInfo, value))
						{
							object value3 = fieldInfo.GetValue(value);
							if (!this.IsDefaultValue(fieldInfo, value3))
							{
								if (flag)
								{
									this.WriteObjectPropertyDelim();
									this.WriteLine();
								}
								else
								{
									flag = true;
								}
								string text2 = JsonNameAttribute.GetJsonName(fieldInfo);
								if (string.IsNullOrEmpty(text2))
								{
									text2 = fieldInfo.Name;
								}
								this.WriteObjectProperty(text2, value3);
							}
						}
					}
				}
			}
			finally
			{
				this.depth--;
			}
			if (flag)
			{
				this.WriteLine();
			}
			this.Writer.Write('}');
		}

		protected virtual void WriteArrayItemDelim()
		{
			this.Writer.Write(',');
		}

		protected virtual void WriteObjectPropertyDelim()
		{
			this.Writer.Write(',');
		}

		protected virtual void WriteLine()
		{
			if (!this.settings.PrettyPrint)
			{
				return;
			}
			this.Writer.WriteLine();
			for (int i = 0; i < this.depth; i++)
			{
				this.Writer.Write(this.settings.Tab);
			}
		}

		private bool IsIgnored(Type objType, MemberInfo member, object obj)
		{
			if (JsonIgnoreAttribute.IsJsonIgnore(member))
			{
				return true;
			}
			string jsonSpecifiedProperty = JsonSpecifiedPropertyAttribute.GetJsonSpecifiedProperty(member);
			if (!string.IsNullOrEmpty(jsonSpecifiedProperty))
			{
				PropertyInfo property = objType.GetProperty(jsonSpecifiedProperty);
				if (property != null)
				{
					object value = property.GetValue(obj, null);
					if (value is bool && !Convert.ToBoolean(value))
					{
						return true;
					}
				}
			}
			if (this.settings.UseXmlSerializationAttributes)
			{
				if (JsonIgnoreAttribute.IsXmlIgnore(member))
				{
					return true;
				}
				PropertyInfo property2 = objType.GetProperty(member.Name + "Specified");
				if (property2 != null)
				{
					object value2 = property2.GetValue(obj, null);
					if (value2 is bool && !Convert.ToBoolean(value2))
					{
						return true;
					}
				}
			}
			return false;
		}

		private bool IsDefaultValue(MemberInfo member, object value)
		{
			DefaultValueAttribute defaultValueAttribute = Attribute.GetCustomAttribute(member, typeof(DefaultValueAttribute)) as DefaultValueAttribute;
			if (defaultValueAttribute == null)
			{
				return false;
			}
			if (defaultValueAttribute.Value == null)
			{
				return value == null;
			}
			return defaultValueAttribute.Value.Equals(value);
		}

		private static Enum[] GetFlagList(Type enumType, object value)
		{
			ulong num = Convert.ToUInt64(value);
			Array values = Enum.GetValues(enumType);
			List<Enum> list = new List<Enum>(values.Length);
			if (num == 0UL)
			{
				list.Add((Enum)Convert.ChangeType(value, enumType));
				return list.ToArray();
			}
			for (int i = values.Length - 1; i >= 0; i--)
			{
				ulong num2 = Convert.ToUInt64(values.GetValue(i));
				if (i != 0 || num2 != 0UL)
				{
					if ((num & num2) == num2)
					{
						num -= num2;
						list.Add(values.GetValue(i) as Enum);
					}
				}
			}
			if (num != 0UL)
			{
				list.Add(Enum.ToObject(enumType, num) as Enum);
			}
			return list.ToArray();
		}

		protected virtual bool InvalidIeee754(decimal value)
		{
			bool result;
			try
			{
				result = ((decimal)((double)value) != value);
			}
			catch
			{
				result = true;
			}
			return result;
		}
	}
}
