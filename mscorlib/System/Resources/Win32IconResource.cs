using System;
using System.IO;

namespace System.Resources
{
	internal class Win32IconResource : Win32Resource
	{
		private ICONDIRENTRY icon;

		public Win32IconResource(int id, int language, ICONDIRENTRY icon) : base(Win32ResourceType.RT_ICON, id, language)
		{
			this.icon = icon;
		}

		public ICONDIRENTRY Icon
		{
			get
			{
				return this.icon;
			}
		}

		public override void WriteTo(Stream s)
		{
			s.Write(this.icon.image, 0, this.icon.image.Length);
		}
	}
}
