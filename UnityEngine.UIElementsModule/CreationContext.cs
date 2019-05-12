using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace UnityEngine.Experimental.UIElements
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct CreationContext
	{
		public static readonly CreationContext Default = default(CreationContext);

		internal CreationContext(Dictionary<string, VisualElement> slotInsertionPoints, VisualTreeAsset vta, VisualElement target)
		{
			this.target = target;
			this.slotInsertionPoints = slotInsertionPoints;
			this.visualTreeAsset = vta;
		}

		public VisualElement target { get; }

		public VisualTreeAsset visualTreeAsset { get; }

		public Dictionary<string, VisualElement> slotInsertionPoints { get; }
	}
}
