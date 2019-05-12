using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Security
{
	/// <summary>Defines the underlying structure of all code access permissions.</summary>
	[MonoTODO("CAS support is experimental (and unsupported).")]
	[ComVisible(true)]
	[Serializable]
	public abstract class CodeAccessPermission : IPermission, ISecurityEncodable, IStackWalk
	{
		/// <summary>Declares that the calling code can access the resource protected by a permission demand through the code that calls this method, even if callers higher in the stack have not been granted permission to access the resource. Using <see cref="M:System.Security.CodeAccessPermission.Assert" /> can create security issues.</summary>
		/// <exception cref="T:System.Security.SecurityException">The calling code does not have <see cref="F:System.Security.Permissions.SecurityPermissionFlag.Assertion" />.-or- There is already an active <see cref="M:System.Security.CodeAccessPermission.Assert" /> for the current frame. </exception>
		[MonoTODO("CAS support is experimental (and unsupported). Imperative mode is not implemented.")]
		public void Assert()
		{
		}

		internal bool CheckAssert(CodeAccessPermission asserted)
		{
			return asserted != null && asserted.GetType() == base.GetType() && this.IsSubsetOf(asserted);
		}

		internal bool CheckDemand(CodeAccessPermission target)
		{
			return target != null && target.GetType() == base.GetType() && this.IsSubsetOf(target);
		}

		internal bool CheckDeny(CodeAccessPermission denied)
		{
			if (denied == null)
			{
				return true;
			}
			Type type = denied.GetType();
			return type != base.GetType() || this.Intersect(denied) == null || denied.IsSubsetOf(PermissionBuilder.Create(type));
		}

		internal bool CheckPermitOnly(CodeAccessPermission target)
		{
			return target != null && target.GetType() == base.GetType() && this.IsSubsetOf(target);
		}

		/// <summary>When implemented by a derived class, creates and returns an identical copy of the current permission object.</summary>
		/// <returns>A copy of the current permission object.</returns>
		public abstract IPermission Copy();

		/// <summary>Forces a <see cref="T:System.Security.SecurityException" /> at run time if all callers higher in the call stack have not been granted the permission specified by the current instance.</summary>
		/// <exception cref="T:System.Security.SecurityException">A caller higher in the call stack does not have the permission specified by the current instance.-or- A caller higher in the call stack has called <see cref="M:System.Security.CodeAccessPermission.Deny" /> on the current permission object. </exception>
		public void Demand()
		{
		}

		/// <summary>Prevents callers higher in the call stack from using the code that calls this method to access the resource specified by the current instance.</summary>
		/// <exception cref="T:System.Security.SecurityException">There is already an active <see cref="M:System.Security.CodeAccessPermission.Deny" /> for the current frame. </exception>
		[MonoTODO("CAS support is experimental (and unsupported). Imperative mode is not implemented.")]
		public void Deny()
		{
		}

		/// <summary>Determines whether the specified <see cref="T:System.Security.CodeAccessPermission" /> object is equal to the current <see cref="T:System.Security.CodeAccessPermission" />.</summary>
		/// <returns>true if the specified <see cref="T:System.Security.CodeAccessPermission" /> object is equal to the current <see cref="T:System.Security.CodeAccessPermission" />; otherwise, false.</returns>
		/// <param name="obj">The <see cref="T:System.Security.CodeAccessPermission" /> object to compare with the current <see cref="T:System.Security.CodeAccessPermission" />. </param>
		[ComVisible(false)]
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj.GetType() != base.GetType())
			{
				return false;
			}
			CodeAccessPermission codeAccessPermission = obj as CodeAccessPermission;
			return this.IsSubsetOf(codeAccessPermission) && codeAccessPermission.IsSubsetOf(this);
		}

		/// <summary>When overridden in a derived class, reconstructs a security object with a specified state from an XML encoding.</summary>
		/// <param name="elem">The XML encoding to use to reconstruct the security object. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="elem" /> parameter is null. </exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="elem" /> parameter does not contain the XML encoding for an instance of the same type as the current instance.-or- The version number of the <paramref name="elem" /> parameter is not supported. </exception>
		public abstract void FromXml(SecurityElement elem);

		/// <summary>Gets a hash code for the <see cref="T:System.Security.CodeAccessPermission" /> object that is suitable for use in hashing algorithms and data structures such as a hash table.</summary>
		/// <returns>A hash code for the current <see cref="T:System.Security.CodeAccessPermission" /> object.</returns>
		[ComVisible(false)]
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		/// <summary>When implemented by a derived class, creates and returns a permission that is the intersection of the current permission and the specified permission.</summary>
		/// <returns>A new permission that represents the intersection of the current permission and the specified permission. This new permission is null if the intersection is empty.</returns>
		/// <param name="target">A permission to intersect with the current permission. It must be of the same type as the current permission. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="target" /> parameter is not null and is not an instance of the same class as the current permission. </exception>
		public abstract IPermission Intersect(IPermission target);

		/// <summary>When implemented by a derived class, determines whether the current permission is a subset of the specified permission.</summary>
		/// <returns>true if the current permission is a subset of the specified permission; otherwise, false.</returns>
		/// <param name="target">A permission that is to be tested for the subset relationship. This permission must be of the same type as the current permission. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="target" /> parameter is not null and is not of the same type as the current permission. </exception>
		public abstract bool IsSubsetOf(IPermission target);

		/// <summary>Creates and returns a string representation of the current permission object.</summary>
		/// <returns>A string representation of the current permission object.</returns>
		public override string ToString()
		{
			SecurityElement securityElement = this.ToXml();
			return securityElement.ToString();
		}

		/// <summary>When overridden in a derived class, creates an XML encoding of the security object and its current state.</summary>
		/// <returns>An XML encoding of the security object, including any state information.</returns>
		public abstract SecurityElement ToXml();

		/// <summary>When overridden in a derived class, creates a permission that is the union of the current permission and the specified permission.</summary>
		/// <returns>A new permission that represents the union of the current permission and the specified permission.</returns>
		/// <param name="other">A permission to combine with the current permission. It must be of the same type as the current permission. </param>
		/// <exception cref="T:System.NotSupportedException">The <paramref name="other" /> parameter is not null. This method is only supported at this level when passed null. </exception>
		public virtual IPermission Union(IPermission other)
		{
			if (other != null)
			{
				throw new NotSupportedException();
			}
			return null;
		}

		/// <summary>Prevents callers higher in the call stack from using the code that calls this method to access all resources except for the resource specified by the current instance.</summary>
		/// <exception cref="T:System.Security.SecurityException">There is already an active <see cref="M:System.Security.CodeAccessPermission.PermitOnly" /> for the current frame. </exception>
		[MonoTODO("CAS support is experimental (and unsupported). Imperative mode is not implemented.")]
		public void PermitOnly()
		{
		}

		/// <summary>Causes all previous overrides for the current frame to be removed and no longer in effect.</summary>
		/// <exception cref="T:System.ExecutionEngineException">There is no previous <see cref="M:System.Security.CodeAccessPermission.Assert" />, <see cref="M:System.Security.CodeAccessPermission.Deny" />, or <see cref="M:System.Security.CodeAccessPermission.PermitOnly" /> for the current frame. </exception>
		[MonoTODO("CAS support is experimental (and unsupported). Imperative mode is not implemented.")]
		public static void RevertAll()
		{
		}

		/// <summary>Causes any previous <see cref="M:System.Security.CodeAccessPermission.Assert" /> for the current frame to be removed and no longer in effect.</summary>
		/// <exception cref="T:System.ExecutionEngineException">There is no previous <see cref="M:System.Security.CodeAccessPermission.Assert" /> for the current frame. </exception>
		[MonoTODO("CAS support is experimental (and unsupported). Imperative mode is not implemented.")]
		public static void RevertAssert()
		{
		}

		/// <summary>Causes any previous <see cref="M:System.Security.CodeAccessPermission.Deny" /> for the current frame to be removed and no longer in effect.</summary>
		/// <exception cref="T:System.ExecutionEngineException">There is no previous <see cref="M:System.Security.CodeAccessPermission.Deny" /> for the current frame. </exception>
		[MonoTODO("CAS support is experimental (and unsupported). Imperative mode is not implemented.")]
		public static void RevertDeny()
		{
		}

		/// <summary>Causes any previous <see cref="M:System.Security.CodeAccessPermission.PermitOnly" /> for the current frame to be removed and no longer in effect.</summary>
		/// <exception cref="T:System.ExecutionEngineException">There is no previous <see cref="M:System.Security.CodeAccessPermission.PermitOnly" /> for the current frame. </exception>
		[MonoTODO("CAS support is experimental (and unsupported). Imperative mode is not implemented.")]
		public static void RevertPermitOnly()
		{
		}

		internal SecurityElement Element(int version)
		{
			SecurityElement securityElement = new SecurityElement("IPermission");
			Type type = base.GetType();
			securityElement.AddAttribute("class", type.FullName + ", " + type.Assembly.ToString().Replace('"', '\''));
			securityElement.AddAttribute("version", version.ToString());
			return securityElement;
		}

		internal static PermissionState CheckPermissionState(PermissionState state, bool allowUnrestricted)
		{
			if (state != PermissionState.None)
			{
				if (state != PermissionState.Unrestricted)
				{
					string message = string.Format(Locale.GetText("Invalid enum {0}"), state);
					throw new ArgumentException(message, "state");
				}
			}
			return state;
		}

		internal static int CheckSecurityElement(SecurityElement se, string parameterName, int minimumVersion, int maximumVersion)
		{
			if (se == null)
			{
				throw new ArgumentNullException(parameterName);
			}
			if (se.Tag != "IPermission")
			{
				string message = string.Format(Locale.GetText("Invalid tag {0}"), se.Tag);
				throw new ArgumentException(message, parameterName);
			}
			int num = minimumVersion;
			string text = se.Attribute("version");
			if (text != null)
			{
				try
				{
					num = int.Parse(text);
				}
				catch (Exception innerException)
				{
					string text2 = Locale.GetText("Couldn't parse version from '{0}'.");
					text2 = string.Format(text2, text);
					throw new ArgumentException(text2, parameterName, innerException);
				}
			}
			if (num < minimumVersion || num > maximumVersion)
			{
				string text3 = Locale.GetText("Unknown version '{0}', expected versions between ['{1}','{2}'].");
				text3 = string.Format(text3, num, minimumVersion, maximumVersion);
				throw new ArgumentException(text3, parameterName);
			}
			return num;
		}

		internal static bool IsUnrestricted(SecurityElement se)
		{
			string text = se.Attribute("Unrestricted");
			return text != null && string.Compare(text, bool.TrueString, true, CultureInfo.InvariantCulture) == 0;
		}

		internal bool ProcessFrame(SecurityFrame frame)
		{
			if (frame.PermitOnly != null)
			{
				bool flag = frame.PermitOnly.IsUnrestricted();
				if (!flag)
				{
					foreach (object obj in frame.PermitOnly)
					{
						IPermission permission = (IPermission)obj;
						if (this.CheckPermitOnly(permission as CodeAccessPermission))
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					CodeAccessPermission.ThrowSecurityException(this, "PermitOnly", frame, SecurityAction.Demand, null);
				}
			}
			if (frame.Deny != null)
			{
				if (frame.Deny.IsUnrestricted())
				{
					CodeAccessPermission.ThrowSecurityException(this, "Deny", frame, SecurityAction.Demand, null);
				}
				foreach (object obj2 in frame.Deny)
				{
					IPermission permission2 = (IPermission)obj2;
					if (!this.CheckDeny(permission2 as CodeAccessPermission))
					{
						CodeAccessPermission.ThrowSecurityException(this, "Deny", frame, SecurityAction.Demand, permission2);
					}
				}
			}
			if (frame.Assert != null)
			{
				if (frame.Assert.IsUnrestricted())
				{
					return true;
				}
				foreach (object obj3 in frame.Assert)
				{
					IPermission permission3 = (IPermission)obj3;
					if (this.CheckAssert(permission3 as CodeAccessPermission))
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		internal static void ThrowInvalidPermission(IPermission target, Type expected)
		{
			string text = Locale.GetText("Invalid permission type '{0}', expected type '{1}'.");
			text = string.Format(text, target.GetType(), expected);
			throw new ArgumentException(text, "target");
		}

		internal static void ThrowExecutionEngineException(SecurityAction stackmod)
		{
			string text = Locale.GetText("No {0} modifier is present on the current stack frame.");
			text = text + Environment.NewLine + "Currently only declarative stack modifiers are supported.";
			throw new ExecutionEngineException(string.Format(text, stackmod));
		}

		internal static void ThrowSecurityException(object demanded, string message, SecurityFrame frame, SecurityAction action, IPermission failed)
		{
			throw new SecurityException(message);
		}
	}
}
