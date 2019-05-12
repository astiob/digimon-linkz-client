using System;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	/// <summary>Discovers the attributes of a parameter and provides access to parameter metadata.</summary>
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	[ComDefaultInterface(typeof(_ParameterInfo))]
	[Serializable]
	public class ParameterInfo : ICustomAttributeProvider, _ParameterInfo
	{
		/// <summary>The Type of the parameter.</summary>
		protected Type ClassImpl;

		/// <summary>The default value of the parameter.</summary>
		protected object DefaultValueImpl;

		/// <summary>The member in which the field is implemented.</summary>
		protected MemberInfo MemberImpl;

		/// <summary>The name of the parameter.</summary>
		protected string NameImpl;

		/// <summary>The zero-based position of the parameter in the parameter list.</summary>
		protected int PositionImpl;

		/// <summary>The attributes of the parameter.</summary>
		protected ParameterAttributes AttrsImpl;

		private UnmanagedMarshal marshalAs;

		/// <summary>Initializes a new instance of the ParameterInfo class.</summary>
		protected ParameterInfo()
		{
		}

		internal ParameterInfo(ParameterBuilder pb, Type type, MemberInfo member, int position)
		{
			this.ClassImpl = type;
			this.MemberImpl = member;
			if (pb != null)
			{
				this.NameImpl = pb.Name;
				this.PositionImpl = pb.Position - 1;
				this.AttrsImpl = (ParameterAttributes)pb.Attributes;
			}
			else
			{
				this.NameImpl = null;
				this.PositionImpl = position - 1;
				this.AttrsImpl = ParameterAttributes.None;
			}
		}

		internal ParameterInfo(ParameterInfo pinfo, MemberInfo member)
		{
			this.ClassImpl = pinfo.ParameterType;
			this.MemberImpl = member;
			this.NameImpl = pinfo.Name;
			this.PositionImpl = pinfo.Position;
			this.AttrsImpl = pinfo.Attributes;
		}

		internal ParameterInfo(Type type, MemberInfo member, UnmanagedMarshal marshalAs)
		{
			this.ClassImpl = type;
			this.MemberImpl = member;
			this.NameImpl = string.Empty;
			this.PositionImpl = -1;
			this.AttrsImpl = ParameterAttributes.Retval;
			this.marshalAs = marshalAs;
		}

		/// <summary>Maps a set of names to a corresponding set of dispatch identifiers.</summary>
		/// <param name="riid">Reserved for future use. Must be IID_NULL.</param>
		/// <param name="rgszNames">Passed-in array of names to be mapped.</param>
		/// <param name="cNames">Count of the names to be mapped.</param>
		/// <param name="lcid">The locale context in which to interpret the names.</param>
		/// <param name="rgDispId">Caller-allocated array which receives the IDs corresponding to the names.</param>
		/// <exception cref="T:System.NotImplementedException">Late-bound access using the COM IDispatch interface is not supported.</exception>
		void _ParameterInfo.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		/// <summary>Retrieves the type information for an object, which can then be used to get the type information for an interface.</summary>
		/// <param name="iTInfo">The type information to return.</param>
		/// <param name="lcid">The locale identifier for the type information.</param>
		/// <param name="ppTInfo">Receives a pointer to the requested type information object.</param>
		/// <exception cref="T:System.NotImplementedException">Late-bound access using the COM IDispatch interface is not supported.</exception>
		void _ParameterInfo.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		/// <summary>Retrieves the number of type information interfaces that an object provides (either 0 or 1).</summary>
		/// <param name="pcTInfo">Points to a location that receives the number of type information interfaces provided by the object.</param>
		/// <exception cref="T:System.NotImplementedException">Late-bound access using the COM IDispatch interface is not supported.</exception>
		void _ParameterInfo.GetTypeInfoCount(out uint pcTInfo)
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
		/// <exception cref="T:System.NotImplementedException">Late-bound access using the COM IDispatch interface is not supported.</exception>
		void _ParameterInfo.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		/// <summary>Gets the parameter type and name represented as a string.</summary>
		/// <returns>A string containing the type and the name of the parameter.</returns>
		public override string ToString()
		{
			Type type = this.ClassImpl;
			while (type.HasElementType)
			{
				type = type.GetElementType();
			}
			bool flag = type.IsPrimitive || this.ClassImpl == typeof(void) || this.ClassImpl.Namespace == this.MemberImpl.DeclaringType.Namespace;
			string text = (!flag) ? this.ClassImpl.FullName : this.ClassImpl.Name;
			if (!this.IsRetval)
			{
				text += ' ';
				text += this.NameImpl;
			}
			return text;
		}

		/// <summary>Gets the Type of this parameter.</summary>
		/// <returns>The Type object that represents the Type of this parameter.</returns>
		public virtual Type ParameterType
		{
			get
			{
				return this.ClassImpl;
			}
		}

		/// <summary>Gets the attributes for this parameter.</summary>
		/// <returns>A ParameterAttributes object representing the attributes for this parameter.</returns>
		public virtual ParameterAttributes Attributes
		{
			get
			{
				return this.AttrsImpl;
			}
		}

		/// <summary>Gets a value indicating the default value if the parameter has a default value.</summary>
		/// <returns>The default value of the parameter, or <see cref="F:System.DBNull.Value" /> if the parameter has no default value.</returns>
		public virtual object DefaultValue
		{
			get
			{
				if (this.ClassImpl == typeof(decimal))
				{
					DecimalConstantAttribute[] array = (DecimalConstantAttribute[])this.GetCustomAttributes(typeof(DecimalConstantAttribute), false);
					if (array.Length > 0)
					{
						return array[0].Value;
					}
				}
				else if (this.ClassImpl == typeof(DateTime))
				{
					DateTimeConstantAttribute[] array2 = (DateTimeConstantAttribute[])this.GetCustomAttributes(typeof(DateTimeConstantAttribute), false);
					if (array2.Length > 0)
					{
						return new DateTime(array2[0].Ticks);
					}
				}
				return this.DefaultValueImpl;
			}
		}

		/// <summary>Gets a value indicating whether this is an input parameter.</summary>
		/// <returns>true if the parameter is an input parameter; otherwise, false.</returns>
		public bool IsIn
		{
			get
			{
				return (this.Attributes & ParameterAttributes.In) != ParameterAttributes.None;
			}
		}

		/// <summary>Gets a value indicating whether this parameter is a locale identifier (lcid).</summary>
		/// <returns>true if the parameter is a locale identifier; otherwise, false.</returns>
		public bool IsLcid
		{
			get
			{
				return (this.Attributes & ParameterAttributes.Lcid) != ParameterAttributes.None;
			}
		}

		/// <summary>Gets a value indicating whether this parameter is optional.</summary>
		/// <returns>true if the parameter is optional; otherwise, false.</returns>
		public bool IsOptional
		{
			get
			{
				return (this.Attributes & ParameterAttributes.Optional) != ParameterAttributes.None;
			}
		}

		/// <summary>Gets a value indicating whether this is an output parameter.</summary>
		/// <returns>true if the parameter is an output parameter; otherwise, false.</returns>
		public bool IsOut
		{
			get
			{
				return (this.Attributes & ParameterAttributes.Out) != ParameterAttributes.None;
			}
		}

		/// <summary>Gets a value indicating whether this is a Retval parameter.</summary>
		/// <returns>true if the parameter is a Retval; otherwise, false.</returns>
		public bool IsRetval
		{
			get
			{
				return (this.Attributes & ParameterAttributes.Retval) != ParameterAttributes.None;
			}
		}

		/// <summary>Gets a value indicating the member in which the parameter is implemented.</summary>
		/// <returns>A MemberInfo object.</returns>
		public virtual MemberInfo Member
		{
			get
			{
				return this.MemberImpl;
			}
		}

		/// <summary>Gets the name of the parameter.</summary>
		/// <returns>A String containing the simple name of this parameter.</returns>
		public virtual string Name
		{
			get
			{
				return this.NameImpl;
			}
		}

		/// <summary>Gets the zero-based position of the parameter in the formal parameter list.</summary>
		/// <returns>An integer representing the position this parameter occupies in the parameter list.</returns>
		public virtual int Position
		{
			get
			{
				return this.PositionImpl;
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int GetMetadataToken();

		/// <summary>Gets a value that identifies this parameter in metadata.</summary>
		/// <returns>A value which, in combination with the module, uniquely identifies this parameter in metadata.</returns>
		public int MetadataToken
		{
			get
			{
				if (this.MemberImpl is PropertyInfo)
				{
					PropertyInfo propertyInfo = (PropertyInfo)this.MemberImpl;
					MethodInfo methodInfo = propertyInfo.GetGetMethod(true);
					if (methodInfo == null)
					{
						methodInfo = propertyInfo.GetSetMethod(true);
					}
					return methodInfo.GetParameters()[this.PositionImpl].MetadataToken;
				}
				if (this.MemberImpl is MethodBase)
				{
					return this.GetMetadataToken();
				}
				throw new ArgumentException("Can't produce MetadataToken for member of type " + this.MemberImpl.GetType());
			}
		}

		/// <summary>Gets all the custom attributes applied to this parameter.</summary>
		/// <returns>An array that contains all the custom attributes applied to this parameter.</returns>
		/// <param name="inherit">This argument is ignored for objects of this type. See Remarks.</param>
		/// <exception cref="T:System.TypeLoadException">A custom attribute type could not be loaded. </exception>
		public virtual object[] GetCustomAttributes(bool inherit)
		{
			return MonoCustomAttrs.GetCustomAttributes(this, inherit);
		}

		/// <summary>Gets the custom attributes of the specified type or its derived types that are applied to this parameter.</summary>
		/// <returns>An array that contains the custom attributes of the specified type or its derived types.</returns>
		/// <param name="attributeType">The type of attribute to search for. Only attributes that are assignable to this type are returned.</param>
		/// <param name="inherit">This argument is ignored for objects of this type. See Remarks.</param>
		/// <exception cref="T:System.ArgumentException">The type must be a type provided by the underlying runtime system.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="attributeType" /> is null.</exception>
		/// <exception cref="T:System.TypeLoadException">A custom attribute type could not be loaded. </exception>
		public virtual object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			return MonoCustomAttrs.GetCustomAttributes(this, attributeType, inherit);
		}

		/// <summary>Determines whether the custom attribute of the specified type or its derived types is applied to this parameter.</summary>
		/// <returns>true if one or more instances of <paramref name="attributeType" /> or its derived types are applied to this parameter; otherwise, false.</returns>
		/// <param name="attributeType">The type of attribute to search for. Only attributes that are assignable to this type are returned.</param>
		/// <param name="inherit">This argument is ignored for objects of this type. See Remarks.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="attributeType" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="attributeType" /> is not a <see cref="T:System.Type" /> object supplied by the common language runtime.</exception>
		public virtual bool IsDefined(Type attributeType, bool inherit)
		{
			return MonoCustomAttrs.IsDefined(this, attributeType, inherit);
		}

		internal object[] GetPseudoCustomAttributes()
		{
			int num = 0;
			if (this.IsIn)
			{
				num++;
			}
			if (this.IsOut)
			{
				num++;
			}
			if (this.IsOptional)
			{
				num++;
			}
			if (this.marshalAs != null)
			{
				num++;
			}
			if (num == 0)
			{
				return null;
			}
			object[] array = new object[num];
			num = 0;
			if (this.IsIn)
			{
				array[num++] = new InAttribute();
			}
			if (this.IsOptional)
			{
				array[num++] = new OptionalAttribute();
			}
			if (this.IsOut)
			{
				array[num++] = new OutAttribute();
			}
			if (this.marshalAs != null)
			{
				array[num++] = this.marshalAs.ToMarshalAsAttribute();
			}
			return array;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Type[] GetTypeModifiers(bool optional);

		/// <summary>Gets the optional custom modifiers of the parameter.</summary>
		/// <returns>An array of <see cref="T:System.Type" /> objects that identify the optional custom modifiers of the current parameter, such as <see cref="T:System.Runtime.CompilerServices.IsConst" /> or <see cref="T:System.Runtime.CompilerServices.IsImplicitlyDereferenced" />.</returns>
		public virtual Type[] GetOptionalCustomModifiers()
		{
			Type[] typeModifiers = this.GetTypeModifiers(true);
			if (typeModifiers == null)
			{
				return Type.EmptyTypes;
			}
			return typeModifiers;
		}

		/// <summary>Gets the required custom modifiers of the parameter.</summary>
		/// <returns>An array of <see cref="T:System.Type" /> objects that identify the required custom modifiers of the current parameter, such as <see cref="T:System.Runtime.CompilerServices.IsConst" /> or <see cref="T:System.Runtime.CompilerServices.IsImplicitlyDereferenced" />.</returns>
		public virtual Type[] GetRequiredCustomModifiers()
		{
			Type[] typeModifiers = this.GetTypeModifiers(false);
			if (typeModifiers == null)
			{
				return Type.EmptyTypes;
			}
			return typeModifiers;
		}

		/// <summary>Gets a value indicating the default value if the parameter has a default value.</summary>
		/// <returns>The default value of the parameter, or <see cref="F:System.DBNull.Value" /> if the parameter has no default value.</returns>
		public virtual object RawDefaultValue
		{
			get
			{
				return this.DefaultValue;
			}
		}
	}
}
