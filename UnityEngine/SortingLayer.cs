using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>SortingLayer allows you to set the render order of multiple sprites easily. There is always a default SortingLayer named "Default" which all sprites are added to initially. Added more SortingLayers to easily control the order of rendering of groups of sprites. Layers can be ordered before or after the default layer.</para>
	/// </summary>
	public struct SortingLayer
	{
		private int m_Id;

		/// <summary>
		///   <para>This is the unique id assigned to the layer. It is not an ordered running value and it should not be used to compare with other layers to determine the sorting order.</para>
		/// </summary>
		public int id
		{
			get
			{
				return this.m_Id;
			}
		}

		/// <summary>
		///   <para>Returns the name of the layer as defined in the TagManager.</para>
		/// </summary>
		public string name
		{
			get
			{
				return SortingLayer.IDToName(this.m_Id);
			}
		}

		/// <summary>
		///   <para>This is the relative value that indicates the sort order of this layer relative to the other layers.</para>
		/// </summary>
		public int value
		{
			get
			{
				return SortingLayer.GetLayerValueFromID(this.m_Id);
			}
		}

		/// <summary>
		///   <para>Returns all the layers defined in this project.</para>
		/// </summary>
		public static SortingLayer[] layers
		{
			get
			{
				int[] sortingLayerIDsInternal = SortingLayer.GetSortingLayerIDsInternal();
				SortingLayer[] array = new SortingLayer[sortingLayerIDsInternal.Length];
				for (int i = 0; i < sortingLayerIDsInternal.Length; i++)
				{
					array[i].m_Id = sortingLayerIDsInternal[i];
				}
				return array;
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int[] GetSortingLayerIDsInternal();

		/// <summary>
		///   <para>Returns the final sorting layer value. To determine the sorting order between the various sorting layers, use this method to retrieve the final sorting value and use CompareTo to determine the order.</para>
		/// </summary>
		/// <param name="id">The unique value of the sorting layer as returned by any renderer's sortingLayerID property.</param>
		/// <returns>
		///   <para>The final sorting value of the layer relative to other layers.</para>
		/// </returns>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetLayerValueFromID(int id);

		/// <summary>
		///   <para>Returns the final sorting layer value. See Also: GetLayerValueFromID.</para>
		/// </summary>
		/// <param name="name">The unique value of the sorting layer as returned by any renderer's sortingLayerID property.</param>
		/// <returns>
		///   <para>The final sorting value of the layer relative to other layers.</para>
		/// </returns>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetLayerValueFromName(string name);

		/// <summary>
		///   <para>Returns the id given the name. Will return 0 if an invalid name was given.</para>
		/// </summary>
		/// <param name="name">The name of the layer.</param>
		/// <returns>
		///   <para>The unique id of the layer with name.</para>
		/// </returns>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int NameToID(string name);

		/// <summary>
		///   <para>Returns the unique id of the layer. Will return "&lt;unknown layer&gt;" if an invalid id is given.</para>
		/// </summary>
		/// <param name="id">The unique id of the layer.</param>
		/// <returns>
		///   <para>The name of the layer with id or "&lt;unknown layer&gt;" for invalid id.</para>
		/// </returns>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string IDToName(int id);

		/// <summary>
		///   <para>Returns true if the id provided is a valid layer id.</para>
		/// </summary>
		/// <param name="id">The unique id of a layer.</param>
		/// <returns>
		///   <para>True if the id provided is valid and assigned to a layer.</para>
		/// </returns>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool IsValid(int id);
	}
}
