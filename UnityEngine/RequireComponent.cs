using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>The RequireComponent attribute lets automatically add required component as a dependency.</para>
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public sealed class RequireComponent : Attribute
	{
		public Type m_Type0;

		public Type m_Type1;

		public Type m_Type2;

		/// <summary>
		///   <para>Require a single component.</para>
		/// </summary>
		/// <param name="requiredComponent"></param>
		public RequireComponent(Type requiredComponent)
		{
			this.m_Type0 = requiredComponent;
		}

		/// <summary>
		///   <para>Require a two components.</para>
		/// </summary>
		/// <param name="requiredComponent"></param>
		/// <param name="requiredComponent2"></param>
		public RequireComponent(Type requiredComponent, Type requiredComponent2)
		{
			this.m_Type0 = requiredComponent;
			this.m_Type1 = requiredComponent2;
		}

		/// <summary>
		///   <para>Require three components.</para>
		/// </summary>
		/// <param name="requiredComponent"></param>
		/// <param name="requiredComponent2"></param>
		/// <param name="requiredComponent3"></param>
		public RequireComponent(Type requiredComponent, Type requiredComponent2, Type requiredComponent3)
		{
			this.m_Type0 = requiredComponent;
			this.m_Type1 = requiredComponent2;
			this.m_Type2 = requiredComponent3;
		}
	}
}
