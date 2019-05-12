using System;

namespace UnityEngine.Experimental.Networking
{
	public interface IMultipartFormSection
	{
		/// <summary>
		///   <para>Returns the name of this section, if any.</para>
		/// </summary>
		/// <returns>
		///   <para>The section's name, or null.</para>
		/// </returns>
		string sectionName { get; }

		/// <summary>
		///   <para>Returns the raw binary data contained in this section. Must not return null or a zero-length array.</para>
		/// </summary>
		/// <returns>
		///   <para>The raw binary data contained in this section. Must not be null or empty.</para>
		/// </returns>
		byte[] sectionData { get; }

		/// <summary>
		///   <para>Returns a string denoting the desired filename of this section on the destination server.</para>
		/// </summary>
		/// <returns>
		///   <para>The desired file name of this section, or null if this is not a file section.</para>
		/// </returns>
		string fileName { get; }

		/// <summary>
		///   <para>Returns the value to use in the Content-Type header for this form section.</para>
		/// </summary>
		/// <returns>
		///   <para>The value to use in the Content-Type header, or null.</para>
		/// </returns>
		string contentType { get; }
	}
}
