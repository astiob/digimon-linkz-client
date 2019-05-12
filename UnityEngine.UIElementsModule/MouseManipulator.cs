using System;
using System.Collections.Generic;

namespace UnityEngine.Experimental.UIElements
{
	public abstract class MouseManipulator : Manipulator
	{
		private ManipulatorActivationFilter m_currentActivator;

		public MouseManipulator()
		{
			this.activators = new List<ManipulatorActivationFilter>();
		}

		public List<ManipulatorActivationFilter> activators { get; private set; }

		protected bool CanStartManipulation(IMouseEvent e)
		{
			foreach (ManipulatorActivationFilter currentActivator in this.activators)
			{
				if (currentActivator.Matches(e))
				{
					this.m_currentActivator = currentActivator;
					return true;
				}
			}
			return false;
		}

		protected bool CanStopManipulation(IMouseEvent e)
		{
			return e.button == (int)this.m_currentActivator.button && base.target.HasCapture();
		}
	}
}
