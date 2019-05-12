using System;
using System.IO;

namespace System.Resources
{
	internal class Win32EncodedResource : Win32Resource
	{
		private byte[] data;

		internal Win32EncodedResource(NameOrId type, NameOrId name, int language, byte[] data) : base(type, name, language)
		{
			this.data = data;
		}

		public byte[] Data
		{
			get
			{
				return this.data;
			}
		}

		public override void WriteTo(Stream s)
		{
			s.Write(this.data, 0, this.data.Length);
		}
	}
}
