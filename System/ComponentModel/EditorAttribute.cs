using System;

namespace System.ComponentModel
{
	/// <summary>Specifies the editor to use to change a property. This class cannot be inherited.</summary>
	[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
	public sealed class EditorAttribute : Attribute
	{
		private string name;

		private string basename;

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.EditorAttribute" /> class with the default editor, which is no editor.</summary>
		public EditorAttribute()
		{
			this.name = string.Empty;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.EditorAttribute" /> class with the type name and base type name of the editor.</summary>
		/// <param name="typeName">The fully qualified type name of the editor. </param>
		/// <param name="baseTypeName">The fully qualified type name of the base class or interface to use as a lookup key for the editor. This class must be or derive from <see cref="T:System.Drawing.Design.UITypeEditor" />. </param>
		public EditorAttribute(string typeName, string baseTypeName)
		{
			this.name = typeName;
			this.basename = baseTypeName;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.EditorAttribute" /> class with the type name and the base type.</summary>
		/// <param name="typeName">The fully qualified type name of the editor. </param>
		/// <param name="baseType">The <see cref="T:System.Type" /> of the base class or interface to use as a lookup key for the editor. This class must be or derive from <see cref="T:System.Drawing.Design.UITypeEditor" />. </param>
		public EditorAttribute(string typeName, Type baseType) : this(typeName, baseType.AssemblyQualifiedName)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.EditorAttribute" /> class with the type and the base type.</summary>
		/// <param name="type">A <see cref="T:System.Type" /> that represents the type of the editor. </param>
		/// <param name="baseType">The <see cref="T:System.Type" /> of the base class or interface to use as a lookup key for the editor. This class must be or derive from <see cref="T:System.Drawing.Design.UITypeEditor" />. </param>
		public EditorAttribute(Type type, Type baseType) : this(type.AssemblyQualifiedName, baseType.AssemblyQualifiedName)
		{
		}

		/// <summary>Gets the name of the base class or interface serving as a lookup key for this editor.</summary>
		/// <returns>The name of the base class or interface serving as a lookup key for this editor.</returns>
		public string EditorBaseTypeName
		{
			get
			{
				return this.basename;
			}
		}

		/// <summary>Gets the name of the editor class in the <see cref="P:System.Type.AssemblyQualifiedName" /> format.</summary>
		/// <returns>The name of the editor class in the <see cref="P:System.Type.AssemblyQualifiedName" /> format.</returns>
		public string EditorTypeName
		{
			get
			{
				return this.name;
			}
		}

		/// <summary>Gets a unique ID for this attribute type.</summary>
		/// <returns>A unique ID for this attribute type.</returns>
		public override object TypeId
		{
			get
			{
				return base.GetType();
			}
		}

		/// <summary>Returns whether the value of the given object is equal to the current <see cref="T:System.ComponentModel.EditorAttribute" />.</summary>
		/// <returns>true if the value of the given object is equal to that of the current object; otherwise, false.</returns>
		/// <param name="obj">The object to test the value equality of. </param>
		public override bool Equals(object obj)
		{
			return obj is EditorAttribute && ((EditorAttribute)obj).EditorBaseTypeName.Equals(this.basename) && ((EditorAttribute)obj).EditorTypeName.Equals(this.name);
		}

		public override int GetHashCode()
		{
			return (this.name + this.basename).GetHashCode();
		}
	}
}
