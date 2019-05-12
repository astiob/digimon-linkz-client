using System;

namespace System.Reflection.Emit
{
	internal struct MonoWin32Resource
	{
		public int res_type;

		public int res_id;

		public int lang_id;

		public byte[] data;

		public MonoWin32Resource(int res_type, int res_id, int lang_id, byte[] data)
		{
			this.res_type = res_type;
			this.res_id = res_id;
			this.lang_id = lang_id;
			this.data = data;
		}
	}
}
