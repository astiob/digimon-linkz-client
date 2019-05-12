using System;
using System.Collections;
using System.Reflection;

namespace System.ComponentModel.Design.Serialization
{
	/// <summary>Provides the information necessary to create an instance of an object. This class cannot be inherited.</summary>
	public sealed class InstanceDescriptor
	{
		private MemberInfo member;

		private ICollection arguments;

		private bool isComplete;

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.Design.Serialization.InstanceDescriptor" /> class using the specified member information and arguments.</summary>
		/// <param name="member">The member information for the descriptor. This can be a <see cref="T:System.Reflection.MethodInfo" />, <see cref="T:System.Reflection.ConstructorInfo" />, <see cref="T:System.Reflection.FieldInfo" />, or <see cref="T:System.Reflection.PropertyInfo" />. If this is a <see cref="T:System.Reflection.MethodInfo" />, <see cref="T:System.Reflection.FieldInfo" />, or <see cref="T:System.Reflection.PropertyInfo" />, it must represent a static member. </param>
		/// <param name="arguments">The collection of arguments to pass to the member. This parameter can be null or an empty collection if there are no arguments. The collection can also consist of other instances of <see cref="T:System.ComponentModel.Design.Serialization.InstanceDescriptor" />. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="member" /> is of type <see cref="T:System.Reflection.MethodInfo" />, <see cref="T:System.Reflection.FieldInfo" />, or <see cref="T:System.Reflection.PropertyInfo" />, and it does not represent a static member.<paramref name="member" /> is of type <see cref="T:System.Reflection.PropertyInfo" /> and is not readable.<paramref name="member" /> is of type <see cref="T:System.Reflection.MethodInfo" /> or <see cref="T:System.Reflection.ConstructorInfo" />, and the number of arguments in <paramref name="arguments" /> does not match the signature of <paramref name="member." /><paramref name="member" /> is of type <see cref="T:System.Reflection.ConstructorInfo" /> and represents a static member.<paramref name="member" /> is of type <see cref="T:System.Reflection.FieldInfo" />, and the number of arguments in <paramref name="arguments" /> is not zero. </exception>
		public InstanceDescriptor(MemberInfo member, ICollection arguments) : this(member, arguments, true)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.Design.Serialization.InstanceDescriptor" /> class using the specified member information, arguments, and value indicating whether the specified information completely describes the instance.</summary>
		/// <param name="member">The member information for the descriptor. This can be a <see cref="T:System.Reflection.MethodInfo" />, <see cref="T:System.Reflection.ConstructorInfo" />, <see cref="T:System.Reflection.FieldInfo" />, or <see cref="T:System.Reflection.PropertyInfo" />. If this is a <see cref="T:System.Reflection.MethodInfo" />, <see cref="T:System.Reflection.FieldInfo" />, or <see cref="T:System.Reflection.PropertyInfo" />, it must represent a static member. </param>
		/// <param name="arguments">The collection of arguments to pass to the member. This parameter can be null or an empty collection if there are no arguments. The collection can also consist of other instances of <see cref="T:System.ComponentModel.Design.Serialization.InstanceDescriptor" />. </param>
		/// <param name="isComplete">true if the specified information completely describes the instance; otherwise, false. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="member" /> is of type <see cref="T:System.Reflection.MethodInfo" />, <see cref="T:System.Reflection.FieldInfo" />, or <see cref="T:System.Reflection.PropertyInfo" />, and it does not represent a static member<paramref name="member" /> is of type <see cref="T:System.Reflection.PropertyInfo" /> and is not readable.<paramref name="member" /> is of type <see cref="T:System.Reflection.MethodInfo" /> or <see cref="T:System.Reflection.ConstructorInfo" /> and the number of arguments in <paramref name="arguments" /> does not match the signature of <paramref name="member" />.<paramref name="member" /> is of type <see cref="T:System.Reflection.ConstructorInfo" /> and represents a static member<paramref name="member" /> is of type <see cref="T:System.Reflection.FieldInfo" />, and the number of arguments in <paramref name="arguments" /> is not zero.</exception>
		public InstanceDescriptor(MemberInfo member, ICollection arguments, bool isComplete)
		{
			this.isComplete = isComplete;
			this.ValidateMember(member, arguments);
			this.member = member;
			this.arguments = arguments;
		}

