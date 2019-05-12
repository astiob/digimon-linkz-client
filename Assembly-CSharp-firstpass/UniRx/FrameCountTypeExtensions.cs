using System;
using UnityEngine;

namespace UniRx
{
	public static class FrameCountTypeExtensions
	{
		public static YieldInstruction GetYieldInstruction(this FrameCountType frameCountType)
		{
			switch (frameCountType)
			{
			case FrameCountType.FixedUpdate:
				return YieldInstructionCache.WaitForFixedUpdate;
			case FrameCountType.EndOfFrame:
				return YieldInstructionCache.WaitForEndOfFrame;
			}
			return null;
		}
	}
}
