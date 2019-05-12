using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;

namespace JsonFx.Json
{
	public class JsonReader
	{
		public const string LiteralFalse = "false";

		public const string LiteralTrue = "true";

		public const string LiteralNull = "null";

		public const string LiteralUndefined = "undefined";

		public const string LiteralNotANumber = "NaN";

		public const string LiteralPositiveInfinity = "Infinity";

		public const string LiteralNegativeInfinity = "-Infinity";

		public const char OperatorNegate = '-';

		public const char OperatorUnaryPlus = '+';

		public const char OperatorArrayStart = '[';

		public const char OperatorArrayEnd = ']';

		public const char OperatorObjectStart = '{';

		public const char OperatorObjectEnd = '}';

		public const char OperatorStringDelim = '"';

		public const char OperatorStringDelimAlt = '\'';

		public const char OperatorValueDelim = ',';

		public const char OperatorNameDelim = ':';

		public const char OperatorCharEscape = '\\';

		protected const string CommentStart = "/*";

		protected const string CommentEnd = "*/";

		protected const string CommentLine = "//";

		protected const string LineEndings = "\r\n";

		public const string TypeGenericIDictionary = "System.Collections.Generic.IDictionary`2";

		protected const string ErrorUnrecognizedToken = "Illegal JSON sequence.";

		protected const string ErrorUnterminatedComment = "Unterminated comment block.";

		protected const string ErrorUnterminatedObject = "Unterminated JSON object.";

		protected const string ErrorUnterminatedArray = "Unterminated JSON array.";

		protected const string ErrorUnterminatedString = "Unterminated JSON string.";

		protected const string ErrorIllegalNumber = "Illegal JSON number.";

		protected const string ErrorExpectedString = "Expected JSON string.";

		protected const string ErrorExpectedObject = "Expected JSON object.";

		protected const string ErrorExpectedArray = "Expected JSON array.";

		protected const string ErrorExpectedPropertyName = "Expected JSON object property name.";

		protected const string ErrorExpectedPropertyNameDelim = "Expected JSON object property name delimiter.";

		protected const string ErrorGenericIDictionary = "Types which implement Generic IDictionary<TKey, TValue> also need to implement IDictionary to be deserialized. ({0})";

		protected const string ErrorGenericIDictionaryKeys = "Types which implement Generic IDictionary<TKey, TValue> need to have string keys to be deserialized. ({0})";

		protected readonly JsonReaderSettings Settings;

		protected readonly string Source;

		protected readonly int SourceLength;

		protected int index;

		public JsonReader(TextReader input) : this(input, new JsonReaderSettings())
		{
		}

		public JsonReader(TextReader input, JsonReaderSettings settings)
		{
			this.Settings = new JsonReaderSettings();
			base..ctor();
			this.Settings = settings;
			this.Source = input.ReadToEnd();
			this.SourceLength = this.Source.Length;
		}

		public JsonReader(Stream input) : this(input, new JsonReaderSettings())
		{
		}

		public JsonReader(Stream input, JsonReaderSettings settings)
		{
			this.Settings = new JsonReaderSettings();
			base..ctor();
			this.Settings = settings;
			using (StreamReader streamReader = new StreamReader(input, true))
			{
				this.Source = streamReader.ReadToEnd();
			}
			this.SourceLength = this.Source.Length;
		}

		public JsonReader(string input) : this(input, new JsonReaderSettings())
		{
		}

		public JsonReader(string input, JsonReaderSettings settings)
		{
			this.Settings = new JsonReaderSettings();
			base..ctor();
			this.Settings = settings;
			this.Source = input;
			this.SourceLength = this.Source.Length;
		}

		public JsonReader(StringBuilder input) : this(input, new JsonReaderSettings())
		{
		}

		public JsonReader(StringBuilder input, JsonReaderSettings settings)
		{
			this.Settings = new JsonReaderSettings();
			base..ctor();
			this.Settings = settings;
			this.Source = input.ToString();
			this.SourceLength = this.Source.Length;
		}

