using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	/// <summary>Instructs obfuscation tools to take the specified actions for an assembly, type, or member.</summary>
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Interface | AttributeTargets.Parameter | AttributeTargets.Delegate, AllowMultiple = true, Inherited = false)]
	public sealed class ObfuscationAttribute : Attribute
	{
		private bool exclude;

		private bool strip;

		private bool applyToMembers;

		private string feature;

		/// <summary>Initializes a new instance of the <see cref="T:System.Reflection.ObfuscationAttribute" /> class.</summary>
		public ObfuscationAttribute()
		{
			this.exclude = true;
			this.strip = true;
			this.applyToMembers = true;
			this.feature = "all";
		}

		/// <summary>Gets or sets a <see cref="T:System.Boolean" /> value indicating whether the obfuscation tool should exclude the type or member from obfuscation.</summary>
		/// <returns>true if the type or member to which this attribute is applied should be excluded from obfuscation; otherwise, false. The default is true.</returns>
		public bool Exclude
		{
			get
			{
				return this.exclude;
			}
			set
			{
				this.exclude = value;
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Boolean" /> value indicating whether the obfuscation tool should remove this attribute after processing.</summary>
		/// <returns>true if an obfuscation tool should remove the attribute after processing; otherwise, false. The default is true.</returns>
		public bool StripAfterObfuscation
		{
			get
			{
				return this.strip;
			}
			set
			{
				this.strip = value;
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Boolean" /> value indicating whether the attribute of a type is to apply to the members of the type.</summary>
		/// <returns>true if the attribute is to apply to the members of the type; otherwise, false. The default is true.</returns>
		public bool ApplyToMembers
		{
			get
			{
				return this.applyToMembers;
			}
			set
			{
				this.applyToMembers = value;
			}
		}

		/// <summary>Gets or sets a string value that is recognized by the obfuscation tool, and which specifies processing options. </summary>
		/// <returns>A string value that is recognized by the obfuscation tool, and which specifies processing options. The default is "all".</returns>
		public string Feature
		{
			get
			{
				return this.feature;
			}
			set
			{
				this.feature = value;
			}
		}
	}
}
