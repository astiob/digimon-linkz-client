using System;
using System.Runtime.InteropServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Describes a ProceduralProperty.</para>
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public sealed class ProceduralPropertyDescription
	{
		/// <summary>
		///   <para>The name of the ProceduralProperty. Used to get and set the values.</para>
		/// </summary>
		public string name;

		/// <summary>
		///   <para>The label of the ProceduralProperty. Can contain space and be overall more user-friendly than the 'name' member.</para>
		/// </summary>
		public string label;

		/// <summary>
		///   <para>The name of the GUI group. Used to display ProceduralProperties in groups.</para>
		/// </summary>
		public string group;

		/// <summary>
		///   <para>The ProceduralPropertyType describes what type of property this is.</para>
		/// </summary>
		public ProceduralPropertyType type;

		/// <summary>
		///   <para>If true, the Float or Vector property is constrained to values within a specified range.</para>
		/// </summary>
		public bool hasRange;

		/// <summary>
		///   <para>If hasRange is true, minimum specifies the minimum allowed value for this Float or Vector property.</para>
		/// </summary>
		public float minimum;

		/// <summary>
		///   <para>If hasRange is true, maximum specifies the maximum allowed value for this Float or Vector property.</para>
		/// </summary>
		public float maximum;

		/// <summary>
		///   <para>Specifies the step size of this Float or Vector property. Zero is no step.</para>
		/// </summary>
		public float step;

		/// <summary>
		///   <para>The available options for a ProceduralProperty of type Enum.</para>
		/// </summary>
		public string[] enumOptions;

		/// <summary>
		///   <para>The names of the individual components of a Vector234 ProceduralProperty.</para>
		/// </summary>
		public string[] componentLabels;
	}
}