		[Obsolete("This has been deprecated in favor of JsonReaderSettings object")]
		public bool AllowNullValueTypes
		{
			get
			{
				return this.Settings.AllowNullValueTypes;
			}
			set
			{
				this.Settings.AllowNullValueTypes = value;
			}
		}

		[Obsolete("This has been deprecated in favor of JsonReaderSettings object")]
		public string TypeHintName
		{
			get
			{
				return this.Settings.TypeHintName;
			}
			set
			{
				this.Settings.TypeHintName = value;
			}
		}

		public object Deserialize()
		{
			return this.Deserialize(null);
		}

		public object Deserialize(int start)
		{
			this.index = start;
			return this.Deserialize(null);
		}

		public object Deserialize(Type type)
		{
			return this.Read(type, false);
		}

		public T Deserialize<T>()
		{
			return (T)((object)this.Read(typeof(T), false));
		}

		public object Deserialize(int start, Type type)
		{
			this.index = start;
			return this.Read(type, false);
		}

		public T Deserialize<T>(int start)
		{
			this.index = start;
			return (T)((object)this.Read(typeof(T), false));
		}

		protected virtual object Read(Type expectedType, bool typeIsHint)
		{
			if (expectedType == typeof(object))
			{
				expectedType = null;
			}
			switch (this.Tokenize())
			{
			case JsonToken.Undefined:
				this.index += "undefined".Length;
				return null;
			case JsonToken.Null:
				this.index += "null".Length;
				return null;
			case JsonToken.False:
				this.index += "false".Length;
				return false;
			case JsonToken.True:
				this.index += "true".Length;
				return true;
			case JsonToken.NaN:
				this.index += "NaN".Length;
				return double.NaN;
			case JsonToken.PositiveInfinity:
				this.index += "Infinity".Length;
				return double.PositiveInfinity;
			case JsonToken.NegativeInfinity:
				this.index += "-Infinity".Length;
				return double.NegativeInfinity;
			case JsonToken.Number:
				return this.ReadNumber((!typeIsHint) ? expectedType : null);
			case JsonToken.String:
				return this.ReadString((!typeIsHint) ? expectedType : null);
			case JsonToken.ArrayStart:
				return this.ReadArray((!typeIsHint) ? expectedType : null);
			case JsonToken.ObjectStart:
				return this.ReadObject((!typeIsHint) ? expectedType : null);
			}
			return null;
		}

