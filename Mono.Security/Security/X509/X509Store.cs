using Mono.Security.X509.Extensions;
using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Text;

namespace Mono.Security.X509
{
	public class X509Store
	{
		private string _storePath;

		private X509CertificateCollection _certificates;

		private ArrayList _crls;

		private bool _crl;

		private string _name;

		internal X509Store(string path, bool crl)
		{
			this._storePath = path;
			this._crl = crl;
		}

		public X509CertificateCollection Certificates
		{
			get
			{
				if (this._certificates == null)
				{
					this._certificates = this.BuildCertificatesCollection(this._storePath);
				}
				return this._certificates;
			}
		}

		public ArrayList Crls
		{
			get
			{
				if (!this._crl)
				{
					this._crls = new ArrayList();
				}
				if (this._crls == null)
				{
					this._crls = this.BuildCrlsCollection(this._storePath);
				}
				return this._crls;
			}
		}

		public string Name
		{
			get
			{
				if (this._name == null)
				{
					int num = this._storePath.LastIndexOf(Path.DirectorySeparatorChar);
					this._name = this._storePath.Substring(num + 1);
				}
				return this._name;
			}
		}

		public void Clear()
		{
			if (this._certificates != null)
			{
				this._certificates.Clear();
			}
			this._certificates = null;
			if (this._crls != null)
			{
				this._crls.Clear();
			}
			this._crls = null;
		}

		public void Import(X509Certificate certificate)
		{
			this.CheckStore(this._storePath, true);
			string path = Path.Combine(this._storePath, this.GetUniqueName(certificate));
			if (!File.Exists(path))
			{
				using (FileStream fileStream = File.Create(path))
				{
					byte[] rawData = certificate.RawData;
					fileStream.Write(rawData, 0, rawData.Length);
					fileStream.Close();
				}
			}
		}

		public void Import(X509Crl crl)
		{
			this.CheckStore(this._storePath, true);
			string path = Path.Combine(this._storePath, this.GetUniqueName(crl));
			if (!File.Exists(path))
			{
				using (FileStream fileStream = File.Create(path))
				{
					byte[] rawData = crl.RawData;
					fileStream.Write(rawData, 0, rawData.Length);
				}
			}
		}

		public void Remove(X509Certificate certificate)
		{
			string path = Path.Combine(this._storePath, this.GetUniqueName(certificate));
			if (File.Exists(path))
			{
				File.Delete(path);
			}
		}

		public void Remove(X509Crl crl)
		{
			string path = Path.Combine(this._storePath, this.GetUniqueName(crl));
			if (File.Exists(path))
			{
				File.Delete(path);
			}
		}

		private string GetUniqueName(X509Certificate certificate)
		{
			byte[] array = this.GetUniqueName(certificate.Extensions);
			string method;
			if (array == null)
			{
				method = "tbp";
				array = certificate.Hash;
			}
			else
			{
				method = "ski";
			}
			return this.GetUniqueName(method, array, ".cer");
		}

		private string GetUniqueName(X509Crl crl)
		{
			byte[] array = this.GetUniqueName(crl.Extensions);
			string method;
			if (array == null)
			{
				method = "tbp";
				array = crl.Hash;
			}
			else
			{
				method = "ski";
			}
			return this.GetUniqueName(method, array, ".crl");
		}

		private byte[] GetUniqueName(X509ExtensionCollection extensions)
		{
			X509Extension x509Extension = extensions["2.5.29.14"];
			if (x509Extension == null)
			{
				return null;
			}
			SubjectKeyIdentifierExtension subjectKeyIdentifierExtension = new SubjectKeyIdentifierExtension(x509Extension);
			return subjectKeyIdentifierExtension.Identifier;
		}

		private string GetUniqueName(string method, byte[] name, string fileExtension)
		{
			StringBuilder stringBuilder = new StringBuilder(method);
			stringBuilder.Append("-");
			foreach (byte b in name)
			{
				stringBuilder.Append(b.ToString("X2", CultureInfo.InvariantCulture));
			}
			stringBuilder.Append(fileExtension);
			return stringBuilder.ToString();
		}

		private byte[] Load(string filename)
		{
			byte[] array = null;
			using (FileStream fileStream = File.OpenRead(filename))
			{
				array = new byte[fileStream.Length];
				fileStream.Read(array, 0, array.Length);
				fileStream.Close();
			}
			return array;
		}

		private X509Certificate LoadCertificate(string filename)
		{
			byte[] data = this.Load(filename);
			return new X509Certificate(data);
		}

		private X509Crl LoadCrl(string filename)
		{
			byte[] crl = this.Load(filename);
			return new X509Crl(crl);
		}

		private bool CheckStore(string path, bool throwException)
		{
			bool result;
			try
			{
				if (Directory.Exists(path))
				{
					result = true;
				}
				else
				{
					Directory.CreateDirectory(path);
					result = Directory.Exists(path);
				}
			}
			catch
			{
				if (throwException)
				{
					throw;
				}
				result = false;
			}
			return result;
		}

		private X509CertificateCollection BuildCertificatesCollection(string storeName)
		{
			X509CertificateCollection x509CertificateCollection = new X509CertificateCollection();
			string path = Path.Combine(this._storePath, storeName);
			if (!this.CheckStore(path, false))
			{
				return x509CertificateCollection;
			}
			string[] files = Directory.GetFiles(path, "*.cer");
			if (files != null && files.Length > 0)
			{
				foreach (string filename in files)
				{
					try
					{
						X509Certificate value = this.LoadCertificate(filename);
						x509CertificateCollection.Add(value);
					}
					catch
					{
					}
				}
			}
			return x509CertificateCollection;
		}

		private ArrayList BuildCrlsCollection(string storeName)
		{
			ArrayList arrayList = new ArrayList();
			string path = Path.Combine(this._storePath, storeName);
			if (!this.CheckStore(path, false))
			{
				return arrayList;
			}
			string[] files = Directory.GetFiles(path, "*.crl");
			if (files != null && files.Length > 0)
			{
				foreach (string filename in files)
				{
					try
					{
						X509Crl value = this.LoadCrl(filename);
						arrayList.Add(value);
					}
					catch
					{
					}
				}
			}
			return arrayList;
		}
	}
}
