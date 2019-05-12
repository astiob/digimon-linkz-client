using System;
using System.Reflection;

namespace System.Runtime.Serialization
{
	internal class FixupRecord : BaseFixupRecord
	{
		public MemberInfo _member;

		public FixupRecord(ObjectRecord objectToBeFixed, MemberInfo member, ObjectRecord objectRequired) : base(objectToBeFixed, objectRequired)
		{
			this._member = member;
		}

		protected override void FixupImpl(ObjectManager manager)
		{
			this.ObjectToBeFixed.SetMemberValue(manager, this._member, this.ObjectRequired.ObjectInstance);
		}
	}
}