		protected virtual object ReadObject(Type objectType)
		{
			if (this.Source[this.index] != '{')
			{
				throw new JsonDeserializationException("Expected JSON object.", this.index);
			}
			Type type = null;
			Dictionary<string, MemberInfo> dictionary = null;
			object obj;
			if (objectType != null)
			{
				obj = this.Settings.Coercion.InstantiateObject(objectType, out dictionary);
				if (dictionary == null)
				{
					Type @interface = objectType.GetInterface("System.Collections.Generic.IDictionary`2");
					if (@interface != null)
					{
						Type[] genericArguments = @interface.GetGenericArguments();
						if (genericArguments.Length == 2)
						{
							if (genericArguments[0] != typeof(string))
							{
								throw new JsonDeserializationException(string.Format("Types which implement Generic IDictionary<TKey, TValue> need to have string keys to be deserialized. ({0})", objectType), this.index);
							}
							if (genericArguments[1] != typeof(object))
							{
								type = genericArguments[1];
							}
						}
					}
				}
			}
			else
			{
				obj = new Dictionary<string, object>();
			}
			JsonToken jsonToken;
			for (;;)
			{
				this.index++;
				if (this.index >= this.SourceLength)
				{
					break;
				}
				jsonToken = this.Tokenize(this.Settings.AllowUnquotedObjectKeys);
				if (jsonToken == JsonToken.ObjectEnd)
				{
					goto Block_9;
				}
				if (jsonToken != JsonToken.String && jsonToken != JsonToken.UnquotedName)
				{
					goto Block_11;
				}
				string text = (jsonToken != JsonToken.String) ? this.ReadUnquotedKey() : ((string)this.ReadString(null));
				MemberInfo memberInfo;
				Type type2;
				if (type == null && dictionary != null)
				{
					type2 = TypeCoercionUtility.GetMemberInfo(dictionary, text, out memberInfo);
				}
				else
				{
					type2 = type;
					memberInfo = null;
				}
				jsonToken = this.Tokenize();
				if (jsonToken != JsonToken.NameDelim)
				{
					goto Block_15;
				}
				this.index++;
				if (this.index >= this.SourceLength)
				{
					goto Block_16;
				}
				object obj2 = this.Read(type2, false);
				if (obj is IDictionary)
				{
					if (objectType == null && this.Settings.IsTypeHintName(text))
					{
						obj = this.Settings.Coercion.ProcessTypeHint((IDictionary)obj, obj2 as string, out objectType, out dictionary);
					}
					else
					{
						((IDictionary)obj)[text] = obj2;
					}
				}
				else
				{
					if (objectType.GetInterface("System.Collections.Generic.IDictionary`2") != null)
					{
						goto Block_20;
					}
					this.Settings.Coercion.SetMemberValue(obj, type2, memberInfo, obj2);
				}
				jsonToken = this.Tokenize();
				if (jsonToken != JsonToken.ValueDelim)
				{
					goto IL_281;
				}
			}
			throw new JsonDeserializationException("Unterminated JSON object.", this.index);
			Block_9:
			goto IL_281;
			Block_11:
			throw new JsonDeserializationException("Expected JSON object property name.", this.index);
			Block_15:
			throw new JsonDeserializationException("Expected JSON object property name delimiter.", this.index);
			Block_16:
			throw new JsonDeserializationException("Unterminated JSON object.", this.index);
			Block_20:
			throw new JsonDeserializationException(string.Format("Types which implement Generic IDictionary<TKey, TValue> also need to implement IDictionary to be deserialized. ({0})", objectType), this.index);
			IL_281:
			if (jsonToken != JsonToken.ObjectEnd)
			{
				throw new JsonDeserializationException("Unterminated JSON object.", this.index);
			}
			this.index++;
			return obj;
		}

		protected IEnumerable ReadArray(Type arrayType)
		{
			if (this.Source[this.index] != '[')
			{
				throw new JsonDeserializationException("Expected JSON array.", this.index);
			}
			bool flag = arrayType != null;
			bool typeIsHint = !flag;
			Type type = null;
			if (flag)
			{
				if (arrayType.HasElementType)
				{
					type = arrayType.GetElementType();
				}
				else if (arrayType.IsGenericType)
				{
					Type[] genericArguments = arrayType.GetGenericArguments();
					if (genericArguments.Length == 1)
					{
						type = genericArguments[0];
					}
				}
			}
			ArrayList arrayList = new ArrayList();
			JsonToken jsonToken;
			for (;;)
			{
				this.index++;
				if (this.index >= this.SourceLength)
				{
					break;
				}
				jsonToken = this.Tokenize();
				if (jsonToken == JsonToken.ArrayEnd)
				{
					goto Block_7;
				}
				object obj = this.Read(type, typeIsHint);
				arrayList.Add(obj);
				if (obj == null)
				{
					if (type != null && type.IsValueType)
					{
						type = null;
					}
					flag = true;
				}
				else if (type != null && !type.IsAssignableFrom(obj.GetType()))
				{
					if (obj.GetType().IsAssignableFrom(type))
					{
						type = obj.GetType();
					}
					else
					{
						type = null;
						flag = true;
					}
				}
				else if (!flag)
				{
					type = obj.GetType();
					flag = true;
				}
				jsonToken = this.Tokenize();
				if (jsonToken != JsonToken.ValueDelim)
				{
					goto IL_157;
				}
			}
			throw new JsonDeserializationException("Unterminated JSON array.", this.index);
			Block_7:
			IL_157:
			if (jsonToken != JsonToken.ArrayEnd)
			{
				throw new JsonDeserializationException("Unterminated JSON array.", this.index);
			}
			this.index++;
			if (type != null && type != typeof(object))
			{
				return arrayList.ToArray(type);
			}
			return arrayList.ToArray();
		}

