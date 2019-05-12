using System;
using System.Globalization;
using System.Text;

namespace System.Xml.Serialization
{
	internal class KeyHelper
	{
		public static void AddField(StringBuilder sb, int n, string val)
		{
			KeyHelper.AddField(sb, n, val, null);
		}

		public static void AddField(StringBuilder sb, int n, string val, string def)
		{
			if (val != def)
			{
				sb.Append(n.ToString());
				sb.Append(val.Length.ToString(CultureInfo.InvariantCulture));
				sb.Append(val);
			}
		}

		public static void AddField(StringBuilder sb, int n, bool val)
		{
			KeyHelper.AddField(sb, n, val, false);
		}

		public static void AddField(StringBuilder sb, int n, bool val, bool def)
		{
			if (val != def)
			{
				sb.Append(n.ToString());
			}
		}

		public static void AddField(StringBuilder sb, int n, int val, int def)
		{
			if (val != def)
			{
				sb.Append(n.ToString());
				sb.Append(val.ToString(CultureInfo.InvariantCulture));
			}
		}

		public static void AddField(StringBuilder sb, int n, Type val)
		{
			if (val != null)
			{
				sb.Append(n.ToString(CultureInfo.InvariantCulture));
				sb.Append(val.ToString());
			}
		}
	}
}
