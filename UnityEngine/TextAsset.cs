using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Text file assets.</para>
	/// </summary>
	public class TextAsset : Object
	{
		/// <summary>
		///   <para>The text contents of the .txt file as a string. (Read Only)</para>
		/// </summary>
		public extern string text { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The raw bytes of the text asset. (Read Only)</para>
		/// </summary>
		public extern byte[] bytes { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public override string ToString()
		{
			return this.text;
		}
	}
}
