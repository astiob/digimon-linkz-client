using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	/// <summary>Represents a delegate, which is a data structure that refers to a static method or to a class instance and an instance method of that class.</summary>
	/// <filterpriority>2</filterpriority>
	[ClassInterface(ClassInterfaceType.AutoDual)]
	[ComVisible(true)]
	[Serializable]
	public abstract class Delegate : ICloneable, ISerializable
	{
		private IntPtr method_ptr;

		private IntPtr invoke_impl;

		private object m_target;

		private IntPtr method;

		private IntPtr delegate_trampoline;

		private IntPtr method_code;

		private MethodInfo method_info;

		private MethodInfo original_method_info;

		private DelegateData data;

		/// <summary>Initializes a delegate that invokes the specified instance method on the specified class instance.</summary>
		/// <param name="target">The class instance on which the delegate invokes <paramref name="method" />. </param>
		/// <param name="method">The name of the instance method that the delegate represents. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="target" /> is null.-or- <paramref name="method" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">There was an error binding to the target method.</exception>
		protected Delegate(object target, string method)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			if (method == null)
			{
				throw new ArgumentNullException("method");
			}
			this.m_target = target;
			this.data = new DelegateData();
			this.data.method_name = method;
		}

		/// <summary>Initializes a delegate that invokes the specified static method from the specified class.</summary>
		/// <param name="target">The <see cref="T:System.Type" /> representing the class that defines <paramref name="method" />. </param>
		/// <param name="method">The name of the static method that the delegate represents. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="target" /> is null.-or- <paramref name="method" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="target" /> is not a RuntimeType. See Runtime Types in Reflection.-or-<paramref name="target" /> represents an open generic type.</exception>
		protected Delegate(Type target, string method)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			if (method == null)
			{
				throw new ArgumentNullException("method");
			}
			this.data = new DelegateData();
			this.data.method_name = method;
			this.data.target_type = target;
		}

		/// <summary>Gets the method represented by the delegate.</summary>
		/// <returns>A <see cref="T:System.Reflection.MethodInfo" /> describing the method represented by the delegate.</returns>
		/// <exception cref="T:System.MemberAccessException">The caller does not have access to the method represented by the delegate (for example, if the method is private). </exception>
		/// <filterpriority>2</filterpriority>
		public MethodInfo Method
		{
			get
			{
				if (this.method_info != null)
				{
					return this.method_info;
				}
				if (this.method != IntPtr.Zero)
				{
					this.method_info = (MethodInfo)MethodBase.GetMethodFromHandleNoGenericCheck(new RuntimeMethodHandle(this.method));
				}
				return this.method_info;
			}
		}

		/// <summary>Gets the class instance on which the current delegate invokes the instance method.</summary>
		/// <returns>The object on which the current delegate invokes the instance method, if the delegate represents an instance method; null if the delegate represents a static method.</returns>
		/// <filterpriority>2</filterpriority>
		public object Target
		{
			get
			{
				return this.m_target;
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern Delegate CreateDelegate_internal(Type type, object target, MethodInfo info, bool throwOnBindFailure);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void SetMulticastInvoke();

		private static bool arg_type_match(Type delArgType, Type argType)
		{
			bool flag = delArgType == argType;
			if (!flag && !argType.IsValueType && argType.IsAssignableFrom(delArgType))
			{
				flag = true;
			}
			return flag;
		}

		private static bool return_type_match(Type delReturnType, Type returnType)
		{
			bool flag = returnType == delReturnType;
			if (!flag && !returnType.IsValueType && delReturnType.IsAssignableFrom(returnType))
			{
				flag = true;
			}
			return flag;
		}

		/// <summary>Creates a delegate of the specified type that represents the specified static or instance method, with the specified first argument and the specified behavior on failure to bind.</summary>
		/// <returns>A delegate of the specified type that represents the specified static or instance method, or null if <paramref name="throwOnBindFailure" /> is false and the delegate cannot be bound to <paramref name="method" />. </returns>
		/// <param name="type">A <see cref="T:System.Type" /> representing the type of delegate to create. </param>
		/// <param name="firstArgument">An <see cref="T:System.Object" /> that is the first argument of the method the delegate represents. For instance methods, it must be compatible with the instance type. </param>
		/// <param name="method">The <see cref="T:System.Reflection.MethodInfo" /> describing the static or instance method the delegate is to represent.</param>
		/// <param name="throwOnBindFailure">true to throw an exception if <paramref name="method" /> cannot be bound; otherwise, false.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="type" /> is null.-or- <paramref name="method" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="type" /> does not inherit <see cref="T:System.MulticastDelegate" />.-or-<paramref name="type" /> is not a RuntimeType. See Runtime Types in Reflection. -or-<paramref name="method" /> cannot be bound, and <paramref name="throwOnBindFailure" /> is true.-or-<paramref name="method" /> is not a RuntimeMethodInfo. See Runtime Types in Reflection.</exception>
		/// <exception cref="T:System.MissingMethodException">The Invoke method of <paramref name="type" /> is not found. </exception>
		/// <exception cref="T:System.MethodAccessException">The caller does not have the permissions necessary to access <paramref name="method" />. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		/// </PermissionSet>
		public static Delegate CreateDelegate(Type type, object firstArgument, MethodInfo method, bool throwOnBindFailure)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (method == null)
			{
				throw new ArgumentNullException("method");
			}
			if (!type.IsSubclassOf(typeof(MulticastDelegate)))
			{
				throw new ArgumentException("type is not a subclass of Multicastdelegate");
			}
			MethodInfo methodInfo = type.GetMethod("Invoke");
			if (!Delegate.return_type_match(methodInfo.ReturnType, method.ReturnType))
			{
				if (throwOnBindFailure)
				{
					throw new ArgumentException("method return type is incompatible");
				}
				return null;
			}
			else
			{
				ParameterInfo[] parameters = methodInfo.GetParameters();
				ParameterInfo[] parameters2 = method.GetParameters();
				bool flag;
				if (firstArgument != null)
				{
					if (!method.IsStatic)
					{
						flag = (parameters2.Length == parameters.Length);
					}
					else
					{
						flag = (parameters2.Length == parameters.Length + 1);
					}
				}
				else if (!method.IsStatic)
				{
					flag = (parameters2.Length + 1 == parameters.Length);
				}
				else
				{
					flag = (parameters2.Length == parameters.Length);
					if (!flag)
					{
						flag = (parameters2.Length == parameters.Length + 1);
					}
				}
				if (!flag)
				{
					if (throwOnBindFailure)
					{
						throw new ArgumentException("method argument length mismatch");
					}
					return null;
				}
				else
				{
					bool flag2;
					if (firstArgument != null)
					{
						if (!method.IsStatic)
						{
							flag2 = Delegate.arg_type_match(firstArgument.GetType(), method.DeclaringType);
							for (int i = 0; i < parameters2.Length; i++)
							{
								flag2 &= Delegate.arg_type_match(parameters[i].ParameterType, parameters2[i].ParameterType);
							}
						}
						else
						{
							flag2 = Delegate.arg_type_match(firstArgument.GetType(), parameters2[0].ParameterType);
							for (int j = 1; j < parameters2.Length; j++)
							{
								flag2 &= Delegate.arg_type_match(parameters[j - 1].ParameterType, parameters2[j].ParameterType);
							}
						}
					}
					else if (!method.IsStatic)
					{
						flag2 = Delegate.arg_type_match(parameters[0].ParameterType, method.DeclaringType);
						for (int k = 0; k < parameters2.Length; k++)
						{
							flag2 &= Delegate.arg_type_match(parameters[k + 1].ParameterType, parameters2[k].ParameterType);
						}
					}
					else if (parameters.Length + 1 == parameters2.Length)
					{
						flag2 = !parameters2[0].ParameterType.IsValueType;
						for (int l = 0; l < parameters.Length; l++)
						{
							flag2 &= Delegate.arg_type_match(parameters[l].ParameterType, parameters2[l + 1].ParameterType);
						}
					}
					else
					{
						flag2 = true;
						for (int m = 0; m < parameters2.Length; m++)
						{
							flag2 &= Delegate.arg_type_match(parameters[m].ParameterType, parameters2[m].ParameterType);
						}
					}
					if (flag2)
					{
						Delegate @delegate = Delegate.CreateDelegate_internal(type, firstArgument, method, throwOnBindFailure);
						if (@delegate != null)
						{
							@delegate.original_method_info = method;
						}
						return @delegate;
					}
					if (throwOnBindFailure)
					{
						throw new ArgumentException("method arguments are incompatible");
					}
					return null;
				}
			}
		}

		/// <summary>Creates a delegate of the specified type that represents the specified static or instance method, with the specified first argument.</summary>
		/// <returns>A delegate of the specified type that represents the specified static or instance method. </returns>
		/// <param name="type">The <see cref="T:System.Type" /> of delegate to create. </param>
		/// <param name="firstArgument">The object to which the delegate is bound, or null to treat <paramref name="method" /> as static (Shared in Visual Basic). </param>
		/// <param name="method">The <see cref="T:System.Reflection.MethodInfo" /> describing the static or instance method the delegate is to represent.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="type" /> is null.-or- <paramref name="method" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="type" /> does not inherit <see cref="T:System.MulticastDelegate" />.-or-<paramref name="type" /> is not a RuntimeType. See Runtime Types in Reflection. -or-<paramref name="method" /> cannot be bound.-or-<paramref name="method" /> is not a RuntimeMethodInfo. See Runtime Types in Reflection.</exception>
		/// <exception cref="T:System.MissingMethodException">The Invoke method of <paramref name="type" /> is not found. </exception>
		/// <exception cref="T:System.MethodAccessException">The caller does not have the permissions necessary to access <paramref name="method" />. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		/// </PermissionSet>
		public static Delegate CreateDelegate(Type type, object firstArgument, MethodInfo method)
		{
			return Delegate.CreateDelegate(type, firstArgument, method, true);
		}

		/// <summary>Creates a delegate of the specified type to represent the specified static method, with the specified behavior on failure to bind.</summary>
		/// <returns>A delegate of the specified type to represent the specified static method.</returns>
		/// <param name="type">The <see cref="T:System.Type" /> of delegate to create. </param>
		/// <param name="method">The <see cref="T:System.Reflection.MethodInfo" /> describing the static or instance method the delegate is to represent.</param>
		/// <param name="throwOnBindFailure">true to throw an exception if <paramref name="method" /> cannot be bound; otherwise, false.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="type" /> is null.-or- <paramref name="method" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="type" /> does not inherit <see cref="T:System.MulticastDelegate" />.-or-<paramref name="type" /> is not a RuntimeType. See Runtime Types in Reflection. -or-<paramref name="method" /> cannot be bound, and <paramref name="throwOnBindFailure" /> is true.-or-<paramref name="method" /> is not a RuntimeMethodInfo. See Runtime Types in Reflection.</exception>
		/// <exception cref="T:System.MissingMethodException">The Invoke method of <paramref name="type" /> is not found. </exception>
		/// <exception cref="T:System.MethodAccessException">The caller does not have the permissions necessary to access <paramref name="method" />. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		/// </PermissionSet>
		public static Delegate CreateDelegate(Type type, MethodInfo method, bool throwOnBindFailure)
		{
			return Delegate.CreateDelegate(type, null, method, throwOnBindFailure);
		}

		/// <summary>Creates a delegate of the specified type to represent the specified static method.</summary>
		/// <returns>A delegate of the specified type to represent the specified static method.</returns>
		/// <param name="type">The <see cref="T:System.Type" /> of delegate to create. </param>
		/// <param name="method">The <see cref="T:System.Reflection.MethodInfo" /> describing the static or instance method the delegate is to represent. Only static methods are supported in the .NET Framework version 1.0 and 1.1.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="type" /> is null.-or- <paramref name="method" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="type" /> does not inherit <see cref="T:System.MulticastDelegate" />.-or-<paramref name="type" /> is not a RuntimeType. See Runtime Types in Reflection. -or- <paramref name="method" /> is not a static method, and the .NET Framework version is 1.0 or 1.1. -or-<paramref name="method" /> cannot be bound.-or-<paramref name="method" /> is not a RuntimeMethodInfo. See Runtime Types in Reflection.</exception>
		/// <exception cref="T:System.MissingMethodException">The Invoke method of <paramref name="type" /> is not found. </exception>
		/// <exception cref="T:System.MethodAccessException">The caller does not have the permissions necessary to access <paramref name="method" />. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		/// </PermissionSet>
		public static Delegate CreateDelegate(Type type, MethodInfo method)
		{
			return Delegate.CreateDelegate(type, method, true);
		}

		/// <summary>Creates a delegate of the specified type that represents the specified instance method to invoke on the specified class instance.</summary>
		/// <returns>A delegate of the specified type that represents the specified instance method to invoke on the specified class instance.</returns>
		/// <param name="type">The <see cref="T:System.Type" /> of delegate to create. </param>
		/// <param name="target">The class instance on which <paramref name="method" /> is invoked. </param>
		/// <param name="method">The name of the instance method that the delegate is to represent. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="type" /> is null.-or- <paramref name="target" /> is null.-or- <paramref name="method" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="type" /> does not inherit <see cref="T:System.MulticastDelegate" />. -or-<paramref name="type" /> is not a RuntimeType. See Runtime Types in Reflection.-or- <paramref name="method" /> is not an instance method. -or-<paramref name="method" /> cannot be bound, for example because it cannot be found.</exception>
		/// <exception cref="T:System.MissingMethodException">The Invoke method of <paramref name="type" /> is not found. </exception>
		/// <exception cref="T:System.MethodAccessException">The caller does not have the permissions necessary to access <paramref name="method" />. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		/// </PermissionSet>
		public static Delegate CreateDelegate(Type type, object target, string method)
		{
			return Delegate.CreateDelegate(type, target, method, false);
		}

		private static MethodInfo GetCandidateMethod(Type type, Type target, string method, BindingFlags bflags, bool ignoreCase, bool throwOnBindFailure)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (method == null)
			{
				throw new ArgumentNullException("method");
			}
			if (!type.IsSubclassOf(typeof(MulticastDelegate)))
			{
				throw new ArgumentException("type is not subclass of MulticastDelegate.");
			}
			MethodInfo methodInfo = type.GetMethod("Invoke");
			ParameterInfo[] parameters = methodInfo.GetParameters();
			Type[] array = new Type[parameters.Length];
			for (int i = 0; i < parameters.Length; i++)
			{
				array[i] = parameters[i].ParameterType;
			}
			BindingFlags bindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.ExactBinding | bflags;
			if (ignoreCase)
			{
				bindingFlags |= BindingFlags.IgnoreCase;
			}
			MethodInfo methodInfo2 = null;
			for (Type type2 = target; type2 != null; type2 = type2.BaseType)
			{
				MethodInfo methodInfo3 = type2.GetMethod(method, bindingFlags, null, array, new ParameterModifier[0]);
				if (methodInfo3 != null && Delegate.return_type_match(methodInfo.ReturnType, methodInfo3.ReturnType))
				{
					methodInfo2 = methodInfo3;
					break;
				}
			}
			if (methodInfo2 != null)
			{
				return methodInfo2;
			}
			if (throwOnBindFailure)
			{
				throw new ArgumentException("Couldn't bind to method '" + method + "'.");
			}
			return null;
		}

		/// <summary>Creates a delegate of the specified type that represents the specified static method of the specified class, with the specified case-sensitivity and the specified behavior on failure to bind.</summary>
		/// <returns>A delegate of the specified type that represents the specified static method of the specified class.</returns>
		/// <param name="type">The <see cref="T:System.Type" /> of delegate to create. </param>
		/// <param name="target">The <see cref="T:System.Type" /> representing the class that implements <paramref name="method" />. </param>
		/// <param name="method">The name of the static method that the delegate is to represent. </param>
		/// <param name="ignoreCase">A Boolean indicating whether to ignore the case when comparing the name of the method.</param>
		/// <param name="throwOnBindFailure">true to throw an exception if <paramref name="method" /> cannot be bound; otherwise, false.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="type" /> is null.-or- <paramref name="target" /> is null.-or- <paramref name="method" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="type" /> does not inherit <see cref="T:System.MulticastDelegate" />.-or- <paramref name="type" /> is not a RuntimeType. See Runtime Types in Reflection. -or-<paramref name="target" /> is not a RuntimeType.-or-<paramref name="target" /> is an open generic type. That is, its <see cref="P:System.Type.ContainsGenericParameters" /> property is true.-or-<paramref name="method" /> is not a static method (Shared method in Visual Basic). -or-<paramref name="method" /> cannot be bound, for example because it cannot be found, and <paramref name="throwOnBindFailure" /> is true. </exception>
		/// <exception cref="T:System.MissingMethodException">The Invoke method of <paramref name="type" /> is not found. </exception>
		/// <exception cref="T:System.MethodAccessException">The caller does not have the permissions necessary to access <paramref name="method" />. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		/// </PermissionSet>
		public static Delegate CreateDelegate(Type type, Type target, string method, bool ignoreCase, bool throwOnBindFailure)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			MethodInfo candidateMethod = Delegate.GetCandidateMethod(type, target, method, BindingFlags.Static, ignoreCase, throwOnBindFailure);
			if (candidateMethod == null)
			{
				return null;
			}
			return Delegate.CreateDelegate_internal(type, null, candidateMethod, throwOnBindFailure);
		}

		/// <summary>Creates a delegate of the specified type that represents the specified static method of the specified class.</summary>
		/// <returns>A delegate of the specified type that represents the specified static method of the specified class.</returns>
		/// <param name="type">The <see cref="T:System.Type" /> of delegate to create. </param>
		/// <param name="target">The <see cref="T:System.Type" /> representing the class that implements <paramref name="method" />. </param>
		/// <param name="method">The name of the static method that the delegate is to represent. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="type" /> is null.-or- <paramref name="target" /> is null.-or- <paramref name="method" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="type" /> does not inherit <see cref="T:System.MulticastDelegate" />.-or- <paramref name="type" /> is not a RuntimeType. See Runtime Types in Reflection. -or-<paramref name="target" /> is not a RuntimeType.-or-<paramref name="target" /> is an open generic type. That is, its <see cref="P:System.Type.ContainsGenericParameters" /> property is true.-or-<paramref name="method" /> is not a static method (Shared method in Visual Basic). -or-<paramref name="method" /> cannot be bound, for example because it cannot be found, and <paramref name="throwOnBindFailure" /> is true.</exception>
		/// <exception cref="T:System.MissingMethodException">The Invoke method of <paramref name="type" /> is not found. </exception>
		/// <exception cref="T:System.MethodAccessException">The caller does not have the permissions necessary to access <paramref name="method" />. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		/// </PermissionSet>
		public static Delegate CreateDelegate(Type type, Type target, string method)
		{
			return Delegate.CreateDelegate(type, target, method, false, true);
		}

		/// <summary>Creates a delegate of the specified type that represents the specified static method of the specified class, with the specified case-sensitivity.</summary>
		/// <returns>A delegate of the specified type that represents the specified static method of the specified class.</returns>
		/// <param name="type">The <see cref="T:System.Type" /> of delegate to create. </param>
		/// <param name="target">The <see cref="T:System.Type" /> representing the class that implements <paramref name="method" />. </param>
		/// <param name="method">The name of the static method that the delegate is to represent. </param>
		/// <param name="ignoreCase">A Boolean indicating whether to ignore the case when comparing the name of the method.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="type" /> is null.-or- <paramref name="target" /> is null.-or- <paramref name="method" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="type" /> does not inherit <see cref="T:System.MulticastDelegate" />.-or- <paramref name="type" /> is not a RuntimeType. See Runtime Types in Reflection. -or-<paramref name="target" /> is not a RuntimeType.-or-<paramref name="target" /> is an open generic type. That is, its <see cref="P:System.Type.ContainsGenericParameters" /> property is true.-or-<paramref name="method" /> is not a static method (Shared method in Visual Basic). -or-<paramref name="method" /> cannot be bound, for example because it cannot be found.</exception>
		/// <exception cref="T:System.MissingMethodException">The Invoke method of <paramref name="type" /> is not found. </exception>
		/// <exception cref="T:System.MethodAccessException">The caller does not have the permissions necessary to access <paramref name="method" />. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		/// </PermissionSet>
		public static Delegate CreateDelegate(Type type, Type target, string method, bool ignoreCase)
		{
			return Delegate.CreateDelegate(type, target, method, ignoreCase, true);
		}

		/// <summary>Creates a delegate of the specified type that represents the specified instance method to invoke on the specified class instance, with the specified case-sensitivity and the specified behavior on failure to bind.</summary>
		/// <returns>A delegate of the specified type that represents the specified instance method to invoke on the specified class instance.</returns>
		/// <param name="type">The <see cref="T:System.Type" /> of delegate to create. </param>
		/// <param name="target">The class instance on which <paramref name="method" /> is invoked. </param>
		/// <param name="method">The name of the instance method that the delegate is to represent. </param>
		/// <param name="ignoreCase">A Boolean indicating whether to ignore the case when comparing the name of the method. </param>
		/// <param name="throwOnBindFailure">true to throw an exception if <paramref name="method" /> cannot be bound; otherwise, false.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="type" /> is null.-or- <paramref name="target" /> is null.-or- <paramref name="method" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="type" /> does not inherit <see cref="T:System.MulticastDelegate" />.-or-<paramref name="type" /> is not a RuntimeType. See Runtime Types in Reflection. -or-  <paramref name="method" /> is not an instance method. -or-<paramref name="method" /> cannot be bound, for example because it cannot be found, and <paramref name="throwOnBindFailure" /> is true.</exception>
		/// <exception cref="T:System.MissingMethodException">The Invoke method of <paramref name="type" /> is not found. </exception>
		/// <exception cref="T:System.MethodAccessException">The caller does not have the permissions necessary to access <paramref name="method" />. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		/// </PermissionSet>
		public static Delegate CreateDelegate(Type type, object target, string method, bool ignoreCase, bool throwOnBindFailure)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			MethodInfo candidateMethod = Delegate.GetCandidateMethod(type, target.GetType(), method, BindingFlags.Instance, ignoreCase, throwOnBindFailure);
			if (candidateMethod == null)
			{
				return null;
			}
			return Delegate.CreateDelegate_internal(type, target, candidateMethod, throwOnBindFailure);
		}

		/// <summary>Creates a delegate of the specified type that represents the specified instance method to invoke on the specified class instance with the specified case-sensitivity.</summary>
		/// <returns>A delegate of the specified type that represents the specified instance method to invoke on the specified class instance.</returns>
		/// <param name="type">The <see cref="T:System.Type" /> of delegate to create. </param>
		/// <param name="target">The class instance on which <paramref name="method" /> is invoked. </param>
		/// <param name="method">The name of the instance method that the delegate is to represent. </param>
		/// <param name="ignoreCase">A Boolean indicating whether to ignore the case when comparing the name of the method. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="type" /> is null.-or- <paramref name="target" /> is null.-or- <paramref name="method" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="type" /> does not inherit <see cref="T:System.MulticastDelegate" />.-or-<paramref name="type" /> is not a RuntimeType. See Runtime Types in Reflection.-or- <paramref name="method" /> is not an instance method. -or-<paramref name="method" /> cannot be bound, for example because it cannot be found.</exception>
		/// <exception cref="T:System.MissingMethodException">The Invoke method of <paramref name="type" /> is not found. </exception>
		/// <exception cref="T:System.MethodAccessException">The caller does not have the permissions necessary to access <paramref name="method" />. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		/// </PermissionSet>
		public static Delegate CreateDelegate(Type type, object target, string method, bool ignoreCase)
		{
			return Delegate.CreateDelegate(type, target, method, ignoreCase, true);
		}

		/// <summary>Dynamically invokes (late-bound) the method represented by the current delegate.</summary>
		/// <returns>The object returned by the method represented by the delegate.</returns>
		/// <param name="args">An array of objects that are the arguments to pass to the method represented by the current delegate.-or- null, if the method represented by the current delegate does not require arguments. </param>
		/// <exception cref="T:System.MemberAccessException">The caller does not have access to the method represented by the delegate (for example, if the method is private).-or- The number, order, or type of parameters listed in <paramref name="args" /> is invalid. </exception>
		/// <exception cref="T:System.Reflection.TargetException">The method represented by the delegate is an instance method and the target object is null.-or- The method represented by the delegate is invoked on an object or a class that does not support it. </exception>
		/// <exception cref="T:System.Reflection.TargetInvocationException">One of the encapsulated methods throws an exception. </exception>
		/// <filterpriority>2</filterpriority>
		public object DynamicInvoke(params object[] args)
		{
			return this.DynamicInvokeImpl(args);
		}

		/// <summary>Dynamically invokes (late-bound) the method represented by the current delegate.</summary>
		/// <returns>The object returned by the method represented by the delegate.</returns>
		/// <param name="args">An array of objects that are the arguments to pass to the method represented by the current delegate.-or- null, if the method represented by the current delegate does not require arguments. </param>
		/// <exception cref="T:System.MemberAccessException">The caller does not have access to the method represented by the delegate (for example, if the method is private).-or- The number, order, or type of parameters listed in <paramref name="args" /> is invalid. </exception>
		/// <exception cref="T:System.Reflection.TargetException">The method represented by the delegate is an instance method and the target object is null.-or- The method represented by the delegate is invoked on an object or a class that does not support it. </exception>
		/// <exception cref="T:System.Reflection.TargetInvocationException">One of the encapsulated methods throws an exception. </exception>
		protected virtual object DynamicInvokeImpl(object[] args)
		{
			if (this.Method == null)
			{
				Type[] array = new Type[args.Length];
				for (int i = 0; i < args.Length; i++)
				{
					array[i] = args[i].GetType();
				}
				this.method_info = this.m_target.GetType().GetMethod(this.data.method_name, array);
			}
			if (this.m_target != null && this.Method.IsStatic)
			{
				if (args != null)
				{
					object[] array2 = new object[args.Length + 1];
					args.CopyTo(array2, 1);
					array2[0] = this.m_target;
					args = array2;
				}
				else
				{
					args = new object[]
					{
						this.m_target
					};
				}
				return this.Method.Invoke(null, args);
			}
			return this.Method.Invoke(this.m_target, args);
		}

		/// <summary>Creates a shallow copy of the delegate.</summary>
		/// <returns>A shallow copy of the delegate.</returns>
		/// <filterpriority>2</filterpriority>
		public virtual object Clone()
		{
			return base.MemberwiseClone();
		}

		/// <summary>Determines whether the specified object and the current delegate are of the same type and share the same targets, methods, and invocation list.</summary>
		/// <returns>true if <paramref name="obj" /> and the current delegate have the same targets, methods, and invocation list; otherwise, false.</returns>
		/// <param name="obj">The object to compare with the current delegate. </param>
		/// <exception cref="T:System.MemberAccessException">The caller does not have access to the method represented by the delegate (for example, if the method is private). </exception>
		/// <filterpriority>2</filterpriority>
		public override bool Equals(object obj)
		{
			Delegate @delegate = obj as Delegate;
			return @delegate != null && (@delegate.m_target == this.m_target && @delegate.method == this.method) && ((@delegate.data == null && this.data == null) || (@delegate.data != null && this.data != null && @delegate.data.target_type == this.data.target_type && @delegate.data.method_name == this.data.method_name));
		}

		/// <summary>Returns a hash code for the delegate.</summary>
		/// <returns>A hash code for the delegate.</returns>
		/// <filterpriority>2</filterpriority>
		public override int GetHashCode()
		{
			return this.method.GetHashCode() ^ ((this.m_target == null) ? 0 : this.m_target.GetHashCode());
		}

		/// <summary>Gets the static method represented by the current delegate.</summary>
		/// <returns>A <see cref="T:System.Reflection.MethodInfo" /> describing the static method represented by the current delegate.</returns>
		/// <exception cref="T:System.MemberAccessException">The caller does not have access to the method represented by the delegate (for example, if the method is private). </exception>
		protected virtual MethodInfo GetMethodImpl()
		{
			return this.Method;
		}

		/// <summary>Not supported.</summary>
		/// <param name="info">Not supported. </param>
		/// <param name="context">Not supported. </param>
		/// <exception cref="T:System.NotSupportedException">This method is not supported.</exception>
		/// <filterpriority>2</filterpriority>
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			DelegateSerializationHolder.GetDelegateData(this, info, context);
		}

		/// <summary>Returns the invocation list of the delegate.</summary>
		/// <returns>An array of delegates representing the invocation list of the current delegate.</returns>
		/// <filterpriority>2</filterpriority>
		public virtual Delegate[] GetInvocationList()
		{
			return new Delegate[]
			{
				this
			};
		}

		/// <summary>Concatenates the invocation lists of two delegates.</summary>
		/// <returns>A new delegate with an invocation list that concatenates the invocation lists of <paramref name="a" /> and <paramref name="b" /> in that order. Returns <paramref name="a" /> if <paramref name="b" /> is null, returns <paramref name="b" /> if <paramref name="a" /> is a null reference, and returns a null reference if both <paramref name="a" /> and <paramref name="b" /> are null references.</returns>
		/// <param name="a">The delegate whose invocation list comes first. </param>
		/// <param name="b">The delegate whose invocation list comes last. </param>
		/// <exception cref="T:System.ArgumentException">Both <paramref name="a" /> and <paramref name="b" /> are not null, and <paramref name="a" /> and <paramref name="b" /> are not instances of the same delegate type. </exception>
		/// <filterpriority>1</filterpriority>
		public static Delegate Combine(Delegate a, Delegate b)
		{
			if (a == null)
			{
				if (b == null)
				{
					return null;
				}
				return b;
			}
			else
			{
				if (b == null)
				{
					return a;
				}
				if (a.GetType() != b.GetType())
				{
					throw new ArgumentException(Locale.GetText("Incompatible Delegate Types."));
				}
				return a.CombineImpl(b);
			}
		}

		/// <summary>Concatenates the invocation lists of an array of delegates.</summary>
		/// <returns>A new delegate with an invocation list that concatenates the invocation lists of the delegates in the <paramref name="delegates" /> array. Returns null if <paramref name="delegates" /> is null, if <paramref name="delegates" /> contains zero elements, or if every entry in <paramref name="delegates" /> is null.</returns>
		/// <param name="delegates">The array of delegates to combine. </param>
		/// <exception cref="T:System.ArgumentException">Not all the non-null entries in <paramref name="delegates" /> are instances of the same delegate type. </exception>
		/// <filterpriority>1</filterpriority>
		[ComVisible(true)]
		public static Delegate Combine(params Delegate[] delegates)
		{
			if (delegates == null)
			{
				return null;
			}
			Delegate @delegate = null;
			foreach (Delegate b in delegates)
			{
				@delegate = Delegate.Combine(@delegate, b);
			}
			return @delegate;
		}

		/// <summary>Concatenates the invocation lists of the specified multicast (combinable) delegate and the current multicast (combinable) delegate.</summary>
		/// <returns>A new multicast (combinable) delegate with an invocation list that concatenates the invocation list of the current multicast (combinable) delegate and the invocation list of <paramref name="d" />, or the current multicast (combinable) delegate if <paramref name="d" /> is null.</returns>
		/// <param name="d">The multicast (combinable) delegate whose invocation list to append to the end of the invocation list of the current multicast (combinable) delegate. </param>
		/// <exception cref="T:System.MulticastNotSupportedException">Always thrown. </exception>
		protected virtual Delegate CombineImpl(Delegate d)
		{
			throw new MulticastNotSupportedException(string.Empty);
		}

		/// <summary>Removes the last occurrence of the invocation list of a delegate from the invocation list of another delegate.</summary>
		/// <returns>A new delegate with an invocation list formed by taking the invocation list of <paramref name="source" /> and removing the last occurrence of the invocation list of <paramref name="value" />, if the invocation list of <paramref name="value" /> is found within the invocation list of <paramref name="source" />. Returns <paramref name="source" /> if <paramref name="value" /> is null or if the invocation list of <paramref name="value" /> is not found within the invocation list of <paramref name="source" />. Returns a null reference if the invocation list of <paramref name="value" /> is equal to the invocation list of <paramref name="source" /> or if <paramref name="source" /> is a null reference.</returns>
		/// <param name="source">The delegate from which to remove the invocation list of <paramref name="value" />. </param>
		/// <param name="value">The delegate that supplies the invocation list to remove from the invocation list of <paramref name="source" />. </param>
		/// <exception cref="T:System.MemberAccessException">The caller does not have access to the method represented by the delegate (for example, if the method is private). </exception>
		/// <exception cref="T:System.ArgumentException">The delegate types do not match.</exception>
		/// <filterpriority>1</filterpriority>
		public static Delegate Remove(Delegate source, Delegate value)
		{
			if (source == null)
			{
				return null;
			}
			return source.RemoveImpl(value);
		}

		/// <summary>Removes the invocation list of a delegate from the invocation list of another delegate.</summary>
		/// <returns>A new delegate with an invocation list formed by taking the invocation list of the current delegate and removing the invocation list of <paramref name="value" />, if the invocation list of <paramref name="value" /> is found within the current delegate's invocation list. Returns the current delegate if <paramref name="value" /> is null or if the invocation list of <paramref name="value" /> is not found within the current delegate's invocation list. Returns null if the invocation list of <paramref name="value" /> is equal to the current delegate's invocation list.</returns>
		/// <param name="d">The delegate that supplies the invocation list to remove from the invocation list of the current delegate. </param>
		/// <exception cref="T:System.MemberAccessException">The caller does not have access to the method represented by the delegate (for example, if the method is private). </exception>
		protected virtual Delegate RemoveImpl(Delegate d)
		{
			if (this.Equals(d))
			{
				return null;
			}
			return this;
		}

		/// <summary>Removes all occurrences of the invocation list of a delegate from the invocation list of another delegate.</summary>
		/// <returns>A new delegate with an invocation list formed by taking the invocation list of <paramref name="source" /> and removing all occurrences of the invocation list of <paramref name="value" />, if the invocation list of <paramref name="value" /> is found within the invocation list of <paramref name="source" />. Returns <paramref name="source" /> if <paramref name="value" /> is null or if the invocation list of <paramref name="value" /> is not found within the invocation list of <paramref name="source" />. Returns a null reference if the invocation list of <paramref name="value" /> is equal to the invocation list of <paramref name="source" />, if <paramref name="source" /> contains only a series of invocation lists that are equal to the invocation list of <paramref name="value" />, or if <paramref name="source" /> is a null reference.</returns>
		/// <param name="source">The delegate from which to remove the invocation list of <paramref name="value" />. </param>
		/// <param name="value">The delegate that supplies the invocation list to remove from the invocation list of <paramref name="source" />. </param>
		/// <exception cref="T:System.MemberAccessException">The caller does not have access to the method represented by the delegate (for example, if the method is private). </exception>
		/// <exception cref="T:System.ArgumentException">The delegate types do not match.</exception>
		/// <filterpriority>1</filterpriority>
		public static Delegate RemoveAll(Delegate source, Delegate value)
		{
			Delegate @delegate = source;
			while ((source = Delegate.Remove(source, value)) != @delegate)
			{
				@delegate = source;
			}
			return @delegate;
		}

		/// <summary>Determines whether the specified delegates are equal.</summary>
		/// <returns>true if <paramref name="d1" /> is equal to <paramref name="d2" />; otherwise, false.</returns>
		/// <param name="d1">The first delegate to compare. </param>
		/// <param name="d2">The second delegate to compare. </param>
		/// <filterpriority>3</filterpriority>
		public static bool operator ==(Delegate d1, Delegate d2)
		{
			if (d1 == null)
			{
				return d2 == null;
			}
			return d2 != null && d1.Equals(d2);
		}

		/// <summary>Determines whether the specified delegates are not equal.</summary>
		/// <returns>true if <paramref name="d1" /> is not equal to <paramref name="d2" />; otherwise, false.</returns>
		/// <param name="d1">The first delegate to compare. </param>
		/// <param name="d2">The second delegate to compare. </param>
		/// <filterpriority>3</filterpriority>
		public static bool operator !=(Delegate d1, Delegate d2)
		{
			return !(d1 == d2);
		}
	}
}
