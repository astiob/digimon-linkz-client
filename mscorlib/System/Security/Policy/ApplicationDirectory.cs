using System;
using System.IO;
using System.Runtime.InteropServices;

namespace System.Security.Policy
{
	/// <summary>Provides the application directory as evidence for policy evaluation. This class cannot be inherited.</summary>
	[ComVisible(true)]
	[Serializable]
	public sealed class ApplicationDirectory : IBuiltInEvidence
	{
		private string directory;

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Policy.ApplicationDirectory" /> class.</summary>
		/// <param name="name">The path of the application directory. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> parameter is null. </exception>
		public ApplicationDirectory(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length < 1)
			{
				throw new FormatException(Locale.GetText("Empty"));
			}
			this.directory = name;
		}

		int IBuiltInEvidence.GetRequiredSize(bool verbose)
		{
			return ((!verbose) ? 1 : 3) + this.directory.Length;
		}

		[MonoTODO("IBuiltInEvidence")]
		int IBuiltInEvidence.InitFromBuffer(char[] buffer, int position)
		{
			return 0;
		}

		[MonoTODO("IBuiltInEvidence")]
		int IBuiltInEvidence.OutputToBuffer(char[] buffer, int position, bool verbose)
		{
			return 0;
		}

		/// <summary>Gets the path of the application directory.</summary>
		/// <returns>The path of the application directory.</returns>
		public string Directory
		{
			get
			{
				return this.directory;
			}
		}

		/// <summary>Creates a new copy of the <see cref="T:System.Security.Policy.ApplicationDirectory" />.</summary>
		/// <returns>A new, identical copy of the <see cref="T:System.Security.Policy.ApplicationDirectory" />.</returns>
		public object Copy()
		{
			return new ApplicationDirectory(this.Directory);
		}

		/// <summary>Determines whether instances of the same type of an evidence object are equivalent.</summary>
		/// <returns>true if the two instances are equivalent; otherwise, false.</returns>
		/// <param name="o">An object of same type as the current evidence object. </param>
		public override bool Equals(object o)
		{
			ApplicationDirectory applicationDirectory = o as ApplicationDirectory;
			if (applicationDirectory != null)
			{
				this.ThrowOnInvalid(applicationDirectory.directory);
				return this.directory == applicationDirectory.directory;
			}
			return false;
		}

		/// <summary>Gets the hash code of the current application directory.</summary>
		/// <returns>The hash code of the current application directory.</returns>
		public override int GetHashCode()
		{
			return this.Directory.GetHashCode();
		}

		/// <summary>Gets a string representation of the state of the <see cref="T:System.Security.Policy.ApplicationDirectory" /> evidence object.</summary>
		/// <returns>A representation of the state of the <see cref="T:System.Security.Policy.ApplicationDirectory" /> evidence object.</returns>
		public override string ToString()
		{
			this.ThrowOnInvalid(this.Directory);
			SecurityElement securityElement = new SecurityElement("System.Security.Policy.ApplicationDirectory");
			securityElement.AddAttribute("version", "1");
			securityElement.AddChild(new SecurityElement("Directory", this.directory));
			return securityElement.ToString();
		}

		private void ThrowOnInvalid(string appdir)
		{
			if (appdir.IndexOfAny(Path.InvalidPathChars) != -1)
			{
				string text = Locale.GetText("Invalid character(s) in directory {0}");
				throw new ArgumentException(string.Format(text, appdir), "other");
			}
		}
	}
}