		protected string ReadUnquotedKey()
		{
			int num = this.index;
			do
			{
				this.index++;
			}
			while (this.Tokenize(true) == JsonToken.UnquotedName);
			return this.Source.Substring(num, this.index - num);
		}

		protected object ReadString(Type expectedType)
		{
			if (this.Source[this.index] != '"' && this.Source[this.index] != '\'')
			{
				throw new JsonDeserializationException("Expected JSON string.", this.index);
			}
			char c = this.Source[this.index];
			this.index++;
			if (this.index >= this.SourceLength)
			{
				throw new JsonDeserializationException("Unterminated JSON string.", this.index);
			}
			int num = this.index;
			StringBuilder stringBuilder = new StringBuilder();
			while (this.Source[this.index] != c)
			{
				if (this.Source[this.index] == '\\')
				{
					stringBuilder.Append(this.Source, num, this.index - num);
					this.index++;
					if (this.index >= this.SourceLength)
					{
						throw new JsonDeserializationException("Unterminated JSON string.", this.index);
					}
					char c2 = this.Source[this.index];
					switch (c2)
					{
					case 'n':
						stringBuilder.Append('\n');
						break;
					default:
						if (c2 != '0')
						{
							if (c2 != 'b')
							{
								if (c2 != 'f')
								{
									stringBuilder.Append(this.Source[this.index]);
								}
								else
								{
									stringBuilder.Append('\f');
								}
							}
							else
							{
								stringBuilder.Append('\b');
							}
						}
						break;
					case 'r':
						stringBuilder.Append('\r');
						break;
					case 't':
						stringBuilder.Append('\t');
						break;
					case 'u':
					{
						int utf;
						if (this.index + 4 < this.SourceLength && int.TryParse(this.Source.Substring(this.index + 1, 4), NumberStyles.AllowHexSpecifier, NumberFormatInfo.InvariantInfo, out utf))
						{
							stringBuilder.Append(char.ConvertFromUtf32(utf));
							this.index += 4;
						}
						else
						{
							stringBuilder.Append(this.Source[this.index]);
						}
						break;
					}
					}
					this.index++;
					if (this.index >= this.SourceLength)
					{
						throw new JsonDeserializationException("Unterminated JSON string.", this.index);
					}
					num = this.index;
				}
				else
				{
					this.index++;
					if (this.index >= this.SourceLength)
					{
						throw new JsonDeserializationException("Unterminated JSON string.", this.index);
					}
				}
			}
			stringBuilder.Append(this.Source, num, this.index - num);
			this.index++;
			if (expectedType != null && expectedType != typeof(string))
			{
				return this.Settings.Coercion.CoerceType(expectedType, stringBuilder.ToString());
			}
			return stringBuilder.ToString();
		}

