using System;

namespace System.Runtime.Serialization
{
	internal abstract class BaseFixupRecord
	{
		protected internal ObjectRecord ObjectToBeFixed;

		protected internal ObjectRecord ObjectRequired;

		public BaseFixupRecord NextSameContainer;

		public BaseFixupRecord NextSameRequired;

		public BaseFixupRecord(ObjectRecord objectToBeFixed, ObjectRecord objectRequired)
		{
			this.ObjectToBeFixed = objectToBeFixed;
			this.ObjectRequired = objectRequired;
		}

		public bool DoFixup(ObjectManager manager, bool strict)
		{
			if (this.ObjectToBeFixed.IsRegistered && this.ObjectRequired.IsInstanceReady)
			{
				this.FixupImpl(manager);
				return true;
			}
			if (!strict)
			{
				return false;
			}
			if (!this.ObjectToBeFixed.IsRegistered)
			{
				throw new SerializationException("An object with ID " + this.ObjectToBeFixed.ObjectID + " was included in a fixup, but it has not been registered");
			}
			if (!this.ObjectRequired.IsRegistered)
			{
				throw new SerializationException("An object with ID " + this.ObjectRequired.ObjectID + " was included in a fixup, but it has not been registered");
			}
			return false;
		}

		protected abstract void FixupImpl(ObjectManager manager);
	}
}