		private void ValidateMember(MemberInfo member, ICollection arguments)
		{
			if (member == null)
			{
				return;
			}
			MemberTypes memberType = member.MemberType;
			switch (memberType)
			{
			case MemberTypes.Constructor:
			{
				ConstructorInfo constructorInfo = (ConstructorInfo)member;
				if (arguments == null && constructorInfo.GetParameters().Length != 0)
				{
					throw new ArgumentException("Invalid number of arguments for this constructor");
				}
				if (arguments.Count != constructorInfo.GetParameters().Length)
				{
					throw new ArgumentException("Invalid number of arguments for this constructor");
				}
				break;
			}
			default:
				if (memberType != MemberTypes.Method)
				{
					if (memberType == MemberTypes.Property)
					{
						PropertyInfo propertyInfo = (PropertyInfo)member;
						if (!propertyInfo.CanRead)
						{
							throw new ArgumentException("Parameter must be readable");
						}
						MethodInfo getMethod = propertyInfo.GetGetMethod();
						if (!getMethod.IsStatic)
						{
							throw new ArgumentException("Parameter must be static");
						}
					}
				}
				else
				{
					MethodInfo methodInfo = (MethodInfo)member;
					if (!methodInfo.IsStatic)
					{
						throw new ArgumentException("InstanceDescriptor only describes static (VB.Net: shared) members", "member");
					}
					if (arguments == null && methodInfo.GetParameters().Length != 0)
					{
						throw new ArgumentException("Invalid number of arguments for this method", "arguments");
					}
					if (arguments.Count != methodInfo.GetParameters().Length)
					{
						throw new ArgumentException("Invalid number of arguments for this method");
					}
				}
				break;
			case MemberTypes.Field:
			{
				FieldInfo fieldInfo = (FieldInfo)member;
				if (!fieldInfo.IsStatic)
				{
					throw new ArgumentException("Parameter must be static");
				}
				if (arguments != null && arguments.Count != 0)
				{
					throw new ArgumentException("Field members do not take any arguments");
				}
				break;
			}
			}
		}

		/// <summary>Gets the collection of arguments that can be used to reconstruct an instance of the object that this instance descriptor represents.</summary>
		/// <returns>An <see cref="T:System.Collections.ICollection" /> of arguments that can be used to create the object.</returns>
		public ICollection Arguments
		{
			get
			{
				if (this.arguments == null)
				{
					return new object[0];
				}
				return this.arguments;
			}
		}

		/// <summary>Gets a value indicating whether the contents of this <see cref="T:System.ComponentModel.Design.Serialization.InstanceDescriptor" /> completely identify the instance.</summary>
		/// <returns>true if the instance is completely described; otherwise, false.</returns>
		public bool IsComplete
		{
			get
			{
				return this.isComplete;
			}
		}

		/// <summary>Gets the member information that describes the instance this descriptor is associated with.</summary>
		/// <returns>A <see cref="T:System.Reflection.MemberInfo" /> that describes the instance that this object is associated with.</returns>
		public MemberInfo MemberInfo
		{
			get
			{
				return this.member;
			}
		}

		/// <summary>Invokes this instance descriptor and returns the object the descriptor describes.</summary>
		/// <returns>The object this instance descriptor describes.</returns>
		public object Invoke()
		{
			if (this.member == null)
			{
				return null;
			}
			object[] array;
			if (this.arguments == null)
			{
				array = new object[0];
			}
			else
			{
				array = new object[this.arguments.Count];
				this.arguments.CopyTo(array, 0);
			}
			MemberTypes memberType = this.member.MemberType;
			switch (memberType)
			{
			case MemberTypes.Constructor:
			{
				ConstructorInfo constructorInfo = (ConstructorInfo)this.member;
				return constructorInfo.Invoke(array);
			}
			default:
			{
				if (memberType == MemberTypes.Method)
				{
					MethodInfo methodInfo = (MethodInfo)this.member;
					return methodInfo.Invoke(null, array);
				}
				if (memberType != MemberTypes.Property)
				{
					return null;
				}
				PropertyInfo propertyInfo = (PropertyInfo)this.member;
				return propertyInfo.GetValue(null, array);
			}
			case MemberTypes.Field:
			{
				FieldInfo fieldInfo = (FieldInfo)this.member;
				return fieldInfo.GetValue(null);
			}
			}
		}
	}
}
