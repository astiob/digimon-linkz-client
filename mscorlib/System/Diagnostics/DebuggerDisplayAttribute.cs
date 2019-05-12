using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics
{
	/// <summary>Determines how a class or field is displayed in the debugger variable windows.</summary>
	/// <filterpriority>1</filterpriority>
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Delegate, AllowMultiple = true)]
	[ComVisible(true)]
	public sealed class DebuggerDisplayAttribute : Attribute
	{
		private string value;

		private string type;

		private string name;

		private string target_type_name;

		private Type target_type;

		/// <summary>Initializes a new instance of the <see cref="T:System.Diagnostics.DebuggerDisplayAttribute" /> class. </summary>
		/// <param name="value">The string to be displayed in the value column for instances of the type; an empty string ("") causes the value column to be hidden.</param>
		public DebuggerDisplayAttribute(string value)
		{
			if (value == null)
			{
				value = string.Empty;
			}
			this.value = value;
			this.type = string.Empty;
			this.name = string.Empty;
		}

		/// <summary>Gets the string to display in the value column of the debugger variable windows.</summary>
		/// <returns>The string to display in the value column of the debugger variable.</returns>
		/// <filterpriority>2</filterpriority>
		public string Value
		{
			get
			{
				return this.value;
			}
		}

		/// <summary>Gets or sets the type of the attribute's target.</summary>
		/// <returns>A <see cref="T:System.Type" /> object that identifies the attribute's target type.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <see cref="P:System.Diagnostics.DebuggerDisplayAttribute.Target" /> is set to null.</exception>
		/// <filterpriority>2</filterpriority>
		public Type Target
		{
			get
			{
				return this.target_type;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.target_type = value;
				this.target_type_name = this.target_type.AssemblyQualifiedName;
			}
		}

		/// <summary>Gets or sets the type name of the attribute's target.</summary>
		/// <returns>The name of the attribute's target type.</returns>
		/// <filterpriority>2</filterpriority>
		public string TargetTypeName
		{
			get
			{
				return this.target_type_name;
			}
			set
			{
				this.target_type_name = value;
			}
		}

		/// <summary>Gets or sets the string to display in the type column of the debugger variable windows.</summary>
		/// <returns>The string to display in the type column of the debugger variable windows.</returns>
		/// <filterpriority>2</filterpriority>
		public string Type
		{
			get
			{
				return this.type;
			}
			set
			{
				this.type = value;
			}
		}

		/// <summary>Gets or sets the name to display in the debugger variable windows.</summary>
		/// <returns>The name to display in the debugger variable windows.</returns>
		/// <filterpriority>2</filterpriority>
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}
	}
}
