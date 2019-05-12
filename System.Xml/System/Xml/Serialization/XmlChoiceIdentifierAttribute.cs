using System;
using System.Reflection;
using System.Text;

namespace System.Xml.Serialization
{
	/// <summary>Specifies that the member can be further detected by using an enumeration.</summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
	public class XmlChoiceIdentifierAttribute : Attribute
	{
		private string memberName;

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Serialization.XmlChoiceIdentifierAttribute" /> class.</summary>
		public XmlChoiceIdentifierAttribute()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Serialization.XmlChoiceIdentifierAttribute" /> class.</summary>
		/// <param name="name">The member name that returns the enumeration used to detect a choice. </param>
		public XmlChoiceIdentifierAttribute(string name)
		{
			this.memberName = name;
		}

		/// <summary>Gets or sets the name of the field that returns the enumeration to use when detecting types.</summary>
		/// <returns>The name of a field that returns an enumeration.</returns>
		public string MemberName
		{
			get
			{
				if (this.memberName == null)
				{
					return string.Empty;
				}
				return this.memberName;
			}
			set
			{
				this.memberName = value;
			}
		}

		internal MemberInfo MemberInfo { get; set; }

		internal void AddKeyHash(StringBuilder sb)
		{
			sb.Append("XCA ");
			KeyHelper.AddField(sb, 1, this.memberName);
			sb.Append('|');
		}
	}
}
