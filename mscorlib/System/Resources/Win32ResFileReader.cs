using System;
using System.Collections;
using System.IO;
using System.Text;

namespace System.Resources
{
	internal class Win32ResFileReader
	{
		private Stream res_file;

		public Win32ResFileReader(Stream s)
		{
			this.res_file = s;
		}

		private int read_int16()
		{
			int num = this.res_file.ReadByte();
			if (num == -1)
			{
				return -1;
			}
			int num2 = this.res_file.ReadByte();
			if (num2 == -1)
			{
				return -1;
			}
			return num | num2 << 8;
		}

		private int read_int32()
		{
			int num = this.read_int16();
			if (num == -1)
			{
				return -1;
			}
			int num2 = this.read_int16();
			if (num2 == -1)
			{
				return -1;
			}
			return num | num2 << 16;
		}

		private void read_padding()
		{
			while (this.res_file.Position % 4L != 0L)
			{
				this.read_int16();
			}
		}

		private NameOrId read_ordinal()
		{
			int num = this.read_int16();
			if ((num & 65535) != 0)
			{
				int id = this.read_int16();
				return new NameOrId(id);
			}
			byte[] array = new byte[16];
			int num2 = 0;
			for (;;)
			{
				int num3 = this.read_int16();
				if (num3 == 0)
				{
					break;
				}
				if (num2 == array.Length)
				{
					byte[] array2 = new byte[array.Length * 2];
					Array.Copy(array, array2, array.Length);
					array = array2;
				}
				array[num2] = (byte)(num3 >> 8);
				array[num2 + 1] = (byte)(num3 & 255);
				num2 += 2;
			}
			return new NameOrId(new string(Encoding.Unicode.GetChars(array, 0, num2)));
		}

		public ICollection ReadResources()
		{
			ArrayList arrayList = new ArrayList();
			for (;;)
			{
				this.read_padding();
				int num = this.read_int32();
				if (num == -1)
				{
					break;
				}
				this.read_int32();
				NameOrId type = this.read_ordinal();
				NameOrId name = this.read_ordinal();
				this.read_padding();
				this.read_int32();
				this.read_int16();
				int language = this.read_int16();
				this.read_int32();
				this.read_int32();
				if (num != 0)
				{
					byte[] array = new byte[num];
					this.res_file.Read(array, 0, num);
					arrayList.Add(new Win32EncodedResource(type, name, language, array));
				}
			}
			return arrayList;
		}
	}
}
