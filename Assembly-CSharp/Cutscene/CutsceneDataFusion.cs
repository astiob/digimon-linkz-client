using System;

namespace Cutscene
{
	public sealed class CutsceneDataFusion : CutsceneDataBase
	{
		public Action endCallback;

		public string baseModelId;

		public string materialModelId;

		public string eggModelId;

		public bool upArousal;
	}
}
