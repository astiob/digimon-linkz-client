using System;

namespace UnityEngine
{
	[AttributeUsage(AttributeTargets.Class)]
	public class CustomGridBrushAttribute : Attribute
	{
		private bool m_HideAssetInstances;

		private bool m_HideDefaultInstance;

		private bool m_DefaultBrush;

		private string m_DefaultName;

		public CustomGridBrushAttribute()
		{
			this.m_HideAssetInstances = false;
			this.m_HideDefaultInstance = false;
			this.m_DefaultBrush = false;
			this.m_DefaultName = "";
		}

		public CustomGridBrushAttribute(bool hideAssetInstances, bool hideDefaultInstance, bool defaultBrush, string defaultName)
		{
			this.m_HideAssetInstances = hideAssetInstances;
			this.m_HideDefaultInstance = hideDefaultInstance;
			this.m_DefaultBrush = defaultBrush;
			this.m_DefaultName = defaultName;
		}

		public bool hideAssetInstances
		{
			get
			{
				return this.m_HideAssetInstances;
			}
		}

		public bool hideDefaultInstance
		{
			get
			{
				return this.m_HideDefaultInstance;
			}
		}

		public bool defaultBrush
		{
			get
			{
				return this.m_DefaultBrush;
			}
		}

		public string defaultName
		{
			get
			{
				return this.m_DefaultName;
			}
		}
	}
}
