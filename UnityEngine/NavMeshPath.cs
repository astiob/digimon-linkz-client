using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>A path as calculated by the navigation system.</para>
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public sealed class NavMeshPath
	{
		internal IntPtr m_Ptr;

		internal Vector3[] m_corners;

		/// <summary>
		///   <para>NavMeshPath constructor.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern NavMeshPath();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void DestroyNavMeshPath();

		~NavMeshPath()
		{
			this.DestroyNavMeshPath();
			this.m_Ptr = IntPtr.Zero;
		}

		/// <summary>
		///   <para>Calculate the corners for the path.</para>
		/// </summary>
		/// <param name="results">Array to store path corners.</param>
		/// <returns>
		///   <para>The number of corners along the path - including start and end points.</para>
		/// </returns>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetCornersNonAlloc(Vector3[] results);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Vector3[] CalculateCornersInternal();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void ClearCornersInternal();

		/// <summary>
		///   <para>Erase all corner points from path.</para>
		/// </summary>
		public void ClearCorners()
		{
			this.ClearCornersInternal();
			this.m_corners = null;
		}

		private void CalculateCorners()
		{
			if (this.m_corners == null)
			{
				this.m_corners = this.CalculateCornersInternal();
			}
		}

		/// <summary>
		///   <para>Corner points of the path. (Read Only)</para>
		/// </summary>
		public Vector3[] corners
		{
			get
			{
				this.CalculateCorners();
				return this.m_corners;
			}
		}

		/// <summary>
		///   <para>Status of the path. (Read Only)</para>
		/// </summary>
		public extern NavMeshPathStatus status { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }
	}
}
