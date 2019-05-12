using System;

namespace Cutscene
{
	public sealed class CutsceneDataInheritance : CutsceneDataBase
	{
		public Action endCallback;

		public string baseModelId;

		public string materialModelId;
	}
}
