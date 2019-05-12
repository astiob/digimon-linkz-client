using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	/// <summary>Defines and represents a field. This class cannot be inherited.</summary>
	[ClassInterface(ClassInterfaceType.None)]
	[ComDefaultInterface(typeof(_FieldBuilder))]
	[ComVisible(true)]
	public sealed class FieldBuilder : FieldInfo, _FieldBuilder
	{
		private FieldAttributes attrs;

		private Type type;

		private string name;

		private object def_value;

		private int offset;

		private int table_idx;

		internal TypeBuilder typeb;

		private byte[] rva_data;

		private CustomAttributeBuilder[] cattrs;

		private UnmanagedMarshal marshal_info;

		private RuntimeFieldHandle handle;

		private Type[] modReq;

		private Type[] modOpt;

		internal FieldBuilder(TypeBuilder tb, string fieldName, Type type, FieldAttributes attributes, Type[] modReq, Type[] modOpt)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			this.attrs = attributes;
			this.name = fieldName;
			this.type = type;
			this.modReq = modReq;
			this.modOpt = modOpt;
			this.offset = -1;
			this.typeb = tb;
			this.table_idx = tb.get_next_table_index(this, 4, true);
		}

		/// <summary>Maps a set of names to a corresponding set of dispatch identifiers.</summary>
		/// <param name="riid">Reserved for future use. Must be IID_NULL.</param>
		/// <param name="rgszNames">Passed-in array of names to be mapped.</param>
		/// <param name="cNames">Count of the names to be mapped.</param>
		/// <param name="lcid">The locale context in which to interpret the names.</param>
		/// <param name="rgDispId">Caller-allocated array which receives the IDs corresponding to the names.</param>
		/// <exception cref="T:System.NotImplementedException">The method is called late-bound using the COM IDispatch interface.</exception>
		void _FieldBuilder.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		/// <summary>Retrieves the type information for an object, which can then be used to get the type information for an interface.</summary>
		/// <param name="iTInfo">The type information to return.</param>
		/// <param name="lcid">The locale identifier for the type information.</param>
		/// <param name="ppTInfo">Receives a pointer to the requested type information object.</param>
		/// <exception cref="T:System.NotImplementedException">The method is called late-bound using the COM IDispatch interface.</exception>
		void _FieldBuilder.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		/// <summary>Retrieves the number of type information interfaces that an object provides (either 0 or 1).</summary>
		/// <param name="pcTInfo">Points to a location that receives the number of type information interfaces provided by the object.</param>
		/// <exception cref="T:System.NotImplementedException">The method is called late-bound using the COM IDispatch interface.</exception>
		void _FieldBuilder.GetTypeInfoCount(out uint pcTInfo)
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
		void _FieldBuilder.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		/// <summary>Indicates the attributes of this field. This property is read-only.</summary>
		/// <returns>The attributes of this field.</returns>
		public override FieldAttributes Attributes
		{
			get
			{
				return this.attrs;
			}
		}

		/// <summary>Indicates a reference to the <see cref="T:System.Type" /> object for the type that declares this field. This property is read-only.</summary>
		/// <returns>A reference to the <see cref="T:System.Type" /> object for the type that declares this field.</returns>
		public override Type DeclaringType
		{
			get
			{
				return this.typeb;
			}
		}

		/// <summary>Indicates the internal metadata handle for this field. This property is read-only.</summary>
		/// <returns>The internal metadata handle for this field.</returns>
		/// <exception cref="T:System.NotSupportedException">This method is not supported. </exception>
		public override RuntimeFieldHandle FieldHandle
		{
			get
			{
				throw this.CreateNotSupportedException();
			}
		}

		/// <summary>Indicates the <see cref="T:System.Type" /> object that represents the type of this field. This property is read-only.</summary>
		/// <returns>The <see cref="T:System.Type" /> object that represents the type of this field.</returns>
		public override Type FieldType
		{
			get
			{
				return this.type;
			}
		}

		/// <summary>Indicates the name of this field. This property is read-only.</summary>
		/// <returns>A <see cref="T:System.String" /> containing the name of this field.</returns>
		public override string Name
		{
			get
			{
				return this.name;
			}
		}

		/// <summary>Indicates the reference to the <see cref="T:System.Type" /> object from which this object was obtained. This property is read-only.</summary>
		/// <returns>A reference to the <see cref="T:System.Type" /> object from which this instance was obtained.</returns>
		public override Type ReflectedType
		{
			get
			{
				return this.typeb;
			}
		}

		/// <summary>Returns all the custom attributes defined for this field.</summary>
		/// <returns>An array of type <see cref="T:System.Object" /> representing all the custom attributes of the constructor represented by this <see cref="T:System.Reflection.Emit.FieldBuilder" /> instance.</returns>
		/// <param name="inherit">Controls inheritance of custom attributes from base classes. </param>
		/// <exception cref="T:System.NotSupportedException">This method is not supported. </exception>
		public override object[] GetCustomAttributes(bool inherit)
		{
			if (this.typeb.is_created)
			{
				return MonoCustomAttrs.GetCustomAttributes(this, inherit);
			}
			throw this.CreateNotSupportedException();
		}

		/// <summary>Returns all the custom attributes defined for this field identified by the given type.</summary>
		/// <returns>An array of type <see cref="T:System.Object" /> representing all the custom attributes of the constructor represented by this <see cref="T:System.Reflection.Emit.FieldBuilder" /> instance.</returns>
		/// <param name="attributeType">The custom attribute type. </param>
		/// <param name="inherit">Controls inheritance of custom attributes from base classes. </param>
		/// <exception cref="T:System.NotSupportedException">This method is not supported. </exception>
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			if (this.typeb.is_created)
			{
				return MonoCustomAttrs.GetCustomAttributes(this, attributeType, inherit);
			}
			throw this.CreateNotSupportedException();
		}

		/// <summary>Returns the token representing this field.</summary>
		/// <returns>Returns the <see cref="T:System.Reflection.Emit.FieldToken" /> object that represents the token for this field.</returns>
		public FieldToken GetToken()
		{
			return new FieldToken(this.MetadataToken);
		}

		/// <summary>Retrieves the value of the field supported by the given object.</summary>
		/// <returns>An <see cref="T:System.Object" /> containing the value of the field reflected by this instance.</returns>
		/// <param name="obj">The object on which to access the field. </param>
		/// <exception cref="T:System.NotSupportedException">This method is not supported. </exception>
		public override object GetValue(object obj)
		{
			throw this.CreateNotSupportedException();
		}

		/// <summary>Indicates whether an attribute having the specified type is defined on a field.</summary>
		/// <returns>true if one or more instance of <paramref name="attributeType" /> is defined on this field; otherwise, false.</returns>
		/// <param name="attributeType">The type of the attribute. </param>
		/// <param name="inherit">Controls inheritance of custom attributes from base classes. </param>
		/// <exception cref="T:System.NotSupportedException">This method is not currently supported. Retrieve the field using <see cref="M:System.Type.GetField(System.String,System.Reflection.BindingFlags)" /> and call <see cref="M:System.Reflection.MemberInfo.IsDefined(System.Type,System.Boolean)" /> on the returned <see cref="T:System.Reflection.FieldInfo" />. </exception>
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			throw this.CreateNotSupportedException();
		}

		internal override int GetFieldOffset()
		{
			return 0;
		}

		internal void SetRVAData(byte[] data)
		{
			this.rva_data = (byte[])data.Clone();
		}

		/// <summary>Sets the default value of this field.</summary>
		/// <param name="defaultValue">The new default value for this field. </param>
		/// <exception cref="T:System.InvalidOperationException">The containing type has been created using <see cref="M:System.Reflection.Emit.TypeBuilder.CreateType" />. </exception>
		/// <exception cref="T:System.ArgumentException">The field is not one of the supported types.-or-The type of <paramref name="defaultValue" /> does not match the type of the field.-or-The field is of type <see cref="T:System.Object" /> or other reference type, and <paramref name="defaultValue" /> is not null.</exception>
		public void SetConstant(object defaultValue)
		{
			this.RejectIfCreated();
			this.def_value = defaultValue;
		}

		/// <summary>Sets a custom attribute using a custom attribute builder.</summary>
		/// <param name="customBuilder">An instance of a helper class to define the custom attribute. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="con" /> is null. </exception>
		/// <exception cref="T:System.InvalidOperationException">The parent type of this field is complete. </exception>
		public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
		{
			this.RejectIfCreated();
			string fullName = customBuilder.Ctor.ReflectedType.FullName;
			if (fullName == "System.Runtime.InteropServices.FieldOffsetAttribute")
			{
				byte[] data = customBuilder.Data;
				this.offset = (int)data[2];
				this.offset |= (int)data[3] << 8;
				this.offset |= (int)data[4] << 16;
				this.offset |= (int)data[5] << 24;
				return;
			}
			if (fullName == "System.NonSerializedAttribute")
			{
				this.attrs |= FieldAttributes.NotSerialized;
				return;
			}
			if (fullName == "System.Runtime.CompilerServices.SpecialNameAttribute")
			{
				this.attrs |= FieldAttributes.SpecialName;
				return;
			}
			if (fullName == "System.Runtime.InteropServices.MarshalAsAttribute")
			{
				this.attrs |= FieldAttributes.HasFieldMarshal;
				this.marshal_info = CustomAttributeBuilder.get_umarshal(customBuilder, true);
				return;
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

		/// <summary>Sets a custom attribute using a specified custom attribute blob.</summary>
		/// <param name="con">The constructor for the custom attribute. </param>
		/// <param name="binaryAttribute">A byte blob representing the attributes. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="con" /> or <paramref name="binaryAttribute" /> is null. </exception>
		/// <exception cref="T:System.InvalidOperationException">The parent type of this field is complete. </exception>
		[ComVisible(true)]
		public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
		{
			this.RejectIfCreated();
			this.SetCustomAttribute(new CustomAttributeBuilder(con, binaryAttribute));
		}

		/// <summary>Describes the native marshaling of the field.</summary>
		/// <param name="unmanagedMarshal">A descriptor specifying the native marshalling of this field. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="unmanagedMarshal" /> is null. </exception>
		/// <exception cref="T:System.InvalidOperationException">The containing type has been created using <see cref="M:System.Reflection.Emit.TypeBuilder.CreateType" />. </exception>
		[Obsolete("An alternate API is available: Emit the MarshalAs custom attribute instead.")]
		public void SetMarshal(UnmanagedMarshal unmanagedMarshal)
		{
			this.RejectIfCreated();
			this.marshal_info = unmanagedMarshal;
			this.attrs |= FieldAttributes.HasFieldMarshal;
		}

		/// <summary>Specifies the field layout.</summary>
		/// <param name="iOffset">The offset of the field within the type containing this field. </param>
		/// <exception cref="T:System.InvalidOperationException">The containing type has been created using <see cref="M:System.Reflection.Emit.TypeBuilder.CreateType" />. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="iOffset" /> is less than zero.</exception>
		public void SetOffset(int iOffset)
		{
			this.RejectIfCreated();
			this.offset = iOffset;
		}

		/// <summary>Sets the value of the field supported by the given object.</summary>
		/// <param name="obj">The object on which to access the field. </param>
		/// <param name="val">The value to assign to the field. </param>
		/// <param name="invokeAttr">A member of IBinder that specifies the type of binding that is desired (for example, IBinder.CreateInstance, IBinder.ExactBinding). </param>
		/// <param name="binder">A set of properties and enabling for binding, coercion of argument types, and invocation of members using reflection. If binder is null, then IBinder.DefaultBinding is used. </param>
		/// <param name="culture">The software preferences of a particular culture. </param>
		/// <exception cref="T:System.NotSupportedException">This method is not supported. </exception>
		public override void SetValue(object obj, object val, BindingFlags invokeAttr, Binder binder, CultureInfo culture)
		{
			throw this.CreateNotSupportedException();
		}

		internal override UnmanagedMarshal UMarshal
		{
			get
			{
				return this.marshal_info;
			}
		}

		private Exception CreateNotSupportedException()
		{
			return new NotSupportedException("The invoked member is not supported in a dynamic module.");
		}

		private void RejectIfCreated()
		{
			if (this.typeb.is_created)
			{
				throw new InvalidOperationException("Unable to change after type has been created.");
			}
		}

		/// <summary>Gets the module in which the type that contains this field is being defined.</summary>
		/// <returns>A <see cref="T:System.Reflection.Module" /> that represents the dynamic module in which this field is being defined.</returns>
		public override Module Module
		{
			get
			{
				return base.Module;
			}
		}
	}
}
