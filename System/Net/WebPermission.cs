using System;
using System.Collections;
using System.Security;
using System.Security.Permissions;
using System.Text.RegularExpressions;

namespace System.Net
{
	/// <summary>Controls rights to access HTTP Internet resources.</summary>
	[MonoTODO("Most private members that include functionallity are not implemented!")]
	[Serializable]
	public sealed class WebPermission : CodeAccessPermission, IUnrestrictedPermission
	{
		private ArrayList m_acceptList = new ArrayList();

		private ArrayList m_connectList = new ArrayList();

		private bool m_noRestriction;

		/// <summary>Creates a new instance of the <see cref="T:System.Net.WebPermission" /> class.</summary>
		public WebPermission()
		{
		}

		/// <summary>Creates a new instance of the <see cref="T:System.Net.WebPermission" /> class that passes all demands or fails all demands.</summary>
		/// <param name="state">A <see cref="T:System.Security.Permissions.PermissionState" /> value. </param>
		public WebPermission(PermissionState state)
		{
			this.m_noRestriction = (state == PermissionState.Unrestricted);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.WebPermission" /> class with the specified access rights for the specified URI.</summary>
		/// <param name="access">A NetworkAccess value that indicates what kind of access to grant to the specified URI. <see cref="F:System.Net.NetworkAccess.Accept" /> indicates that the application is allowed to accept connections from the Internet on a local resource. <see cref="F:System.Net.NetworkAccess.Connect" /> indicates that the application is allowed to connect to specific Internet resources. </param>
		/// <param name="uriString">A URI string to which access rights are granted. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="uriString" /> is null. </exception>
		public WebPermission(NetworkAccess access, string uriString)
		{
			this.AddPermission(access, uriString);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.WebPermission" /> class with the specified access rights for the specified URI regular expression.</summary>
		/// <param name="access">A <see cref="T:System.Net.NetworkAccess" /> value that indicates what kind of access to grant to the specified URI. <see cref="F:System.Net.NetworkAccess.Accept" /> indicates that the application is allowed to accept connections from the Internet on a local resource. <see cref="F:System.Net.NetworkAccess.Connect" /> indicates that the application is allowed to connect to specific Internet resources. </param>
		/// <param name="uriRegex">A regular expression that describes the URI to which access is to be granted. </param>
		public WebPermission(NetworkAccess access, System.Text.RegularExpressions.Regex uriRegex)
		{
			this.AddPermission(access, uriRegex);
		}

		/// <summary>This property returns an enumeration of a single accept permissions held by this <see cref="T:System.Net.WebPermission" />. The possible objects types contained in the returned enumeration are <see cref="T:System.String" /> and <see cref="T:System.Text.RegularExpressions.Regex" />.</summary>
		/// <returns>The <see cref="T:System.Collections.IEnumerator" /> interface that contains accept permissions.</returns>
		public IEnumerator AcceptList
		{
			get
			{
				return this.m_acceptList.GetEnumerator();
			}
		}

		/// <summary>This property returns an enumeration of a single connect permissions held by this <see cref="T:System.Net.WebPermission" />. The possible objects types contained in the returned enumeration are <see cref="T:System.String" /> and <see cref="T:System.Text.RegularExpressions.Regex" />.</summary>
		/// <returns>The <see cref="T:System.Collections.IEnumerator" /> interface that contains connect permissions.</returns>
		public IEnumerator ConnectList
		{
			get
			{
				return this.m_connectList.GetEnumerator();
			}
		}

		/// <summary>Adds the specified URI string with the specified access rights to the current <see cref="T:System.Net.WebPermission" />.</summary>
		/// <param name="access">A <see cref="T:System.Net.NetworkAccess" /> that specifies the access rights that are granted to the URI. </param>
		/// <param name="uriString">A string that describes the URI to which access rights are granted. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="uriString" /> is null. </exception>
		public void AddPermission(NetworkAccess access, string uriString)
		{
			WebPermissionInfo info = new WebPermissionInfo(WebPermissionInfoType.InfoString, uriString);
			this.AddPermission(access, info);
		}

		/// <summary>Adds the specified URI with the specified access rights to the current <see cref="T:System.Net.WebPermission" />.</summary>
		/// <param name="access">A NetworkAccess that specifies the access rights that are granted to the URI. </param>
		/// <param name="uriRegex">A regular expression that describes the set of URIs to which access rights are granted. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="uriRegex" /> parameter is null. </exception>
		public void AddPermission(NetworkAccess access, System.Text.RegularExpressions.Regex uriRegex)
		{
			WebPermissionInfo info = new WebPermissionInfo(uriRegex);
			this.AddPermission(access, info);
		}

		internal void AddPermission(NetworkAccess access, WebPermissionInfo info)
		{
			if (access != NetworkAccess.Connect)
			{
				if (access != NetworkAccess.Accept)
				{
					string text = Locale.GetText("Unknown NetworkAccess value {0}.");
					throw new ArgumentException(string.Format(text, access), "access");
				}
				this.m_acceptList.Add(info);
			}
			else
			{
				this.m_connectList.Add(info);
			}
		}

		/// <summary>Creates a copy of a <see cref="T:System.Net.WebPermission" />.</summary>
		/// <returns>A new instance of the <see cref="T:System.Net.WebPermission" /> class that has the same values as the original. </returns>
		public override IPermission Copy()
		{
			return new WebPermission((!this.m_noRestriction) ? PermissionState.None : PermissionState.Unrestricted)
			{
				m_connectList = (ArrayList)this.m_connectList.Clone(),
				m_acceptList = (ArrayList)this.m_acceptList.Clone()
			};
		}

		/// <summary>Returns the logical intersection of two <see cref="T:System.Net.WebPermission" /> instances.</summary>
		/// <returns>A new <see cref="T:System.Net.WebPermission" /> that represents the intersection of the current instance and the <paramref name="target" /> parameter. If the intersection is empty, the method returns null.</returns>
		/// <param name="target">The <see cref="T:System.Net.WebPermission" /> to compare with the current instance. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="target" /> is not null or of type <see cref="T:System.Net.WebPermission" /></exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		public override IPermission Intersect(IPermission target)
		{
			if (target == null)
			{
				return null;
			}
			WebPermission webPermission = target as WebPermission;
			if (webPermission == null)
			{
				throw new ArgumentException("Argument not of type WebPermission");
			}
			if (this.m_noRestriction)
			{
				IPermission result;
				if (this.IntersectEmpty(webPermission))
				{
					IPermission permission = null;
					result = permission;
				}
				else
				{
					result = webPermission.Copy();
				}
				return result;
			}
			if (webPermission.m_noRestriction)
			{
				IPermission result2;
				if (this.IntersectEmpty(this))
				{
					IPermission permission = null;
					result2 = permission;
				}
				else
				{
					result2 = this.Copy();
				}
				return result2;
			}
			WebPermission webPermission2 = new WebPermission(PermissionState.None);
			this.Intersect(this.m_connectList, webPermission.m_connectList, webPermission2.m_connectList);
			this.Intersect(this.m_acceptList, webPermission.m_acceptList, webPermission2.m_acceptList);
			return (!this.IntersectEmpty(webPermission2)) ? webPermission2 : null;
		}

		private bool IntersectEmpty(WebPermission permission)
		{
			return !permission.m_noRestriction && permission.m_connectList.Count == 0 && permission.m_acceptList.Count == 0;
		}

		[MonoTODO]
		private void Intersect(ArrayList list1, ArrayList list2, ArrayList result)
		{
			throw new NotImplementedException();
		}

		/// <summary>Determines whether the current <see cref="T:System.Net.WebPermission" /> is a subset of the specified object.</summary>
		/// <returns>true if the current instance is a subset of the <paramref name="target" /> parameter; otherwise, false. If the target is null, the method returns true for an empty current permission that is not unrestricted and false otherwise.</returns>
		/// <param name="target">The <see cref="T:System.Net.WebPermission" /> to compare to the current <see cref="T:System.Net.WebPermission" />. </param>
		/// <exception cref="T:System.ArgumentException">The target parameter is not an instance of <see cref="T:System.Net.WebPermission" />. </exception>
		/// <exception cref="T:System.NotSupportedException">The current instance contains a Regex-encoded right and there is not exactly the same right found in the target instance. </exception>
		public override bool IsSubsetOf(IPermission target)
		{
			if (target == null)
			{
				return !this.m_noRestriction && this.m_connectList.Count == 0 && this.m_acceptList.Count == 0;
			}
			WebPermission webPermission = target as WebPermission;
			if (webPermission == null)
			{
				throw new ArgumentException("Parameter target must be of type WebPermission");
			}
			return webPermission.m_noRestriction || (!this.m_noRestriction && ((this.m_acceptList.Count == 0 && this.m_connectList.Count == 0) || ((webPermission.m_acceptList.Count != 0 || webPermission.m_connectList.Count != 0) && this.IsSubsetOf(this.m_connectList, webPermission.m_connectList) && this.IsSubsetOf(this.m_acceptList, webPermission.m_acceptList))));
		}

		[MonoTODO]
		private bool IsSubsetOf(ArrayList list1, ArrayList list2)
		{
			throw new NotImplementedException();
		}

		/// <summary>Checks the overall permission state of the <see cref="T:System.Net.WebPermission" />.</summary>
		/// <returns>true if the <see cref="T:System.Net.WebPermission" /> was created with the <see cref="F:System.Security.Permissions.PermissionState.Unrestricted" /><see cref="T:System.Security.Permissions.PermissionState" />; otherwise, false.</returns>
		public bool IsUnrestricted()
		{
			return this.m_noRestriction;
		}

		/// <summary>Creates an XML encoding of a <see cref="T:System.Net.WebPermission" /> and its current state.</summary>
		/// <returns>A <see cref="T:System.Security.SecurityElement" /> that contains an XML-encoded representation of the <see cref="T:System.Net.WebPermission" />, including state information.</returns>
		public override SecurityElement ToXml()
		{
			SecurityElement securityElement = new SecurityElement("IPermission");
			securityElement.AddAttribute("class", base.GetType().AssemblyQualifiedName);
			securityElement.AddAttribute("version", "1");
			if (this.m_noRestriction)
			{
				securityElement.AddAttribute("Unrestricted", "true");
				return securityElement;
			}
			if (this.m_connectList.Count > 0)
			{
				this.ToXml(securityElement, "ConnectAccess", this.m_connectList.GetEnumerator());
			}
			if (this.m_acceptList.Count > 0)
			{
				this.ToXml(securityElement, "AcceptAccess", this.m_acceptList.GetEnumerator());
			}
			return securityElement;
		}

		private void ToXml(SecurityElement root, string childName, IEnumerator enumerator)
		{
			SecurityElement securityElement = new SecurityElement(childName, null);
			root.AddChild(securityElement);
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				WebPermissionInfo webPermissionInfo = obj as WebPermissionInfo;
				if (webPermissionInfo != null)
				{
					SecurityElement securityElement2 = new SecurityElement("URI");
					securityElement2.AddAttribute("uri", webPermissionInfo.Info);
					securityElement.AddChild(securityElement2);
				}
			}
		}

		/// <summary>Reconstructs a <see cref="T:System.Net.WebPermission" /> from an XML encoding.</summary>
		/// <param name="securityElement">The XML encoding from which to reconstruct the <see cref="T:System.Net.WebPermission" />. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="securityElement" /> parameter is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="securityElement" /> is not a permission element for this type. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		public override void FromXml(SecurityElement securityElement)
		{
			if (securityElement == null)
			{
				throw new ArgumentNullException("securityElement");
			}
			if (securityElement.Tag != "IPermission")
			{
				throw new ArgumentException("securityElement");
			}
			string text = securityElement.Attribute("Unrestricted");
			if (text != null)
			{
				this.m_noRestriction = (string.Compare(text, "true", true) == 0);
				if (this.m_noRestriction)
				{
					return;
				}
			}
			this.m_noRestriction = false;
			this.m_connectList = new ArrayList();
			this.m_acceptList = new ArrayList();
			ArrayList children = securityElement.Children;
			foreach (object obj in children)
			{
				SecurityElement securityElement2 = (SecurityElement)obj;
				if (securityElement2.Tag == "ConnectAccess")
				{
					this.FromXml(securityElement2.Children, NetworkAccess.Connect);
				}
				else if (securityElement2.Tag == "AcceptAccess")
				{
					this.FromXml(securityElement2.Children, NetworkAccess.Accept);
				}
			}
		}

		private void FromXml(ArrayList endpoints, NetworkAccess access)
		{
			throw new NotImplementedException();
		}

		/// <summary>Returns the logical union between two instances of the <see cref="T:System.Net.WebPermission" /> class.</summary>
		/// <returns>A <see cref="T:System.Net.WebPermission" /> that represents the union of the current instance and the <paramref name="target" /> parameter. If either WebPermission is <see cref="F:System.Security.Permissions.PermissionState.Unrestricted" />, the method returns a <see cref="T:System.Net.WebPermission" /> that is <see cref="F:System.Security.Permissions.PermissionState.Unrestricted" />. If the target is null, the method returns a copy of the current <see cref="T:System.Net.WebPermission" />.</returns>
		/// <param name="target">The <see cref="T:System.Net.WebPermission" /> to combine with the current <see cref="T:System.Net.WebPermission" />. </param>
		/// <exception cref="T:System.ArgumentException">target is not null or of type <see cref="T:System.Net.WebPermission" />. </exception>
		public override IPermission Union(IPermission target)
		{
			if (target == null)
			{
				return null;
			}
			WebPermission webPermission = target as WebPermission;
			if (webPermission == null)
			{
				throw new ArgumentException("Argument not of type WebPermission");
			}
			if (this.m_noRestriction || webPermission.m_noRestriction)
			{
				return new WebPermission(PermissionState.Unrestricted);
			}
			WebPermission webPermission2 = (WebPermission)webPermission.Copy();
			webPermission2.m_acceptList.InsertRange(webPermission2.m_acceptList.Count, this.m_acceptList);
			webPermission2.m_connectList.InsertRange(webPermission2.m_connectList.Count, this.m_connectList);
			return webPermission2;
		}
	}
}
