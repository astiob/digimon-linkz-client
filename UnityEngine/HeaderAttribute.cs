using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Use this PropertyAttribute to add a header above some fields in the Inspector.</para>
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
	public class HeaderAttribute : PropertyAttribute
	{
		/// <summary>
		///   <para>The header text.</para>
		/// </summary>
		public readonly string header;

		/// <summary>
		///   <para>Add a header above some fields in the Inspector.</para>
		/// </summary>
		/// <param name="header">The header text.</param>
		public HeaderAttribute(string header)
		{
			this.header = header;
		}
	}
}
