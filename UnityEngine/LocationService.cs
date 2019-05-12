using System;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine
{
	/// <summary>
	///   <para>Interface into location functionality.</para>
	/// </summary>
	public sealed class LocationService
	{
		/// <summary>
		///   <para>Specifies whether location service is enabled in user settings.</para>
		/// </summary>
		public extern bool isEnabledByUser { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Returns location service status.</para>
		/// </summary>
		public extern LocationServiceStatus status { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Last measured device geographical location.</para>
		/// </summary>
		public extern LocationInfo lastData { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Starts location service updates.  Last location coordinates could be.</para>
		/// </summary>
		/// <param name="desiredAccuracyInMeters"></param>
		/// <param name="updateDistanceInMeters"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Start([DefaultValue("10f")] float desiredAccuracyInMeters, [DefaultValue("10f")] float updateDistanceInMeters);

		/// <summary>
		///   <para>Starts location service updates.  Last location coordinates could be.</para>
		/// </summary>
		/// <param name="desiredAccuracyInMeters"></param>
		/// <param name="updateDistanceInMeters"></param>
		[ExcludeFromDocs]
		public void Start(float desiredAccuracyInMeters)
		{
			float updateDistanceInMeters = 10f;
			this.Start(desiredAccuracyInMeters, updateDistanceInMeters);
		}

		[ExcludeFromDocs]
		public void Start()
		{
			float updateDistanceInMeters = 10f;
			float desiredAccuracyInMeters = 10f;
			this.Start(desiredAccuracyInMeters, updateDistanceInMeters);
		}

		/// <summary>
		///   <para>Stops location service updates. This could be useful for saving battery life.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Stop();
	}
}
