using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	/// <summary>Provides access to manifest resources, which are XML files that describe application dependencies.  </summary>
	[ComVisible(true)]
	public class ManifestResourceInfo
	{
		private Assembly _assembly;

		private string _filename;

		private ResourceLocation _location;

		internal ManifestResourceInfo()
		{
		}

		internal ManifestResourceInfo(Assembly assembly, string filename, ResourceLocation location)
		{
			this._assembly = assembly;
			this._filename = filename;
			this._location = location;
		}

		/// <summary>Indicates the name of the file containing the manifest resource, if not the same as the manifest file. This property is read-only.</summary>
		/// <returns>A String that is the manifest resource's file name.</returns>
		public virtual string FileName
		{
			get
			{
				return this._filename;
			}
		}

		/// <summary>Indicates the containing assembly. This property is read-only.</summary>
		/// <returns>An <see cref="T:System.Reflection.Assembly" /> object representing the manifest resource's containing assembly.</returns>
		public virtual Assembly ReferencedAssembly
		{
			get
			{
				return this._assembly;
			}
		}

		/// <summary>Indicates the manifest resource's location. This property is read-only.</summary>
		/// <returns>A combination of the <see cref="T:System.Reflection.ResourceLocation" /> flags.</returns>
		public virtual ResourceLocation ResourceLocation
		{
			get
			{
				return this._location;
			}
		}
	}
}
