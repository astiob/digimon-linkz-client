using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>LayerMask allow you to display the LayerMask popup menu in the inspector.</para>
	/// </summary>
	public struct LayerMask
	{
		private int m_Mask;

		/// <summary>
		///   <para>Converts a layer mask value to an integer value.</para>
		/// </summary>
		public int value
		{
			get
			{
				return this.m_Mask;
			}
			set
			{
				this.m_Mask = value;
			}
		}

		/// <summary>
		///   <para>Given a layer number, returns the name of the layer as defined in either a Builtin or a User Layer in the.</para>
		/// </summary>
		/// <param name="layer"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string LayerToName(int layer);

		/// <summary>
		///   <para>Given a layer name, returns the layer index as defined by either a Builtin or a User Layer in the.</para>
		/// </summary>
		/// <param name="layerName"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int NameToLayer(string layerName);

		/// <summary>
		///   <para>Given a set of layer names as defined by either a Builtin or a User Layer in the, returns the equivalent layer mask for all of them.</para>
		/// </summary>
		/// <param name="layerNames">List of layer names to convert to a layer mask.</param>
		/// <returns>
		///   <para>The layer mask created from the layerNames.</para>
		/// </returns>
		public static int GetMask(params string[] layerNames)
		{
			int num = 0;
			foreach (string layerName in layerNames)
			{
				int num2 = LayerMask.NameToLayer(layerName);
				if (num2 != 0)
				{
					num |= 1 << num2;
				}
			}
			return num;
		}

		public static implicit operator int(LayerMask mask)
		{
			return mask.m_Mask;
		}

		public static implicit operator LayerMask(int intVal)
		{
			LayerMask result;
			result.m_Mask = intVal;
			return result;
		}
	}
}
