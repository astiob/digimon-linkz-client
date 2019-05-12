using Mono.Security;
using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Reflection.Emit
{
	/// <summary>Defines and represents a dynamic assembly.</summary>
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	[ComDefaultInterface(typeof(_AssemblyBuilder))]
	public sealed class AssemblyBuilder : Assembly, _AssemblyBuilder
	{
		private const AssemblyBuilderAccess COMPILER_ACCESS = (AssemblyBuilderAccess)2048;

		private UIntPtr dynamic_assembly;

		private MethodInfo entry_point;

		private ModuleBuilder[] modules;

		private string name;

		private string dir;

		private CustomAttributeBuilder[] cattrs;

		private MonoResource[] resources;

		private byte[] public_key;

		private string version;

		private string culture;

		private uint algid;

		private uint flags;

		private PEFileKinds pekind = PEFileKinds.Dll;

		private bool delay_sign;

		private uint access;

		private Module[] loaded_modules;

		private MonoWin32Resource[] win32_resources;

		private RefEmitPermissionSet[] permissions_minimum;

		private RefEmitPermissionSet[] permissions_optional;

		private RefEmitPermissionSet[] permissions_refused;

		private PortableExecutableKinds peKind;

		private ImageFileMachine machine;

		private bool corlib_internal;

		private Type[] type_forwarders;

		private byte[] pktoken;

		internal Type corlib_object_type = typeof(object);

		internal Type corlib_value_type = typeof(ValueType);

		internal Type corlib_enum_type = typeof(Enum);

		internal Type corlib_void_type = typeof(void);

		private ArrayList resource_writers;

		private Win32VersionResource version_res;

		private bool created;

		private bool is_module_only;

		private StrongName sn;

		private NativeResourceType native_resource;

		private readonly bool is_compiler_context;

		private string versioninfo_culture;

		private ModuleBuilder manifest_module;

		internal AssemblyBuilder(AssemblyName n, string directory, AssemblyBuilderAccess access, bool corlib_internal)
		{
			this.is_compiler_context = ((access & (AssemblyBuilderAccess)2048) != (AssemblyBuilderAccess)0);
			access &= (AssemblyBuilderAccess)(-2049);
			if (!Enum.IsDefined(typeof(AssemblyBuilderAccess), access))
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Argument value {0} is not valid.", new object[]
				{
					(int)access
				}), "access");
			}
			this.name = n.Name;
			this.access = (uint)access;
			this.flags = (uint)n.Flags;
			if (this.IsSave && (directory == null || directory.Length == 0))
			{
				this.dir = Directory.GetCurrentDirectory();
			}
			else
			{
				this.dir = directory;
			}
			if (n.CultureInfo != null)
			{
				this.culture = n.CultureInfo.Name;
				this.versioninfo_culture = n.CultureInfo.Name;
			}
			Version version = n.Version;
			if (version != null)
			{
				this.version = version.ToString();
			}
			if (n.KeyPair != null)
			{
				this.sn = n.KeyPair.StrongName();
			}
			else
			{
				byte[] publicKey = n.GetPublicKey();
				if (publicKey != null && publicKey.Length > 0)
				{
					this.sn = new StrongName(publicKey);
				}
			}
			if (this.sn != null)
			{
				this.flags |= 1u;
			}
			this.corlib_internal = corlib_internal;
			if (this.sn != null)
			{
				this.pktoken = new byte[this.sn.PublicKeyToken.Length * 2];
				int num = 0;
				foreach (byte b in this.sn.PublicKeyToken)
				{
					string text = b.ToString("x2");
					this.pktoken[num++] = (byte)text[0];
					this.pktoken[num++] = (byte)text[1];
				}
			}
			AssemblyBuilder.basic_init(this);
		}

		/// <summary>Maps a set of names to a corresponding set of dispatch identifiers.</summary>
		/// <param name="riid">Reserved for future use. Must be IID_NULL.</param>
		/// <param name="rgszNames">Passed-in array of names to be mapped.</param>
		/// <param name="cNames">Count of the names to be mapped.</param>
		/// <param name="lcid">The locale context in which to interpret the names.</param>
		/// <param name="rgDispId">Caller-allocated array which receives the IDs corresponding to the names.</param>
		/// <exception cref="T:System.NotImplementedException">The method is called late-bound using the COM IDispatch interface.</exception>
		void _AssemblyBuilder.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		/// <summary>Retrieves the type information for an object, which can then be used to get the type information for an interface.</summary>
		/// <param name="iTInfo">The type information to return.</param>
		/// <param name="lcid">The locale identifier for the type information.</param>
		/// <param name="ppTInfo">Receives a pointer to the requested type information object.</param>
		/// <exception cref="T:System.NotImplementedException">The method is called late-bound using the COM IDispatch interface.</exception>
		void _AssemblyBuilder.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		/// <summary>Retrieves the number of type information interfaces that an object provides (either 0 or 1).</summary>
		/// <param name="pcTInfo">Points to a location that receives the number of type information interfaces provided by the object.</param>
		/// <exception cref="T:System.NotImplementedException">The method is called late-bound using the COM IDispatch interface.</exception>
		void _AssemblyBuilder.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		/// <summary>Provides access to properties and methods exposed by an object.</summary>
		/// <param name="dispIdMember">Identifies the member.</param>
		/// <param name="riid">Reserved for future use. Must be IID_NULL.</param>
		/// <param name="lcid">The locale context in which to interpret arguments.</param>
		/// <param name="wFlags">Flags describing the context of the call.</param>
		/// <param name="pDispParams">Pointer to a structure containing an array of arguments, an array of argument DISPIDs for named arguments, and counts for the number of elements in the arrays.</param>
		/// <param name="pVarResult">Pointer to the location where the result is to be stored.</param>
		/// <param name="pExcepInfo">Pointer to a structure that contains exception information.</param>
		/// <param name="puArgErr">The index of the first argument that has an error.</param>
		/// <exception cref="T:System.NotImplementedException">The method is called late-bound using the COM IDispatch interface.</exception>
		void _AssemblyBuilder.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void basic_init(AssemblyBuilder ab);

		/// <summary>Gets the location of the assembly, as specified originally (such as in an <see cref="T:System.Reflection.AssemblyName" /> object).</summary>
		/// <returns>The location of the assembly, as specified originally.</returns>
		/// <exception cref="T:System.NotSupportedException">This method is not currently supported. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		public override string CodeBase
		{
			get
			{
				throw this.not_supported();
			}
		}

		/// <summary>Returns the entry point of this assembly.</summary>
		/// <returns>The entry point of this assembly.</returns>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		public override MethodInfo EntryPoint
		{
			get
			{
				return this.entry_point;
			}
		}

		/// <summary>Gets the location, in codebase format, of the loaded file that contains the manifest if it is not shadow-copied.</summary>
		/// <returns>The location of the loaded file that contains the manifest. If the loaded file has been shadow-copied, the Location is that of the file before being shadow-copied.</returns>
		/// <exception cref="T:System.NotSupportedException">This method is not currently supported. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		public override string Location
		{
			get
			{
				throw this.not_supported();
			}
		}

		/// <summary>Gets the version of the common language runtime that will be saved in the file containing the manifest.</summary>
		/// <returns>A string representing the common language runtime version.</returns>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public override string ImageRuntimeVersion
		{
			get
			{
				return base.ImageRuntimeVersion;
			}
		}

		/// <summary>Gets a value indicating whether the dynamic assembly is in the reflection-only context.</summary>
		/// <returns>true if the dynamic assembly is in the reflection-only context; otherwise, false.</returns>
		[MonoTODO]
		public override bool ReflectionOnly
		{
			get
			{
				return base.ReflectionOnly;
			}
		}

		/// <summary>Adds an existing resource file to this assembly.</summary>
		/// <param name="name">The logical name of the resource. </param>
		/// <param name="fileName">The physical file name (.resources file) to which the logical name is mapped. This should not include a path; the file must be in the same directory as the assembly to which it is added. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="name" /> has been previously defined.-or- There is another file in the assembly named <paramref name="fileName" />.-or- The length of <paramref name="name" /> is zero.-or- The length of <paramref name="fileName" /> is zero, or if <paramref name="fileName" /> includes a path. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> or <paramref name="fileName" /> is null. </exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The file <paramref name="fileName" /> is not found. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public void AddResourceFile(string name, string fileName)
		{
			this.AddResourceFile(name, fileName, ResourceAttributes.Public);
		}

		/// <summary>Adds an existing resource file to this assembly.</summary>
		/// <param name="name">The logical name of the resource. </param>
		/// <param name="fileName">The physical file name (.resources file) to which the logical name is mapped. This should not include a path; the file must be in the same directory as the assembly to which it is added. </param>
		/// <param name="attribute">The resource attributes. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="name" /> has been previously defined.-or- There is another file in the assembly named <paramref name="fileName" />.-or- The length of <paramref name="name" /> is zero or if the length of <paramref name="fileName" /> is zero.-or- <paramref name="fileName" /> includes a path. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> or <paramref name="fileName" /> is null. </exception>
		/// <exception cref="T:System.IO.FileNotFoundException">If the file <paramref name="fileName" /> is not found. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public void AddResourceFile(string name, string fileName, ResourceAttributes attribute)
		{
			this.AddResourceFile(name, fileName, attribute, true);
		}

		private void AddResourceFile(string name, string fileName, ResourceAttributes attribute, bool fileNeedsToExists)
		{
			this.check_name_and_filename(name, fileName, fileNeedsToExists);
			if (this.dir != null)
			{
				fileName = Path.Combine(this.dir, fileName);
			}
			if (this.resources != null)
			{
				MonoResource[] destinationArray = new MonoResource[this.resources.Length + 1];
				Array.Copy(this.resources, destinationArray, this.resources.Length);
				this.resources = destinationArray;
			}
			else
			{
				this.resources = new MonoResource[1];
			}
			int num = this.resources.Length - 1;
			this.resources[num].name = name;
			this.resources[num].filename = fileName;
			this.resources[num].attrs = attribute;
		}

		internal void AddPermissionRequests(PermissionSet required, PermissionSet optional, PermissionSet refused)
		{
		}

		internal void EmbedResourceFile(string name, string fileName)
		{
			this.EmbedResourceFile(name, fileName, ResourceAttributes.Public);
		}

		internal void EmbedResourceFile(string name, string fileName, ResourceAttributes attribute)
		{
			if (this.resources != null)
			{
				MonoResource[] destinationArray = new MonoResource[this.resources.Length + 1];
				Array.Copy(this.resources, destinationArray, this.resources.Length);
				this.resources = destinationArray;
			}
			else
			{
				this.resources = new MonoResource[1];
			}
			int num = this.resources.Length - 1;
			this.resources[num].name = name;
			this.resources[num].attrs = attribute;
			try
			{
				FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
				long length = fileStream.Length;
				this.resources[num].data = new byte[length];
				fileStream.Read(this.resources[num].data, 0, (int)length);
				fileStream.Close();
			}
			catch
			{
			}
		}

		internal void EmbedResource(string name, byte[] blob, ResourceAttributes attribute)
		{
			if (this.resources != null)
			{
				MonoResource[] destinationArray = new MonoResource[this.resources.Length + 1];
				Array.Copy(this.resources, destinationArray, this.resources.Length);
				this.resources = destinationArray;
			}
			else
			{
				this.resources = new MonoResource[1];
			}
			int num = this.resources.Length - 1;
			this.resources[num].name = name;
			this.resources[num].attrs = attribute;
			this.resources[num].data = blob;
		}

		internal void AddTypeForwarder(Type t)
		{
			if (t == null)
			{
				throw new ArgumentNullException("t");
			}
			if (this.type_forwarders == null)
			{
				this.type_forwarders = new Type[]
				{
					t
				};
			}
			else
			{
				Type[] array = new Type[this.type_forwarders.Length + 1];
				Array.Copy(this.type_forwarders, array, this.type_forwarders.Length);
				array[this.type_forwarders.Length] = t;
				this.type_forwarders = array;
			}
		}

		/// <summary>Defines a named transient dynamic module in this assembly.</summary>
		/// <returns>A <see cref="T:System.Reflection.Emit.ModuleBuilder" /> representing the defined dynamic module.</returns>
		/// <param name="name">The name of the dynamic module. Must be less than 260 characters in length. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="name" /> begins with white space.-or- The length of <paramref name="name" /> is zero.-or- The length of <paramref name="name" /> is greater than or equal to 260. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> is null. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		/// <exception cref="T:System.ExecutionEngineException">The assembly for default symbol writer cannot be loaded.-or- The type that implements the default symbol writer interface cannot be found. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*" />
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		/// </PermissionSet>
		public ModuleBuilder DefineDynamicModule(string name)
		{
			return this.DefineDynamicModule(name, name, false, true);
		}

		/// <summary>Defines a named transient dynamic module in this assembly and specifies whether symbol information should be emitted.</summary>
		/// <returns>A <see cref="T:System.Reflection.Emit.ModuleBuilder" /> representing the defined dynamic module.</returns>
		/// <param name="name">The name of the dynamic module. Must be less than 260 characters in length. </param>
		/// <param name="emitSymbolInfo">true if symbol information is to be emitted; otherwise, false. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="name" /> begins with white space.-or- The length of <paramref name="name" /> is zero.-or- The length of <paramref name="name" /> is greater than or equal to 260. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> is null. </exception>
		/// <exception cref="T:System.ExecutionEngineException">The assembly for default symbol writer cannot be loaded.-or- The type that implements the default symbol writer interface cannot be found. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*" />
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		/// </PermissionSet>
		public ModuleBuilder DefineDynamicModule(string name, bool emitSymbolInfo)
		{
			return this.DefineDynamicModule(name, name, emitSymbolInfo, true);
		}

		/// <summary>Defines a persistable dynamic module with the given name that will be saved to the specified file. No symbol information is emitted.</summary>
		/// <returns>A <see cref="T:System.Reflection.Emit.ModuleBuilder" /> object representing the defined dynamic module.</returns>
		/// <param name="name">The name of the dynamic module. Must be less than 260 characters in length. </param>
		/// <param name="fileName">The name of the file to which the dynamic module should be saved. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> or <paramref name="fileName" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">The length of <paramref name="name" /> or <paramref name="fileName" /> is zero.-or- The length of <paramref name="name" /> is greater than or equal to 260.-or- <paramref name="fileName" /> contains a path specification (a directory component, for example).-or- There is a conflict with the name of another file that belongs to this assembly. </exception>
		/// <exception cref="T:System.InvalidOperationException">This assembly has been previously saved. </exception>
		/// <exception cref="T:System.NotSupportedException">This assembly was called on a dynamic assembly with <see cref="F:System.Reflection.Emit.AssemblyBuilderAccess.Run" /> attribute. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		/// <exception cref="T:System.ExecutionEngineException">The assembly for default symbol writer cannot be loaded.-or- The type that implements the default symbol writer interface cannot be found. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*" />
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		/// </PermissionSet>
		public ModuleBuilder DefineDynamicModule(string name, string fileName)
		{
			return this.DefineDynamicModule(name, fileName, false, false);
		}

		/// <summary>Defines a persistable dynamic module, specifying the module name, the name of the file to which the module will be saved, and whether symbol information should be emitted using the default symbol writer.</summary>
		/// <returns>A <see cref="T:System.Reflection.Emit.ModuleBuilder" /> object representing the defined dynamic module.</returns>
		/// <param name="name">The name of the dynamic module. Must be less than 260 characters in length. </param>
		/// <param name="fileName">The name of the file to which the dynamic module should be saved. </param>
		/// <param name="emitSymbolInfo">If true, symbolic information is written using the default symbol writer. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> or <paramref name="fileName" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">The length of <paramref name="name" /> or <paramref name="fileName" /> is zero.-or- The length of <paramref name="name" /> is greater than or equal to 260.-or- <paramref name="fileName" /> contains a path specification (a directory component, for example).-or- There is a conflict with the name of another file that belongs to this assembly. </exception>
		/// <exception cref="T:System.InvalidOperationException">This assembly has been previously saved. </exception>
		/// <exception cref="T:System.NotSupportedException">This assembly was called on a dynamic assembly with the <see cref="F:System.Reflection.Emit.AssemblyBuilderAccess.Run" /> attribute. </exception>
		/// <exception cref="T:System.ExecutionEngineException">The assembly for default symbol writer cannot be loaded.-or- The type that implements the default symbol writer interface cannot be found. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*" />
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		/// </PermissionSet>
		public ModuleBuilder DefineDynamicModule(string name, string fileName, bool emitSymbolInfo)
		{
			return this.DefineDynamicModule(name, fileName, emitSymbolInfo, false);
		}

		private ModuleBuilder DefineDynamicModule(string name, string fileName, bool emitSymbolInfo, bool transient)
		{
			this.check_name_and_filename(name, fileName, false);
			if (!transient)
			{
				if (Path.GetExtension(fileName) == string.Empty)
				{
					throw new ArgumentException("Module file name '" + fileName + "' must have file extension.");
				}
				if (!this.IsSave)
				{
					throw new NotSupportedException("Persistable modules are not supported in a dynamic assembly created with AssemblyBuilderAccess.Run");
				}
				if (this.created)
				{
					throw new InvalidOperationException("Assembly was already saved.");
				}
			}
			ModuleBuilder moduleBuilder = new ModuleBuilder(this, name, fileName, emitSymbolInfo, transient);
			if (this.modules != null && this.is_module_only)
			{
				throw new InvalidOperationException("A module-only assembly can only contain one module.");
			}
			if (this.modules != null)
			{
				ModuleBuilder[] destinationArray = new ModuleBuilder[this.modules.Length + 1];
				Array.Copy(this.modules, destinationArray, this.modules.Length);
				this.modules = destinationArray;
			}
			else
			{
				this.modules = new ModuleBuilder[1];
			}
			this.modules[this.modules.Length - 1] = moduleBuilder;
			return moduleBuilder;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Module InternalAddModule(string fileName);

		internal Module AddModule(string fileName)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException(fileName);
			}
			Module module = this.InternalAddModule(fileName);
			if (this.loaded_modules != null)
			{
				Module[] destinationArray = new Module[this.loaded_modules.Length + 1];
				Array.Copy(this.loaded_modules, destinationArray, this.loaded_modules.Length);
				this.loaded_modules = destinationArray;
			}
			else
			{
				this.loaded_modules = new Module[1];
			}
			this.loaded_modules[this.loaded_modules.Length - 1] = module;
			return module;
		}

		/// <summary>Defines a standalone managed resource for this assembly with the default public resource attribute.</summary>
		/// <returns>A <see cref="T:System.Resources.ResourceWriter" /> object for the specified resource.</returns>
		/// <param name="name">The logical name of the resource. </param>
		/// <param name="description">A textual description of the resource. </param>
		/// <param name="fileName">The physical file name (.resources file) to which the logical name is mapped. This should not include a path. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="name" /> has been previously defined.-or- There is another file in the assembly named <paramref name="fileName" />.-or- The length of <paramref name="name" /> is zero.-or- The length of <paramref name="fileName" /> is zero.-or- <paramref name="fileName" /> includes a path. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> or <paramref name="fileName" /> is null. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public IResourceWriter DefineResource(string name, string description, string fileName)
		{
			return this.DefineResource(name, description, fileName, ResourceAttributes.Public);
		}

		/// <summary>Defines a standalone managed resource for this assembly. Attributes can be specified for the managed resource.</summary>
		/// <returns>A <see cref="T:System.Resources.ResourceWriter" /> object for the specified resource.</returns>
		/// <param name="name">The logical name of the resource. </param>
		/// <param name="description">A textual description of the resource. </param>
		/// <param name="fileName">The physical file name (.resources file) to which the logical name is mapped. This should not include a path. </param>
		/// <param name="attribute">The resource attributes. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="name" /> has been previously defined or if there is another file in the assembly named <paramref name="fileName" />.-or- The length of <paramref name="name" /> is zero.-or- The length of <paramref name="fileName" /> is zero.-or- <paramref name="fileName" /> includes a path. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> or <paramref name="fileName" /> is null. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public IResourceWriter DefineResource(string name, string description, string fileName, ResourceAttributes attribute)
		{
			this.AddResourceFile(name, fileName, attribute, false);
			IResourceWriter resourceWriter = new ResourceWriter(fileName);
			if (this.resource_writers == null)
			{
				this.resource_writers = new ArrayList();
			}
			this.resource_writers.Add(resourceWriter);
			return resourceWriter;
		}

		private void AddUnmanagedResource(Win32Resource res)
		{
			MemoryStream memoryStream = new MemoryStream();
			res.WriteTo(memoryStream);
			if (this.win32_resources != null)
			{
				MonoWin32Resource[] destinationArray = new MonoWin32Resource[this.win32_resources.Length + 1];
				Array.Copy(this.win32_resources, destinationArray, this.win32_resources.Length);
				this.win32_resources = destinationArray;
			}
			else
			{
				this.win32_resources = new MonoWin32Resource[1];
			}
			this.win32_resources[this.win32_resources.Length - 1] = new MonoWin32Resource(res.Type.Id, res.Name.Id, res.Language, memoryStream.ToArray());
		}

		/// <summary>Defines an unmanaged resource for this assembly as an opaque blob of bytes.</summary>
		/// <param name="resource">The opaque blob of bytes representing the unmanaged resource. </param>
		/// <exception cref="T:System.ArgumentException">An unmanaged resource was previously defined. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="resource" /> is null. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		[MonoTODO("Not currently implemenented")]
		public void DefineUnmanagedResource(byte[] resource)
		{
			if (resource == null)
			{
				throw new ArgumentNullException("resource");
			}
			if (this.native_resource != NativeResourceType.None)
			{
				throw new ArgumentException("Native resource has already been defined.");
			}
			this.native_resource = NativeResourceType.Unmanaged;
			throw new NotImplementedException();
		}

		/// <summary>Defines an unmanaged resource file for this assembly given the name of the resource file.</summary>
		/// <param name="resourceFileName">The name of the resource file. </param>
		/// <exception cref="T:System.ArgumentException">An unmanaged resource was previously defined.-or- The file <paramref name="resourceFileName" /> is not readable.-or- <paramref name="resourceFileName" /> is the empty string (""). </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="resourceFileName" /> is null. </exception>
		/// <exception cref="T:System.IO.FileNotFoundException">
		///   <paramref name="resourceFileName" /> is not found.-or- <paramref name="resourceFileName" /> is a directory. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public void DefineUnmanagedResource(string resourceFileName)
		{
			if (resourceFileName == null)
			{
				throw new ArgumentNullException("resourceFileName");
			}
			if (resourceFileName.Length == 0)
			{
				throw new ArgumentException("resourceFileName");
			}
			if (!File.Exists(resourceFileName) || Directory.Exists(resourceFileName))
			{
				throw new FileNotFoundException("File '" + resourceFileName + "' does not exists or is a directory.");
			}
			if (this.native_resource != NativeResourceType.None)
			{
				throw new ArgumentException("Native resource has already been defined.");
			}
			this.native_resource = NativeResourceType.Unmanaged;
			using (FileStream fileStream = new FileStream(resourceFileName, FileMode.Open, FileAccess.Read))
			{
				Win32ResFileReader win32ResFileReader = new Win32ResFileReader(fileStream);
				foreach (object obj in win32ResFileReader.ReadResources())
				{
					Win32EncodedResource win32EncodedResource = (Win32EncodedResource)obj;
					if (win32EncodedResource.Name.IsName || win32EncodedResource.Type.IsName)
					{
						throw new InvalidOperationException("resource files with named resources or non-default resource types are not supported.");
					}
					this.AddUnmanagedResource(win32EncodedResource);
				}
			}
		}

		/// <summary>Defines an unmanaged version information resource using the information specified in the assembly's AssemblyName object and the assembly's custom attributes.</summary>
		/// <exception cref="T:System.ArgumentException">An unmanaged version information resource was previously defined.-or- The unmanaged version information is too large to persist. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		public void DefineVersionInfoResource()
		{
			if (this.native_resource != NativeResourceType.None)
			{
				throw new ArgumentException("Native resource has already been defined.");
			}
			this.native_resource = NativeResourceType.Assembly;
			this.version_res = new Win32VersionResource(1, 0, this.IsCompilerContext);
		}

		/// <summary>Defines an unmanaged version information resource for this assembly with the given specifications.</summary>
		/// <param name="product">The name of the product with which this assembly is distributed. </param>
		/// <param name="productVersion">The version of the product with which this assembly is distributed. </param>
		/// <param name="company">The name of the company that produced this assembly. </param>
		/// <param name="copyright">Describes all copyright notices, trademarks, and registered trademarks that apply to this assembly. This should include the full text of all notices, legal symbols, copyright dates, trademark numbers, and so on. In English, this string should be in the format "Copyright Microsoft Corp. 1990-2001". </param>
		/// <param name="trademark">Describes all trademarks and registered trademarks that apply to this assembly. This should include the full text of all notices, legal symbols, trademark numbers, and so on. In English, this string should be in the format "Windows is a trademark of Microsoft Corporation". </param>
		/// <exception cref="T:System.ArgumentException">An unmanaged version information resource was previously defined.-or- The unmanaged version information is too large to persist. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		public void DefineVersionInfoResource(string product, string productVersion, string company, string copyright, string trademark)
		{
			if (this.native_resource != NativeResourceType.None)
			{
				throw new ArgumentException("Native resource has already been defined.");
			}
			this.native_resource = NativeResourceType.Explicit;
			this.version_res = new Win32VersionResource(1, 0, false);
			this.version_res.ProductName = ((product == null) ? " " : product);
			this.version_res.ProductVersion = ((productVersion == null) ? " " : productVersion);
			this.version_res.CompanyName = ((company == null) ? " " : company);
			this.version_res.LegalCopyright = ((copyright == null) ? " " : copyright);
			this.version_res.LegalTrademarks = ((trademark == null) ? " " : trademark);
		}

		internal void DefineIconResource(string iconFileName)
		{
			if (iconFileName == null)
			{
				throw new ArgumentNullException("iconFileName");
			}
			if (iconFileName.Length == 0)
			{
				throw new ArgumentException("iconFileName");
			}
			if (!File.Exists(iconFileName) || Directory.Exists(iconFileName))
			{
				throw new FileNotFoundException("File '" + iconFileName + "' does not exists or is a directory.");
			}
			using (FileStream fileStream = new FileStream(iconFileName, FileMode.Open, FileAccess.Read))
			{
				Win32IconFileReader win32IconFileReader = new Win32IconFileReader(fileStream);
				ICONDIRENTRY[] array = win32IconFileReader.ReadIcons();
				Win32IconResource[] array2 = new Win32IconResource[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array2[i] = new Win32IconResource(i + 1, 0, array[i]);
					this.AddUnmanagedResource(array2[i]);
				}
				Win32GroupIconResource res = new Win32GroupIconResource(1, 0, array2);
				this.AddUnmanagedResource(res);
			}
		}

		private void DefineVersionInfoResourceImpl(string fileName)
		{
			if (this.versioninfo_culture != null)
			{
				this.version_res.FileLanguage = new CultureInfo(this.versioninfo_culture).LCID;
			}
			this.version_res.Version = ((this.version != null) ? this.version : "0.0.0.0");
			if (this.cattrs != null)
			{
				NativeResourceType nativeResourceType = this.native_resource;
				if (nativeResourceType != NativeResourceType.Assembly)
				{
					if (nativeResourceType == NativeResourceType.Explicit)
					{
						foreach (CustomAttributeBuilder customAttributeBuilder in this.cattrs)
						{
							string fullName = customAttributeBuilder.Ctor.ReflectedType.FullName;
							if (fullName == "System.Reflection.AssemblyCultureAttribute")
							{
								if (!this.IsCompilerContext)
								{
									this.version_res.FileLanguage = new CultureInfo(customAttributeBuilder.string_arg()).LCID;
								}
							}
							else if (fullName == "System.Reflection.AssemblyDescriptionAttribute")
							{
								this.version_res.Comments = customAttributeBuilder.string_arg();
							}
						}
					}
				}
				else
				{
					foreach (CustomAttributeBuilder customAttributeBuilder2 in this.cattrs)
					{
						string fullName2 = customAttributeBuilder2.Ctor.ReflectedType.FullName;
						if (fullName2 == "System.Reflection.AssemblyProductAttribute")
						{
							this.version_res.ProductName = customAttributeBuilder2.string_arg();
						}
						else if (fullName2 == "System.Reflection.AssemblyCompanyAttribute")
						{
							this.version_res.CompanyName = customAttributeBuilder2.string_arg();
						}
						else if (fullName2 == "System.Reflection.AssemblyCopyrightAttribute")
						{
							this.version_res.LegalCopyright = customAttributeBuilder2.string_arg();
						}
						else if (fullName2 == "System.Reflection.AssemblyTrademarkAttribute")
						{
							this.version_res.LegalTrademarks = customAttributeBuilder2.string_arg();
						}
						else if (fullName2 == "System.Reflection.AssemblyCultureAttribute")
						{
							if (!this.IsCompilerContext)
							{
								this.version_res.FileLanguage = new CultureInfo(customAttributeBuilder2.string_arg()).LCID;
							}
						}
						else if (fullName2 == "System.Reflection.AssemblyFileVersionAttribute")
						{
							string text = customAttributeBuilder2.string_arg();
							if (!this.IsCompilerContext || (text != null && text.Length != 0))
							{
								this.version_res.FileVersion = text;
							}
						}
						else if (fullName2 == "System.Reflection.AssemblyInformationalVersionAttribute")
						{
							this.version_res.ProductVersion = customAttributeBuilder2.string_arg();
						}
						else if (fullName2 == "System.Reflection.AssemblyTitleAttribute")
						{
							this.version_res.FileDescription = customAttributeBuilder2.string_arg();
						}
						else if (fullName2 == "System.Reflection.AssemblyDescriptionAttribute")
						{
							this.version_res.Comments = customAttributeBuilder2.string_arg();
						}
					}
				}
			}
			this.version_res.OriginalFilename = fileName;
			if (this.IsCompilerContext)
			{
				this.version_res.InternalName = fileName;
				if (this.version_res.ProductVersion.Trim().Length == 0)
				{
					this.version_res.ProductVersion = this.version_res.FileVersion;
				}
			}
			else
			{
				this.version_res.InternalName = Path.GetFileNameWithoutExtension(fileName);
			}
			this.AddUnmanagedResource(this.version_res);
		}

		/// <summary>Returns the dynamic module with the specified name.</summary>
		/// <returns>A ModuleBuilder object representing the requested dynamic module.</returns>
		/// <param name="name">The name of the requested dynamic module. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">The length of <paramref name="name" /> is zero. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		public ModuleBuilder GetDynamicModule(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException("Empty name is not legal.", "name");
			}
			if (this.modules != null)
			{
				for (int i = 0; i < this.modules.Length; i++)
				{
					if (this.modules[i].name == name)
					{
						return this.modules[i];
					}
				}
			}
			return null;
		}

		/// <summary>Gets the exported types defined in this assembly.</summary>
		/// <returns>An array of <see cref="T:System.Type" /> containing the exported types defined in this assembly.</returns>
		/// <exception cref="T:System.NotSupportedException">This method is not implemented. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		public override Type[] GetExportedTypes()
		{
			throw this.not_supported();
		}

		/// <summary>Gets a <see cref="T:System.IO.FileStream" /> for the specified file in the file table of the manifest of this assembly.</summary>
		/// <returns>A <see cref="T:System.IO.FileStream" /> for the specified file, or null, if the file is not found.</returns>
		/// <param name="name">The name of the specified file. </param>
		/// <exception cref="T:System.NotSupportedException">This method is not currently supported. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		public override FileStream GetFile(string name)
		{
			throw this.not_supported();
		}

		/// <summary>Gets the files in the file table of an assembly manifest, specifying whether to include resource modules.</summary>
		/// <returns>An array of <see cref="T:System.IO.FileStream" /> objects.</returns>
		/// <param name="getResourceModules">true to include resource modules; otherwise, false. </param>
		/// <exception cref="T:System.NotSupportedException">This method is not currently supported. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		public override FileStream[] GetFiles(bool getResourceModules)
		{
			throw this.not_supported();
		}

		internal override Module[] GetModulesInternal()
		{
			if (this.modules == null)
			{
				return new Module[0];
			}
			return (Module[])this.modules.Clone();
		}

		internal override Type[] GetTypes(bool exportedOnly)
		{
			Type[] array = null;
			if (this.modules != null)
			{
				for (int i = 0; i < this.modules.Length; i++)
				{
					Type[] types = this.modules[i].GetTypes();
					if (array == null)
					{
						array = types;
					}
					else
					{
						Type[] destinationArray = new Type[array.Length + types.Length];
						Array.Copy(array, 0, destinationArray, 0, array.Length);
						Array.Copy(types, 0, destinationArray, array.Length, types.Length);
					}
				}
			}
			if (this.loaded_modules != null)
			{
				for (int j = 0; j < this.loaded_modules.Length; j++)
				{
					Type[] types2 = this.loaded_modules[j].GetTypes();
					if (array == null)
					{
						array = types2;
					}
					else
					{
						Type[] destinationArray2 = new Type[array.Length + types2.Length];
						Array.Copy(array, 0, destinationArray2, 0, array.Length);
						Array.Copy(types2, 0, destinationArray2, array.Length, types2.Length);
					}
				}
			}
			return (array != null) ? array : Type.EmptyTypes;
		}

		/// <summary>Returns information about how the given resource has been persisted.</summary>
		/// <returns>
		///   <see cref="T:System.Reflection.ManifestResourceInfo" /> populated with information about the resource's topology, or null if the resource is not found.</returns>
		/// <param name="resourceName">The name of the resource. </param>
		/// <exception cref="T:System.NotSupportedException">This method is not currently supported. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		public override ManifestResourceInfo GetManifestResourceInfo(string resourceName)
		{
			throw this.not_supported();
		}

		/// <summary>Loads the specified manifest resource from this assembly.</summary>
		/// <returns>An array of type String containing the names of all the resources.</returns>
		/// <exception cref="T:System.NotSupportedException">This method is not supported on a dynamic assembly. To get the manifest resource names, use <see cref="M:System.Reflection.Assembly.GetManifestResourceNames" />. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		public override string[] GetManifestResourceNames()
		{
			throw this.not_supported();
		}

		/// <summary>Loads the specified manifest resource from this assembly.</summary>
		/// <returns>A <see cref="T:System.IO.Stream" /> representing this manifest resource.</returns>
		/// <param name="name">The name of the manifest resource being requested. </param>
		/// <exception cref="T:System.NotSupportedException">This method is not currently supported. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		public override Stream GetManifestResourceStream(string name)
		{
			throw this.not_supported();
		}

		/// <summary>Loads the specified manifest resource, scoped by the namespace of the specified type, from this assembly.</summary>
		/// <returns>A <see cref="T:System.IO.Stream" /> representing this manifest resource.</returns>
		/// <param name="type">The type whose namespace is used to scope the manifest resource name. </param>
		/// <param name="name">The name of the manifest resource being requested. </param>
		/// <exception cref="T:System.NotSupportedException">This method is not currently supported. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		public override Stream GetManifestResourceStream(Type type, string name)
		{
			throw this.not_supported();
		}

		internal bool IsCompilerContext
		{
			get
			{
				return this.is_compiler_context;
			}
		}

		internal bool IsSave
		{
			get
			{
				return this.access != 1u;
			}
		}

		internal bool IsRun
		{
			get
			{
				return this.access == 1u || this.access == 3u;
			}
		}

		internal string AssemblyDir
		{
			get
			{
				return this.dir;
			}
		}

		internal bool IsModuleOnly
		{
			get
			{
				return this.is_module_only;
			}
			set
			{
				this.is_module_only = value;
			}
		}

		internal override Module GetManifestModule()
		{
			if (this.manifest_module == null)
			{
				this.manifest_module = this.DefineDynamicModule("Default Dynamic Module");
			}
			return this.manifest_module;
		}

		/// <summary>Saves this dynamic assembly to disk, specifying the nature of code in the assembly's executables and the target platform.</summary>
		/// <param name="assemblyFileName">The file name of the assembly.</param>
		/// <param name="portableExecutableKind">A bitwise combination of the <see cref="T:System.Reflection.PortableExecutableKinds" /> values that specifies the nature of the code.</param>
		/// <param name="imageFileMachine">One of the <see cref="T:System.Reflection.ImageFileMachine" /> values that specifies the target platform.</param>
		/// <exception cref="T:System.ArgumentException">The length of <paramref name="assemblyFileName" /> is 0.-or- There are two or more modules resource files in the assembly with the same name.-or- The target directory of the assembly is invalid.-or- <paramref name="assemblyFileName" /> is not a simple file name (for example, has a directory or drive component), or more than one unmanaged resource, including a version information resources, was defined in this assembly.-or- The CultureInfo string in <see cref="T:System.Reflection.AssemblyCultureAttribute" /> is not a valid string and <see cref="M:System.Reflection.Emit.AssemblyBuilder.DefineVersionInfoResource(System.String,System.String,System.String,System.String,System.String)" /> was called prior to calling this method. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="assemblyFileName" /> is null. </exception>
		/// <exception cref="T:System.InvalidOperationException">This assembly has been saved before.-or- This assembly has access Run<see cref="T:System.Reflection.Emit.AssemblyBuilderAccess" />. </exception>
		/// <exception cref="T:System.IO.IOException">An output error occurs during the save. </exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <see cref="M:System.Reflection.Emit.TypeBuilder.CreateType" /> has not been called for any of the types in the modules of the assembly to be written to disk. </exception>
		[MonoLimitation("No support for PE32+ assemblies for AMD64 and IA64")]
		public void Save(string assemblyFileName, PortableExecutableKinds portableExecutableKind, ImageFileMachine imageFileMachine)
		{
			this.peKind = portableExecutableKind;
			this.machine = imageFileMachine;
			if ((this.peKind & PortableExecutableKinds.PE32Plus) != PortableExecutableKinds.NotAPortableExecutableImage || (this.peKind & PortableExecutableKinds.Unmanaged32Bit) != PortableExecutableKinds.NotAPortableExecutableImage)
			{
				throw new NotImplementedException(this.peKind.ToString());
			}
			if (this.machine == ImageFileMachine.IA64 || this.machine == ImageFileMachine.AMD64)
			{
				throw new NotImplementedException(this.machine.ToString());
			}
			if (this.resource_writers != null)
			{
				foreach (object obj in this.resource_writers)
				{
					IResourceWriter resourceWriter = (IResourceWriter)obj;
					resourceWriter.Generate();
					resourceWriter.Close();
				}
			}
			ModuleBuilder moduleBuilder = null;
			if (this.modules != null)
			{
				foreach (ModuleBuilder moduleBuilder2 in this.modules)
				{
					if (moduleBuilder2.FullyQualifiedName == assemblyFileName)
					{
						moduleBuilder = moduleBuilder2;
					}
				}
			}
			if (moduleBuilder == null)
			{
				moduleBuilder = this.DefineDynamicModule("RefEmit_OnDiskManifestModule", assemblyFileName);
			}
			if (!this.is_module_only)
			{
				moduleBuilder.IsMain = true;
			}
			if (this.entry_point != null && this.entry_point.DeclaringType.Module != moduleBuilder)
			{
				Type[] array2;
				if (this.entry_point.GetParameters().Length == 1)
				{
					array2 = new Type[]
					{
						typeof(string)
					};
				}
				else
				{
					array2 = Type.EmptyTypes;
				}
				MethodBuilder methodBuilder = moduleBuilder.DefineGlobalMethod("__EntryPoint$", MethodAttributes.Static, this.entry_point.ReturnType, array2);
				ILGenerator ilgenerator = methodBuilder.GetILGenerator();
				if (array2.Length == 1)
				{
					ilgenerator.Emit(OpCodes.Ldarg_0);
				}
				ilgenerator.Emit(OpCodes.Tailcall);
				ilgenerator.Emit(OpCodes.Call, this.entry_point);
				ilgenerator.Emit(OpCodes.Ret);
				this.entry_point = methodBuilder;
			}
			if (this.version_res != null)
			{
				this.DefineVersionInfoResourceImpl(assemblyFileName);
			}
			if (this.sn != null)
			{
				this.public_key = this.sn.PublicKey;
			}
			foreach (ModuleBuilder moduleBuilder3 in this.modules)
			{
				if (moduleBuilder3 != moduleBuilder)
				{
					moduleBuilder3.Save();
				}
			}
			moduleBuilder.Save();
			if (this.sn != null && this.sn.CanSign)
			{
				this.sn.Sign(Path.Combine(this.AssemblyDir, assemblyFileName));
			}
			this.created = true;
		}

		/// <summary>Saves this dynamic assembly to disk.</summary>
		/// <param name="assemblyFileName">The file name of the assembly. </param>
		/// <exception cref="T:System.ArgumentException">The length of <paramref name="assemblyFileName" /> is 0.-or- There are two or more modules resource files in the assembly with the same name.-or- The target directory of the assembly is invalid.-or- <paramref name="assemblyFileName" /> is not a simple file name (for example, has a directory or drive component), or more than one unmanaged resource, including a version information resource, was defined in this assembly.-or- The CultureInfo string in <see cref="T:System.Reflection.AssemblyCultureAttribute" /> is not a valid string and <see cref="M:System.Reflection.Emit.AssemblyBuilder.DefineVersionInfoResource(System.String,System.String,System.String,System.String,System.String)" /> was called prior to calling this method. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="assemblyFileName" /> is null. </exception>
		/// <exception cref="T:System.InvalidOperationException">This assembly has been saved before.-or- This assembly has access Run<see cref="T:System.Reflection.Emit.AssemblyBuilderAccess" /></exception>
		/// <exception cref="T:System.IO.IOException">An output error occurs during the save. </exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <see cref="M:System.Reflection.Emit.TypeBuilder.CreateType" /> has not been called for any of the types in the modules of the assembly to be written to disk. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public void Save(string assemblyFileName)
		{
			this.Save(assemblyFileName, PortableExecutableKinds.ILOnly, ImageFileMachine.I386);
		}

		/// <summary>Sets the entry point for this dynamic assembly, assuming that a console application is being built.</summary>
		/// <param name="entryMethod">A reference to the method that represents the entry point for this dynamic assembly. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="entryMethod" /> is null. </exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="entryMethod" /> is not contained within this assembly. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		public void SetEntryPoint(MethodInfo entryMethod)
		{
			this.SetEntryPoint(entryMethod, PEFileKinds.ConsoleApplication);
		}

		/// <summary>Sets the entry point for this assembly and defines the type of the portable executable (PE file) being built.</summary>
		/// <param name="entryMethod">A reference to the method that represents the entry point for this dynamic assembly. </param>
		/// <param name="fileKind">The type of the assembly executable being built. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="entryMethod" /> is null. </exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="entryMethod" /> is not contained within this assembly. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		public void SetEntryPoint(MethodInfo entryMethod, PEFileKinds fileKind)
		{
			if (entryMethod == null)
			{
				throw new ArgumentNullException("entryMethod");
			}
			if (entryMethod.DeclaringType.Assembly != this)
			{
				throw new InvalidOperationException("Entry method is not defined in the same assembly.");
			}
			this.entry_point = entryMethod;
			this.pekind = fileKind;
		}

		/// <summary>Set a custom attribute on this assembly using a custom attribute builder.</summary>
		/// <param name="customBuilder">An instance of a helper class to define the custom attribute. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="con" /> is null. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
		{
			if (customBuilder == null)
			{
				throw new ArgumentNullException("customBuilder");
			}
			if (this.IsCompilerContext)
			{
				string fullName = customBuilder.Ctor.ReflectedType.FullName;
				if (fullName == "System.Reflection.AssemblyVersionAttribute")
				{
					this.version = this.create_assembly_version(customBuilder.string_arg());
					return;
				}
				if (fullName == "System.Reflection.AssemblyCultureAttribute")
				{
					this.culture = this.GetCultureString(customBuilder.string_arg());
				}
				else if (fullName == "System.Reflection.AssemblyAlgorithmIdAttribute")
				{
					byte[] data = customBuilder.Data;
					int num = 2;
					this.algid = (uint)data[num];
					this.algid |= (uint)((uint)data[num + 1] << 8);
					this.algid |= (uint)((uint)data[num + 2] << 16);
					this.algid |= (uint)((uint)data[num + 3] << 24);
				}
				else if (fullName == "System.Reflection.AssemblyFlagsAttribute")
				{
					byte[] data = customBuilder.Data;
					int num = 2;
					this.flags |= (uint)data[num];
					this.flags |= (uint)((uint)data[num + 1] << 8);
					this.flags |= (uint)((uint)data[num + 2] << 16);
					this.flags |= (uint)((uint)data[num + 3] << 24);
					if (this.sn == null)
					{
						this.flags &= 4294967294u;
					}
				}
			}
			if (this.cattrs != null)
			{
				CustomAttributeBuilder[] array = new CustomAttributeBuilder[this.cattrs.Length + 1];
				this.cattrs.CopyTo(array, 0);
				array[this.cattrs.Length] = customBuilder;
				this.cattrs = array;
			}
			else
			{
				this.cattrs = new CustomAttributeBuilder[1];
				this.cattrs[0] = customBuilder;
			}
		}

		/// <summary>Set a custom attribute on this assembly using a specified custom attribute blob.</summary>
		/// <param name="con">The constructor for the custom attribute. </param>
		/// <param name="binaryAttribute">A byte blob representing the attributes. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="con" /> or <paramref name="binaryAttribute" /> is null. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="con" /> is not a RuntimeConstructorInfo.</exception>
		[ComVisible(true)]
		public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
		{
			if (con == null)
			{
				throw new ArgumentNullException("con");
			}
			if (binaryAttribute == null)
			{
				throw new ArgumentNullException("binaryAttribute");
			}
			this.SetCustomAttribute(new CustomAttributeBuilder(con, binaryAttribute));
		}

		internal void SetCorlibTypeBuilders(Type corlib_object_type, Type corlib_value_type, Type corlib_enum_type)
		{
			this.corlib_object_type = corlib_object_type;
			this.corlib_value_type = corlib_value_type;
			this.corlib_enum_type = corlib_enum_type;
		}

		internal void SetCorlibTypeBuilders(Type corlib_object_type, Type corlib_value_type, Type corlib_enum_type, Type corlib_void_type)
		{
			this.SetCorlibTypeBuilders(corlib_object_type, corlib_value_type, corlib_enum_type);
			this.corlib_void_type = corlib_void_type;
		}

		private Exception not_supported()
		{
			return new NotSupportedException("The invoked member is not supported in a dynamic module.");
		}

		private void check_name_and_filename(string name, string fileName, bool fileNeedsToExists)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException("Empty name is not legal.", "name");
			}
			if (fileName.Length == 0)
			{
				throw new ArgumentException("Empty file name is not legal.", "fileName");
			}
			if (Path.GetFileName(fileName) != fileName)
			{
				throw new ArgumentException("fileName '" + fileName + "' must not include a path.", "fileName");
			}
			string text = fileName;
			if (this.dir != null)
			{
				text = Path.Combine(this.dir, fileName);
			}
			if (fileNeedsToExists && !File.Exists(text))
			{
				throw new FileNotFoundException("Could not find file '" + fileName + "'");
			}
			if (this.resources != null)
			{
				for (int i = 0; i < this.resources.Length; i++)
				{
					if (this.resources[i].filename == text)
					{
						throw new ArgumentException("Duplicate file name '" + fileName + "'");
					}
					if (this.resources[i].name == name)
					{
						throw new ArgumentException("Duplicate name '" + name + "'");
					}
				}
			}
			if (this.modules != null)
			{
				for (int j = 0; j < this.modules.Length; j++)
				{
					if (!this.modules[j].IsTransient() && this.modules[j].FileName == fileName)
					{
						throw new ArgumentException("Duplicate file name '" + fileName + "'");
					}
					if (this.modules[j].Name == name)
					{
						throw new ArgumentException("Duplicate name '" + name + "'");
					}
				}
			}
		}

		private string create_assembly_version(string version)
		{
			string[] array = version.Split(new char[]
			{
				'.'
			});
			int[] array2 = new int[4];
			if (array.Length < 0 || array.Length > 4)
			{
				throw new ArgumentException("The version specified '" + version + "' is invalid");
			}
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == "*")
				{
					DateTime now = DateTime.Now;
					if (i == 2)
					{
						array2[2] = (now - new DateTime(2000, 1, 1)).Days;
						if (array.Length == 3)
						{
							array2[3] = (now.Second + now.Minute * 60 + now.Hour * 3600) / 2;
						}
					}
					else
					{
						if (i != 3)
						{
							throw new ArgumentException("The version specified '" + version + "' is invalid");
						}
						array2[3] = (now.Second + now.Minute * 60 + now.Hour * 3600) / 2;
					}
				}
				else
				{
					try
					{
						array2[i] = int.Parse(array[i]);
					}
					catch (FormatException)
					{
						throw new ArgumentException("The version specified '" + version + "' is invalid");
					}
				}
			}
			return string.Concat(new object[]
			{
				array2[0],
				".",
				array2[1],
				".",
				array2[2],
				".",
				array2[3]
			});
		}

		private string GetCultureString(string str)
		{
			return (!(str == "neutral")) ? str : string.Empty;
		}

		internal override AssemblyName UnprotectedGetName()
		{
			AssemblyName assemblyName = base.UnprotectedGetName();
			if (this.sn != null)
			{
				assemblyName.SetPublicKey(this.sn.PublicKey);
				assemblyName.SetPublicKeyToken(this.sn.PublicKeyToken);
			}
			return assemblyName;
		}
	}
}
