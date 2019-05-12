using Mono.Security.X509;
using System;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Mono.Security.Authenticode
{
	public class SoftwarePublisherCertificate
	{
		private const string header = "-----BEGIN PKCS7-----";

		private const string footer = "-----END PKCS7-----";

		private PKCS7.SignedData pkcs7;

		public SoftwarePublisherCertificate()
		{
			this.pkcs7 = new PKCS7.SignedData();
			this.pkcs7.ContentInfo.ContentType = "1.2.840.113549.1.7.1";
		}

		public SoftwarePublisherCertificate(byte[] data) : this()
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			PKCS7.ContentInfo contentInfo = new PKCS7.ContentInfo(data);
			if (contentInfo.ContentType != "1.2.840.113549.1.7.2")
			{
				throw new ArgumentException(Locale.GetText("Unsupported ContentType"));
			}
			this.pkcs7 = new PKCS7.SignedData(contentInfo.Content);
		}

		public X509CertificateCollection Certificates
		{
			get
			{
				return this.pkcs7.Certificates;
			}
		}

		public ArrayList Crls
		{
			get
			{
				return this.pkcs7.Crls;
			}
		}

		public byte[] GetBytes()
		{
			PKCS7.ContentInfo contentInfo = new PKCS7.ContentInfo("1.2.840.113549.1.7.2");
			contentInfo.Content.Add(this.pkcs7.ASN1);
			return contentInfo.GetBytes();
		}

		public static SoftwarePublisherCertificate CreateFromFile(string filename)
		{
			if (filename == null)
			{
				throw new ArgumentNullException("filename");
			}
			byte[] array = null;
			using (FileStream fileStream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				array = new byte[fileStream.Length];
				fileStream.Read(array, 0, array.Length);
				fileStream.Close();
			}
			if (array.Length < 2)
			{
				return null;
			}
			if (array[0] != 48)
			{
				try
				{
					array = SoftwarePublisherCertificate.PEM(array);
				}
				catch (Exception inner)
				{
					throw new CryptographicException("Invalid encoding", inner);
				}
			}
			return new SoftwarePublisherCertificate(array);
		}

		private static byte[] PEM(byte[] data)
		{
			string text = (data[1] != 0) ? Encoding.ASCII.GetString(data) : Encoding.Unicode.GetString(data);
			int num = text.IndexOf("-----BEGIN PKCS7-----") + "-----BEGIN PKCS7-----".Length;
			int num2 = text.IndexOf("-----END PKCS7-----", num);
			string s = (num != -1 && num2 != -1) ? text.Substring(num, num2 - num) : text;
			return Convert.FromBase64String(s);
		}
	}
}
