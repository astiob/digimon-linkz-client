using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Specify a tooltip for a field.</para>
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
	public class TooltipAttribute : PropertyAttribute
	{
		/// <summary>
		///   <para>The tooltip text.</para>
		/// </summary>
		public readonly string tooltip;

		/// <summary>
		///   <para>Specify a tooltip for a field.</para>
		/// </summary>
		/// <param name="tooltip">The tooltip text.</param>
		public TooltipAttribute(string tooltip)
		{
			this.tooltip = tooltip;
		}
	}
}
