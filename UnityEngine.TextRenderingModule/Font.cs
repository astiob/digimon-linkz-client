﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine
{
	public sealed class Font : Object
	{
		public Font()
		{
			Font.Internal_CreateFont(this, null);
		}

		public Font(string name)
		{
			Font.Internal_CreateFont(this, name);
		}

		private Font(string[] names, int size)
		{
			Font.Internal_CreateDynamicFont(this, names, size);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string[] GetOSInstalledFontNames();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_CreateFont([Writable] Font _font, string name);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_CreateDynamicFont([Writable] Font _font, string[] _names, int size);

		public static Font CreateDynamicFontFromOSFont(string fontname, int size)
		{
			return new Font(new string[]
			{
				fontname
			}, size);
		}

		public static Font CreateDynamicFontFromOSFont(string[] fontnames, int size)
		{
			return new Font(fontnames, size);
		}

		public extern Material material { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool HasCharacter(char c);

		public extern string[] fontNames { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern CharacterInfo[] characterInfo { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void RequestCharactersInTexture(string characters, [UnityEngine.Internal.DefaultValue("0")] int size, [UnityEngine.Internal.DefaultValue("FontStyle.Normal")] FontStyle style);

		[ExcludeFromDocs]
		public void RequestCharactersInTexture(string characters, int size)
		{
			FontStyle style = FontStyle.Normal;
			this.RequestCharactersInTexture(characters, size, style);
		}

		[ExcludeFromDocs]
		public void RequestCharactersInTexture(string characters)
		{
			FontStyle style = FontStyle.Normal;
			int size = 0;
			this.RequestCharactersInTexture(characters, size, style);
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action<Font> textureRebuilt;

		[RequiredByNativeCode]
		private static void InvokeTextureRebuilt_Internal(Font font)
		{
			Action<Font> action = Font.textureRebuilt;
			if (action != null)
			{
				action(font);
			}
			if (font.m_FontTextureRebuildCallback != null)
			{
				font.m_FontTextureRebuildCallback();
			}
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private event Font.FontTextureRebuildCallback m_FontTextureRebuildCallback;

		[Obsolete("Font.textureRebuildCallback has been deprecated. Use Font.textureRebuilt instead.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public Font.FontTextureRebuildCallback textureRebuildCallback
		{
			get
			{
				return this.m_FontTextureRebuildCallback;
			}
			set
			{
				this.m_FontTextureRebuildCallback = value;
			}
		}

		public static int GetMaxVertsForString(string str)
		{
			return str.Length * 4 + 4;
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool GetCharacterInfo(char ch, out CharacterInfo info, [UnityEngine.Internal.DefaultValue("0")] int size, [UnityEngine.Internal.DefaultValue("FontStyle.Normal")] FontStyle style);

		[ExcludeFromDocs]
		public bool GetCharacterInfo(char ch, out CharacterInfo info, int size)
		{
			FontStyle style = FontStyle.Normal;
			return this.GetCharacterInfo(ch, out info, size, style);
		}

		[ExcludeFromDocs]
		public bool GetCharacterInfo(char ch, out CharacterInfo info)
		{
			FontStyle style = FontStyle.Normal;
			int size = 0;
			return this.GetCharacterInfo(ch, out info, size, style);
		}

		public extern bool dynamic { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public extern int ascent { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public extern int lineHeight { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public extern int fontSize { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		[EditorBrowsable(EditorBrowsableState.Never)]
		public delegate void FontTextureRebuildCallback();
	}
}
