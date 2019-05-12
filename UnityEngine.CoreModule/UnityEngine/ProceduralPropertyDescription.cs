using System;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	[Obsolete("Built-in support for Substance Designer materials has been deprecated and will be removed in Unity 2018.1. To continue using Substance Designer materials in Unity 2018.1, you will need to install Allegorithmic's external importer from the Asset Store.", false)]
	[UsedByNativeCode]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class ProceduralPropertyDescription
	{
		public string name;

		public string label;

		public string group;

		public ProceduralPropertyType type;

		public bool hasRange;

		public float minimum;

		public float maximum;

		public float step;

		public string[] enumOptions;

		public string[] componentLabels;
	}
}
