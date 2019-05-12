using System;
using System.Runtime.InteropServices;

namespace System.Security.Principal
{
	/// <summary>Represents a generic user.</summary>
	[ComVisible(true)]
	[Serializable]
	public class GenericIdentity : IIdentity
	{
		private string m_name;

		private string m_type;

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Principal.GenericIdentity" /> class representing the user with the specified name and authentication type.</summary>
		/// <param name="name">The name of the user on whose behalf the code is running. </param>
		/// <param name="type">The type of authentication used to identify the user. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> parameter is null.-or- The <paramref name="type" /> parameter is null. </exception>
		public GenericIdentity(string name, string type)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			this.m_name = name;
			this.m_type = type;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Principal.GenericIdentity" /> class representing the user with the specified name.</summary>
		/// <param name="name">The name of the user on whose behalf the code is running. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> parameter is null. </exception>
		public GenericIdentity(string name) : this(name, string.Empty)
		{
		}

		/// <summary>Gets the type of authentication used to identify the user.</summary>
		/// <returns>The type of authentication used to identify the user.</returns>
		public virtual string AuthenticationType
		{
			get
			{
				return this.m_type;
			}
		}

		/// <summary>Gets the user's name.</summary>
		/// <returns>The name of the user on whose behalf the code is being run.</returns>
		public virtual string Name
		{
			get
			{
				return this.m_name;
			}
		}

		/// <summary>Gets a value indicating whether the user has been authenticated.</summary>
		/// <returns>true if the user was has been authenticated; otherwise, false.</returns>
		public virtual bool IsAuthenticated
		{
			get
			{
				return this.m_name.Length > 0;
			}
		}
	}
}
