using System;

namespace System.Runtime.Serialization
{
	internal class ArrayFixupRecord : BaseFixupRecord
	{
		private int _index;

		public ArrayFixupRecord(ObjectRecord objectToBeFixed, int index, ObjectRecord objectRequired) : base(objectToBeFixed, objectRequired)
		{
			this._index = index;
		}

		protected override void FixupImpl(ObjectManager manager)
		{
			Array array = (Array)this.ObjectToBeFixed.ObjectInstance;
			array.SetValue(this.ObjectRequired.ObjectInstance, this._index);
		}
	}
}
