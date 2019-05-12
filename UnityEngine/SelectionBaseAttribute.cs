using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Add this attribute to a script class to mark its GameObject as a selection base object for Scene View picking.</para>
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
	public class SelectionBaseAttribute : Attribute
	{
	}
}
