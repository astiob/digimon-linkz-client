using System;

namespace System.Runtime.InteropServices
{
	/// <summary>Specifies the version number of an exported type library.</summary>
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
	[ComVisible(true)]
	public sealed class TypeLibVersionAttribute : Attribute
	{
		private int major;

		private int minor;

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.InteropServices.TypeLibVersionAttribute" /> class with the major and minor version numbers of the type library.</summary>
		/// <param name="major">The major version number of the type library. </param>
		/// <param name="minor">The minor version number of the type library. </param>
		public TypeLibVersionAttribute(int major, int minor)
		{
			this.major = major;
			this.minor = minor;
		}

		/// <summary>Gets the major version number of the type library.</summary>
		/// <returns>The major version number of the type library.</returns>
		public int MajorVersion
		{
			get
			{
				return this.major;
			}
		}

		/// <summary>Gets the minor version number of the type library.</summary>
		/// <returns>The minor version number of the type library.</returns>
		public int MinorVersion
		{
			get
			{
				return this.minor;
			}
		}
	}
}
