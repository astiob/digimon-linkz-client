using System;
using System.Configuration.Assemblies;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Activation;
using System.Security.Policy;
using System.Text;

namespace System
{
	/// <summary>Contains methods to create types of objects locally or remotely, or obtain references to existing remote objects. This class cannot be inherited. </summary>
	/// <filterpriority>2</filterpriority>
	[ComDefaultInterface(typeof(_Activator))]
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	public sealed class Activator : _Activator
	{
		private const BindingFlags _flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance;

		private const BindingFlags _accessFlags = BindingFlags.IgnoreCase | BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;

		private Activator()
		{
		}

		/// <summary>Maps a set of names to a corresponding set of dispatch identifiers.</summary>
		/// <param name="riid">Reserved for future use. Must be IID_NULL.</param>
		/// <param name="rgszNames">The passed-in array of names to map.</param>
		/// <param name="cNames">The count of the names to map.</param>
		/// <param name="lcid">The locale context in which to interpret the names.</param>
		/// <param name="rgDispId">The caller-allocated array that receives the IDs corresponding to the names.</param>
		/// <exception cref="T:System.NotImplementedException">Late-bound access using the COM IDispatch interface is not supported.</exception>
		void _Activator.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		/// <summary>Retrieves the type information for an object, which can then be used to get the type information for an interface.</summary>
		/// <param name="iTInfo">The type information to return.</param>
		/// <param name="lcid">The locale identifier for the type information.</param>
		/// <param name="ppTInfo">An <see cref="T:System.IntPtr" /> object that receives a pointer to the requested type information object.</param>
		/// <exception cref="T:System.NotImplementedException">Late-bound access using the COM IDispatch interface is not supported.</exception>
		void _Activator.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		/// <summary>Retrieves the number of type information interfaces that an object provides (either 0 or 1).</summary>
		/// <param name="pcTInfo">When this method returns, contains a pointer to a location that receives the number of type information interfaces provided by the object. This parameter is passed uninitialized.</param>
		/// <exception cref="T:System.NotImplementedException">Late-bound access using the COM IDispatch interface is not supported.</exception>
		void _Activator.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		/// <summary>Provides access to properties and methods exposed by an object.</summary>
		/// <param name="dispIdMember">A dispatch identifier that identifies the member.</param>
		/// <param name="riid">Reserved for future use. Must be IID_NULL.</param>
		/// <param name="lcid">The locale context in which to interpret arguments.</param>
		/// <param name="wFlags">Flags describing the context of the call.</param>
		/// <param name="pDispParams">A pointer to a structure that contains an array of arguments, an array of argument DISPIDs for named arguments, and counts for the number of elements in the arrays.</param>
		/// <param name="pVarResult">A pointer to the location where the result is to be stored.</param>
		/// <param name="pExcepInfo">A pointer to a structure that contains exception information.</param>
		/// <param name="puArgErr">The index of the first argument that has an error.</param>
		/// <exception cref="T:System.NotImplementedException">Late-bound access using the COM IDispatch interface is not supported.</exception>
		void _Activator.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		/// <summary>Creates an instance of the COM object whose name is specified, using the named assembly file and the constructor that best matches the specified parameters.</summary>
		/// <returns>A handle that must be unwrapped to access the newly created instance.</returns>
		/// <param name="assemblyName">The name of a file that contains an assembly where the type named <paramref name="typeName" /> is sought. </param>
		/// <param name="typeName">The name of the preferred type. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="typeName" /> or <paramref name="assemblyName" /> is null. </exception>
		/// <exception cref="T:System.TypeLoadException">An instance cannot be created through COM. -or-<paramref name="typename" /> was not found in <paramref name="assemblyName" />.</exception>
		/// <exception cref="T:System.MissingMethodException">No matching constructor was found. </exception>
		/// <exception cref="T:System.IO.FileNotFoundException">
		///   <paramref name="assemblyName" /> is not found, or the module you are trying to load does not specify a file name extension. </exception>
		/// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract class.-or-This member was invoked with a late-binding mechanism. </exception>
		/// <exception cref="T:System.NotSupportedException">The caller cannot provide activation attributes for an object that does not inherit from <see cref="T:System.MarshalByRefObject" />. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="assemblyName" /> is the empty string (""). </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		/// </PermissionSet>
		[MonoTODO("No COM support")]
		public static ObjectHandle CreateComInstanceFrom(string assemblyName, string typeName)
		{
			if (assemblyName == null)
			{
				throw new ArgumentNullException("assemblyName");
			}
			if (typeName == null)
			{
				throw new ArgumentNullException("typeName");
			}
			if (assemblyName.Length == 0)
			{
				throw new ArgumentException("assemblyName");
			}
			throw new NotImplementedException();
		}

		/// <summary>Creates an instance of the COM object whose name is specified, using the named assembly file and the default constructor.</summary>
		/// <returns>A handle that must be unwrapped to access the newly created instance.</returns>
		/// <param name="assemblyName">The name of a file that contains an assembly where the type named <paramref name="typeName" /> is sought. </param>
		/// <param name="typeName">The name of the preferred type. </param>
		/// <param name="hashValue">The value of the computed hash code. </param>
		/// <param name="hashAlgorithm">The hash algorithm used for hashing files and generating the strong name. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="typeName" /> or <paramref name="assemblyName" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="assemblyName" /> is the empty string (""). </exception>
		/// <exception cref="T:System.IO.PathTooLongException">An assembly or module was loaded twice with two different evidences, or the assembly name is longer than MAX_PATH characters. </exception>
		/// <exception cref="T:System.IO.FileNotFoundException">
		///   <paramref name="assemblyName" /> is not found, or the module you are trying to load does not specify a file name extension. </exception>
		/// <exception cref="T:System.IO.FileLoadException">
		///   <paramref name="assemblyName" /> is found but cannot be loaded. </exception>
		/// <exception cref="T:System.BadImageFormatException">
		///   <paramref name="assemblyName" /> is not a valid assembly. </exception>
		/// <exception cref="T:System.Security.SecurityException">A code base that does not start with "file://" was specified without the required WebPermission. </exception>
		/// <exception cref="T:System.TypeLoadException">An instance cannot be created through COM.-or- <paramref name="typename" /> was not found in <paramref name="assemblyName" />. </exception>
		/// <exception cref="T:System.MissingMethodException">No matching constructor was found. </exception>
		/// <exception cref="T:System.MemberAccessException">An instance of an abstract class cannot be created. -or-This member was invoked with a late-binding mechanism. </exception>
		/// <exception cref="T:System.NotSupportedException">The caller cannot provide activation attributes for an object that does not inherit from <see cref="T:System.MarshalByRefObject" />. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		/// </PermissionSet>
		[MonoTODO("Mono does not support COM")]
		public static ObjectHandle CreateComInstanceFrom(string assemblyName, string typeName, byte[] hashValue, AssemblyHashAlgorithm hashAlgorithm)
		{
			if (assemblyName == null)
			{
				throw new ArgumentNullException("assemblyName");
			}
			if (typeName == null)
			{
				throw new ArgumentNullException("typeName");
			}
			if (assemblyName.Length == 0)
			{
				throw new ArgumentException("assemblyName");
			}
			throw new NotImplementedException();
		}

		/// <summary>Creates an instance of the type whose name is specified, using the named assembly file and default constructor.</summary>
		/// <returns>A handle that must be unwrapped to access the newly created instance.</returns>
		/// <param name="assemblyFile">The name of a file that contains an assembly where the type named <paramref name="typeName" /> is sought. </param>
		/// <param name="typeName">The name of the preferred type. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="typeName" /> is null. </exception>
		/// <exception cref="T:System.MissingMethodException">No matching public constructor was found. </exception>
		/// <exception cref="T:System.TypeLoadException">
		///   <paramref name="typename" /> was not found in <paramref name="assemblyFile" />. </exception>
		/// <exception cref="T:System.IO.FileNotFoundException">
		///   <paramref name="assemblyFile" /> was not found. </exception>
		/// <exception cref="T:System.MethodAccessException">The caller does not have permission to call this constructor. </exception>
		/// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism. </exception>
		/// <exception cref="T:System.Reflection.TargetInvocationException">The constructor, which was invoked through reflection, threw an exception. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does have the required <see cref="T:System.Security.Permissions.FileIOPermission" />. </exception>
		/// <exception cref="T:System.BadImageFormatException">
		///   <paramref name="assemblyFile" /> is not a valid assembly. -or-The common language runtime (CLR) version 2.0 or later is currently loaded, and <paramref name="assemblyName" /> was compiled for a version of the CLR that is later than the currently loaded version. Note that the .NET Framework versions 2.0, 3.0, and 3.5 all use CLR version 2.0.</exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		/// </PermissionSet>
		public static ObjectHandle CreateInstanceFrom(string assemblyFile, string typeName)
		{
			return Activator.CreateInstanceFrom(assemblyFile, typeName, null);
		}

		/// <summary>Creates an instance of the type whose name is specified, using the named assembly file and default constructor.</summary>
		/// <returns>A handle that must be unwrapped to access the newly created instance.</returns>
		/// <param name="assemblyFile">The name of a file that contains an assembly where the type named <paramref name="typeName" /> is sought. </param>
		/// <param name="typeName">The name of the preferred type. </param>
		/// <param name="activationAttributes">An array of one or more attributes that can participate in activation. This is typically an array that contains a single <see cref="T:System.Runtime.Remoting.Activation.UrlAttribute" /> object. The <see cref="T:System.Runtime.Remoting.Activation.UrlAttribute" /> specifies the URL that is required to activate a remote object. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="typeName" /> is null. </exception>
		/// <exception cref="T:System.MissingMethodException">No matching public constructor was found. </exception>
		/// <exception cref="T:System.TypeLoadException">
		///   <paramref name="typename" /> was not found in <paramref name="assemblyFile" />. </exception>
		/// <exception cref="T:System.IO.FileNotFoundException">
		///   <paramref name="assemblyFile" /> was not found. </exception>
		/// <exception cref="T:System.MethodAccessException">The caller does not have permission to call this constructor. </exception>
		/// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism. </exception>
		/// <exception cref="T:System.Reflection.TargetInvocationException">The constructor, which was invoked through reflection, threw an exception. </exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="activationAttributes" /> is not an empty array, and the type being created does not derive from <see cref="T:System.MarshalByRefObject" />. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does have the required <see cref="T:System.Security.Permissions.FileIOPermission" />. </exception>
		/// <exception cref="T:System.BadImageFormatException">
		///   <paramref name="assemblyFile" /> is not a valid assembly. -or-The common language runtime (CLR) version 2.0 or later is currently loaded, and <paramref name="assemblyName" /> was compiled for a version of the CLR that is later than the currently loaded version. Note that the .NET Framework versions 2.0, 3.0, and 3.5 all use CLR version 2.0.</exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		/// </PermissionSet>
		public static ObjectHandle CreateInstanceFrom(string assemblyFile, string typeName, object[] activationAttributes)
		{
			return Activator.CreateInstanceFrom(assemblyFile, typeName, false, BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, null, null, null, activationAttributes, null);
		}

		/// <summary>Creates an instance of the type whose name is specified, using the named assembly file and the constructor that best matches the specified parameters.</summary>
		/// <returns>A handle that must be unwrapped to access the newly created instance.</returns>
		/// <param name="assemblyFile">The name of a file that contains an assembly where the type named <paramref name="typeName" /> is sought. </param>
		/// <param name="typeName">The name of the preferred type. </param>
		/// <param name="ignoreCase">true to specify that the search for <paramref name="typeName" /> is not case-sensitive; false to specify that the search is case-sensitive. </param>
		/// <param name="bindingAttr">A combination of zero or more bit flags that affect the search for the <paramref name="typeName" /> constructor. If <paramref name="bindingAttr" /> is zero, a case-sensitive search for public constructors is conducted. </param>
		/// <param name="binder">An object that uses <paramref name="bindingAttr" /> and <paramref name="args" /> to seek and identify the <paramref name="typeName" /> constructor. If <paramref name="binder" /> is null, the default binder is used. </param>
		/// <param name="args">An array of arguments that match in number, order, and type the parameters of the constructor to invoke. If <paramref name="args" /> is an empty array or null, the constructor that takes no parameters (the default constructor) is invoked. </param>
		/// <param name="culture">Culture-specific information that governs the coercion of <paramref name="args" /> to the formal types declared for the <paramref name="typeName" /> constructor. If <paramref name="culture" /> is null, the <see cref="T:System.Globalization.CultureInfo" /> for the current thread is used. </param>
		/// <param name="activationAttributes">An array of one or more attributes that can participate in activation. This is typically an array that contains a single <see cref="T:System.Runtime.Remoting.Activation.UrlAttribute" /> object. The <see cref="T:System.Runtime.Remoting.Activation.UrlAttribute" /> specifies the URL that is required to activate a remote object. </param>
		/// <param name="securityInfo">Information used to make security policy decisions and grant code permissions. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="typeName" /> is null. </exception>
		/// <exception cref="T:System.MissingMethodException">No matching constructor was found. </exception>
		/// <exception cref="T:System.TypeLoadException">
		///   <paramref name="typename" /> was not found in <paramref name="assemblyFile" />. </exception>
		/// <exception cref="T:System.IO.FileNotFoundException">
		///   <paramref name="assemblyFile" /> was not found. </exception>
		/// <exception cref="T:System.MethodAccessException">The caller does not have permission to call this constructor. </exception>
		/// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism. </exception>
		/// <exception cref="T:System.Reflection.TargetInvocationException">The constructor, which was invoked through reflection, threw an exception. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does have the required <see cref="T:System.Security.Permissions.FileIOPermission" />. </exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="activationAttributes" /> is not an empty array, and the type being created does not derive from <see cref="T:System.MarshalByRefObject" />. </exception>
		/// <exception cref="T:System.BadImageFormatException">
		///   <paramref name="assemblyFile" /> is not a valid assembly. -or-The common language runtime (CLR) version 2.0 or later is currently loaded, and <paramref name="assemblyName" /> was compiled for a version of the CLR that is later than the currently loaded version. Note that the .NET Framework versions 2.0, 3.0, and 3.5 all use CLR version 2.0.</exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		/// </PermissionSet>
		public static ObjectHandle CreateInstanceFrom(string assemblyFile, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes, Evidence securityInfo)
		{
			Assembly assembly = Assembly.LoadFrom(assemblyFile, securityInfo);
			if (assembly == null)
			{
				return null;
			}
			Type type = assembly.GetType(typeName, true, ignoreCase);
			if (type == null)
			{
				return null;
			}
			object obj = Activator.CreateInstance(type, bindingAttr, binder, args, culture, activationAttributes);
			return (obj == null) ? null : new ObjectHandle(obj);
		}

		/// <summary>Creates an instance of the type whose name is specified, using the named assembly and default constructor.</summary>
		/// <returns>A handle that must be unwrapped to access the newly created instance.</returns>
		/// <param name="assemblyName">The name of the assembly where the type named <paramref name="typeName" /> is sought. If <paramref name="assemblyName" /> is null, the executing assembly is searched. </param>
		/// <param name="typeName">The name of the preferred type. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="typeName" /> is null. </exception>
		/// <exception cref="T:System.MissingMethodException">No matching public constructor was found. </exception>
		/// <exception cref="T:System.TypeLoadException">
		///   <paramref name="typename" /> was not found in <paramref name="assemblyName" />. </exception>
		/// <exception cref="T:System.IO.FileNotFoundException">
		///   <paramref name="assemblyName" /> was not found. </exception>
		/// <exception cref="T:System.MethodAccessException">The caller does not have permission to call this constructor. </exception>
		/// <exception cref="T:System.MemberAccessException">You cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism. </exception>
		/// <exception cref="T:System.Reflection.TargetInvocationException">The constructor, which was invoked through reflection, threw an exception. </exception>
		/// <exception cref="T:System.Runtime.InteropServices.InvalidComObjectException">The COM type was not obtained through <see cref="Overload:System.Type.GetTypeFromProgID" /> or <see cref="Overload:System.Type.GetTypeFromCLSID" />. </exception>
		/// <exception cref="T:System.NotSupportedException">Creation of <see cref="T:System.TypedReference" />, <see cref="T:System.ArgIterator" />, <see cref="T:System.Void" />, and <see cref="T:System.RuntimeArgumentHandle" /> types, or arrays of those types, is not supported. </exception>
		/// <exception cref="T:System.BadImageFormatException">
		///   <paramref name="assemblyName" /> is not a valid assembly. -or-The common language runtime (CLR) version 2.0 or later is currently loaded, and <paramref name="assemblyName" /> was compiled for a version of the CLR that is later than the currently loaded version. Note that the .NET Framework versions 2.0, 3.0, and 3.5 all use CLR version 2.0.</exception>
		/// <exception cref="T:System.IO.FileLoadException">An assembly or module was loaded twice with two different evidences. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		/// </PermissionSet>
		public static ObjectHandle CreateInstance(string assemblyName, string typeName)
		{
			if (assemblyName == null)
			{
				assemblyName = Assembly.GetCallingAssembly().GetName().Name;
			}
			return Activator.CreateInstance(assemblyName, typeName, null);
		}

		/// <summary>Creates an instance of the type whose name is specified, using the named assembly and default constructor.</summary>
		/// <returns>A handle that must be unwrapped to access the newly created instance.</returns>
		/// <param name="assemblyName">The name of the assembly where the type named <paramref name="typeName" /> is sought. If <paramref name="assemblyName" /> is null, the executing assembly is searched. </param>
		/// <param name="typeName">The name of the preferred type. </param>
		/// <param name="activationAttributes">An array of one or more attributes that can participate in activation. This is typically an array that contains a single <see cref="T:System.Runtime.Remoting.Activation.UrlAttribute" /> object. The <see cref="T:System.Runtime.Remoting.Activation.UrlAttribute" /> specifies the URL that is required to activate a remote object. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="typeName" /> is null. </exception>
		/// <exception cref="T:System.MissingMethodException">No matching public constructor was found. </exception>
		/// <exception cref="T:System.TypeLoadException">
		///   <paramref name="typename" /> was not found in <paramref name="assemblyName" />. </exception>
		/// <exception cref="T:System.IO.FileNotFoundException">
		///   <paramref name="assemblyName" /> was not found. </exception>
		/// <exception cref="T:System.MethodAccessException">The caller does not have permission to call this constructor. </exception>
		/// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism. </exception>
		/// <exception cref="T:System.Runtime.InteropServices.InvalidComObjectException">The COM type was not obtained through <see cref="Overload:System.Type.GetTypeFromProgID" /> or <see cref="Overload:System.Type.GetTypeFromCLSID" />. </exception>
		/// <exception cref="T:System.NotSupportedException">Creation of <see cref="T:System.TypedReference" />, <see cref="T:System.ArgIterator" />, <see cref="T:System.Void" />, and <see cref="T:System.RuntimeArgumentHandle" /> types, or arrays of those types, is not supported.-or- <paramref name="activationAttributes" /> is not an empty array, and the type being created does not derive from <see cref="T:System.MarshalByRefObject" />.-or-<paramref name="activationAttributes" /> is not a <see cref="T:System.Runtime.Remoting.Activation.UrlAttribute" />array. </exception>
		/// <exception cref="T:System.BadImageFormatException">
		///   <paramref name="assemblyName" /> is not a valid assembly. -or-The common language runtime (CLR) version 2.0 or later is currently loaded, and <paramref name="assemblyName" /> was compiled for a version of the CLR that is later than the currently loaded version. Note that the .NET Framework versions 2.0, 3.0, and 3.5 all use CLR version 2.0.</exception>
		/// <exception cref="T:System.IO.FileLoadException">An assembly or module was loaded twice with two different evidences. </exception>
		/// <exception cref="T:System.Reflection.TargetInvocationException">An error occurred when attempting remote activation in a target specified in <paramref name="activationAttributes" />.</exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*" />
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence, RemotingConfiguration" />
		/// </PermissionSet>
		public static ObjectHandle CreateInstance(string assemblyName, string typeName, object[] activationAttributes)
		{
			if (assemblyName == null)
			{
				assemblyName = Assembly.GetCallingAssembly().GetName().Name;
			}
			return Activator.CreateInstance(assemblyName, typeName, false, BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, null, null, null, activationAttributes, null);
		}

		/// <summary>Creates an instance of the type whose name is specified, using the named assembly and the constructor that best matches the specified parameters.</summary>
		/// <returns>A handle that must be unwrapped to access the newly created instance.</returns>
		/// <param name="assemblyName">The name of the assembly where the type named <paramref name="typeName" /> is sought. If <paramref name="assemblyName" /> is null, the executing assembly is searched. </param>
		/// <param name="typeName">The name of the preferred type. </param>
		/// <param name="ignoreCase">true to specify that the search for <paramref name="typeName" /> is not case-sensitive; false to specify that the search is case-sensitive. </param>
		/// <param name="bindingAttr">A combination of zero or more bit flags that affect the search for the <paramref name="typeName" /> constructor. If <paramref name="bindingAttr" /> is zero, a case-sensitive search for public constructors is conducted. </param>
		/// <param name="binder">An object that uses <paramref name="bindingAttr" /> and <paramref name="args" /> to seek and identify the <paramref name="typeName" /> constructor. If <paramref name="binder" /> is null, the default binder is used. </param>
		/// <param name="args">An array of arguments that match in number, order, and type the parameters of the constructor to invoke. If <paramref name="args" /> is an empty array or null, the constructor that takes no parameters (the default constructor) is invoked. </param>
		/// <param name="culture">Culture-specific information that governs the coercion of <paramref name="args" /> to the formal types declared for the <paramref name="typeName" /> constructor. If <paramref name="culture" /> is null, the <see cref="T:System.Globalization.CultureInfo" /> for the current thread is used. </param>
		/// <param name="activationAttributes">An array of one or more attributes that can participate in activation. This is typically an array that contains a single <see cref="T:System.Runtime.Remoting.Activation.UrlAttribute" /> object. The <see cref="T:System.Runtime.Remoting.Activation.UrlAttribute" /> specifies the URL that is required to activate a remote object. </param>
		/// <param name="securityInfo">Information used to make security policy decisions and grant code permissions. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="typeName" /> is null. </exception>
		/// <exception cref="T:System.MissingMethodException">No matching constructor was found. </exception>
		/// <exception cref="T:System.TypeLoadException">
		///   <paramref name="typename" /> was not found in <paramref name="assemblyName" />. </exception>
		/// <exception cref="T:System.IO.FileNotFoundException">
		///   <paramref name="assemblyName" /> was not found. </exception>
		/// <exception cref="T:System.MethodAccessException">The caller does not have permission to call this constructor. </exception>
		/// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism. </exception>
		/// <exception cref="T:System.Reflection.TargetInvocationException">The constructor, which was invoked through reflection, threw an exception. </exception>
		/// <exception cref="T:System.Runtime.InteropServices.InvalidComObjectException">The COM type was not obtained through <see cref="Overload:System.Type.GetTypeFromProgID" /> or <see cref="Overload:System.Type.GetTypeFromCLSID" />. </exception>
		/// <exception cref="T:System.NotSupportedException">Creation of <see cref="T:System.TypedReference" />, <see cref="T:System.ArgIterator" />, <see cref="T:System.Void" />, and <see cref="T:System.RuntimeArgumentHandle" /> types, or arrays of those types, is not supported.-or- <paramref name="activationAttributes" /> is not an empty array, and the type being created does not derive from <see cref="T:System.MarshalByRefObject" />. -or-The constructor that best matches <paramref name="args" /> has varargs arguments.</exception>
		/// <exception cref="T:System.BadImageFormatException">
		///   <paramref name="assemblyName" /> is not a valid assembly. -or-The common language runtime (CLR) version 2.0 or later is currently loaded, and <paramref name="assemblyName" /> was compiled for a version of the CLR that is later than the currently loaded version. Note that the .NET Framework versions 2.0, 3.0, and 3.5 all use CLR version 2.0.</exception>
		/// <exception cref="T:System.IO.FileLoadException">An assembly or module was loaded twice with two different evidences. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*" />
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence, RemotingConfiguration" />
		/// </PermissionSet>
		public static ObjectHandle CreateInstance(string assemblyName, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes, Evidence securityInfo)
		{
			Assembly assembly;
			if (assemblyName == null)
			{
				assembly = Assembly.GetCallingAssembly();
			}
			else
			{
				assembly = Assembly.Load(assemblyName, securityInfo);
			}
			Type type = assembly.GetType(typeName, true, ignoreCase);
			object obj = Activator.CreateInstance(type, bindingAttr, binder, args, culture, activationAttributes);
			return (obj == null) ? null : new ObjectHandle(obj);
		}

		/// <summary>Creates an instance of the type designated by the specified <see cref="T:System.ActivationContext" /> object.</summary>
		/// <returns>A handle that must be unwrapped to access the newly created object.</returns>
		/// <param name="activationContext">An activation context object that specifies the object to create.</param>
		[MonoNotSupported("no ClickOnce in mono")]
		public static ObjectHandle CreateInstance(ActivationContext activationContext)
		{
			throw new NotImplementedException();
		}

		/// <summary>Creates an instance of the type that is designated by the specified <see cref="T:System.ActivationContext" /> object and activated with the specified custom activation data.</summary>
		/// <returns>A handle that must be unwrapped to access the newly created object.</returns>
		/// <param name="activationContext">An activation context object that specifies the object to create.</param>
		/// <param name="activationCustomData">An array of Unicode strings that contain custom activation data.</param>
		[MonoNotSupported("no ClickOnce in mono")]
		public static ObjectHandle CreateInstance(ActivationContext activationContext, string[] activationCustomData)
		{
			throw new NotImplementedException();
		}

		/// <summary>Creates an instance of the type whose name is specified in the specified remote domain, using the named assembly file and default constructor.</summary>
		/// <returns>A handle that must be unwrapped to access the newly created instance.</returns>
		/// <param name="domain">The remote domain where the type named <paramref name="typeName" /> is created.</param>
		/// <param name="assemblyFile">The name of a file that contains an assembly where the type named <paramref name="typeName" /> is sought. </param>
		/// <param name="typeName">The name of the preferred type. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="domain" /> or <paramref name="typeName" /> is null. </exception>
		/// <exception cref="T:System.MissingMethodException">No matching public constructor was found. </exception>
		/// <exception cref="T:System.TypeLoadException">
		///   <paramref name="typename" /> was not found in <paramref name="assemblyFile" />. </exception>
		/// <exception cref="T:System.IO.FileNotFoundException">
		///   <paramref name="assemblyFile" /> was not found. </exception>
		/// <exception cref="T:System.MethodAccessException">The caller does not have permission to call this constructor. </exception>
		/// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism. </exception>
		/// <exception cref="T:System.Reflection.TargetInvocationException">The constructor, which was invoked through reflection, threw an exception. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does have the required <see cref="T:System.Security.Permissions.FileIOPermission" />. </exception>
		/// <exception cref="T:System.BadImageFormatException">
		///   <paramref name="assemblyFile" /> is not a valid assembly. -or-The common language runtime (CLR) version 2.0 or later is currently loaded, and <paramref name="assemblyName" /> was compiled for a version of the CLR that is later than the currently loaded version. Note that the .NET Framework versions 2.0, 3.0, and 3.5 all use CLR version 2.0.</exception>
		public static ObjectHandle CreateInstanceFrom(AppDomain domain, string assemblyFile, string typeName)
		{
			if (domain == null)
			{
				throw new ArgumentNullException("domain");
			}
			return domain.CreateInstanceFrom(assemblyFile, typeName);
		}

		/// <summary>Creates an instance of the type whose name is specified in the specified remote domain, using the named assembly file and the constructor that best matches the specified parameters.</summary>
		/// <returns>A handle that must be unwrapped to access the newly created instance.</returns>
		/// <param name="domain">The remote domain where the type named <paramref name="typeName" /> is created.</param>
		/// <param name="assemblyFile">The name of a file that contains an assembly where the type named <paramref name="typeName" /> is sought. </param>
		/// <param name="typeName">The name of the preferred type. </param>
		/// <param name="ignoreCase">true to specify that the search for <paramref name="typeName" /> is not case-sensitive; false to specify that the search is case-sensitive. </param>
		/// <param name="bindingAttr">A combination of zero or more bit flags that affect the search for the <paramref name="typeName" /> constructor. If <paramref name="bindingAttr" /> is zero, a case-sensitive search for public constructors is conducted. </param>
		/// <param name="binder">An object that uses <paramref name="bindingAttr" /> and <paramref name="args" /> to seek and identify the <paramref name="typeName" /> constructor. If <paramref name="binder" /> is null, the default binder is used. </param>
		/// <param name="args">An array of arguments that match in number, order, and type the parameters of the constructor to invoke. If <paramref name="args" /> is an empty array or null, the constructor that takes no parameters (the default constructor) is invoked. </param>
		/// <param name="culture">Culture-specific information that governs the coercion of <paramref name="args" /> to the formal types declared for the <paramref name="typeName" /> constructor. If <paramref name="culture" /> is null, the <see cref="T:System.Globalization.CultureInfo" /> for the current thread is used. </param>
		/// <param name="activationAttributes">An array of one or more attributes that can participate in activation. This is typically an array that contains a single <see cref="T:System.Runtime.Remoting.Activation.UrlAttribute" /> object. The <see cref="T:System.Runtime.Remoting.Activation.UrlAttribute" /> specifies the URL that is required to activate a remote object. </param>
		/// <param name="securityAttributes">Information used to make security policy decisions and grant code permissions. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="domain" /> or <paramref name="typeName" /> is null. </exception>
		/// <exception cref="T:System.MissingMethodException">No matching constructor was found. </exception>
		/// <exception cref="T:System.TypeLoadException">
		///   <paramref name="typename" /> was not found in <paramref name="assemblyFile" />. </exception>
		/// <exception cref="T:System.IO.FileNotFoundException">
		///   <paramref name="assemblyFile" /> was not found. </exception>
		/// <exception cref="T:System.MethodAccessException">The caller does not have permission to call this constructor. </exception>
		/// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism. </exception>
		/// <exception cref="T:System.Reflection.TargetInvocationException">The constructor, which was invoked through reflection, threw an exception. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does have the required <see cref="T:System.Security.Permissions.FileIOPermission" />. </exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="activationAttributes" /> is not an empty array, and the type being created does not derive from <see cref="T:System.MarshalByRefObject" />. </exception>
		/// <exception cref="T:System.BadImageFormatException">
		///   <paramref name="assemblyFile" /> is not a valid assembly. -or-The common language runtime (CLR) version 2.0 or later is currently loaded, and <paramref name="assemblyName" /> was compiled for a version of the CLR that is later than the currently loaded version. Note that the .NET Framework versions 2.0, 3.0, and 3.5 all use CLR version 2.0.</exception>
		public static ObjectHandle CreateInstanceFrom(AppDomain domain, string assemblyFile, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes, Evidence securityAttributes)
		{
			if (domain == null)
			{
				throw new ArgumentNullException("domain");
			}
			return domain.CreateInstanceFrom(assemblyFile, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes, securityAttributes);
		}

		/// <summary>Creates an instance of the type whose name is specified in the specified remote domain, using the named assembly and default constructor.</summary>
		/// <returns>A handle that must be unwrapped to access the newly created instance.</returns>
		/// <param name="domain">The remote domain where the type named <paramref name="typeName" /> is created.</param>
		/// <param name="assemblyName">The name of the assembly where the type named <paramref name="typeName" /> is sought. If <paramref name="assemblyName" /> is null, the executing assembly is searched. </param>
		/// <param name="typeName">The name of the preferred type. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="typeName" /> or <paramref name="domain" /> is null. </exception>
		/// <exception cref="T:System.MissingMethodException">No matching public constructor was found. </exception>
		/// <exception cref="T:System.TypeLoadException">
		///   <paramref name="typename" /> was not found in <paramref name="assemblyName" />. </exception>
		/// <exception cref="T:System.IO.FileNotFoundException">
		///   <paramref name="assemblyName" /> was not found. </exception>
		/// <exception cref="T:System.MethodAccessException">The caller does not have permission to call this constructor. </exception>
		/// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract type. -or-This member was invoked with a late-binding mechanism. </exception>
		/// <exception cref="T:System.Reflection.TargetInvocationException">The constructor, which was invoked through reflection, threw an exception. </exception>
		/// <exception cref="T:System.Runtime.InteropServices.InvalidComObjectException">The COM type was not obtained through <see cref="Overload:System.Type.GetTypeFromProgID" /> or <see cref="Overload:System.Type.GetTypeFromCLSID" />. </exception>
		/// <exception cref="T:System.NotSupportedException">Creation of <see cref="T:System.TypedReference" />, <see cref="T:System.ArgIterator" />, <see cref="T:System.Void" />, and <see cref="T:System.RuntimeArgumentHandle" /> types, or arrays of those types, is not supported. </exception>
		/// <exception cref="T:System.BadImageFormatException">
		///   <paramref name="assemblyName" /> is not a valid assembly. -or-The common language runtime (CLR) version 2.0 or later is currently loaded, and <paramref name="assemblyName" /> was compiled for a version of the CLR that is later than the currently loaded version. Note that the .NET Framework versions 2.0, 3.0, and 3.5 all use CLR version 2.0.</exception>
		/// <exception cref="T:System.IO.FileLoadException">An assembly or module was loaded twice with two different evidences. </exception>
		public static ObjectHandle CreateInstance(AppDomain domain, string assemblyName, string typeName)
		{
			if (domain == null)
			{
				throw new ArgumentNullException("domain");
			}
			return domain.CreateInstance(assemblyName, typeName);
		}

		/// <summary>Creates an instance of the type whose name is specified in the specified remote domain, using the named assembly and the constructor that best matches the specified parameters.</summary>
		/// <returns>A handle that must be unwrapped to access the newly created instance.</returns>
		/// <param name="domain">The domain where the type named <paramref name="typeName" /> is created.</param>
		/// <param name="assemblyName">The name of the assembly where the type named <paramref name="typeName" /> is sought. If <paramref name="assemblyName" /> is null, the executing assembly is searched. </param>
		/// <param name="typeName">The name of the preferred type. </param>
		/// <param name="ignoreCase">true to specify that the search for <paramref name="typeName" /> is not case-sensitive; false to specify that the search is case-sensitive. </param>
		/// <param name="bindingAttr">A combination of zero or more bit flags that affect the search for the <paramref name="typeName" /> constructor. If <paramref name="bindingAttr" /> is zero, a case-sensitive search for public constructors is conducted. </param>
		/// <param name="binder">An object that uses <paramref name="bindingAttr" /> and <paramref name="args" /> to seek and identify the <paramref name="typeName" /> constructor. If <paramref name="binder" /> is null, the default binder is used. </param>
		/// <param name="args">An array of arguments that match in number, order, and type the parameters of the constructor to invoke. If <paramref name="args" /> is an empty array or null, the constructor that takes no parameters (the default constructor) is invoked. </param>
		/// <param name="culture">Culture-specific information that governs the coercion of <paramref name="args" /> to the formal types declared for the <paramref name="typeName" /> constructor. If <paramref name="culture" /> is null, the <see cref="T:System.Globalization.CultureInfo" /> for the current thread is used. </param>
		/// <param name="activationAttributes">An array of one or more attributes that can participate in activation. This is typically an array that contains a single <see cref="T:System.Runtime.Remoting.Activation.UrlAttribute" /> object. The <see cref="T:System.Runtime.Remoting.Activation.UrlAttribute" /> specifies the URL that is required to activate a remote object. </param>
		/// <param name="securityAttributes">Information used to make security policy decisions and grant code permissions. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="domain" /> or <paramref name="typeName" /> is null. </exception>
		/// <exception cref="T:System.MissingMethodException">No matching constructor was found. </exception>
		/// <exception cref="T:System.TypeLoadException">
		///   <paramref name="typename" /> was not found in <paramref name="assemblyName" />. </exception>
		/// <exception cref="T:System.IO.FileNotFoundException">
		///   <paramref name="assemblyName" /> was not found. </exception>
		/// <exception cref="T:System.MethodAccessException">The caller does not have permission to call this constructor. </exception>
		/// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism. </exception>
		/// <exception cref="T:System.Reflection.TargetInvocationException">The constructor, which was invoked through reflection, threw an exception. </exception>
		/// <exception cref="T:System.Runtime.InteropServices.InvalidComObjectException">The COM type was not obtained through <see cref="Overload:System.Type.GetTypeFromProgID" /> or <see cref="Overload:System.Type.GetTypeFromCLSID" />. </exception>
		/// <exception cref="T:System.NotSupportedException">Creation of <see cref="T:System.TypedReference" />, <see cref="T:System.ArgIterator" />, <see cref="T:System.Void" />, and <see cref="T:System.RuntimeArgumentHandle" /> types, or arrays of those types, is not supported.-or- <paramref name="activationAttributes" /> is not an empty array, and the type being created does not derive from <see cref="T:System.MarshalByRefObject" />. -or-The constructor that best matches <paramref name="args" /> has varargs arguments.</exception>
		/// <exception cref="T:System.BadImageFormatException">
		///   <paramref name="assemblyName" /> is not a valid assembly. -or-The common language runtime (CLR) version 2.0 or later is currently loaded, and <paramref name="assemblyName" /> was compiled for a version of the CLR that is later than the currently loaded version. Note that the .NET Framework versions 2.0, 3.0, and 3.5 all use CLR version 2.0.</exception>
		/// <exception cref="T:System.IO.FileLoadException">An assembly or module was loaded twice with two different evidences. </exception>
		public static ObjectHandle CreateInstance(AppDomain domain, string assemblyName, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes, Evidence securityAttributes)
		{
			if (domain == null)
			{
				throw new ArgumentNullException("domain");
			}
			return domain.CreateInstance(assemblyName, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes, securityAttributes);
		}

		/// <summary>Creates an instance of the type designated by the specified generic type parameter, using the parameterless constructor.</summary>
		/// <returns>A reference to the newly created object.</returns>
		/// <typeparam name="T">The type to create.</typeparam>
		/// <exception cref="T:System.MissingMethodException">The type that is specified for <paramref name="T" /> does not have a parameterless constructor.</exception>
		public static T CreateInstance<T>()
		{
			return (T)((object)Activator.CreateInstance(typeof(T)));
		}

		/// <summary>Creates an instance of the specified type using that type's default constructor.</summary>
		/// <returns>A reference to the newly created object.</returns>
		/// <param name="type">The type of object to create. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="type" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="type" /> is not a RuntimeType. -or-<paramref name="type" /> is an open generic type (that is, the <see cref="P:System.Type.ContainsGenericParameters" /> property returns true).</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="type" /> cannot be a <see cref="T:System.Reflection.Emit.TypeBuilder" />.-or- Creation of <see cref="T:System.TypedReference" />, <see cref="T:System.ArgIterator" />, <see cref="T:System.Void" />, and <see cref="T:System.RuntimeArgumentHandle" /> types, or arrays of those types, is not supported.</exception>
		/// <exception cref="T:System.Reflection.TargetInvocationException">The constructor being called throws an exception. </exception>
		/// <exception cref="T:System.MethodAccessException">The caller does not have permission to call this constructor. </exception>
		/// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism. </exception>
		/// <exception cref="T:System.Runtime.InteropServices.InvalidComObjectException">The COM type was not obtained through <see cref="Overload:System.Type.GetTypeFromProgID" /> or <see cref="Overload:System.Type.GetTypeFromCLSID" />. </exception>
		/// <exception cref="T:System.MissingMethodException">No matching public constructor was found. </exception>
		/// <exception cref="T:System.Runtime.InteropServices.COMException">
		///   <paramref name="type" /> is a COM object but the class identifier used to obtain the type is invalid, or the identified class is not registered. </exception>
		/// <exception cref="T:System.TypeLoadException">
		///   <paramref name="type" /> is not a valid type. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		/// </PermissionSet>
		public static object CreateInstance(Type type)
		{
			return Activator.CreateInstance(type, false);
		}

		/// <summary>Creates an instance of the specified type using the constructor that best matches the specified parameters.</summary>
		/// <returns>A reference to the newly created object.</returns>
		/// <param name="type">The type of object to create. </param>
		/// <param name="args">An array of arguments that match in number, order, and type the parameters of the constructor to invoke. If <paramref name="args" /> is an empty array or null, the constructor that takes no parameters (the default constructor) is invoked. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="type" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="type" /> is not a RuntimeType. -or-<paramref name="type" /> is an open generic type (that is, the <see cref="P:System.Type.ContainsGenericParameters" /> property returns true).</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="type" /> cannot be a <see cref="T:System.Reflection.Emit.TypeBuilder" />.-or- Creation of <see cref="T:System.TypedReference" />, <see cref="T:System.ArgIterator" />, <see cref="T:System.Void" />, and <see cref="T:System.RuntimeArgumentHandle" /> types, or arrays of those types, is not supported. -or-The constructor that best matches <paramref name="args" /> has varargs arguments.</exception>
		/// <exception cref="T:System.Reflection.TargetInvocationException">The constructor being called throws an exception. </exception>
		/// <exception cref="T:System.MethodAccessException">The caller does not have permission to call this constructor. </exception>
		/// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism. </exception>
		/// <exception cref="T:System.Runtime.InteropServices.InvalidComObjectException">The COM type was not obtained through <see cref="Overload:System.Type.GetTypeFromProgID" /> or <see cref="Overload:System.Type.GetTypeFromCLSID" />. </exception>
		/// <exception cref="T:System.MissingMethodException">No matching public constructor was found. </exception>
		/// <exception cref="T:System.Runtime.InteropServices.COMException">
		///   <paramref name="type" /> is a COM object but the class identifier used to obtain the type is invalid, or the identified class is not registered. </exception>
		/// <exception cref="T:System.TypeLoadException">
		///   <paramref name="type" /> is not a valid type. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*" />
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence, RemotingConfiguration" />
		/// </PermissionSet>
		public static object CreateInstance(Type type, params object[] args)
		{
			return Activator.CreateInstance(type, args, new object[0]);
		}

		/// <summary>Creates an instance of the specified type using the constructor that best matches the specified parameters.</summary>
		/// <returns>A reference to the newly created object.</returns>
		/// <param name="type">The type of object to create. </param>
		/// <param name="args">An array of arguments that match in number, order, and type the parameters of the constructor to invoke. If <paramref name="args" /> is an empty array or null, the constructor that takes no parameters (the default constructor) is invoked. </param>
		/// <param name="activationAttributes">An array of one or more attributes that can participate in activation. This is typically an array that contains a single <see cref="T:System.Runtime.Remoting.Activation.UrlAttribute" /> object. The <see cref="T:System.Runtime.Remoting.Activation.UrlAttribute" /> specifies the URL that is required to activate a remote object. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="type" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="type" /> is not a RuntimeType. -or-<paramref name="type" /> is an open generic type (that is, the <see cref="P:System.Type.ContainsGenericParameters" /> property returns true).</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="type" /> cannot be a <see cref="T:System.Reflection.Emit.TypeBuilder" />.-or- Creation of <see cref="T:System.TypedReference" />, <see cref="T:System.ArgIterator" />, <see cref="T:System.Void" />, and <see cref="T:System.RuntimeArgumentHandle" /> types, or arrays of those types, is not supported.-or- <paramref name="activationAttributes" /> is not an empty array, and the type being created does not derive from <see cref="T:System.MarshalByRefObject" />. -or-The constructor that best matches <paramref name="args" /> has varargs arguments.</exception>
		/// <exception cref="T:System.Reflection.TargetInvocationException">The constructor being called throws an exception. </exception>
		/// <exception cref="T:System.MethodAccessException">The caller does not have permission to call this constructor. </exception>
		/// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism. </exception>
		/// <exception cref="T:System.Runtime.InteropServices.InvalidComObjectException">The COM type was not obtained through <see cref="Overload:System.Type.GetTypeFromProgID" /> or <see cref="Overload:System.Type.GetTypeFromCLSID" />. </exception>
		/// <exception cref="T:System.MissingMethodException">No matching public constructor was found. </exception>
		/// <exception cref="T:System.Runtime.InteropServices.COMException">
		///   <paramref name="type" /> is a COM object but the class identifier used to obtain the type is invalid, or the identified class is not registered. </exception>
		/// <exception cref="T:System.TypeLoadException">
		///   <paramref name="type" /> is not a valid type. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*" />
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence, RemotingConfiguration" />
		/// </PermissionSet>
		public static object CreateInstance(Type type, object[] args, object[] activationAttributes)
		{
			return Activator.CreateInstance(type, BindingFlags.Default, Binder.DefaultBinder, args, null, activationAttributes);
		}

		/// <summary>Creates an instance of the specified type using the constructor that best matches the specified parameters.</summary>
		/// <returns>A reference to the newly created object.</returns>
		/// <param name="type">The type of object to create. </param>
		/// <param name="bindingAttr">A combination of zero or more bit flags that affect the search for the <paramref name="type" /> constructor. If <paramref name="bindingAttr" /> is zero, a case-sensitive search for public constructors is conducted. </param>
		/// <param name="binder">An object that uses <paramref name="bindingAttr" /> and <paramref name="args" /> to seek and identify the <paramref name="type" /> constructor. If <paramref name="binder" /> is null, the default binder is used. </param>
		/// <param name="args">An array of arguments that match in number, order, and type the parameters of the constructor to invoke. If <paramref name="args" /> is an empty array or null, the constructor that takes no parameters (the default constructor) is invoked. </param>
		/// <param name="culture">Culture-specific information that governs the coercion of <paramref name="args" /> to the formal types declared for the <paramref name="type" /> constructor. If <paramref name="culture" /> is null, the <see cref="T:System.Globalization.CultureInfo" /> for the current thread is used. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="type" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="type" /> is not a RuntimeType. -or-<paramref name="type" /> is an open generic type (that is, the <see cref="P:System.Type.ContainsGenericParameters" /> property returns true).</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="type" /> cannot be a <see cref="T:System.Reflection.Emit.TypeBuilder" />.-or- Creation of <see cref="T:System.TypedReference" />, <see cref="T:System.ArgIterator" />, <see cref="T:System.Void" />, and <see cref="T:System.RuntimeArgumentHandle" /> types, or arrays of those types, is not supported. -or-The constructor that best matches <paramref name="args" /> has varargs arguments.</exception>
		/// <exception cref="T:System.Reflection.TargetInvocationException">The constructor being called throws an exception. </exception>
		/// <exception cref="T:System.MethodAccessException">The caller does not have permission to call this constructor. </exception>
		/// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism. </exception>
		/// <exception cref="T:System.Runtime.InteropServices.InvalidComObjectException">The COM type was not obtained through <see cref="Overload:System.Type.GetTypeFromProgID" /> or <see cref="Overload:System.Type.GetTypeFromCLSID" />. </exception>
		/// <exception cref="T:System.MissingMethodException">No matching constructor was found. </exception>
		/// <exception cref="T:System.Runtime.InteropServices.COMException">
		///   <paramref name="type" /> is a COM object but the class identifier used to obtain the type is invalid, or the identified class is not registered. </exception>
		/// <exception cref="T:System.TypeLoadException">
		///   <paramref name="type" /> is not a valid type. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*" />
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence, RemotingConfiguration" />
		/// </PermissionSet>
		public static object CreateInstance(Type type, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture)
		{
			return Activator.CreateInstance(type, bindingAttr, binder, args, culture, new object[0]);
		}

		/// <summary>Creates an instance of the specified type using the constructor that best matches the specified parameters.</summary>
		/// <returns>A reference to the newly created object.</returns>
		/// <param name="type">The type of object to create. </param>
		/// <param name="bindingAttr">A combination of zero or more bit flags that affect the search for the <paramref name="type" /> constructor. If <paramref name="bindingAttr" /> is zero, a case-sensitive search for public constructors is conducted. </param>
		/// <param name="binder">An object that uses <paramref name="bindingAttr" /> and <paramref name="args" /> to seek and identify the <paramref name="type" /> constructor. If <paramref name="binder" /> is null, the default binder is used. </param>
		/// <param name="args">An array of arguments that match in number, order, and type the parameters of the constructor to invoke. If <paramref name="args" /> is an empty array or null, the constructor that takes no parameters (the default constructor) is invoked. </param>
		/// <param name="culture">Culture-specific information that governs the coercion of <paramref name="args" /> to the formal types declared for the <paramref name="type" /> constructor. If <paramref name="culture" /> is null, the <see cref="T:System.Globalization.CultureInfo" /> for the current thread is used. </param>
		/// <param name="activationAttributes">An array of one or more attributes that can participate in activation. This is typically an array that contains a single <see cref="T:System.Runtime.Remoting.Activation.UrlAttribute" /> object. The <see cref="T:System.Runtime.Remoting.Activation.UrlAttribute" /> specifies the URL that is required to activate a remote object. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="type" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="type" /> is not a RuntimeType. -or-<paramref name="type" /> is an open generic type (that is, the <see cref="P:System.Type.ContainsGenericParameters" /> property returns true).</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="type" /> cannot be a <see cref="T:System.Reflection.Emit.TypeBuilder" />.-or- Creation of <see cref="T:System.TypedReference" />, <see cref="T:System.ArgIterator" />, <see cref="T:System.Void" />, and <see cref="T:System.RuntimeArgumentHandle" /> types, or arrays of those types, is not supported.-or- <paramref name="activationAttributes" /> is not an empty array, and the type being created does not derive from <see cref="T:System.MarshalByRefObject" />. -or-The constructor that best matches <paramref name="args" /> has varargs arguments.</exception>
		/// <exception cref="T:System.Reflection.TargetInvocationException">The constructor being called throws an exception. </exception>
		/// <exception cref="T:System.MethodAccessException">The caller does not have permission to call this constructor. </exception>
		/// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism. </exception>
		/// <exception cref="T:System.Runtime.InteropServices.InvalidComObjectException">The COM type was not obtained through <see cref="Overload:System.Type.GetTypeFromProgID" /> or <see cref="Overload:System.Type.GetTypeFromCLSID" />. </exception>
		/// <exception cref="T:System.MissingMethodException">No matching constructor was found. </exception>
		/// <exception cref="T:System.Runtime.InteropServices.COMException">
		///   <paramref name="type" /> is a COM object but the class identifier used to obtain the type is invalid, or the identified class is not registered. </exception>
		/// <exception cref="T:System.TypeLoadException">
		///   <paramref name="type" /> is not a valid type. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*" />
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence, RemotingConfiguration" />
		/// </PermissionSet>
		public static object CreateInstance(Type type, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes)
		{
			Activator.CheckType(type);
			if (type.ContainsGenericParameters)
			{
				throw new ArgumentException(type + " is an open generic type", "type");
			}
			if ((bindingAttr & (BindingFlags.IgnoreCase | BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy)) == BindingFlags.Default)
			{
				bindingAttr |= (BindingFlags.Instance | BindingFlags.Public);
			}
			int num = 0;
			if (args != null)
			{
				num = args.Length;
			}
			Type[] array = (num != 0) ? new Type[num] : Type.EmptyTypes;
			for (int i = 0; i < num; i++)
			{
				if (args[i] != null)
				{
					array[i] = args[i].GetType();
				}
			}
			if (binder == null)
			{
				binder = Binder.DefaultBinder;
			}
			ConstructorInfo constructorInfo = (ConstructorInfo)binder.SelectMethod(bindingAttr, type.GetConstructors(bindingAttr), array, null);
			if (constructorInfo != null)
			{
				Activator.CheckAbstractType(type);
				if (activationAttributes != null && activationAttributes.Length > 0)
				{
					if (!type.IsMarshalByRef)
					{
						string text = Locale.GetText("Type '{0}' doesn't derive from MarshalByRefObject.", new object[]
						{
							type.FullName
						});
						throw new NotSupportedException(text);
					}
					object obj = ActivationServices.CreateProxyFromAttributes(type, activationAttributes);
					if (obj != null)
					{
						constructorInfo.Invoke(obj, bindingAttr, binder, args, culture);
						return obj;
					}
				}
				return constructorInfo.Invoke(bindingAttr, binder, args, culture);
			}
			if (type.IsValueType && array.Length == 0)
			{
				return Activator.CreateInstanceInternal(type);
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Type type2 in array)
			{
				stringBuilder.Append((type2 == null) ? "(unknown)" : type2.ToString());
				stringBuilder.Append(", ");
			}
			if (stringBuilder.Length > 2)
			{
				stringBuilder.Length -= 2;
			}
			throw new MissingMethodException(string.Format(Locale.GetText("No constructor found for {0}::.ctor({1})"), type.FullName, stringBuilder));
		}

		/// <summary>Creates an instance of the specified type using that type's default constructor.</summary>
		/// <returns>A reference to the newly created object.</returns>
		/// <param name="type">The type of object to create. </param>
		/// <param name="nonPublic">true if a public or nonpublic default constructor can match; false if only a public default constructor can match. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="type" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="type" /> is not a RuntimeType. -or-<paramref name="type" /> is an open generic type (that is, the <see cref="P:System.Type.ContainsGenericParameters" /> property returns true).</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="type" /> cannot be a <see cref="T:System.Reflection.Emit.TypeBuilder" />.-or- Creation of <see cref="T:System.TypedReference" />, <see cref="T:System.ArgIterator" />, <see cref="T:System.Void" />, and <see cref="T:System.RuntimeArgumentHandle" /> types, or arrays of those types, is not supported.</exception>
		/// <exception cref="T:System.Reflection.TargetInvocationException">The constructor being called throws an exception. </exception>
		/// <exception cref="T:System.MethodAccessException">The caller does not have permission to call this constructor. </exception>
		/// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism. </exception>
		/// <exception cref="T:System.Runtime.InteropServices.InvalidComObjectException">The COM type was not obtained through <see cref="Overload:System.Type.GetTypeFromProgID" /> or <see cref="Overload:System.Type.GetTypeFromCLSID" />. </exception>
		/// <exception cref="T:System.MissingMethodException">No matching public constructor was found. </exception>
		/// <exception cref="T:System.Runtime.InteropServices.COMException">
		///   <paramref name="type" /> is a COM object but the class identifier used to obtain the type is invalid, or the identified class is not registered. </exception>
		/// <exception cref="T:System.TypeLoadException">
		///   <paramref name="type" /> is not a valid type. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		/// </PermissionSet>
		public static object CreateInstance(Type type, bool nonPublic)
		{
			Activator.CheckType(type);
			if (type.ContainsGenericParameters)
			{
				throw new ArgumentException(type + " is an open generic type", "type");
			}
			Activator.CheckAbstractType(type);
			MonoType monoType = type as MonoType;
			ConstructorInfo constructorInfo;
			if (monoType != null)
			{
				constructorInfo = monoType.GetDefaultConstructor();
				if (!nonPublic && constructorInfo != null && !constructorInfo.IsPublic)
				{
					constructorInfo = null;
				}
			}
			else
			{
				BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public;
				if (nonPublic)
				{
					bindingFlags |= BindingFlags.NonPublic;
				}
				constructorInfo = type.GetConstructor(bindingFlags, null, CallingConventions.Any, Type.EmptyTypes, null);
			}
			if (constructorInfo != null)
			{
				return constructorInfo.Invoke(null);
			}
			if (type.IsValueType)
			{
				return Activator.CreateInstanceInternal(type);
			}
			throw new MissingMethodException(Locale.GetText("Default constructor not found."), ".ctor() of " + type.FullName);
		}

		private static void CheckType(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (type == typeof(TypedReference) || type == typeof(ArgIterator) || type == typeof(void) || type == typeof(RuntimeArgumentHandle))
			{
				string text = Locale.GetText("CreateInstance cannot be used to create this type ({0}).", new object[]
				{
					type.FullName
				});
				throw new NotSupportedException(text);
			}
		}

		private static void CheckAbstractType(Type type)
		{
			if (type.IsAbstract)
			{
				string text = Locale.GetText("Cannot create an abstract class '{0}'.", new object[]
				{
					type.FullName
				});
				throw new MissingMethodException(text);
			}
		}

		/// <summary>Creates a proxy for the well-known object indicated by the specified type and URL.</summary>
		/// <returns>A proxy that points to an endpoint served by the requested well-known object.</returns>
		/// <param name="type">The type of the well-known object to which you want to connect. </param>
		/// <param name="url">The URL of the well-known object. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="type" /> or <paramref name="url" /> is null. </exception>
		/// <exception cref="T:System.Runtime.Remoting.RemotingException">
		///   <paramref name="type" /> is not marshaled by reference.-or-<paramref name="type" /> is an interface. </exception>
		/// <exception cref="T:System.MemberAccessException">This member was invoked with a late-binding mechanism. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="RemotingConfiguration" />
		/// </PermissionSet>
		public static object GetObject(Type type, string url)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			return RemotingServices.Connect(type, url);
		}

		/// <summary>Creates a proxy for the well-known object indicated by the specified type, URL, and channel data.</summary>
		/// <returns>A proxy that points to an endpoint served by the requested well-known object.</returns>
		/// <param name="type">The type of the well-known object to which you want to connect. </param>
		/// <param name="url">The URL of the well-known object. </param>
		/// <param name="state">Channel-specific data or null. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="type" /> or <paramref name="url" /> is null. </exception>
		/// <exception cref="T:System.Runtime.Remoting.RemotingException">
		///   <paramref name="type" /> is not marshaled by reference.-or-<paramref name="type" /> is an interface. </exception>
		/// <exception cref="T:System.MemberAccessException">This member was invoked with a late-binding mechanism. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="RemotingConfiguration" />
		/// </PermissionSet>
		public static object GetObject(Type type, string url, object state)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			return RemotingServices.Connect(type, url, state);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern object CreateInstanceInternal(Type type);
	}
}
