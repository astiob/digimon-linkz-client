using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System.Resources
{
	/// <summary>Provides convenient access to culture-specific resources at run time.</summary>
	[ComVisible(true)]
	[Serializable]
	public class ResourceManager
	{
		private static Hashtable ResourceCache = new Hashtable();

		private static Hashtable NonExistent = Hashtable.Synchronized(new Hashtable());

		/// <summary>A constant readonly value indicating the version of resource file headers that the current implementation of <see cref="T:System.Resources.ResourceManager" /> can interpret and produce.</summary>
		public static readonly int HeaderVersionNumber = 1;

		/// <summary>Holds the number used to identify resource files.</summary>
		public static readonly int MagicNumber = -1091581234;

		/// <summary>Indicates the root name of the resource files that the <see cref="T:System.Resources.ResourceManager" /> searches for resources.</summary>
		protected string BaseNameField;

		/// <summary>Indicates the main <see cref="T:System.Reflection.Assembly" /> that contains the resources.</summary>
		protected Assembly MainAssembly;

		/// <summary>Contains a <see cref="T:System.Collections.Hashtable" /> that returns a mapping from cultures to <see cref="T:System.Resources.ResourceSet" /> objects.</summary>
		protected Hashtable ResourceSets;

		private bool ignoreCase;

		private Type resourceSource;

		private Type resourceSetType = typeof(RuntimeResourceSet);

		private string resourceDir;

		private CultureInfo neutral_culture;

		private UltimateResourceFallbackLocation fallbackLocation;

		/// <summary>Initializes a new instance of the <see cref="T:System.Resources.ResourceManager" /> class with default values.</summary>
		protected ResourceManager()
		{
		}

		/// <summary>Creates a <see cref="T:System.Resources.ResourceManager" /> that looks up resources in satellite assemblies based on information from the specified <see cref="T:System.Type" />.</summary>
		/// <param name="resourceSource">A <see cref="T:System.Type" /> from which the <see cref="T:System.Resources.ResourceManager" /> derives all information for finding .resources files. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="resourceSource" /> parameter is null. </exception>
		public ResourceManager(Type resourceSource)
		{
			if (resourceSource == null)
			{
				throw new ArgumentNullException("resourceSource");
			}
			this.resourceSource = resourceSource;
			this.BaseNameField = resourceSource.Name;
			this.MainAssembly = resourceSource.Assembly;
			this.ResourceSets = ResourceManager.GetResourceSets(this.MainAssembly, this.BaseNameField);
			this.neutral_culture = ResourceManager.GetNeutralResourcesLanguage(this.MainAssembly);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Resources.ResourceManager" /> class that looks up resources contained in files with the specified root name using the given assembly.</summary>
		/// <param name="baseName">The root name of the resource file without its extension and along with any fully qualified namespace name. For example, the root name for the resource file named "MyApplication.MyResource.en-US.resources" is "MyApplication.MyResource".</param>
		/// <param name="assembly">The main assembly for the resources. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="baseName" /> or <paramref name="assembly" /> parameter is null. </exception>
		public ResourceManager(string baseName, Assembly assembly)
		{
			if (baseName == null)
			{
				throw new ArgumentNullException("baseName");
			}
			if (assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}
			this.BaseNameField = baseName;
			this.MainAssembly = assembly;
			this.ResourceSets = ResourceManager.GetResourceSets(this.MainAssembly, this.BaseNameField);
			this.neutral_culture = ResourceManager.GetNeutralResourcesLanguage(this.MainAssembly);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Resources.ResourceManager" /> class that looks up resources contained in files derived from the specified root name using the given <see cref="T:System.Reflection.Assembly" />.</summary>
		/// <param name="baseName">The root name of the resource file without its extension and along with any fully qualified namespace name. For example, the root name for the resource file named "MyApplication.MyResource.en-US.resources" is "MyApplication.MyResource". </param>
		/// <param name="assembly">The main assembly for the resources. </param>
		/// <param name="usingResourceSet">The <see cref="T:System.Type" /> of the custom <see cref="T:System.Resources.ResourceSet" /> to use. If null, the default runtime <see cref="T:System.Resources.ResourceSet" /> is used. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="usingResourceset" /> is not a derived class of <see cref="T:System.Resources.ResourceSet" />. </exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="baseName" /> or <paramref name="assembly" /> parameter is null. </exception>
		public ResourceManager(string baseName, Assembly assembly, Type usingResourceSet)
		{
			if (baseName == null)
			{
				throw new ArgumentNullException("baseName");
			}
			if (assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}
			this.BaseNameField = baseName;
			this.MainAssembly = assembly;
			this.ResourceSets = ResourceManager.GetResourceSets(this.MainAssembly, this.BaseNameField);
			this.resourceSetType = this.CheckResourceSetType(usingResourceSet, true);
			this.neutral_culture = ResourceManager.GetNeutralResourcesLanguage(this.MainAssembly);
		}

		private ResourceManager(string baseName, string resourceDir, Type usingResourceSet)
		{
			if (baseName == null)
			{
				throw new ArgumentNullException("baseName");
			}
			if (resourceDir == null)
			{
				throw new ArgumentNullException("resourceDir");
			}
			this.BaseNameField = baseName;
			this.resourceDir = resourceDir;
			this.resourceSetType = this.CheckResourceSetType(usingResourceSet, false);
			this.ResourceSets = ResourceManager.GetResourceSets(this.MainAssembly, this.BaseNameField);
		}

		private static Hashtable GetResourceSets(Assembly assembly, string basename)
		{
			Hashtable resourceCache = ResourceManager.ResourceCache;
			Hashtable result;
			lock (resourceCache)
			{
				string text = string.Empty;
				if (assembly != null)
				{
					text = assembly.FullName;
				}
				else
				{
					text = basename.GetHashCode().ToString() + "@@";
				}
				if (basename != null && basename != string.Empty)
				{
					text = text + "!" + basename;
				}
				else
				{
					text = text + "!" + text.GetHashCode();
				}
				Hashtable hashtable = ResourceManager.ResourceCache[text] as Hashtable;
				if (hashtable == null)
				{
					hashtable = Hashtable.Synchronized(new Hashtable());
					ResourceManager.ResourceCache[text] = hashtable;
				}
				result = hashtable;
			}
			return result;
		}

		private Type CheckResourceSetType(Type usingResourceSet, bool verifyType)
		{
			if (usingResourceSet == null)
			{
				return this.resourceSetType;
			}
			if (verifyType && !typeof(ResourceSet).IsAssignableFrom(usingResourceSet))
			{
				throw new ArgumentException("Type parameter must refer to a subclass of ResourceSet.", "usingResourceSet");
			}
			return usingResourceSet;
		}

		/// <summary>Returns a <see cref="T:System.Resources.ResourceManager" /> that searches a specific directory for resources instead of in the assembly manifest.</summary>
		/// <returns>The newly created <see cref="T:System.Resources.ResourceManager" /> that searches a specific directory for resources instead of in the assembly manifest.</returns>
		/// <param name="baseName">The root name of the resources. For example, the root name for the resource file named "MyResource.en-US.resources" is "MyResource". </param>
		/// <param name="resourceDir">The name of the directory to search for the resources. </param>
		/// <param name="usingResourceSet">The <see cref="T:System.Type" /> of the custom <see cref="T:System.Resources.ResourceSet" /> to use. If null, the default runtime <see cref="T:System.Resources.ResourceSet" /> is used. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="baseName" /> or <paramref name="resourceDir" /> parameter is null. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public static ResourceManager CreateFileBasedResourceManager(string baseName, string resourceDir, Type usingResourceSet)
		{
			return new ResourceManager(baseName, resourceDir, usingResourceSet);
		}

		/// <summary>Gets the root name of the resource files that the <see cref="T:System.Resources.ResourceManager" /> searches for resources.</summary>
		/// <returns>The root name of the resource files that the <see cref="T:System.Resources.ResourceManager" /> searches for resources.</returns>
		public virtual string BaseName
		{
			get
			{
				return this.BaseNameField;
			}
		}

		/// <summary>Gets or sets a Boolean value indicating whether the current instance of ResourceManager allows case-insensitive resource lookups in the <see cref="M:System.Resources.ResourceManager.GetString(System.String)" /> and <see cref="M:System.Resources.ResourceManager.GetObject(System.String)" /> methods.</summary>
		/// <returns>A Boolean value indicating whether the case of the resource names should be ignored.</returns>
		public virtual bool IgnoreCase
		{
			get
			{
				return this.ignoreCase;
			}
			set
			{
				this.ignoreCase = value;
			}
		}

		/// <summary>Gets the <see cref="T:System.Type" /> of the <see cref="T:System.Resources.ResourceSet" /> the <see cref="T:System.Resources.ResourceManager" /> uses to construct a <see cref="T:System.Resources.ResourceSet" /> object.</summary>
		/// <returns>The <see cref="T:System.Type" /> of the <see cref="T:System.Resources.ResourceSet" /> the <see cref="T:System.Resources.ResourceManager" /> uses to construct a <see cref="T:System.Resources.ResourceSet" /> object.</returns>
		public virtual Type ResourceSetType
		{
			get
			{
				return this.resourceSetType;
			}
		}

		/// <summary>Returns the value of the specified <see cref="T:System.Object" /> resource.</summary>
		/// <returns>The value of the resource localized for the caller's current culture settings. If a match is not possible, null is returned. The resource value can be null.</returns>
		/// <param name="name">The name of the resource to get. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> parameter is null. </exception>
		/// <exception cref="T:System.Resources.MissingManifestResourceException">No usable set of resources has been found, and there are no neutral culture resources. </exception>
		public virtual object GetObject(string name)
		{
			return this.GetObject(name, null);
		}

		/// <summary>Gets the value of the <see cref="T:System.Object" /> resource localized for the specified culture.</summary>
		/// <returns>The value of the resource, localized for the specified culture. If a "best match" is not possible, null is returned.</returns>
		/// <param name="name">The name of the resource to get. </param>
		/// <param name="culture">The <see cref="T:System.Globalization.CultureInfo" /> object that represents the culture for which the resource is localized. Note that if the resource is not localized for this culture, the lookup will fall back using the culture's <see cref="P:System.Globalization.CultureInfo.Parent" /> property, stopping after checking in the neutral culture.If this value is null, the <see cref="T:System.Globalization.CultureInfo" /> is obtained using the culture's <see cref="P:System.Globalization.CultureInfo.CurrentUICulture" /> property. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> parameter is null. </exception>
		/// <exception cref="T:System.Resources.MissingManifestResourceException">No usable set of resources have been found, and there are no neutral culture resources. </exception>
		public virtual object GetObject(string name, CultureInfo culture)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (culture == null)
			{
				culture = CultureInfo.CurrentUICulture;
			}
			lock (this)
			{
				ResourceSet resourceSet = this.InternalGetResourceSet(culture, true, true);
				object @object;
				if (resourceSet != null)
				{
					@object = resourceSet.GetObject(name, this.ignoreCase);
					if (@object != null)
					{
						return @object;
					}
				}
				for (;;)
				{
					culture = culture.Parent;
					resourceSet = this.InternalGetResourceSet(culture, true, true);
					if (resourceSet != null)
					{
						@object = resourceSet.GetObject(name, this.ignoreCase);
						if (@object != null)
						{
							break;
						}
					}
					if (culture.Equals(this.neutral_culture) || culture.Equals(CultureInfo.InvariantCulture))
					{
						goto IL_A7;
					}
				}
				return @object;
				IL_A7:;
			}
			return null;
		}

		/// <summary>Gets the <see cref="T:System.Resources.ResourceSet" /> for a particular culture.</summary>
		/// <returns>The specified <see cref="T:System.Resources.ResourceSet" />.</returns>
		/// <param name="culture">The <see cref="T:System.Globalization.CultureInfo" /> to look for. </param>
		/// <param name="createIfNotExists">If true and if the <see cref="T:System.Resources.ResourceSet" /> has not been loaded yet, load it. </param>
		/// <param name="tryParents">If the <see cref="T:System.Resources.ResourceSet" /> cannot be loaded, try parent <see cref="T:System.Globalization.CultureInfo" /> objects to see if they exist. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="culture" /> parameter is null. </exception>
		public virtual ResourceSet GetResourceSet(CultureInfo culture, bool createIfNotExists, bool tryParents)
		{
			if (culture == null)
			{
				throw new ArgumentNullException("culture");
			}
			ResourceSet result;
			lock (this)
			{
				result = this.InternalGetResourceSet(culture, createIfNotExists, tryParents);
			}
			return result;
		}

		/// <summary>Returns the value of the specified <see cref="T:System.String" /> resource.</summary>
		/// <returns>The value of the resource localized for the caller's current culture settings. If a match is not possible, null is returned.</returns>
		/// <param name="name">The name of the resource to get. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> parameter is null. </exception>
		/// <exception cref="T:System.InvalidOperationException">The value of the specified resource is not a string. </exception>
		/// <exception cref="T:System.Resources.MissingManifestResourceException">No usable set of resources has been found, and there are no neutral culture resources. </exception>
		public virtual string GetString(string name)
		{
			return this.GetString(name, null);
		}

		/// <summary>Gets the value of the <see cref="T:System.String" /> resource localized for the specified culture.</summary>
		/// <returns>The value of the resource localized for the specified culture. If a best match is not possible, null is returned.</returns>
		/// <param name="name">The name of the resource to get. </param>
		/// <param name="culture">The <see cref="T:System.Globalization.CultureInfo" /> object that represents the culture for which the resource is localized. Note that if the resource is not localized for this culture, the lookup will fall back using the current thread's <see cref="P:System.Globalization.CultureInfo.Parent" /> property, stopping after looking in the neutral culture.If this value is null, the <see cref="T:System.Globalization.CultureInfo" /> is obtained using the current thread's <see cref="P:System.Globalization.CultureInfo.CurrentUICulture" /> property. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> parameter is null. </exception>
		/// <exception cref="T:System.InvalidOperationException">The value of the specified resource is not a <see cref="T:System.String" />. </exception>
		/// <exception cref="T:System.Resources.MissingManifestResourceException">No usable set of resources has been found, and there are no neutral culture resources. </exception>
		public virtual string GetString(string name, CultureInfo culture)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (culture == null)
			{
				culture = CultureInfo.CurrentUICulture;
			}
			lock (this)
			{
				ResourceSet resourceSet = this.InternalGetResourceSet(culture, true, true);
				string @string;
				if (resourceSet != null)
				{
					@string = resourceSet.GetString(name, this.ignoreCase);
					if (@string != null)
					{
						return @string;
					}
				}
				for (;;)
				{
					culture = culture.Parent;
					resourceSet = this.InternalGetResourceSet(culture, true, true);
					if (resourceSet != null)
					{
						@string = resourceSet.GetString(name, this.ignoreCase);
						if (@string != null)
						{
							break;
						}
					}
					if (culture.Equals(this.neutral_culture) || culture.Equals(CultureInfo.InvariantCulture))
					{
						goto IL_A7;
					}
				}
				return @string;
				IL_A7:;
			}
			return null;
		}

		/// <summary>Generates the name for the resource file for the given <see cref="T:System.Globalization.CultureInfo" />.</summary>
		/// <returns>The name that can be used for a resource file for the given <see cref="T:System.Globalization.CultureInfo" />.</returns>
		/// <param name="culture">The <see cref="T:System.Globalization.CultureInfo" /> for which a resource file name is constructed. </param>
		protected virtual string GetResourceFileName(CultureInfo culture)
		{
			if (culture.Equals(CultureInfo.InvariantCulture))
			{
				return this.BaseNameField + ".resources";
			}
			return this.BaseNameField + "." + culture.Name + ".resources";
		}

		private string GetResourceFilePath(CultureInfo culture)
		{
			if (this.resourceDir != null)
			{
				return Path.Combine(this.resourceDir, this.GetResourceFileName(culture));
			}
			return this.GetResourceFileName(culture);
		}

		private Stream GetManifestResourceStreamNoCase(Assembly ass, string fn)
		{
			string manifestResourceName = this.GetManifestResourceName(fn);
			foreach (string text in ass.GetManifestResourceNames())
			{
				if (string.Compare(manifestResourceName, text, true, CultureInfo.InvariantCulture) == 0)
				{
					return ass.GetManifestResourceStream(text);
				}
			}
			return null;
		}

		/// <summary>Returns an <see cref="T:System.IO.UnmanagedMemoryStream" /> object from the specified resource.</summary>
		/// <returns>An <see cref="T:System.IO.UnmanagedMemoryStream" /> object.</returns>
		/// <param name="name">The name of a resource.</param>
		/// <exception cref="T:System.InvalidOperationException">The value of the specified resource is not a <see cref="T:System.IO.MemoryStream" /> object.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> is null.</exception>
		/// <exception cref="T:System.Resources.MissingManifestResourceException">No usable set of resources is found, and there are no neutral resources.</exception>
		[CLSCompliant(false)]
		[ComVisible(false)]
		public UnmanagedMemoryStream GetStream(string name)
		{
			return this.GetStream(name, null);
		}

		/// <summary>Returns an <see cref="T:System.IO.UnmanagedMemoryStream" /> object from the specified resource, using the specified culture.</summary>
		/// <returns>An <see cref="T:System.IO.UnmanagedMemoryStream" /> object.</returns>
		/// <param name="name">The name of a resource.</param>
		/// <param name="culture">A <see cref="T:System.Globalization.CultureInfo" /> object that specifies the culture to use for the resource lookup. If <paramref name="culture" /> is null, the culture for the current thread is used.</param>
		/// <exception cref="T:System.InvalidOperationException">The value of the specified resource is not a <see cref="T:System.IO.MemoryStream" /> object.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> is null.</exception>
		/// <exception cref="T:System.Resources.MissingManifestResourceException">No usable set of resources is found, and there are no neutral resources.</exception>
		[CLSCompliant(false)]
		[ComVisible(false)]
		public UnmanagedMemoryStream GetStream(string name, CultureInfo culture)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (culture == null)
			{
				culture = CultureInfo.CurrentUICulture;
			}
			ResourceSet resourceSet = this.InternalGetResourceSet(culture, true, true);
			return resourceSet.GetStream(name, this.ignoreCase);
		}

		/// <summary>Provides the implementation for finding a <see cref="T:System.Resources.ResourceSet" />.</summary>
		/// <returns>The specified <see cref="T:System.Resources.ResourceSet" />.</returns>
		/// <param name="culture">The <see cref="T:System.Globalization.CultureInfo" /> to look for. </param>
		/// <param name="createIfNotExists">If true and if the <see cref="T:System.Resources.ResourceSet" /> has not been loaded yet, load it. </param>
		/// <param name="tryParents">If the <see cref="T:System.Resources.ResourceSet" /> cannot be loaded, try parent <see cref="T:System.Globalization.CultureInfo" /> objects to see if they exist. </param>
		/// <exception cref="T:System.Resources.MissingManifestResourceException">The main assembly does not contain a .resources file and it is required to look up a resource. </exception>
		/// <exception cref="T:System.ExecutionEngineException">There was an internal error in the runtime.</exception>
		/// <exception cref="T:System.Resources.MissingSatelliteAssemblyException">The satellite assembly associated with <paramref name="culture" /> could not be located.</exception>
		protected virtual ResourceSet InternalGetResourceSet(CultureInfo culture, bool createIfNotExists, bool tryParents)
		{
			if (culture == null)
			{
				throw new ArgumentNullException("key");
			}
			ResourceSet resourceSet = (ResourceSet)this.ResourceSets[culture];
			if (resourceSet != null)
			{
				return resourceSet;
			}
			if (ResourceManager.NonExistent.Contains(culture))
			{
				return null;
			}
			if (this.MainAssembly != null)
			{
				CultureInfo cultureInfo = culture;
				if (culture.Equals(this.neutral_culture))
				{
					cultureInfo = CultureInfo.InvariantCulture;
				}
				Stream stream = null;
				string resourceFileName = this.GetResourceFileName(cultureInfo);
				if (!cultureInfo.Equals(CultureInfo.InvariantCulture))
				{
					Version satelliteContractVersion = ResourceManager.GetSatelliteContractVersion(this.MainAssembly);
					try
					{
						Assembly satelliteAssemblyNoThrow = this.MainAssembly.GetSatelliteAssemblyNoThrow(cultureInfo, satelliteContractVersion);
						if (satelliteAssemblyNoThrow != null)
						{
							stream = satelliteAssemblyNoThrow.GetManifestResourceStream(resourceFileName);
							if (stream == null)
							{
								stream = this.GetManifestResourceStreamNoCase(satelliteAssemblyNoThrow, resourceFileName);
							}
						}
					}
					catch (Exception)
					{
					}
				}
				else
				{
					stream = this.MainAssembly.GetManifestResourceStream(this.resourceSource, resourceFileName);
					if (stream == null)
					{
						stream = this.GetManifestResourceStreamNoCase(this.MainAssembly, resourceFileName);
					}
				}
				if (stream != null && createIfNotExists)
				{
					object[] args = new object[]
					{
						stream
					};
					resourceSet = (ResourceSet)Activator.CreateInstance(this.resourceSetType, args);
				}
				else if (cultureInfo.Equals(CultureInfo.InvariantCulture))
				{
					throw this.AssemblyResourceMissing(resourceFileName);
				}
			}
			else if (this.resourceDir != null || this.BaseNameField != null)
			{
				string resourceFilePath = this.GetResourceFilePath(culture);
				if (createIfNotExists && File.Exists(resourceFilePath))
				{
					object[] args2 = new object[]
					{
						resourceFilePath
					};
					resourceSet = (ResourceSet)Activator.CreateInstance(this.resourceSetType, args2);
				}
				else if (culture.Equals(CultureInfo.InvariantCulture))
				{
					string message = string.Format("Could not find any resources appropriate for the specified culture (or the neutral culture) on disk.{0}baseName: {1}  locationInfo: {2}  fileName: {3}", new object[]
					{
						Environment.NewLine,
						this.BaseNameField,
						"<null>",
						this.GetResourceFileName(culture)
					});
					throw new MissingManifestResourceException(message);
				}
			}
			if (resourceSet == null && tryParents && !culture.Equals(CultureInfo.InvariantCulture))
			{
				resourceSet = this.InternalGetResourceSet(culture.Parent, createIfNotExists, tryParents);
			}
			if (resourceSet != null)
			{
				this.ResourceSets[culture] = resourceSet;
			}
			else
			{
				ResourceManager.NonExistent[culture] = culture;
			}
			return resourceSet;
		}

		/// <summary>Tells the <see cref="T:System.Resources.ResourceManager" /> to call <see cref="M:System.Resources.ResourceSet.Close" /> on all <see cref="T:System.Resources.ResourceSet" /> objects and release all resources.</summary>
		public virtual void ReleaseAllResources()
		{
			lock (this)
			{
				foreach (object obj in this.ResourceSets.Values)
				{
					ResourceSet resourceSet = (ResourceSet)obj;
					resourceSet.Close();
				}
				this.ResourceSets.Clear();
			}
		}

		/// <summary>Returns the <see cref="T:System.Globalization.CultureInfo" /> for the main assembly's neutral resources by reading the value of the <see cref="T:System.Resources.NeutralResourcesLanguageAttribute" /> on a specified <see cref="T:System.Reflection.Assembly" />.</summary>
		/// <returns>The culture from the <see cref="T:System.Resources.NeutralResourcesLanguageAttribute" />, if found; otherwise, <see cref="P:System.Globalization.CultureInfo.InvariantCulture" />.</returns>
		/// <param name="a">The assembly for which to return a <see cref="T:System.Globalization.CultureInfo" />. </param>
		protected static CultureInfo GetNeutralResourcesLanguage(Assembly a)
		{
			object[] customAttributes = a.GetCustomAttributes(typeof(NeutralResourcesLanguageAttribute), false);
			if (customAttributes.Length == 0)
			{
				return CultureInfo.InvariantCulture;
			}
			NeutralResourcesLanguageAttribute neutralResourcesLanguageAttribute = (NeutralResourcesLanguageAttribute)customAttributes[0];
			return new CultureInfo(neutralResourcesLanguageAttribute.CultureName);
		}

		/// <summary>Returns the <see cref="T:System.Version" /> specified by the <see cref="T:System.Resources.SatelliteContractVersionAttribute" /> in the given assembly.</summary>
		/// <returns>The satellite contract <see cref="T:System.Version" /> of the given assembly, or null if no version was found.</returns>
		/// <param name="a">The <see cref="T:System.Reflection.Assembly" /> for which to look up the <see cref="T:System.Resources.SatelliteContractVersionAttribute" />. </param>
		/// <exception cref="T:System.ArgumentException">The <see cref="T:System.Version" /> found in the assembly <paramref name="a" /> is invalid. </exception>
		protected static Version GetSatelliteContractVersion(Assembly a)
		{
			object[] customAttributes = a.GetCustomAttributes(typeof(SatelliteContractVersionAttribute), false);
			if (customAttributes.Length == 0)
			{
				return null;
			}
			SatelliteContractVersionAttribute satelliteContractVersionAttribute = (SatelliteContractVersionAttribute)customAttributes[0];
			return new Version(satelliteContractVersionAttribute.Version);
		}

		/// <summary>Gets or sets the location from which to retrieve neutral fallback resources.</summary>
		/// <returns>One of the <see cref="T:System.Resources.UltimateResourceFallbackLocation" /> values.</returns>
		[MonoTODO("the property exists but is not respected")]
		protected UltimateResourceFallbackLocation FallbackLocation
		{
			get
			{
				return this.fallbackLocation;
			}
			set
			{
				this.fallbackLocation = value;
			}
		}

		private MissingManifestResourceException AssemblyResourceMissing(string fileName)
		{
			AssemblyName assemblyName = (this.MainAssembly == null) ? null : this.MainAssembly.GetName();
			string manifestResourceName = this.GetManifestResourceName(fileName);
			string message = string.Format("Could not find any resources appropriate for the specified culture or the neutral culture.  Make sure \"{0}\" was correctly embedded or linked into assembly \"{1}\" at compile time, or that all the satellite assemblies required are loadable and fully signed.", manifestResourceName, (assemblyName == null) ? string.Empty : assemblyName.Name);
			throw new MissingManifestResourceException(message);
		}

		private string GetManifestResourceName(string fn)
		{
			string result;
			if (this.resourceSource != null)
			{
				if (this.resourceSource.Namespace != null && this.resourceSource.Namespace.Length > 0)
				{
					result = this.resourceSource.Namespace + "." + fn;
				}
				else
				{
					result = fn;
				}
			}
			else
			{
				result = fn;
			}
			return result;
		}
	}
}
