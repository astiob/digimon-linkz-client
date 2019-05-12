using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics
{
	/// <summary>Specifies the display proxy for a type.</summary>
	/// <filterpriority>1</filterpriority>
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
	public sealed class DebuggerTypeProxyAttribute : Attribute
	{
		private string proxy_type_name;

		private string target_type_name;

		private Type target_type;

		/// <summary>Initializes a new instance of the <see cref="T:System.Diagnostics.DebuggerTypeProxyAttribute" /> class using the type name of the proxy. </summary>
		/// <param name="typeName">The type name of the proxy type.</param>
		public DebuggerTypeProxyAttribute(string typeName)
		{
			this.proxy_type_name = typeName;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Diagnostics.DebuggerTypeProxyAttribute" /> class using the type of the proxy. </summary>
		/// <param name="type">A <see cref="T:System.Type" /> object that represents the proxy type.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="type" /> is null.</exception>
		public DebuggerTypeProxyAttribute(Type type)
		{
			this.proxy_type_name = type.Name;
		}

		/// <summary>Gets the type name of the proxy type. </summary>
		/// <returns>The type name of the proxy type.</returns>
		/// <filterpriority>2</filterpriority>
		public string ProxyTypeName
		{
			get
			{
				return this.proxy_type_name;
			}
		}

		/// <summary>Gets or sets the target type for the attribute.</summary>
		/// <returns>A <see cref="T:System.Type" /> object identifying the target type for the attribute.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <see cref="P:System.Diagnostics.DebuggerTypeProxyAttribute.Target" /> is set to null.</exception>
		/// <filterpriority>2</filterpriority>
		public Type Target
		{
			get
			{
				return this.target_type;
			}
			set
			{
				this.target_type = value;
				this.target_type_name = this.target_type.Name;
			}
		}

		/// <summary>Gets or sets the name of the target type.</summary>
		/// <returns>The name of the target type.</returns>
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
	}
}
