using System;

namespace UnityEngine.Experimental.UIElements
{
	public class TemplateContainer : VisualElement
	{
		public readonly string templateId;

		private VisualElement m_ContentContainer;

		public TemplateContainer(string templateId)
		{
			this.templateId = templateId;
			this.m_ContentContainer = this;
		}

		public override VisualElement contentContainer
		{
			get
			{
				return this.m_ContentContainer;
			}
		}

		internal void SetContentContainer(VisualElement content)
		{
			this.m_ContentContainer = content;
		}
	}
}
