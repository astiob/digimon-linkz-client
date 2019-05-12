using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine.Scripting;

namespace UnityEngine
{
	internal sealed class UnityLogWriter : TextWriter
	{
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void WriteStringToUnityLog(string s);

		public static void Init()
		{
			Console.SetOut(new UnityLogWriter());
		}

		public override Encoding Encoding
		{
			get
			{
				return Encoding.UTF8;
			}
		}

		public override void Write(char value)
		{
			UnityLogWriter.WriteStringToUnityLog(value.ToString());
		}

		public override void Write(string s)
		{
			UnityLogWriter.WriteStringToUnityLog(s);
		}
	}
}
