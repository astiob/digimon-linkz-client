using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.U2D
{
	public sealed class SpriteAtlasManager
	{
		[CompilerGenerated]
		private static Action<SpriteAtlas> <>f__mg$cache0;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event SpriteAtlasManager.RequestAtlasCallback atlasRequested;

		[RequiredByNativeCode]
		private static bool RequestAtlas(string tag)
		{
			bool result;
			if (SpriteAtlasManager.atlasRequested != null)
			{
				SpriteAtlasManager.RequestAtlasCallback requestAtlasCallback = SpriteAtlasManager.atlasRequested;
				if (SpriteAtlasManager.<>f__mg$cache0 == null)
				{
					SpriteAtlasManager.<>f__mg$cache0 = new Action<SpriteAtlas>(SpriteAtlasManager.Register);
				}
				requestAtlasCallback(tag, SpriteAtlasManager.<>f__mg$cache0);
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void Register(SpriteAtlas spriteAtlas);

		// Note: this type is marked as 'beforefieldinit'.
		static SpriteAtlasManager()
		{
			SpriteAtlasManager.atlasRequested = null;
		}

		public delegate void RequestAtlasCallback(string tag, Action<SpriteAtlas> action);
	}
}