		protected object ReadNumber(Type expectedType)
		{
			bool flag = false;
			bool flag2 = false;
			int num = this.index;
			int num2 = 0;
			if (this.Source[this.index] == '-')
			{
				this.index++;
				if (this.index >= this.SourceLength || !char.IsDigit(this.Source[this.index]))
				{
					throw new JsonDeserializationException("Illegal JSON number.", this.index);
				}
			}
			while (this.index < this.SourceLength && char.IsDigit(this.Source[this.index]))
			{
				this.index++;
			}
			if (this.index < this.SourceLength && this.Source[this.index] == '.')
			{
				flag = true;
				this.index++;
				if (this.index >= this.SourceLength || !char.IsDigit(this.Source[this.index]))
				{
					throw new JsonDeserializationException("Illegal JSON number.", this.index);
				}
				while (this.index < this.SourceLength && char.IsDigit(this.Source[this.index]))
				{
					this.index++;
				}
			}
			int num3 = this.index - num - ((!flag) ? 0 : 1);
			if (this.index < this.SourceLength && (this.Source[this.index] == 'e' || this.Source[this.index] == 'E'))
			{
				flag2 = true;
				this.index++;
				if (this.index >= this.SourceLength)
				{
					throw new JsonDeserializationException("Illegal JSON number.", this.index);
				}
				int num4 = this.index;
				if (this.Source[this.index] == '-' || this.Source[this.index] == '+')
				{
					this.index++;
					if (this.index >= this.SourceLength || !char.IsDigit(this.Source[this.index]))
					{
						throw new JsonDeserializationException("Illegal JSON number.", this.index);
					}
				}
				else if (!char.IsDigit(this.Source[this.index]))
				{
					throw new JsonDeserializationException("Illegal JSON number.", this.index);
				}
				while (this.index < this.SourceLength && char.IsDigit(this.Source[this.index]))
				{
					this.index++;
				}
				int.TryParse(this.Source.Substring(num4, this.index - num4), NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out num2);
			}
			string s = this.Source.Substring(num, this.index - num);
			if (!flag && !flag2 && num3 < 19)
			{
				decimal num5 = decimal.Parse(s, NumberStyles.Integer, NumberFormatInfo.InvariantInfo);
				if (expectedType != null)
				{
					return this.Settings.Coercion.CoerceType(expectedType, num5);
				}
				if (num5 >= -2147483648m && num5 <= 2147483647m)
				{
					return (int)num5;
				}
				if (num5 >= -9223372036854775808m && num5 <= 9223372036854775807m)
				{
					return (long)num5;
				}
				return num5;
			}
			else
			{
				if (expectedType == typeof(decimal))
				{
					return decimal.Parse(s, NumberStyles.Float, NumberFormatInfo.InvariantInfo);
				}
				double num6 = double.Parse(s, NumberStyles.Float, NumberFormatInfo.InvariantInfo);
				if (expectedType != null)
				{
					return this.Settings.Coercion.CoerceType(expectedType, num6);
				}
				return num6;
			}
		}

		public static object Deserialize(string value)
		{
			return JsonReader.Deserialize(value, 0, null);
		}

		public static T Deserialize<T>(string value)
		{
			return (T)((object)JsonReader.Deserialize(value, 0, typeof(T)));
		}

		public static object Deserialize(string value, int start)
		{
			return JsonReader.Deserialize(value, start, null);
		}

		public static T Deserialize<T>(string value, int start)
		{
			return (T)((object)JsonReader.Deserialize(value, start, typeof(T)));
		}

		public static object Deserialize(string value, Type type)
		{
			return JsonReader.Deserialize(value, 0, type);
		}

		public static object Deserialize(string value, int start, Type type)
		{
			return new JsonReader(value).Deserialize(start, type);
		}

		protected JsonToken Tokenize()
		{
			return this.Tokenize(false);
		}

