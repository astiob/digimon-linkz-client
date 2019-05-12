using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>The ContextMenu attribute allows you to add commands to the context menu.</para>
	/// </summary>
	public sealed class ContextMenu : Attribute
	{
		private string m_ItemName;

		/// <summary>
		///   <para>Adds the function to the context menu of the component.</para>
		/// </summary>
		/// <param name="name"></param>
		public ContextMenu(string name)
		{
			this.m_ItemName = name;
		}

		public string menuItem
		{
			get
			{
				return this.m_ItemName;
			}
		}
	}
}
