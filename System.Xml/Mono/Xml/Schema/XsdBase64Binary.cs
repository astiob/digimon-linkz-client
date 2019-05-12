using System;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
	internal class XsdBase64Binary : XsdString
	{
		private static string ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";

		private static byte[] decodeTable;

		internal XsdBase64Binary()
		{
		}

		static XsdBase64Binary()
		{
			int length = XsdBase64Binary.ALPHABET.Length;
			XsdBase64Binary.decodeTable = new byte[123];
			for (int i = 0; i < XsdBase64Binary.decodeTable.Length; i++)
			{
				XsdBase64Binary.decodeTable[i] = byte.MaxValue;
			}
			for (int j = 0; j < length; j++)
			{
				char c = XsdBase64Binary.ALPHABET[j];
				XsdBase64Binary.decodeTable[(int)c] = (byte)j;
			}
		}

		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.Base64Binary;
			}
		}

		public override Type ValueType
		{
			get
			{
				return typeof(byte[]);
			}
		}

		public override object ParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr)
		{
			byte[] bytes = new ASCIIEncoding().GetBytes(s);
			FromBase64Transform fromBase64Transform = new FromBase64Transform();
			return fromBase64Transform.TransformFinalBlock(bytes, 0, bytes.Length);
		}

		internal override int Length(string s)
		{
			int num = 0;
			int num2 = 0;
			int length = s.Length;
			for (int i = 0; i < length; i++)
			{
				char c = s[i];
				if (!char.IsWhiteSpace(c))
				{
					if (XsdBase64Binary.isData(c))
					{
						num++;
					}
					else
					{
						if (!XsdBase64Binary.isPad(c))
						{
							return -1;
						}
						num2++;
					}
				}
			}
			if (num2 > 2)
			{
				return -1;
			}
			if (num2 > 0)
			{
				num2 = 3 - num2;
			}
			return num / 4 * 3 + num2;
		}

		protected static bool isPad(char octect)
		{
			return octect == '=';
		}

		protected static bool isData(char octect)
		{
			return octect <= 'z' && XsdBase64Binary.decodeTable[(int)octect] != byte.MaxValue;
		}

		internal override ValueType ParseValueType(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr)
		{
			return new StringValueType(this.ParseValue(s, nameTable, nsmgr) as string);
		}
	}
}
