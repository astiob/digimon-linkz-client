using System;

namespace System.ComponentModel
{
	/// <summary>Represents an attribute of a toolbox item.</summary>
	[AttributeUsage(AttributeTargets.All)]
	public class ToolboxItemAttribute : Attribute
	{
		private const string defaultItemType = "System.Drawing.Design.ToolboxItem, System.Drawing, Version=2.0.5.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.ToolboxItemAttribute" /> class and sets the type to the default, <see cref="T:System.Drawing.Design.ToolboxItem" />. This field is read-only.</summary>
		public static readonly ToolboxItemAttribute Default = new ToolboxItemAttribute("System.Drawing.Design.ToolboxItem, System.Drawing, Version=2.0.5.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.ToolboxItemAttribute" /> class and sets the type to null. This field is read-only.</summary>
		public static readonly ToolboxItemAttribute None = new ToolboxItemAttribute(false);

		private Type itemType;

		private string itemTypeName;

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.ToolboxItemAttribute" /> class and specifies whether to use default initialization values.</summary>
		/// <param name="defaultType">true to create a toolbox item attribute for a default type; false to associate no default toolbox item support for this attribute. </param>
		public ToolboxItemAttribute(bool defaultType)
		{
			if (defaultType)
			{
				this.itemTypeName = "System.Drawing.Design.ToolboxItem, System.Drawing, Version=2.0.5.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.ToolboxItemAttribute" /> class using the specified name of the type.</summary>
		/// <param name="toolboxItemTypeName">The names of the type of the toolbox item and of the assembly that contains the type. </param>
		public ToolboxItemAttribute(string toolboxItemName)
		{
			this.itemTypeName = toolboxItemName;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.ToolboxItemAttribute" /> class using the specified type of the toolbox item.</summary>
		/// <param name="toolboxItemType">The type of the toolbox item. </param>
		public ToolboxItemAttribute(Type toolboxItemType)
		{
			this.itemType = toolboxItemType;
		}

		/// <summary>Gets or sets the type of the toolbox item.</summary>
		/// <returns>The type of the toolbox item.</returns>
		/// <exception cref="T:System.ArgumentException">The type cannot be found. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public Type ToolboxItemType
		{
			get
			{
				if (this.itemType == null && this.itemTypeName != null)
				{
					try
					{
						this.itemType = Type.GetType(this.itemTypeName, true);
					}
					catch (Exception innerException)
					{
						throw new ArgumentException("Failed to create ToolboxItem of type: " + this.itemTypeName, innerException);
					}
				}
				return this.itemType;
			}
		}

		/// <summary>Gets or sets the name of the type of the current <see cref="T:System.Drawing.Design.ToolboxItem" />.</summary>
		/// <returns>The fully qualified type name of the current toolbox item.</returns>
		public string ToolboxItemTypeName
		{
			get
			{
				if (this.itemTypeName == null)
				{
					if (this.itemType == null)
					{
						return string.Empty;
					}
					this.itemTypeName = this.itemType.AssemblyQualifiedName;
				}
				return this.itemTypeName;
			}
		}

		/// <param name="obj">The object to compare.</param>
		public override bool Equals(object o)
		{
			ToolboxItemAttribute toolboxItemAttribute = o as ToolboxItemAttribute;
			return toolboxItemAttribute != null && toolboxItemAttribute.ToolboxItemTypeName == this.ToolboxItemTypeName;
		}

		public override int GetHashCode()
		{
			if (this.itemTypeName != null)
			{
				return this.itemTypeName.GetHashCode();
			}
			return base.GetHashCode();
		}

		/// <summary>Gets a value indicating whether the current value of the attribute is the default value for the attribute.</summary>
		/// <returns>true if the current value of the attribute is the default; otherwise, false.</returns>
		public override bool IsDefaultAttribute()
		{
			return this.Equals(ToolboxItemAttribute.Default);
		}
	}
}
