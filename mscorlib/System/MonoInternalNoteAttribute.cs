using System;

namespace System
{
	[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
	internal class MonoInternalNoteAttribute : MonoTODOAttribute
	{
		public MonoInternalNoteAttribute(string comment) : base(comment)
		{
		}
	}
}
