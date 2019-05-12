using System;

namespace UnityEngine.Experimental.UIElements
{
	internal class VisualElementPanelActivator
	{
		private IVisualElementPanelActivatable m_Activatable;

		public VisualElementPanelActivator(IVisualElementPanelActivatable activatable)
		{
			this.m_Activatable = activatable;
		}

		public bool isActive { get; private set; }

		public bool isDetaching { get; private set; }

		public void SetActive(bool action)
		{
			if (this.isActive != action)
			{
				this.isActive = action;
				if (this.isActive)
				{
					this.m_Activatable.element.RegisterCallback<AttachToPanelEvent>(new EventCallback<AttachToPanelEvent>(this.OnEnter), Capture.NoCapture);
					this.m_Activatable.element.RegisterCallback<DetachFromPanelEvent>(new EventCallback<DetachFromPanelEvent>(this.OnLeave), Capture.NoCapture);
					this.SendActivation();
				}
				else
				{
					this.m_Activatable.element.UnregisterCallback<AttachToPanelEvent>(new EventCallback<AttachToPanelEvent>(this.OnEnter), Capture.NoCapture);
					this.m_Activatable.element.UnregisterCallback<DetachFromPanelEvent>(new EventCallback<DetachFromPanelEvent>(this.OnLeave), Capture.NoCapture);
					this.SendDeactivation();
				}
			}
		}

		public void SendActivation()
		{
			if (this.m_Activatable.CanBeActivated())
			{
				this.m_Activatable.OnPanelActivate();
			}
		}

		public void SendDeactivation()
		{
			if (this.m_Activatable.CanBeActivated())
			{
				this.m_Activatable.OnPanelDeactivate();
			}
		}

		private void OnEnter(AttachToPanelEvent evt)
		{
			if (this.isActive)
			{
				this.SendActivation();
			}
		}

		private void OnLeave(DetachFromPanelEvent evt)
		{
			if (this.isActive)
			{
				this.isDetaching = true;
				try
				{
					this.SendDeactivation();
				}
				finally
				{
					this.isDetaching = false;
				}
			}
		}
	}
}
