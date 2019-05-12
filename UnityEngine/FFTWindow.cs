using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Spectrum analysis windowing types.</para>
	/// </summary>
	public enum FFTWindow
	{
		/// <summary>
		///   <para>W[n] = 1.0.</para>
		/// </summary>
		Rectangular,
		/// <summary>
		///   <para>W[n] = TRI(2n/N).</para>
		/// </summary>
		Triangle,
		/// <summary>
		///   <para>W[n] = 0.54 - (0.46 * COS(n/N) ).</para>
		/// </summary>
		Hamming,
		/// <summary>
		///   <para>W[n] = 0.5 * (1.0 - COS(n/N) ).</para>
		/// </summary>
		Hanning,
		/// <summary>
		///   <para>W[n] = 0.42 - (0.5 * COS(nN) ) + (0.08 * COS(2.0 * nN) ).</para>
		/// </summary>
		Blackman,
		/// <summary>
		///   <para>W[n] = 0.35875 - (0.48829 * COS(1.0 * nN)) + (0.14128 * COS(2.0 * nN)) - (0.01168 * COS(3.0 * n/N)).</para>
		/// </summary>
		BlackmanHarris
	}
}
