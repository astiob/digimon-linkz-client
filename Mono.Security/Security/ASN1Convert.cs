using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Mono.Security
{
	public static class ASN1Convert
	{
		public static ASN1 FromDateTime(DateTime dt)
		{
			if (dt.Year < 2050)
			{
				return new ASN1(23, Encoding.ASCII.GetBytes(dt.ToUniversalTime().ToString("yyMMddHHmmss", CultureInfo.InvariantCulture) + "Z"));
			}
			return new ASN1(24, Encoding.ASCII.GetBytes(dt.ToUniversalTime().ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture) + "Z"));
		}

		public static ASN1 FromInt32(int value)
		{
			byte[] bytes = BitConverterLE.GetBytes(value);
			Array.Reverse(bytes);
			int num = 0;
			while (num < bytes.Length && bytes[num] == 0)
			{
				num++;
			}
			ASN1 asn = new ASN1(2);
			int num2 = num;
			if (num2 != 0)
			{
				if (num2 != 4)
				{
					byte[] array = new byte[4 - num];
					Buffer.BlockCopy(bytes, num, array, 0, array.Length);
					asn.Value = array;
				}
				else
				{
					asn.Value = new byte[1];
				}
			}
			else
			{
				asn.Value = bytes;
			}
			return asn;
		}

		public static ASN1 FromOid(string oid)
		{
			if (oid == null)
			{
				throw new ArgumentNullException("oid");
			}
			return new ASN1(CryptoConfig.EncodeOID(oid));
		}

		public static ASN1 FromUnsignedBigInteger(byte[] big)
		{
			if (big == null)
			{
				throw new ArgumentNullException("big");
			}
			if (big[0] >= 128)
			{
				int num = big.Length + 1;
				byte[] array = new byte[num];
				Buffer.BlockCopy(big, 0, array, 1, num - 1);
				big = array;
			}
			return new ASN1(2, big);
		}

		public static int ToInt32(ASN1 asn1)
		{
			if (asn1 == null)
			{
				throw new ArgumentNullException("asn1");
			}
			if (asn1.Tag != 2)
			{
				throw new FormatException("Only integer can be converted");
			}
			int num = 0;
			for (int i = 0; i < asn1.Value.Length; i++)
			{
				num = (num << 8) + (int)asn1.Value[i];
			}
			return num;
		}

		public static string ToOid(ASN1 asn1)
		{
			if (asn1 == null)
			{
				throw new ArgumentNullException("asn1");
			}
			byte[] value = asn1.Value;
			StringBuilder stringBuilder = new StringBuilder();
			byte b = value[0] / 40;
			byte b2 = value[0] % 40;
			if (b > 2)
			{
				b2 += (b - 2) * 40;
				b = 2;
			}
			stringBuilder.Append(b.ToString(CultureInfo.InvariantCulture));
			stringBuilder.Append(".");
			stringBuilder.Append(b2.ToString(CultureInfo.InvariantCulture));
			ulong num = 0UL;
			b = 1;
			while ((int)b < value.Length)
			{
				num = (num << 7 | (ulong)(value[(int)b] & 127));
				if ((value[(int)b] & 128) != 128)
				{
					stringBuilder.Append(".");
					stringBuilder.Append(num.ToString(CultureInfo.InvariantCulture));
					num = 0UL;
				}
				b += 1;
			}
			return stringBuilder.ToString();
		}

		public static DateTime ToDateTime(ASN1 time)
		{
			if (time == null)
			{
				throw new ArgumentNullException("time");
			}
			string text = Encoding.ASCII.GetString(time.Value);
			string format = null;
			switch (text.Length)
			{
			case 11:
				format = "yyMMddHHmmZ";
				break;
			case 13:
			{
				int num = (int)Convert.ToInt16(text.Substring(0, 2), CultureInfo.InvariantCulture);
				if (num >= 50)
				{
					text = "19" + text;
				}
				else
				{
					text = "20" + text;
				}
				format = "yyyyMMddHHmmssZ";
				break;
			}
			case 15:
				format = "yyyyMMddHHmmssZ";
				break;
			case 17:
			{
				int num = (int)Convert.ToInt16(text.Substring(0, 2), CultureInfo.InvariantCulture);
				string text2 = (num < 50) ? "20" : "19";
				char c = (text[12] != '+') ? '+' : '-';
				text = string.Format("{0}{1}{2}{3}{4}:{5}{6}", new object[]
				{
					text2,
					text.Substring(0, 12),
					c,
					text[13],
					text[14],
					text[15],
					text[16]
				});
				format = "yyyyMMddHHmmsszzz";
				break;
			}
			}
			return DateTime.ParseExact(text, format, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
		}
	}
}
