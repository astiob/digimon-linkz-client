using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>The AddComponentMenu attribute allows you to place a script anywhere in the "Component" menu, instead of just the "Component-&gt;Scripts" menu.</para>
	/// </summary>
	public sealed class AddComponentMenu : Attribute
	{
		private string m_AddComponentMenu;

		private int m_Ordering;

		/// <summary>
		///   <para>Add an item in the Component menu.</para>
		/// </summary>
		/// <param name="menuName">The path to the component.</param>
		/// <param name="order">Where in the component menu to add the new item.</param>
		public AddComponentMenu(string menuName)
		{
			this.m_AddComponentMenu = menuName;
			this.m_Ordering = 0;
		}

		/// <summary>
		///   <para>Add an item in the Component menu.</para>
		/// </summary>
		/// <param name="menuName">The path to the component.</param>
		/// <param name="order">Where in the component menu to add the new item.</param>
		public AddComponentMenu(string menuName, int order)
		{
			this.m_AddComponentMenu = menuName;
			this.m_Ordering = order;
		}

		public string componentMenu
		{
			get
			{
				return this.m_AddComponentMenu;
			}
		}

		/// <summary>
		///   <para>The order of the component in the component menu (lower is higher to the top).</para>
		/// </summary>
		public int componentOrder
		{
			get
			{
				return this.m_Ordering;
			}
		}
	}
}
