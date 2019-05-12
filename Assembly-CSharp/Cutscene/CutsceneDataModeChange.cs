using System;

namespace Cutscene
{
	public sealed class CutsceneDataModeChange : CutsceneDataBase
	{
		public Action endCallback;

		public string beforeModelId;

		public string afterModelId;
	}
}
