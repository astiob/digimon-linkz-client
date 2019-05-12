using System;
using System.ComponentModel.Design;

namespace System.ComponentModel
{
	/// <summary>Specifies the class used to implement design-time services for a component.</summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
	public sealed class DesignerAttribute : Attribute
	{
		private string name;

		private string basetypename;

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.DesignerAttribute" /> class using the name of the type that provides design-time services.</summary>
		/// <param name="designerTypeName">The concatenation of the fully qualified name of the type that provides design-time services for the component this attribute is bound to, and the name of the assembly this type resides in. </param>
		public DesignerAttribute(string designerTypeName)
		{
			if (designerTypeName == null)
			{
				throw new NullReferenceException();
			}
			this.name = designerTypeName;
			this.basetypename = typeof(System.ComponentModel.Design.IDesigner).FullName;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.DesignerAttribute" /> class using the type that provides design-time services.</summary>
		/// <param name="designerType">A <see cref="T:System.Type" /> that represents the class that provides design-time services for the component this attribute is bound to. </param>
		public DesignerAttribute(Type designerType) : this(designerType.AssemblyQualifiedName)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.DesignerAttribute" /> class, using the name of the designer class and the base class for the designer.</summary>
		/// <param name="designerTypeName">The concatenation of the fully qualified name of the type that provides design-time services for the component this attribute is bound to, and the name of the assembly this type resides in. </param>
		/// <param name="designerBaseType">A <see cref="T:System.Type" /> that represents the base class to associate with the <paramref name="designerTypeName" />. </param>
		public DesignerAttribute(string designerTypeName, Type designerBaseType) : this(designerTypeName, designerBaseType.AssemblyQualifiedName)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.DesignerAttribute" /> class using the types of the designer and designer base class.</summary>
		/// <param name="designerType">A <see cref="T:System.Type" /> that represents the class that provides design-time services for the component this attribute is bound to. </param>
		/// <param name="designerBaseType">A <see cref="T:System.Type" /> that represents the base class to associate with the <paramref name="designerType" />. </param>
		public DesignerAttribute(Type designerType, Type designerBaseType) : this(designerType.AssemblyQualifiedName, designerBaseType.AssemblyQualifiedName)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.DesignerAttribute" /> class using the designer type and the base class for the designer.</summary>
		/// <param name="designerTypeName">The concatenation of the fully qualified name of the type that provides design-time services for the component this attribute is bound to, and the name of the assembly this type resides in. </param>
		/// <param name="designerBaseTypeName">The fully qualified name of the base class to associate with the designer class. </param>
		public DesignerAttribute(string designerTypeName, string designerBaseTypeName)
		{
			if (designerTypeName == null)
			{
				throw new NullReferenceException();
			}
			this.name = designerTypeName;
			this.basetypename = designerBaseTypeName;
		}

		/// <summary>Gets the name of the base type of this designer.</summary>
		/// <returns>The name of the base type of this designer.</returns>
		public string DesignerBaseTypeName
		{
			get
			{
				return this.basetypename;
			}
		}

		/// <summary>Gets the name of the designer type associated with this designer attribute.</summary>
		/// <returns>The name of the designer type associated with this designer attribute.</returns>
		public string DesignerTypeName
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
				string text = this.basetypename;
				int num = text.IndexOf(',');
				if (num != -1)
				{
					text = text.Substring(0, num);
				}
				return base.GetType().ToString() + text;
			}
		}

		/// <summary>Returns whether the value of the given object is equal to the current <see cref="T:System.ComponentModel.DesignerAttribute" />.</summary>
		/// <returns>true if the value of the given object is equal to that of the current; otherwise, false.</returns>
		/// <param name="obj">The object to test the value equality of. </param>
		public override bool Equals(object obj)
		{
			return obj is DesignerAttribute && ((DesignerAttribute)obj).DesignerBaseTypeName.Equals(this.basetypename) && ((DesignerAttribute)obj).DesignerTypeName.Equals(this.name);
		}

		public override int GetHashCode()
		{
			return (this.name + this.basetypename).GetHashCode();
		}
	}
}
