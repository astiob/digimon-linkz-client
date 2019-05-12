using System;
using System.Collections.Generic;

namespace Cutscene
{
	public sealed class CutsceneSound
	{
		private List<int> playSeHandleList;

		public CutsceneSound()
		{
			this.playSeHandleList = new List<int>();
		}

		public int PlaySE(string filePath)
		{
			int num = SoundMng.Instance().PlaySE_Ex(filePath);
			if (0 <= num)
			{
				this.playSeHandleList.Add(num);
			}
			return num;
		}

		public void StopAllSE()
		{
			for (int i = 0; i < this.playSeHandleList.Count; i++)
			{
				int handle = this.playSeHandleList[i];
				SoundMng.Instance().StopSE_Ex(handle);
			}
			this.playSeHandleList.Clear();
		}
	}
}
