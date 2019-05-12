using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>SharedBetweenAnimatorsAttribute is an attribute that specify that this StateMachineBehaviour should be instantiate only once and shared among all Animator instance. This attribute reduce the memory footprint for each controller instance.</para>
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public sealed class SharedBetweenAnimatorsAttribute : Attribute
	{
	}
}
