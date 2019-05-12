using System;

namespace UnityEngine.Rendering
{
	/// <summary>
	///   <para>Depth or stencil comparison function.</para>
	/// </summary>
	public enum CompareFunction
	{
		/// <summary>
		///   <para>Depth or stencil test is disabled.</para>
		/// </summary>
		Disabled,
		/// <summary>
		///   <para>Never pass depth or stencil test.</para>
		/// </summary>
		Never,
		/// <summary>
		///   <para>Pass depth or stencil test when new value is less than old one.</para>
		/// </summary>
		Less,
		/// <summary>
		///   <para>Pass depth or stencil test when values are equal.</para>
		/// </summary>
		Equal,
		/// <summary>
		///   <para>Pass depth or stencil test when new value is less or equal than old one.</para>
		/// </summary>
		LessEqual,
		/// <summary>
		///   <para>Pass depth or stencil test when new value is greater than old one.</para>
		/// </summary>
		Greater,
		/// <summary>
		///   <para>Pass depth or stencil test when values are different.</para>
		/// </summary>
		NotEqual,
		/// <summary>
		///   <para>Pass depth or stencil test when new value is greater or equal than old one.</para>
		/// </summary>
		GreaterEqual,
		/// <summary>
		///   <para>Always pass depth or stencil test.</para>
		/// </summary>
		Always
	}
}
