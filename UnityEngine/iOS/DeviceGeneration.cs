using System;

namespace UnityEngine.iOS
{
	/// <summary>
	///   <para>iOS device generation.</para>
	/// </summary>
	public enum DeviceGeneration
	{
		Unknown,
		/// <summary>
		///   <para>iPhone, first generation.</para>
		/// </summary>
		iPhone,
		/// <summary>
		///   <para>iPhone, second generation.</para>
		/// </summary>
		iPhone3G,
		/// <summary>
		///   <para>iPhone, third generation.</para>
		/// </summary>
		iPhone3GS,
		/// <summary>
		///   <para>iPod Touch, first generation.</para>
		/// </summary>
		iPodTouch1Gen,
		/// <summary>
		///   <para>iPod Touch, second generation.</para>
		/// </summary>
		iPodTouch2Gen,
		/// <summary>
		///   <para>iPod Touch, third generation.</para>
		/// </summary>
		iPodTouch3Gen,
		/// <summary>
		///   <para>iPad, first generation.</para>
		/// </summary>
		iPad1Gen,
		/// <summary>
		///   <para>iPhone, fourth generation.</para>
		/// </summary>
		iPhone4,
		/// <summary>
		///   <para>iPod Touch, fourth generation.</para>
		/// </summary>
		iPodTouch4Gen,
		/// <summary>
		///   <para>iPad, second generation.</para>
		/// </summary>
		iPad2Gen,
		/// <summary>
		///   <para>iPhone, fifth generation.</para>
		/// </summary>
		iPhone4S,
		/// <summary>
		///   <para>iPad, third generation.</para>
		/// </summary>
		iPad3Gen,
		/// <summary>
		///   <para>iPhone5.</para>
		/// </summary>
		iPhone5,
		/// <summary>
		///   <para>iPod Touch, fifth generation.</para>
		/// </summary>
		iPodTouch5Gen,
		/// <summary>
		///   <para>iPadMini, first generation.</para>
		/// </summary>
		iPadMini1Gen,
		/// <summary>
		///   <para>iPad, fourth generation.</para>
		/// </summary>
		iPad4Gen,
		/// <summary>
		///   <para>iPhone 5C.</para>
		/// </summary>
		iPhone5C,
		/// <summary>
		///   <para>iPhone 5S.</para>
		/// </summary>
		iPhone5S,
		/// <summary>
		///   <para>iPad Air.</para>
		/// </summary>
		iPadAir1,
		/// <summary>
		///   <para>iPad Air (fifth generation).</para>
		/// </summary>
		[Obsolete("Please use iPadAir1 instead.")]
		iPad5Gen = 19,
		/// <summary>
		///   <para>iPadMini Retina (second generation).</para>
		/// </summary>
		iPadMini2Gen,
		/// <summary>
		///   <para>iPhone 6.</para>
		/// </summary>
		iPhone6,
		/// <summary>
		///   <para>iPhone 6 plus.</para>
		/// </summary>
		iPhone6Plus,
		/// <summary>
		///   <para>iPad Mini 3.</para>
		/// </summary>
		iPadMini3Gen,
		/// <summary>
		///   <para>iPad Air 2.</para>
		/// </summary>
		iPadAir2,
		/// <summary>
		///   <para>iPhone6S value.</para>
		/// </summary>
		iPhone6S,
		/// <summary>
		///   <para>iPhone6Plus value.</para>
		/// </summary>
		iPhone6SPlus,
		/// <summary>
		///   <para>iPadProGen1 value.</para>
		/// </summary>
		iPadPro1Gen,
		/// <summary>
		///   <para>iPadMini 4th Generation.</para>
		/// </summary>
		iPadMini4Gen,
		/// <summary>
		///   <para>Yet unknown iPhone.</para>
		/// </summary>
		iPhoneUnknown = 10001,
		/// <summary>
		///   <para>Yet unknown iPad.</para>
		/// </summary>
		iPadUnknown,
		/// <summary>
		///   <para>Yet unknown iPodTouch.</para>
		/// </summary>
		iPodTouchUnknown
	}
}
