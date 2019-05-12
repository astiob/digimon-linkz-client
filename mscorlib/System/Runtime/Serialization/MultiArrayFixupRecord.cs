using System;

namespace System.Runtime.Serialization
{
	internal class MultiArrayFixupRecord : BaseFixupRecord
	{
		private int[] _indices;

		public MultiArrayFixupRecord(ObjectRecord objectToBeFixed, int[] indices, ObjectRecord objectRequired) : base(objectToBeFixed, objectRequired)
		{
			this._indices = indices;
		}

		protected override void FixupImpl(ObjectManager manager)
		{
			this.ObjectToBeFixed.SetArrayValue(manager, this.ObjectRequired.ObjectInstance, this._indices);
		}
	}
}
