using System;

namespace Cutscene
{
	public sealed class CutsceneDataEvolution : CutsceneDataBase
	{
		public Action endCallback;

		public string beforeModelId;

		public string beforeGrowStep;

		public string afterModelId;

		public string afterGrowStep;
	}
}
