using System;
using System.Runtime.InteropServices;

namespace System
{
	/// <summary>Indicates that the COM threading model for an application is single-threaded apartment (STA). </summary>
	/// <filterpriority>1</filterpriority>
	[AttributeUsage(AttributeTargets.Method)]
	[ComVisible(true)]
	public sealed class STAThreadAttribute : Attribute
	{
	}
}
