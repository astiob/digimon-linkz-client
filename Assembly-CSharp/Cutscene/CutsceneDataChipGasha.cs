using System;
using UnityEngine;

namespace Cutscene
{
	public sealed class CutsceneDataChipGasha : CutsceneDataBase
	{
		public Action<RenderTexture> endCallback;

		public GameWebAPI.RespDataGA_ExecChip.UserAssetList[] gashaResult;
	}
}
