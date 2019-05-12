using System;
using System.Globalization;

namespace System.Xml.XPath
{
	internal class XPathFunctions
	{
		public static bool ToBoolean(object arg)
		{
			if (arg == null)
			{
				throw new ArgumentNullException();
			}
			if (arg is bool)
			{
				return (bool)arg;
			}
			if (arg is double)
			{
				double num = (double)arg;
				return num != 0.0 && !double.IsNaN(num);
			}
			if (arg is string)
			{
				return ((string)arg).Length != 0;
			}
			if (arg is XPathNodeIterator)
			{
				XPathNodeIterator xpathNodeIterator = (XPathNodeIterator)arg;
				return xpathNodeIterator.MoveNext();
			}
			if (arg is XPathNavigator)
			{
				return XPathFunctions.ToBoolean(((XPathNavigator)arg).SelectChildren(XPathNodeType.All));
			}
			throw new ArgumentException();
		}

		public static bool ToBoolean(bool b)
		{
			return b;
		}

		public static bool ToBoolean(double d)
		{
			return d != 0.0 && !double.IsNaN(d);
		}

		public static bool ToBoolean(string s)
		{
			return s != null && s.Length > 0;
		}

		public static bool ToBoolean(BaseIterator iter)
		{
			return iter != null && iter.MoveNext();
		}

		public static string ToString(object arg)
		{
			if (arg == null)
			{
				throw new ArgumentNullException();
			}
			if (arg is string)
			{
				return (string)arg;
			}
			if (arg is bool)
			{
				return (!(bool)arg) ? "false" : "true";
			}
			if (arg is double)
			{
				return XPathFunctions.ToString((double)arg);
			}
			if (arg is XPathNodeIterator)
			{
				XPathNodeIterator xpathNodeIterator = (XPathNodeIterator)arg;
				if (!xpathNodeIterator.MoveNext())
				{
					return string.Empty;
				}
				return xpathNodeIterator.Current.Value;
			}
			else
			{
				if (arg is XPathNavigator)
				{
					return ((XPathNavigator)arg).Value;
				}
				throw new ArgumentException();
			}
		}

		public static string ToString(double d)
		{
			if (d == double.NegativeInfinity)
			{
				return "-Infinity";
			}
			if (d == double.PositiveInfinity)
			{
				return "Infinity";
			}
			return d.ToString("R", NumberFormatInfo.InvariantInfo);
		}

		public static double ToNumber(object arg)
		{
			if (arg == null)
			{
				throw new ArgumentNullException();
			}
			if (arg is BaseIterator || arg is XPathNavigator)
			{
				arg = XPathFunctions.ToString(arg);
			}
			if (arg is string)
			{
				string arg2 = arg as string;
				return XPathFunctions.ToNumber(arg2);
			}
			if (arg is double)
			{
				return (double)arg;
			}
			if (arg is bool)
			{
				return Convert.ToDouble((bool)arg);
			}
			throw new ArgumentException();
		}

		public static double ToNumber(string arg)
		{
			if (arg == null)
			{
				throw new ArgumentNullException();
			}
			string text = arg.Trim(XmlChar.WhitespaceChars);
			if (text.Length == 0)
			{
				return double.NaN;
			}
			double result;
			try
			{
				if (text[0] == '.')
				{
					text = '.' + text;
				}
				result = double.Parse(text, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, NumberFormatInfo.InvariantInfo);
			}
			catch (OverflowException)
			{
				result = double.NaN;
			}
			catch (FormatException)
			{
				result = double.NaN;
			}
			return result;
		}
	}
}
