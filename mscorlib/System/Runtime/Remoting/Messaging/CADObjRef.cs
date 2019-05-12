using System;

namespace System.Runtime.Remoting.Messaging
{
	internal class CADObjRef
	{
		private ObjRef objref;

		public int SourceDomain;

		public CADObjRef(ObjRef o, int sourceDomain)
		{
			this.objref = o;
			this.SourceDomain = sourceDomain;
		}

		public string TypeName
		{
			get
			{
				return this.objref.TypeInfo.TypeName;
			}
		}

		public string URI
		{
			get
			{
				return this.objref.URI;
			}
		}
	}
}
