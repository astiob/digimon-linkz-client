using System;

namespace Cutscene
{
	public sealed class CutsceneDataJogress : CutsceneDataBase
	{
		public Action endCallback;

		public string beforeModelId;

		public string afterModelId;

		public string partnerModelId;
	}
}
