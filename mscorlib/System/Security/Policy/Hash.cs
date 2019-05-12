using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;

namespace System.Security.Policy
{
	/// <summary>Provides evidence about the hash value for an assembly. This class cannot be inherited.</summary>
	[ComVisible(true)]
	[Serializable]
	public sealed class Hash : ISerializable, IBuiltInEvidence
	{
		private Assembly assembly;

		private byte[] data;

		internal byte[] _md5;

		internal byte[] _sha1;

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Policy.Hash" /> class.</summary>
		/// <param name="assembly">The <see cref="T:System.Reflection.Assembly" /> for which to compute the hash value. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="assembly" /> parameter is null. </exception>
		public Hash(Assembly assembly)
		{
			if (assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}
			this.assembly = assembly;
		}

		internal Hash()
		{
		}

		internal Hash(SerializationInfo info, StreamingContext context)
		{
			this.data = (byte[])info.GetValue("RawData", typeof(byte[]));
		}

		int IBuiltInEvidence.GetRequiredSize(bool verbose)
		{
			return (!verbose) ? 0 : 5;
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

		/// <summary>Gets the <see cref="T:System.Security.Cryptography.MD5" /> hash value for the assembly.</summary>
		/// <returns>A byte array that represents the <see cref="T:System.Security.Cryptography.MD5" /> hash value for the assembly.</returns>
		public byte[] MD5
		{
			get
			{
				if (this._md5 != null)
				{
					return this._md5;
				}
				if (this.assembly == null && this._sha1 != null)
				{
					string text = Locale.GetText("No assembly data. This instance was initialized with an MSHA1 digest value.");
					throw new SecurityException(text);
				}
				HashAlgorithm hashAlg = System.Security.Cryptography.MD5.Create();
				this._md5 = this.GenerateHash(hashAlg);
				return this._md5;
			}
		}

		/// <summary>Gets the <see cref="T:System.Security.Cryptography.SHA1" /> hash value for the assembly.</summary>
		/// <returns>A byte array that represents the <see cref="T:System.Security.Cryptography.SHA1" /> hash value for the assembly.</returns>
		public byte[] SHA1
		{
			get
			{
				if (this._sha1 != null)
				{
					return this._sha1;
				}
				if (this.assembly == null && this._md5 != null)
				{
					string text = Locale.GetText("No assembly data. This instance was initialized with an MD5 digest value.");
					throw new SecurityException(text);
				}
				HashAlgorithm hashAlg = System.Security.Cryptography.SHA1.Create();
				this._sha1 = this.GenerateHash(hashAlg);
				return this._sha1;
			}
		}

		/// <summary>Computes the hash value for the assembly using the specified hash algorithm.</summary>
		/// <returns>A byte array that represents the hash value for the assembly.</returns>
		/// <param name="hashAlg">The hash algorithm to use to compute the hash value for the assembly. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="hashAlg" /> parameter is null. </exception>
		/// <exception cref="T:System.Security.SecurityException">The hash value for the assembly cannot be generated.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		///   <IPermission class="System.Security.Permissions.KeyContainerPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public byte[] GenerateHash(HashAlgorithm hashAlg)
		{
			if (hashAlg == null)
			{
				throw new ArgumentNullException("hashAlg");
			}
			return hashAlg.ComputeHash(this.GetData());
		}

		/// <summary>Gets the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object with the parameter name and additional exception information.</summary>
		/// <param name="info">The object that holds the serialized object data. </param>
		/// <param name="context">The contextual information about the source or destination. </param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("RawData", this.GetData());
		}

		/// <summary>Returns a string representation of the current <see cref="T:System.Security.Policy.Hash" />.</summary>
		/// <returns>A representation of the current <see cref="T:System.Security.Policy.Hash" />.</returns>
		public override string ToString()
		{
			SecurityElement securityElement = new SecurityElement(base.GetType().FullName);
			securityElement.AddAttribute("version", "1");
			StringBuilder stringBuilder = new StringBuilder();
			byte[] array = this.GetData();
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.Append(array[i].ToString("X2"));
			}
			securityElement.AddChild(new SecurityElement("RawData", stringBuilder.ToString()));
			return securityElement.ToString();
		}

		private byte[] GetData()
		{
			if (this.assembly == null && this.data == null)
			{
				string text = Locale.GetText("No assembly data.");
				throw new SecurityException(text);
			}
			if (this.data == null)
			{
				FileStream fileStream = new FileStream(this.assembly.Location, FileMode.Open, FileAccess.Read);
				this.data = new byte[fileStream.Length];
				fileStream.Read(this.data, 0, (int)fileStream.Length);
			}
			return this.data;
		}

		/// <summary>Creates a <see cref="T:System.Security.Policy.Hash" /> object containing an <see cref="T:System.Security.Cryptography.MD5" /> hash value.</summary>
		/// <returns>A <see cref="T:System.Security.Policy.Hash" /> object containing the hash value provided by the <paramref name="md5" /> parameter.</returns>
		/// <param name="md5">A byte array containing an <see cref="T:System.Security.Cryptography.MD5" /> hash value.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="md5" /> parameter is null. </exception>
		public static Hash CreateMD5(byte[] md5)
		{
			if (md5 == null)
			{
				throw new ArgumentNullException("md5");
			}
			return new Hash
			{
				_md5 = md5
			};
		}

		/// <summary>Creates a <see cref="T:System.Security.Policy.Hash" /> object containing an <see cref="T:System.Security.Cryptography.SHA1" /> hash value.</summary>
		/// <returns>A <see cref="T:System.Security.Policy.Hash" /> object containing the hash value provided by the <paramref name="sha1" /> parameter.</returns>
		/// <param name="sha1">A byte array containing a <see cref="T:System.Security.Cryptography.SHA1" /> hash value.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="sha1" /> parameter is null. </exception>
		public static Hash CreateSHA1(byte[] sha1)
		{
			if (sha1 == null)
			{
				throw new ArgumentNullException("sha1");
			}
			return new Hash
			{
				_sha1 = sha1
			};
		}
	}
}
