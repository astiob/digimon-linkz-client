using System;

namespace UnityEngine.Experimental.UIElements
{
	public abstract class Manipulator : IManipulator
	{
		private VisualElement m_Target;

		protected abstract void RegisterCallbacksOnTarget();

		protected abstract void UnregisterCallbacksFromTarget();

		public VisualElement target
		{
			get
			{
				return this.m_Target;
			}
			set
			{
				if (this.target != null)
				{
					this.UnregisterCallbacksFromTarget();
				}
				this.m_Target = value;
				if (this.target != null)
				{
					this.RegisterCallbacksOnTarget();
				}
			}
		}
	}
}
