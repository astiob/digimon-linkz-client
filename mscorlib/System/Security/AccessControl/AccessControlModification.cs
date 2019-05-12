using System;

namespace System.Security.AccessControl
{
	/// <summary>Specifies the type of access control modification to perform. This enumeration is used by methods of the <see cref="T:System.Security.AccessControl.ObjectSecurity" /> class and its descendents.</summary>
	public enum AccessControlModification
	{
		/// <summary>Add the specified authorization rule to the access control list (ACL).</summary>
		Add,
		/// <summary>Remove all authorization rules from the ACL, then add the specified authorization rule to the ACL.</summary>
		Set,
		/// <summary>Remove authorization rules that contain the same SID as the specified authorization rule from the ACL, and then add the specified authorization rule to the ACL.</summary>
		Reset,
		/// <summary>Remove authorization rules that contain the same security identifier (SID) and access mask as the specified authorization rule from the ACL.</summary>
		Remove,
		/// <summary>Remove authorization rules that contain the same SID as the specified authorization rule from the ACL.</summary>
		RemoveAll,
		/// <summary>Remove authorization rules that exactly match the specified authorization rule from the ACL.</summary>
		RemoveSpecific
	}
}
