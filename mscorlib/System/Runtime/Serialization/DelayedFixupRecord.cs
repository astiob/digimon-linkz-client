using System;

namespace System.Runtime.Serialization
{
	internal class DelayedFixupRecord : BaseFixupRecord
	{
		public string _memberName;

		public DelayedFixupRecord(ObjectRecord objectToBeFixed, string memberName, ObjectRecord objectRequired) : base(objectToBeFixed, objectRequired)
		{
			this._memberName = memberName;
		}

		protected override void FixupImpl(ObjectManager manager)
		{
			this.ObjectToBeFixed.SetMemberValue(manager, this._memberName, this.ObjectRequired.ObjectInstance);
		}
	}
}
