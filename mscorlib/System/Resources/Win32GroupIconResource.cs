using System;
using System.IO;

namespace System.Resources
{
	internal class Win32GroupIconResource : Win32Resource
	{
		private Win32IconResource[] icons;

		public Win32GroupIconResource(int id, int language, Win32IconResource[] icons) : base(Win32ResourceType.RT_GROUP_ICON, id, language)
		{
			this.icons = icons;
		}

		public override void WriteTo(Stream s)
		{
			using (BinaryWriter binaryWriter = new BinaryWriter(s))
			{
				binaryWriter.Write(0);
				binaryWriter.Write(1);
				binaryWriter.Write((short)this.icons.Length);
				for (int i = 0; i < this.icons.Length; i++)
				{
					Win32IconResource win32IconResource = this.icons[i];
					ICONDIRENTRY icon = win32IconResource.Icon;
					binaryWriter.Write(icon.bWidth);
					binaryWriter.Write(icon.bHeight);
					binaryWriter.Write(icon.bColorCount);
					binaryWriter.Write(0);
					binaryWriter.Write(icon.wPlanes);
					binaryWriter.Write(icon.wBitCount);
					binaryWriter.Write(icon.image.Length);
					binaryWriter.Write((short)win32IconResource.Name.Id);
				}
			}
		}
	}
}
