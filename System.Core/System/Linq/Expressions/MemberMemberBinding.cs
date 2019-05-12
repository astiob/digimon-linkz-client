using System;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Reflection.Emit;

namespace System.Linq.Expressions
{
	/// <summary>Represents initializing members of a member of a newly created object.</summary>
	public sealed class MemberMemberBinding : MemberBinding
	{
		private ReadOnlyCollection<MemberBinding> bindings;

		internal MemberMemberBinding(MemberInfo member, ReadOnlyCollection<MemberBinding> bindings) : base(MemberBindingType.MemberBinding, member)
		{
			this.bindings = bindings;
		}

		/// <summary>Gets the bindings that describe how to initialize the members of a member.</summary>
		/// <returns>A <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection`1" /> of <see cref="T:System.Linq.Expressions.MemberBinding" /> objects that describe how to initialize the members of the member.</returns>
		public ReadOnlyCollection<MemberBinding> Bindings
		{
			get
			{
				return this.bindings;
			}
		}

		internal override void Emit(EmitContext ec, LocalBuilder local)
		{
			LocalBuilder local2 = base.EmitLoadMember(ec, local);
			foreach (MemberBinding memberBinding in this.bindings)
			{
				memberBinding.Emit(ec, local2);
			}
		}
	}
}
