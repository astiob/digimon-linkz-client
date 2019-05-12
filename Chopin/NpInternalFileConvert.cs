using System;
using System.IO;

namespace CROOZ.Chopin.Core
{
	public class NpInternalFileConvert
	{
		private \uE014 \uE000;

		public NpInternalFileConvert(string path, NpAesConvert crypto)
		{
			this.\uE000 = new \uE014(path, crypto.\uE001);
		}

		public Stream GetStream()
		{
			return this.\uE000;
		}

		public void Flush()
		{
			this.\uE000.Flush();
		}

		public void Close()
		{
			this.\uE000.Close();
		}
	}
}
