using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace UnityEngine.Experimental.UIElements
{
	[Serializable]
	internal class TemplateAsset : VisualElementAsset
	{
		[SerializeField]
		private string m_TemplateAlias;

		[SerializeField]
		private List<VisualTreeAsset.SlotUsageEntry> m_SlotUsages;

		public TemplateAsset(string templateAlias) : base(typeof(TemplateContainer).FullName)
		{
			Assert.IsFalse(string.IsNullOrEmpty(templateAlias), "Template alias must not be null or empty");
			this.m_TemplateAlias = templateAlias;
		}

		public string templateAlias
		{
			get
			{
				return this.m_TemplateAlias;
			}
			set
			{
				this.m_TemplateAlias = value;
			}
		}

		internal List<VisualTreeAsset.SlotUsageEntry> slotUsages
		{
			get
			{
				return this.m_SlotUsages;
			}
			set
			{
				this.m_SlotUsages = value;
			}
		}

		public void AddSlotUsage(string slotName, int resId)
		{
			if (this.m_SlotUsages == null)
			{
				this.m_SlotUsages = new List<VisualTreeAsset.SlotUsageEntry>();
			}
			this.m_SlotUsages.Add(new VisualTreeAsset.SlotUsageEntry(slotName, resId));
		}
	}
}
