using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Interface into compass functionality.</para>
	/// </summary>
	public sealed class Compass
	{
		/// <summary>
		///   <para>The heading in degrees relative to the magnetic North Pole. (Read Only)</para>
		/// </summary>
		public extern float magneticHeading { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The heading in degrees relative to the geographic North Pole. (Read Only)</para>
		/// </summary>
		public extern float trueHeading { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Accuracy of heading reading in degrees.</para>
		/// </summary>
		public extern float headingAccuracy { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The raw geomagnetic data measured in microteslas. (Read Only)</para>
		/// </summary>
		public Vector3 rawVector
		{
			get
			{
				Vector3 result;
				this.INTERNAL_get_rawVector(out result);
				return result;
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_rawVector(out Vector3 value);

		/// <summary>
		///   <para>Timestamp (in seconds since 1970) when the heading was last time updated. (Read Only)</para>
		/// </summary>
		public extern double timestamp { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Used to enable or disable compass. Note, that if you want Input.compass.trueHeading property to contain a valid value, you must also enable location updates by calling Input.location.Start().</para>
		/// </summary>
		public extern bool enabled { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }
	}
}