		protected JsonToken Tokenize(bool allowUnquotedString)
		{
			if (this.index >= this.SourceLength)
			{
				return JsonToken.End;
			}
			while (char.IsWhiteSpace(this.Source[this.index]))
			{
				this.index++;
				if (this.index >= this.SourceLength)
				{
					return JsonToken.End;
				}
			}
			if (this.Source[this.index] == "/*"[0])
			{
				if (this.index + 1 >= this.SourceLength)
				{
					throw new JsonDeserializationException("Illegal JSON sequence.", this.index);
				}
				this.index++;
				bool flag = false;
				if (this.Source[this.index] == "/*"[1])
				{
					flag = true;
				}
				else if (this.Source[this.index] != "//"[1])
				{
					throw new JsonDeserializationException("Illegal JSON sequence.", this.index);
				}
				this.index++;
				if (flag)
				{
					int num = this.index - 2;
					if (this.index + 1 >= this.SourceLength)
					{
						throw new JsonDeserializationException("Unterminated comment block.", num);
					}
					while (this.Source[this.index] != "*/"[0] || this.Source[this.index + 1] != "*/"[1])
					{
						this.index++;
						if (this.index + 1 >= this.SourceLength)
						{
							throw new JsonDeserializationException("Unterminated comment block.", num);
						}
					}
					this.index += 2;
					if (this.index >= this.SourceLength)
					{
						return JsonToken.End;
					}
				}
				else
				{
					while ("\r\n".IndexOf(this.Source[this.index]) < 0)
					{
						this.index++;
						if (this.index >= this.SourceLength)
						{
							return JsonToken.End;
						}
					}
				}
				while (char.IsWhiteSpace(this.Source[this.index]))
				{
					this.index++;
					if (this.index >= this.SourceLength)
					{
						return JsonToken.End;
					}
				}
			}
			if (this.Source[this.index] == '+')
			{
				this.index++;
				if (this.index >= this.SourceLength)
				{
					return JsonToken.End;
				}
			}
			char c = this.Source[this.index];
			switch (c)
			{
			case '[':
				return JsonToken.ArrayStart;
			default:
				switch (c)
				{
				case '{':
					return JsonToken.ObjectStart;
				default:
					if (c == '"' || c == '\'')
					{
						return JsonToken.String;
					}
					if (c == ',')
					{
						return JsonToken.ValueDelim;
					}
					if (c == ':')
					{
						return JsonToken.NameDelim;
					}
					if (char.IsDigit(this.Source[this.index]) || (this.Source[this.index] == '-' && this.index + 1 < this.SourceLength && char.IsDigit(this.Source[this.index + 1])))
					{
						return JsonToken.Number;
					}
					if (this.MatchLiteral("false"))
					{
						return JsonToken.False;
					}
					if (this.MatchLiteral("true"))
					{
						return JsonToken.True;
					}
					if (this.MatchLiteral("null"))
					{
						return JsonToken.Null;
					}
					if (this.MatchLiteral("NaN"))
					{
						return JsonToken.NaN;
					}
					if (this.MatchLiteral("Infinity"))
					{
						return JsonToken.PositiveInfinity;
					}
					if (this.MatchLiteral("-Infinity"))
					{
						return JsonToken.NegativeInfinity;
					}
					if (this.MatchLiteral("undefined"))
					{
						return JsonToken.Undefined;
					}
					if (allowUnquotedString)
					{
						return JsonToken.UnquotedName;
					}
					throw new JsonDeserializationException("Illegal JSON sequence.", this.index);
				case '}':
					return JsonToken.ObjectEnd;
				}
				break;
			case ']':
				return JsonToken.ArrayEnd;
			}
		}

		protected bool MatchLiteral(string literal)
		{
			int length = literal.Length;
			int num = 0;
			int num2 = this.index;
			while (num < length && num2 < this.SourceLength)
			{
				if (literal[num] != this.Source[num2])
				{
					return false;
				}
				num++;
				num2++;
			}
			return true;
		}

		public static T CoerceType<T>(object value, T typeToMatch)
		{
			return (T)((object)new TypeCoercionUtility().CoerceType(typeof(T), value));
		}

		public static T CoerceType<T>(object value)
		{
			return (T)((object)new TypeCoercionUtility().CoerceType(typeof(T), value));
		}

		public static object CoerceType(Type targetType, object value)
		{
			return new TypeCoercionUtility().CoerceType(targetType, value);
		}
	}
}
