using System;

namespace Cutscene
{
	public sealed class CutsceneDataVersionUp : CutsceneDataBase
	{
		public Action endCallback;

		public string beforeModelId;

		public string afterModelId;
	}
}
