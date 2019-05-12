using System;

namespace Cutscene
{
	public sealed class CutsceneDataTraining : CutsceneDataBase
	{
		public string baseModelId;

		public int materialNum;

		public Action endCallback;
	}
}
