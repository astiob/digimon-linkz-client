using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Assembly level attribute. Any classes in an assembly with this attribute will be considered to be Editor Classes.</para>
	/// </summary>
	[AttributeUsage(AttributeTargets.Assembly)]
	public class AssemblyIsEditorAssembly : Attribute
	{
	}
}
