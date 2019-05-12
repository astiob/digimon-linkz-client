using System;
using System.IO;

namespace System.Reflection.Emit
{
	internal struct MonoResource
	{
		public byte[] data;

		public string name;

		public string filename;

		public ResourceAttributes attrs;

		public int offset;

		public Stream stream;
	}
}
