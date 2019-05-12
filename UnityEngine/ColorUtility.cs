using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>A collection of common color functions.</para>
	/// </summary>
	public sealed class ColorUtility
	{
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool DoTryParseHtmlColor(string htmlString, out Color32 color);

		public static bool TryParseHtmlString(string htmlString, out Color color)
		{
			Color32 c;
			bool result = ColorUtility.DoTryParseHtmlColor(htmlString, out c);
			color = c;
			return result;
		}

		/// <summary>
		///   <para>Returns the color as a hexadecimal string in the format "RRGGBB".</para>
		/// </summary>
		/// <param name="color">The color to be converted.</param>
		/// <returns>
		///   <para>Hexadecimal string representing the color.</para>
		/// </returns>
		public static string ToHtmlStringRGB(Color color)
		{
			Color32 color2 = color;
			return string.Format("{0:X2}{1:X2}{2:X2}", color2.r, color2.g, color2.b);
		}

		/// <summary>
		///   <para>Returns the color as a hexadecimal string in the format "RRGGBBAA".</para>
		/// </summary>
		/// <param name="color">The color to be converted.</param>
		/// <returns>
		///   <para>Hexadecimal string representing the color.</para>
		/// </returns>
		public static string ToHtmlStringRGBA(Color color)
		{
			Color32 color2 = color;
			return string.Format("{0:X2}{1:X2}{2:X2}{3:X2}", new object[]
			{
				color2.r,
				color2.g,
				color2.b,
				color2.a
			});
		}
	}
}
