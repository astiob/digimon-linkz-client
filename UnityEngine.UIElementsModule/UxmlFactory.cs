using System;

namespace UnityEngine.Experimental.UIElements
{
	public abstract class UxmlFactory<T> : IUxmlFactory where T : VisualElement
	{
		public Type CreatesType
		{
			get
			{
				return typeof(T);
			}
		}

		public VisualElement Create(IUxmlAttributes bag, CreationContext cc)
		{
			return this.DoCreate(bag, cc);
		}

		protected abstract T DoCreate(IUxmlAttributes bag, CreationContext cc);
	}
}
