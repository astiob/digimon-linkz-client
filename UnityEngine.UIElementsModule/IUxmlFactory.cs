using System;

namespace UnityEngine.Experimental.UIElements
{
	internal interface IUxmlFactory
	{
		Type CreatesType { get; }

		VisualElement Create(IUxmlAttributes bag, CreationContext cc);
	}
}
