using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace com.adjust.sdk
{
	public class JSONNode
	{
		public virtual void Add(string aKey, JSONNode aItem)
		{
		}

		public virtual JSONNode this[int aIndex]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public virtual JSONNode this[string aKey]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public virtual string Value
		{
			get
			{
				return string.Empty;
			}
			set
			{
			}
		}

		public virtual int Count
		{
			get
			{
				return 0;
			}
		}

		public virtual void Add(JSONNode aItem)
		{
			this.Add(string.Empty, aItem);
		}

		public virtual JSONNode Remove(string aKey)
		{
			return null;
		}

		public virtual JSONNode Remove(int aIndex)
		{
			return null;
		}

		public virtual JSONNode Remove(JSONNode aNode)
		{
			return aNode;
		}

		public virtual IEnumerable<JSONNode> Childs
		{
			get
			{
				yield break;
			}
		}

		public IEnumerable<JSONNode> DeepChilds
		{
			get
			{
				foreach (JSONNode C in this.Childs)
				{
					foreach (JSONNode D in C.DeepChilds)
					{
						yield return D;
					}
				}
				yield break;
			}
		}

		public override string ToString()
		{
			return "JSONNode";
		}

		public virtual string ToString(string aPrefix)
		{
			return "JSONNode";
		}

		public virtual int AsInt
		{
			get
			{
				int result = 0;
				if (int.TryParse(this.Value, out result))
				{
					return result;
				}
				return 0;
			}
			set
			{
				this.Value = value.ToString();
			}
		}

		public virtual float AsFloat
		{
			get
			{
				float result = 0f;
				if (float.TryParse(this.Value, out result))
				{
					return result;
				}
				return 0f;
			}
			set
			{
				this.Value = value.ToString();
			}
		}

		public virtual double AsDouble
		{
			get
			{
				double result = 0.0;
				if (double.TryParse(this.Value, out result))
				{
					return result;
				}
				return 0.0;
			}
			set
			{
				this.Value = value.ToString();
			}
		}

		public virtual bool AsBool
		{
			get
			{
				bool result = false;
				if (bool.TryParse(this.Value, out result))
				{
					return result;
				}
				return !string.IsNullOrEmpty(this.Value);
			}
			set
			{
				this.Value = ((!value) ? "false" : "true");
			}
		}

		public virtual JSONArray AsArray
		{
			get
			{
				return this as JSONArray;
			}
		}

		public virtual JSONClass AsObject
		{
			get
			{
				return this as JSONClass;
			}
		}

		public override bool Equals(object obj)
		{
			return object.ReferenceEquals(this, obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		internal static string Escape(string aText)
		{
			string text = string.Empty;
			foreach (char c in aText)
			{
				char c2 = c;
				switch (c2)
				{
				case '\b':
					text += "\\b";
					break;
				case '\t':
					text += "\\t";
					break;
				case '\n':
					text += "\\n";
					break;
				default:
					if (c2 != '"')
					{
						if (c2 != '\\')
						{
							text += c;
						}
						else
						{
							text += "\\\\";
						}
					}
					else
					{
						text += "\\\"";
					}
					break;
				case '\f':
					text += "\\f";
					break;
				case '\r':
					text += "\\r";
					break;
				}
			}
			return text;
		}

		public static JSONNode Parse(string aJSON)
		{
			Stack<JSONNode> stack = new Stack<JSONNode>();
			JSONNode jsonnode = null;
			int i = 0;
			string text = string.Empty;
			string text2 = string.Empty;
			bool flag = false;
			while (i < aJSON.Length)
			{
				char c = aJSON[i];
				switch (c)
				{
				case '\t':
					goto IL_333;
				case '\n':
				case '\r':
					break;
				default:
					switch (c)
					{
					case ' ':
						goto IL_333;
					default:
						switch (c)
						{
						case '[':
							if (flag)
							{
								text += aJSON[i];
								goto IL_467;
							}
							stack.Push(new JSONArray());
							if (jsonnode != null)
							{
								text2 = text2.Trim();
								if (jsonnode is JSONArray)
								{
									jsonnode.Add(stack.Peek());
								}
								else if (text2 != string.Empty)
								{
									jsonnode.Add(text2, stack.Peek());
								}
							}
							text2 = string.Empty;
							text = string.Empty;
							jsonnode = stack.Peek();
							goto IL_467;
						case '\\':
							i++;
							if (flag)
							{
								char c2 = aJSON[i];
								char c3 = c2;
								switch (c3)
								{
								case 'n':
									text += '\n';
									break;
								default:
									if (c3 != 'b')
									{
										if (c3 != 'f')
										{
											text += c2;
										}
										else
										{
											text += '\f';
										}
									}
									else
									{
										text += '\b';
									}
									break;
								case 'r':
									text += '\r';
									break;
								case 't':
									text += '\t';
									break;
								case 'u':
								{
									string s = aJSON.Substring(i + 1, 4);
									text += (char)int.Parse(s, NumberStyles.AllowHexSpecifier);
									i += 4;
									break;
								}
								}
							}
							goto IL_467;
						case ']':
							break;
						default:
							switch (c)
							{
							case '{':
								if (flag)
								{
									text += aJSON[i];
									goto IL_467;
								}
								stack.Push(new JSONClass());
								if (jsonnode != null)
								{
									text2 = text2.Trim();
									if (jsonnode is JSONArray)
									{
										jsonnode.Add(stack.Peek());
									}
									else if (text2 != string.Empty)
									{
										jsonnode.Add(text2, stack.Peek());
									}
								}
								text2 = string.Empty;
								text = string.Empty;
								jsonnode = stack.Peek();
								goto IL_467;
							default:
								if (c != ',')
								{
									if (c != ':')
									{
										text += aJSON[i];
										goto IL_467;
									}
									if (flag)
									{
										text += aJSON[i];
										goto IL_467;
									}
									text2 = text;
									text = string.Empty;
									goto IL_467;
								}
								else
								{
									if (flag)
									{
										text += aJSON[i];
										goto IL_467;
									}
									if (text != string.Empty)
									{
										if (jsonnode is JSONArray)
										{
											jsonnode.Add(text);
										}
										else if (text2 != string.Empty)
										{
											jsonnode.Add(text2, text);
										}
									}
									text2 = string.Empty;
									text = string.Empty;
									goto IL_467;
								}
								break;
							case '}':
								break;
							}
							break;
						}
						if (flag)
						{
							text += aJSON[i];
						}
						else
						{
							if (stack.Count == 0)
							{
								throw new Exception("JSON Parse: Too many closing brackets");
							}
							stack.Pop();
							if (text != string.Empty)
							{
								text2 = text2.Trim();
								if (jsonnode is JSONArray)
								{
									jsonnode.Add(text);
								}
								else if (text2 != string.Empty)
								{
									jsonnode.Add(text2, text);
								}
							}
							text2 = string.Empty;
							text = string.Empty;
							if (stack.Count > 0)
							{
								jsonnode = stack.Peek();
							}
						}
						break;
					case '"':
						flag ^= true;
						break;
					}
					break;
				}
				IL_467:
				i++;
				continue;
				IL_333:
				if (flag)
				{
					text += aJSON[i];
				}
				goto IL_467;
			}
			if (flag)
			{
				throw new Exception("JSON Parse: Quotation marks seems to be messed up.");
			}
			return jsonnode;
		}

		public virtual void Serialize(BinaryWriter aWriter)
		{
		}

		public void SaveToStream(Stream aData)
		{
			BinaryWriter aWriter = new BinaryWriter(aData);
			this.Serialize(aWriter);
		}

		public void SaveToCompressedStream(Stream aData)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		public void SaveToCompressedFile(string aFileName)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		public string SaveToCompressedBase64()
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		public static JSONNode Deserialize(BinaryReader aReader)
		{
			JSONBinaryTag jsonbinaryTag = (JSONBinaryTag)aReader.ReadByte();
			switch (jsonbinaryTag)
			{
			case JSONBinaryTag.Array:
			{
				int num = aReader.ReadInt32();
				JSONArray jsonarray = new JSONArray();
				for (int i = 0; i < num; i++)
				{
					jsonarray.Add(JSONNode.Deserialize(aReader));
				}
				return jsonarray;
			}
			case JSONBinaryTag.Class:
			{
				int num2 = aReader.ReadInt32();
				JSONClass jsonclass = new JSONClass();
				for (int j = 0; j < num2; j++)
				{
					string aKey = aReader.ReadString();
					JSONNode aItem = JSONNode.Deserialize(aReader);
					jsonclass.Add(aKey, aItem);
				}
				return jsonclass;
			}
			case JSONBinaryTag.Value:
				return new JSONData(aReader.ReadString());
			case JSONBinaryTag.IntValue:
				return new JSONData(aReader.ReadInt32());
			case JSONBinaryTag.DoubleValue:
				return new JSONData(aReader.ReadDouble());
			case JSONBinaryTag.BoolValue:
				return new JSONData(aReader.ReadBoolean());
			case JSONBinaryTag.FloatValue:
				return new JSONData(aReader.ReadSingle());
			default:
				throw new Exception("Error deserializing JSON. Unknown tag: " + jsonbinaryTag);
			}
		}

		public static JSONNode LoadFromCompressedFile(string aFileName)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		public static JSONNode LoadFromCompressedStream(Stream aData)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		public static JSONNode LoadFromCompressedBase64(string aBase64)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		public static JSONNode LoadFromStream(Stream aData)
		{
			JSONNode result;
			using (BinaryReader binaryReader = new BinaryReader(aData))
			{
				result = JSONNode.Deserialize(binaryReader);
			}
			return result;
		}

		public static JSONNode LoadFromBase64(string aBase64)
		{
			byte[] buffer = Convert.FromBase64String(aBase64);
			return JSONNode.LoadFromStream(new MemoryStream(buffer)
			{
				Position = 0L
			});
		}

		public static implicit operator JSONNode(string s)
		{
			return new JSONData(s);
		}

		public static implicit operator string(JSONNode d)
		{
			return (!(d == null)) ? d.Value : null;
		}

		public static bool operator ==(JSONNode a, object b)
		{
			return (b == null && a is JSONLazyCreator) || object.ReferenceEquals(a, b);
		}

		public static bool operator !=(JSONNode a, object b)
		{
			return !(a == b);
		}
	}
}
