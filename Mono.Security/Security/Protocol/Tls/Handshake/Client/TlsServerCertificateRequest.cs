using System;
using System.Text;

namespace Mono.Security.Protocol.Tls.Handshake.Client
{
	internal class TlsServerCertificateRequest : HandshakeMessage
	{
		private ClientCertificateType[] certificateTypes;

		private string[] distinguisedNames;

		public TlsServerCertificateRequest(Context context, byte[] buffer) : base(context, HandshakeType.CertificateRequest, buffer)
		{
		}

		public override void Update()
		{
			base.Update();
			base.Context.ServerSettings.CertificateTypes = this.certificateTypes;
			base.Context.ServerSettings.DistinguisedNames = this.distinguisedNames;
			base.Context.ServerSettings.CertificateRequest = true;
		}

		protected override void ProcessAsSsl3()
		{
			this.ProcessAsTls1();
		}

		protected override void ProcessAsTls1()
		{
			int num = (int)base.ReadByte();
			this.certificateTypes = new ClientCertificateType[num];
			for (int i = 0; i < num; i++)
			{
				this.certificateTypes[i] = (ClientCertificateType)base.ReadByte();
			}
			if (base.ReadInt16() != 0)
			{
				ASN1 asn = new ASN1(base.ReadBytes((int)base.ReadInt16()));
				this.distinguisedNames = new string[asn.Count];
				for (int j = 0; j < asn.Count; j++)
				{
					ASN1 asn2 = new ASN1(asn[j].Value);
					this.distinguisedNames[j] = Encoding.UTF8.GetString(asn2[1].Value);
				}
			}
		}
	}
}
