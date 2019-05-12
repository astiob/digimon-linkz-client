using System;
using UnityEngine;

namespace Cutscene
{
	public sealed class CutsceneDataTicketGasha : CutsceneDataBase
	{
		public Action<RenderTexture> endCallback;

		public GameWebAPI.RespDataGA_ExecTicket.UserDungeonTicketList[] gashaResult;
	}
}
