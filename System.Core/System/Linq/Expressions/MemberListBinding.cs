using System;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Reflection.Emit;

namespace System.Linq.Expressions
{
	/// <summary>Represents initializing the elements of a collection member of a newly created object.</summary>
	public sealed class MemberListBinding : MemberBinding
	{
		private ReadOnlyCollection<ElementInit> initializers;

		internal MemberListBinding(MemberInfo member, ReadOnlyCollection<ElementInit> initializers) : base(MemberBindingType.ListBinding, member)
		{
			this.initializers = initializers;
		}

		/// <summary>Gets the element initializers for initializing a collection member of a newly created object.</summary>
		/// <returns>A <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection`1" /> of <see cref="T:System.Linq.Expressions.ElementInit" /> objects to initialize a collection member with.</returns>
		public ReadOnlyCollection<ElementInit> Initializers
		{
			get
			{
				return this.initializers;
			}
		}

		internal override void Emit(EmitContext ec, LocalBuilder local)
		{
			LocalBuilder local2 = base.EmitLoadMember(ec, local);
			foreach (ElementInit elementInit in this.initializers)
			{
				elementInit.Emit(ec, local2);
			}
		}
	}
}
