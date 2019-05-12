using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Net.Mime;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace System.Net.Mail
{
	/// <summary>Allows applications to send e-mail by using the Simple Mail Transfer Protocol (SMTP).</summary>
	public class SmtpClient
	{
		private string host;

		private int port;

		private int timeout = 100000;

		private ICredentialsByHost credentials;

		private string pickupDirectoryLocation;

		private SmtpDeliveryMethod deliveryMethod;

		private bool enableSsl;

		private System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates;

		private System.Net.Sockets.TcpClient client;

		private Stream stream;

		private StreamWriter writer;

		private StreamReader reader;

		private int boundaryIndex;

		private MailAddress defaultFrom;

		private MailMessage messageInProcess;

		private System.ComponentModel.BackgroundWorker worker;

		private object user_async_state;

		private SmtpClient.AuthMechs authMechs;

		private Mutex mutex = new Mutex();

		private System.Net.Security.RemoteCertificateValidationCallback callback = delegate(object sender, X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
		{
			if (ServicePointManager.ServerCertificateValidationCallback != null)
			{
				return ServicePointManager.ServerCertificateValidationCallback(sender, certificate, chain, sslPolicyErrors);
			}
			if (sslPolicyErrors != System.Net.Security.SslPolicyErrors.None)
			{
				throw new InvalidOperationException("SSL authentication error: " + sslPolicyErrors);
			}
			return true;
		};

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Mail.SmtpClient" /> class by using configuration file settings. </summary>
		public SmtpClient() : this(null, 0)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Mail.SmtpClient" /> class that sends e-mail by using the specified SMTP server. </summary>
		/// <param name="host">A <see cref="T:System.String" /> that contains the name or IP address of the host computer used for SMTP transactions.</param>
		public SmtpClient(string host) : this(host, 0)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Mail.SmtpClient" /> class that sends e-mail by using the specified SMTP server and port.</summary>
		/// <param name="host">A <see cref="T:System.String" /> that contains the name or IP address of the host used for SMTP transactions.</param>
		/// <param name="port">An <see cref="T:System.Int32" /> greater than zero that contains the port to be used on <paramref name="host" />.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="port" /> cannot be less than zero.</exception>
		public SmtpClient(string host, int port)
		{
			if (!string.IsNullOrEmpty(host))
			{
				this.host = host;
			}
			if (port != 0)
			{
				this.port = port;
			}
		}

		/// <summary>Occurs when an asynchronous e-mail send operation completes.</summary>
		public event SendCompletedEventHandler SendCompleted;

		/// <summary>Specify which certificates should be used to establish the Secure Sockets Layer (SSL) connection.</summary>
		/// <returns>An <see cref="T:System.Security.Cryptography.X509Certificates.X509CertificateCollection" />, holding one or more client certificates. The default value is derived from the mail configuration attributes in a configuration file.</returns>
		[MonoTODO("Client certificates not used")]
		public System.Security.Cryptography.X509Certificates.X509CertificateCollection ClientCertificates
		{
			get
			{
				if (this.clientCertificates == null)
				{
					this.clientCertificates = new System.Security.Cryptography.X509Certificates.X509CertificateCollection();
				}
				return this.clientCertificates;
			}
		}

		/// <summary>Gets or sets the Service Provider Name (SPN) to use for authentication when using extended protection.</summary>
		/// <returns>A <see cref="T:System.String" /> that specifies the SPN to use for extended protection. The default value for this SPN is of the form "SMTPSVC/&lt;host&gt;" where &lt;host&gt; is the hostname of the SMTP mail server. </returns>
		private string TargetName { get; set; }

		/// <summary>Gets or sets the credentials used to authenticate the sender.</summary>
		/// <returns>An <see cref="T:System.Net.ICredentialsByHost" /> that represents the credentials to use for authentication; or null if no credentials have been specified.</returns>
		/// <exception cref="T:System.InvalidOperationException">You cannot change the value of this property when an email is being sent.</exception>
		public ICredentialsByHost Credentials
		{
			get
			{
				return this.credentials;
			}
			set
			{
				this.CheckState();
				this.credentials = value;
			}
		}

		/// <summary>Specifies how outgoing email messages will be handled.</summary>
		/// <returns>An <see cref="T:System.Net.Mail.SmtpDeliveryMethod" /> that indicates how email messages are delivered.</returns>
		public SmtpDeliveryMethod DeliveryMethod
		{
			get
			{
				return this.deliveryMethod;
			}
			set
			{
				this.CheckState();
				this.deliveryMethod = value;
			}
		}

		/// <summary>Specify whether the <see cref="T:System.Net.Mail.SmtpClient" /> uses Secure Sockets Layer (SSL) to encrypt the connection.</summary>
		/// <returns>true if the <see cref="T:System.Net.Mail.SmtpClient" /> uses SSL; otherwise, false. The default is false.</returns>
		public bool EnableSsl
		{
			get
			{
				return this.enableSsl;
			}
			set
			{
				this.CheckState();
				this.enableSsl = value;
			}
		}

		/// <summary>Gets or sets the name or IP address of the host used for SMTP transactions.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the name or IP address of the computer to use for SMTP transactions.</returns>
		/// <exception cref="T:System.ArgumentNullException">The value specified for a set operation is null.</exception>
		/// <exception cref="T:System.ArgumentException">The value specified for a set operation is equal to <see cref="F:System.String.Empty" /> ("").</exception>
		/// <exception cref="T:System.InvalidOperationException">You cannot change the value of this property when an email is being sent.</exception>
		public string Host
		{
			get
			{
				return this.host;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (value.Length == 0)
				{
					throw new ArgumentException("An empty string is not allowed.", "value");
				}
				this.CheckState();
				this.host = value;
			}
		}

		/// <summary>Gets or sets the folder where applications save mail messages to be processed by the local SMTP server.</summary>
		/// <returns>A <see cref="T:System.String" /> that specifies the pickup directory for mail messages.</returns>
		public string PickupDirectoryLocation
		{
			get
			{
				return this.pickupDirectoryLocation;
			}
			set
			{
				this.pickupDirectoryLocation = value;
			}
		}

		/// <summary>Gets or sets the port used for SMTP transactions.</summary>
		/// <returns>An <see cref="T:System.Int32" /> that contains the port number on the SMTP host. The default value is 25.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value specified for a set operation is less than or equal to zero.</exception>
		/// <exception cref="T:System.InvalidOperationException">You cannot change the value of this property when an email is being sent.</exception>
		public int Port
		{
			get
			{
				return this.port;
			}
			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.CheckState();
				this.port = value;
			}
		}

		/// <summary>Gets the network connection used to transmit the e-mail message.</summary>
		/// <returns>A <see cref="T:System.Net.ServicePoint" /> that connects to the <see cref="P:System.Net.Mail.SmtpClient.Host" /> property used for SMTP.</returns>
		/// <exception cref="T:System.InvalidOperationException">
		///   <see cref="P:System.Net.Mail.SmtpClient.Host" /> is null or the empty string ("").-or-<see cref="P:System.Net.Mail.SmtpClient.Port" /> is zero.</exception>
		[MonoTODO]
		public ServicePoint ServicePoint
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		/// <summary>Gets or sets a value that specifies the amount of time after which a synchronous <see cref="Overload:System.Net.Mail.SmtpClient.Send" /> call times out.</summary>
		/// <returns>An <see cref="T:System.Int32" /> that specifies the time-out value in milliseconds. The default value is 100,000 (100 seconds).</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value specified for a set operation was less than zero.</exception>
		/// <exception cref="T:System.InvalidOperationException">You cannot change the value of this property when an email is being sent.</exception>
		public int Timeout
		{
			get
			{
				return this.timeout;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.CheckState();
				this.timeout = value;
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Boolean" /> value that controls whether the <see cref="P:System.Net.CredentialCache.DefaultCredentials" /> are sent with requests.</summary>
		/// <returns>true if the default credentials are used; otherwise false. The default value is false.</returns>
		/// <exception cref="T:System.InvalidOperationException">You cannot change the value of this property when an e-mail is being sent.</exception>
		public bool UseDefaultCredentials
		{
			get
			{
				return false;
			}
			[MonoNotSupported("no DefaultCredential support in Mono")]
			set
			{
				if (value)
				{
					throw new NotImplementedException("Default credentials are not supported");
				}
				this.CheckState();
			}
		}

		private void CheckState()
		{
			if (this.messageInProcess != null)
			{
				throw new InvalidOperationException("Cannot set Timeout while Sending a message");
			}
		}

		private static string EncodeAddress(MailAddress address)
		{
			string text = System.Net.Mime.ContentType.EncodeSubjectRFC2047(address.DisplayName, Encoding.UTF8);
			return string.Concat(new string[]
			{
				"\"",
				text,
				"\" <",
				address.Address,
				">"
			});
		}

		private static string EncodeAddresses(MailAddressCollection addresses)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			foreach (MailAddress address in addresses)
			{
				if (!flag)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append(SmtpClient.EncodeAddress(address));
				flag = false;
			}
			return stringBuilder.ToString();
		}

		private string EncodeSubjectRFC2047(MailMessage message)
		{
			return System.Net.Mime.ContentType.EncodeSubjectRFC2047(message.Subject, message.SubjectEncoding);
		}

		private string EncodeBody(MailMessage message)
		{
			string body = message.Body;
			Encoding bodyEncoding = message.BodyEncoding;
			System.Net.Mime.TransferEncoding contentTransferEncoding = message.ContentTransferEncoding;
			if (contentTransferEncoding == System.Net.Mime.TransferEncoding.Base64)
			{
				return Convert.ToBase64String(bodyEncoding.GetBytes(body), Base64FormattingOptions.InsertLineBreaks);
			}
			if (contentTransferEncoding != System.Net.Mime.TransferEncoding.SevenBit)
			{
				return this.ToQuotedPrintable(body, bodyEncoding);
			}
			return body;
		}

		private string EncodeBody(AlternateView av)
		{
			byte[] array = new byte[av.ContentStream.Length];
			av.ContentStream.Read(array, 0, array.Length);
			System.Net.Mime.TransferEncoding transferEncoding = av.TransferEncoding;
			if (transferEncoding == System.Net.Mime.TransferEncoding.Base64)
			{
				return Convert.ToBase64String(array, Base64FormattingOptions.InsertLineBreaks);
			}
			if (transferEncoding != System.Net.Mime.TransferEncoding.SevenBit)
			{
				return this.ToQuotedPrintable(array);
			}
			return Encoding.ASCII.GetString(array);
		}

		private void EndSection(string section)
		{
			this.SendData(string.Format("--{0}--", section));
			this.SendData(string.Empty);
		}

		private string GenerateBoundary()
		{
			string result = SmtpClient.GenerateBoundary(this.boundaryIndex);
			this.boundaryIndex++;
			return result;
		}

		private static string GenerateBoundary(int index)
		{
			return string.Format("--boundary_{0}_{1}", index, Guid.NewGuid().ToString("D"));
		}

		private bool IsError(SmtpClient.SmtpResponse status)
		{
			return status.StatusCode >= (SmtpStatusCode)400;
		}

		/// <summary>Raises the <see cref="E:System.Net.Mail.SmtpClient.SendCompleted" /> event.</summary>
		/// <param name="e">An <see cref="T:System.ComponentModel.AsyncCompletedEventArgs" /> that contains event data.</param>
		protected void OnSendCompleted(System.ComponentModel.AsyncCompletedEventArgs e)
		{
			try
			{
				if (this.SendCompleted != null)
				{
					this.SendCompleted(this, e);
				}
			}
			finally
			{
				this.worker = null;
				this.user_async_state = null;
			}
		}

		private void CheckCancellation()
		{
			if (this.worker != null && this.worker.CancellationPending)
			{
				throw new SmtpClient.CancellationException();
			}
		}

		private SmtpClient.SmtpResponse Read()
		{
			byte[] array = new byte[512];
			int num = 0;
			bool flag = false;
			do
			{
				this.CheckCancellation();
				int num2 = this.stream.Read(array, num, array.Length - num);
				if (num2 <= 0)
				{
					break;
				}
				int num3 = num + num2 - 1;
				if (num3 > 4 && (array[num3] == 10 || array[num3] == 13))
				{
					int num4 = num3 - 3;
					while (num4 >= 0 && array[num4] != 10 && array[num4] != 13)
					{
						num4--;
					}
					flag = (array[num4 + 4] == 32);
				}
				num += num2;
				if (num == array.Length)
				{
					byte[] array2 = new byte[array.Length * 2];
					Array.Copy(array, 0, array2, 0, array.Length);
					array = array2;
				}
			}
			while (!flag);
			if (num > 0)
			{
				Encoding encoding = new ASCIIEncoding();
				string @string = encoding.GetString(array, 0, num - 1);
				return SmtpClient.SmtpResponse.Parse(@string);
			}
			throw new IOException("Connection closed");
		}

		private void ResetExtensions()
		{
			this.authMechs = SmtpClient.AuthMechs.None;
		}

		private void ParseExtensions(string extens)
		{
			char[] separator = new char[]
			{
				' '
			};
			string[] array = extens.Split(new char[]
			{
				'\n'
			});
			foreach (string text in array)
			{
				if (text.Length >= 4)
				{
					string text2 = text.Substring(4);
					if (text2.StartsWith("AUTH ", StringComparison.Ordinal))
					{
						string[] array3 = text2.Split(separator);
						for (int j = 1; j < array3.Length; j++)
						{
							string text3 = array3[j].Trim();
							string text4 = text3;
							switch (text4)
							{
							case "CRAM-MD5":
								this.authMechs |= SmtpClient.AuthMechs.CramMD5;
								break;
							case "DIGEST-MD5":
								this.authMechs |= SmtpClient.AuthMechs.DigestMD5;
								break;
							case "GSSAPI":
								this.authMechs |= SmtpClient.AuthMechs.GssAPI;
								break;
							case "KERBEROS_V4":
								this.authMechs |= SmtpClient.AuthMechs.Kerberos4;
								break;
							case "LOGIN":
								this.authMechs |= SmtpClient.AuthMechs.Login;
								break;
							case "PLAIN":
								this.authMechs |= SmtpClient.AuthMechs.Plain;
								break;
							}
						}
					}
				}
			}
		}

		/// <summary>Sends the specified message to an SMTP server for delivery.</summary>
		/// <param name="message">A <see cref="T:System.Net.Mail.MailMessage" /> that contains the message to send.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <see cref="P:System.Net.Mail.MailMessage.From" /> is null.-or-<see cref="P:System.Net.Mail.MailMessage.To" /> is null.-or- <paramref name="message" /> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">There are no recipients in <see cref="P:System.Net.Mail.MailMessage.To" />, <see cref="P:System.Net.Mail.MailMessage.CC" />, and <see cref="P:System.Net.Mail.MailMessage.Bcc" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">This <see cref="T:System.Net.Mail.SmtpClient" /> has a <see cref="Overload:System.Net.Mail.SmtpClient.SendAsync" /> call in progress.-or- <see cref="P:System.Net.Mail.SmtpClient.Host" /> is null.-or-<see cref="P:System.Net.Mail.SmtpClient.Host" /> is equal to the empty string ("").-or- <see cref="P:System.Net.Mail.SmtpClient.Port" /> is zero.</exception>
		/// <exception cref="T:System.ObjectDisposedException">This object has been disposed.</exception>
		/// <exception cref="T:System.Net.Mail.SmtpException">The connection to the SMTP server failed.-or-Authentication failed.-or-The operation timed out.</exception>
		/// <exception cref="T:System.Net.Mail.SmtpFailedRecipientsException">The message could not be delivered to one or more of the recipients in <see cref="P:System.Net.Mail.MailMessage.To" />, <see cref="P:System.Net.Mail.MailMessage.CC" />, or <see cref="P:System.Net.Mail.MailMessage.Bcc" />.</exception>
		public void Send(MailMessage message)
		{
			if (message == null)
			{
				throw new ArgumentNullException("message");
			}
			if (this.deliveryMethod == SmtpDeliveryMethod.Network && (this.Host == null || this.Host.Trim().Length == 0))
			{
				throw new InvalidOperationException("The SMTP host was not specified");
			}
			if (this.deliveryMethod == SmtpDeliveryMethod.PickupDirectoryFromIis)
			{
				throw new NotSupportedException("IIS delivery is not supported");
			}
			if (this.port == 0)
			{
				this.port = 25;
			}
			this.mutex.WaitOne();
			try
			{
				this.messageInProcess = message;
				if (this.deliveryMethod == SmtpDeliveryMethod.SpecifiedPickupDirectory)
				{
					this.SendToFile(message);
				}
				else
				{
					this.SendInternal(message);
				}
			}
			catch (SmtpClient.CancellationException)
			{
			}
			catch (SmtpException)
			{
				throw;
			}
			catch (Exception innerException)
			{
				throw new SmtpException("Message could not be sent.", innerException);
			}
			finally
			{
				this.mutex.ReleaseMutex();
				this.messageInProcess = null;
			}
		}

		private void SendInternal(MailMessage message)
		{
			this.CheckCancellation();
			try
			{
				this.client = new System.Net.Sockets.TcpClient(this.host, this.port);
				this.stream = this.client.GetStream();
				this.writer = new StreamWriter(this.stream);
				this.reader = new StreamReader(this.stream);
				this.SendCore(message);
			}
			finally
			{
				if (this.writer != null)
				{
					this.writer.Close();
				}
				if (this.reader != null)
				{
					this.reader.Close();
				}
				if (this.stream != null)
				{
					this.stream.Close();
				}
				if (this.client != null)
				{
					this.client.Close();
				}
			}
		}

		private void SendToFile(MailMessage message)
		{
			if (!Path.IsPathRooted(this.pickupDirectoryLocation))
			{
				throw new SmtpException("Only absolute directories are allowed for pickup directory.");
			}
			string path = Path.Combine(this.pickupDirectoryLocation, Guid.NewGuid() + ".eml");
			try
			{
				this.writer = new StreamWriter(path);
				MailAddress from = message.From;
				if (from == null)
				{
					from = this.defaultFrom;
				}
				this.SendHeader("Date", DateTime.Now.ToString("ddd, dd MMM yyyy HH':'mm':'ss zzz", DateTimeFormatInfo.InvariantInfo));
				this.SendHeader("From", from.ToString());
				this.SendHeader("To", message.To.ToString());
				if (message.CC.Count > 0)
				{
					this.SendHeader("Cc", message.CC.ToString());
				}
				this.SendHeader("Subject", this.EncodeSubjectRFC2047(message));
				foreach (string name in message.Headers.AllKeys)
				{
					this.SendHeader(name, message.Headers[name]);
				}
				this.AddPriorityHeader(message);
				this.boundaryIndex = 0;
				if (message.Attachments.Count > 0)
				{
					this.SendWithAttachments(message);
				}
				else
				{
					this.SendWithoutAttachments(message, null, false);
				}
			}
			finally
			{
				if (this.writer != null)
				{
					this.writer.Close();
				}
				this.writer = null;
			}
		}

		private void SendCore(MailMessage message)
		{
			SmtpClient.SmtpResponse status = this.Read();
			if (this.IsError(status))
			{
				throw new SmtpException(status.StatusCode, status.Description);
			}
			status = this.SendCommand("EHLO " + Dns.GetHostName());
			if (this.IsError(status))
			{
				status = this.SendCommand("HELO " + Dns.GetHostName());
				if (this.IsError(status))
				{
					throw new SmtpException(status.StatusCode, status.Description);
				}
			}
			else
			{
				string description = status.Description;
				if (description != null)
				{
					this.ParseExtensions(description);
				}
			}
			if (this.enableSsl)
			{
				this.InitiateSecureConnection();
				this.ResetExtensions();
				this.writer = new StreamWriter(this.stream);
				this.reader = new StreamReader(this.stream);
				status = this.SendCommand("EHLO " + Dns.GetHostName());
				if (this.IsError(status))
				{
					status = this.SendCommand("HELO " + Dns.GetHostName());
					if (this.IsError(status))
					{
						throw new SmtpException(status.StatusCode, status.Description);
					}
				}
				else
				{
					string description2 = status.Description;
					if (description2 != null)
					{
						this.ParseExtensions(description2);
					}
				}
			}
			if (this.authMechs != SmtpClient.AuthMechs.None)
			{
				this.Authenticate();
			}
			MailAddress from = message.From;
			if (from == null)
			{
				from = this.defaultFrom;
			}
			status = this.SendCommand("MAIL FROM:<" + from.Address + '>');
			if (this.IsError(status))
			{
				throw new SmtpException(status.StatusCode, status.Description);
			}
			List<SmtpFailedRecipientException> list = new List<SmtpFailedRecipientException>();
			for (int i = 0; i < message.To.Count; i++)
			{
				status = this.SendCommand("RCPT TO:<" + message.To[i].Address + '>');
				if (this.IsError(status))
				{
					list.Add(new SmtpFailedRecipientException(status.StatusCode, message.To[i].Address));
				}
			}
			for (int j = 0; j < message.CC.Count; j++)
			{
				status = this.SendCommand("RCPT TO:<" + message.CC[j].Address + '>');
				if (this.IsError(status))
				{
					list.Add(new SmtpFailedRecipientException(status.StatusCode, message.CC[j].Address));
				}
			}
			for (int k = 0; k < message.Bcc.Count; k++)
			{
				status = this.SendCommand("RCPT TO:<" + message.Bcc[k].Address + '>');
				if (this.IsError(status))
				{
					list.Add(new SmtpFailedRecipientException(status.StatusCode, message.Bcc[k].Address));
				}
			}
			if (list.Count > 0)
			{
				throw new SmtpFailedRecipientsException("failed recipients", list.ToArray());
			}
			status = this.SendCommand("DATA");
			if (this.IsError(status))
			{
				throw new SmtpException(status.StatusCode, status.Description);
			}
			string text = DateTime.Now.ToString("ddd, dd MMM yyyy HH':'mm':'ss zzz", DateTimeFormatInfo.InvariantInfo);
			text = text.Remove(text.Length - 3, 1);
			this.SendHeader("Date", text);
			this.SendHeader("From", SmtpClient.EncodeAddress(from));
			this.SendHeader("To", SmtpClient.EncodeAddresses(message.To));
			if (message.CC.Count > 0)
			{
				this.SendHeader("Cc", SmtpClient.EncodeAddresses(message.CC));
			}
			this.SendHeader("Subject", this.EncodeSubjectRFC2047(message));
			string value = "normal";
			switch (message.Priority)
			{
			case MailPriority.Normal:
				value = "normal";
				break;
			case MailPriority.Low:
				value = "non-urgent";
				break;
			case MailPriority.High:
				value = "urgent";
				break;
			}
			this.SendHeader("Priority", value);
			if (message.Sender != null)
			{
				this.SendHeader("Sender", SmtpClient.EncodeAddress(message.Sender));
			}
			if (message.ReplyToList.Count > 0)
			{
				this.SendHeader("Reply-To", SmtpClient.EncodeAddresses(message.ReplyToList));
			}
			foreach (string name in message.Headers.AllKeys)
			{
				this.SendHeader(name, message.Headers[name]);
			}
			this.AddPriorityHeader(message);
			this.boundaryIndex = 0;
			if (message.Attachments.Count > 0)
			{
				this.SendWithAttachments(message);
			}
			else
			{
				this.SendWithoutAttachments(message, null, false);
			}
			this.SendDot();
			status = this.Read();
			if (this.IsError(status))
			{
				throw new SmtpException(status.StatusCode, status.Description);
			}
			try
			{
				status = this.SendCommand("QUIT");
			}
			catch (IOException)
			{
			}
		}

		/// <summary>Sends the specified e-mail message to an SMTP server for delivery. The message sender, recipients, subject, and message body are specified using <see cref="T:System.String" /> objects.</summary>
		/// <param name="from">A <see cref="T:System.String" /> that contains the address information of the message sender.</param>
		/// <param name="recipients">A <see cref="T:System.String" /> that contains the addresses that the message is sent to.</param>
		/// <param name="subject">A <see cref="T:System.String" /> that contains the subject line for the message.</param>
		/// <param name="body">A <see cref="T:System.String" /> that contains the message body.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="from" /> is null.-or-<paramref name="recipient" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="from" /> is <see cref="F:System.String.Empty" />.-or-<paramref name="recipient" /> is <see cref="F:System.String.Empty" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">This <see cref="T:System.Net.Mail.SmtpClient" /> has a <see cref="Overload:System.Net.Mail.SmtpClient.SendAsync" /> call in progress.-or- <see cref="P:System.Net.Mail.SmtpClient.Host" /> is null.-or-<see cref="P:System.Net.Mail.SmtpClient.Host" /> is equal to the empty string ("").-or- <see cref="P:System.Net.Mail.SmtpClient.Port" /> is zero.</exception>
		/// <exception cref="T:System.ObjectDisposedException">This object has been disposed.</exception>
		/// <exception cref="T:System.Net.Mail.SmtpException">The connection to the SMTP server failed.-or-Authentication failed.-or-The operation timed out.</exception>
		/// <exception cref="T:System.Net.Mail.SmtpFailedRecipientsException">The message could not be delivered to one or more of the recipients in <paramref name="recipients" />. </exception>
		public void Send(string from, string to, string subject, string body)
		{
			this.Send(new MailMessage(from, to, subject, body));
		}

		private void SendDot()
		{
			this.writer.Write(".\r\n");
			this.writer.Flush();
		}

		private void SendData(string data)
		{
			if (string.IsNullOrEmpty(data))
			{
				this.writer.Write("\r\n");
				this.writer.Flush();
				return;
			}
			StringReader stringReader = new StringReader(data);
			bool flag = this.deliveryMethod == SmtpDeliveryMethod.Network;
			string text;
			while ((text = stringReader.ReadLine()) != null)
			{
				this.CheckCancellation();
				if (flag)
				{
					int i;
					for (i = 0; i < text.Length; i++)
					{
						if (text[i] != '.')
						{
							break;
						}
					}
					if (i > 0 && i == text.Length)
					{
						text += ".";
					}
				}
				this.writer.Write(text);
				this.writer.Write("\r\n");
			}
			this.writer.Flush();
		}

		/// <summary>Sends the specified e-mail message to an SMTP server for delivery. This method does not block the calling thread and allows the caller to pass an object to the method that is invoked when the operation completes. </summary>
		/// <param name="message">A <see cref="T:System.Net.Mail.MailMessage" /> that contains the message to send.</param>
		/// <param name="userToken">A user-defined object that is passed to the method invoked when the asynchronous operation completes.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <see cref="P:System.Net.Mail.MailMessage.From" /> is null.-or-<see cref="P:System.Net.Mail.MailMessage.To" /> is null.-or- <paramref name="message" /> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">There are no recipients in <see cref="P:System.Net.Mail.MailMessage.To" />, <see cref="P:System.Net.Mail.MailMessage.CC" />, and <see cref="P:System.Net.Mail.MailMessage.Bcc" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">This <see cref="T:System.Net.Mail.SmtpClient" /> has a <see cref="Overload:System.Net.Mail.SmtpClient.SendAsync" /> call in progress.-or- <see cref="P:System.Net.Mail.SmtpClient.Host" /> is null.-or-<see cref="P:System.Net.Mail.SmtpClient.Host" /> is equal to the empty string ("").-or- <see cref="P:System.Net.Mail.SmtpClient.Port" /> is zero.</exception>
		/// <exception cref="T:System.ObjectDisposedException">This object has been disposed.</exception>
		/// <exception cref="T:System.Net.Mail.SmtpException">The connection to the SMTP server failed.-or-Authentication failed.-or-The operation timed out.</exception>
		/// <exception cref="T:System.Net.Mail.SmtpFailedRecipientsException">The <paramref name="message" /> could not be delivered to one or more of the recipients in <see cref="P:System.Net.Mail.MailMessage.To" />, <see cref="P:System.Net.Mail.MailMessage.CC" />, or <see cref="P:System.Net.Mail.MailMessage.Bcc" />.</exception>
		public void SendAsync(MailMessage message, object userToken)
		{
			if (this.worker != null)
			{
				throw new InvalidOperationException("Another SendAsync operation is in progress");
			}
			this.worker = new System.ComponentModel.BackgroundWorker();
			this.worker.DoWork += delegate(object o, System.ComponentModel.DoWorkEventArgs ea)
			{
				try
				{
					this.user_async_state = ea.Argument;
					this.Send(message);
				}
				catch (Exception ex)
				{
					ea.Result = ex;
					throw ex;
				}
			};
			this.worker.WorkerSupportsCancellation = true;
			this.worker.RunWorkerCompleted += delegate(object o, System.ComponentModel.RunWorkerCompletedEventArgs ea)
			{
				this.OnSendCompleted(new System.ComponentModel.AsyncCompletedEventArgs(ea.Error, ea.Cancelled, this.user_async_state));
			};
			this.worker.RunWorkerAsync(userToken);
		}

		/// <summary>Sends an e-mail message to an SMTP server for delivery. The message sender, recipients, subject, and message body are specified using <see cref="T:System.String" /> objects. This method does not block the calling thread and allows the caller to pass an object to the method that is invoked when the operation completes.</summary>
		/// <param name="from">A <see cref="T:System.String" /> that contains the address information of the message sender.</param>
		/// <param name="recipients">A <see cref="T:System.String" /> that contains the address that the message is sent to.</param>
		/// <param name="subject">A <see cref="T:System.String" /> that contains the subject line for the message.</param>
		/// <param name="body">A <see cref="T:System.String" /> that contains the message body.</param>
		/// <param name="userToken">A user-defined object that is passed to the method invoked when the asynchronous operation completes.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="from" /> is null.-or-<paramref name="recipient" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="from" /> is <see cref="F:System.String.Empty" />.-or-<paramref name="recipient" /> is <see cref="F:System.String.Empty" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">This <see cref="T:System.Net.Mail.SmtpClient" /> has a <see cref="Overload:System.Net.Mail.SmtpClient.SendAsync" /> call in progress.-or- <see cref="P:System.Net.Mail.SmtpClient.Host" /> is null.-or-<see cref="P:System.Net.Mail.SmtpClient.Host" /> is equal to the empty string ("").-or- <see cref="P:System.Net.Mail.SmtpClient.Port" /> is zero.</exception>
		/// <exception cref="T:System.ObjectDisposedException">This object has been disposed.</exception>
		/// <exception cref="T:System.Net.Mail.SmtpException">The connection to the SMTP server failed.-or-Authentication failed.-or-The operation timed out.</exception>
		/// <exception cref="T:System.Net.Mail.SmtpFailedRecipientsException">The message could not be delivered to one or more of the recipients in <paramref name="recipients" />. </exception>
		public void SendAsync(string from, string to, string subject, string body, object userToken)
		{
			this.SendAsync(new MailMessage(from, to, subject, body), userToken);
		}

		/// <summary>Cancels an asynchronous operation to send an e-mail message.</summary>
		/// <exception cref="T:System.ObjectDisposedException">This object has been disposed.</exception>
		public void SendAsyncCancel()
		{
			if (this.worker == null)
			{
				throw new InvalidOperationException("SendAsync operation is not in progress");
			}
			this.worker.CancelAsync();
		}

		private void AddPriorityHeader(MailMessage message)
		{
			MailPriority priority = message.Priority;
			if (priority != MailPriority.Low)
			{
				if (priority == MailPriority.High)
				{
					this.SendHeader("Priority", "Urgent");
					this.SendHeader("Importance", "high");
					this.SendHeader("X-Priority", "1");
				}
			}
			else
			{
				this.SendHeader("Priority", "Non-Urgent");
				this.SendHeader("Importance", "low");
				this.SendHeader("X-Priority", "5");
			}
		}

		private void SendSimpleBody(MailMessage message)
		{
			this.SendHeader("Content-Type", message.BodyContentType.ToString());
			if (message.ContentTransferEncoding != System.Net.Mime.TransferEncoding.SevenBit)
			{
				this.SendHeader("Content-Transfer-Encoding", SmtpClient.GetTransferEncodingName(message.ContentTransferEncoding));
			}
			this.SendData(string.Empty);
			this.SendData(this.EncodeBody(message));
		}

		private void SendBodylessSingleAlternate(AlternateView av)
		{
			this.SendHeader("Content-Type", av.ContentType.ToString());
			if (av.TransferEncoding != System.Net.Mime.TransferEncoding.SevenBit)
			{
				this.SendHeader("Content-Transfer-Encoding", SmtpClient.GetTransferEncodingName(av.TransferEncoding));
			}
			this.SendData(string.Empty);
			this.SendData(this.EncodeBody(av));
		}

		private void SendWithoutAttachments(MailMessage message, string boundary, bool attachmentExists)
		{
			if (message.Body == null && message.AlternateViews.Count == 1)
			{
				this.SendBodylessSingleAlternate(message.AlternateViews[0]);
			}
			else if (message.AlternateViews.Count > 0)
			{
				this.SendBodyWithAlternateViews(message, boundary, attachmentExists);
			}
			else
			{
				this.SendSimpleBody(message);
			}
		}

		private void SendWithAttachments(MailMessage message)
		{
			string text = this.GenerateBoundary();
			this.SendHeader("Content-Type", new System.Net.Mime.ContentType
			{
				Boundary = text,
				MediaType = "multipart/mixed",
				CharSet = null
			}.ToString());
			this.SendData(string.Empty);
			Attachment attachment = null;
			if (message.AlternateViews.Count > 0)
			{
				this.SendWithoutAttachments(message, text, true);
			}
			else
			{
				attachment = Attachment.CreateAttachmentFromString(message.Body, null, message.BodyEncoding, (!message.IsBodyHtml) ? "text/plain" : "text/html");
				message.Attachments.Insert(0, attachment);
			}
			try
			{
				this.SendAttachments(message, attachment, text);
			}
			finally
			{
				if (attachment != null)
				{
					message.Attachments.Remove(attachment);
				}
			}
			this.EndSection(text);
		}

		private void SendBodyWithAlternateViews(MailMessage message, string boundary, bool attachmentExists)
		{
			AlternateViewCollection alternateViews = message.AlternateViews;
			string text = this.GenerateBoundary();
			System.Net.Mime.ContentType contentType = new System.Net.Mime.ContentType();
			contentType.Boundary = text;
			contentType.MediaType = "multipart/alternative";
			if (!attachmentExists)
			{
				this.SendHeader("Content-Type", contentType.ToString());
				this.SendData(string.Empty);
			}
			AlternateView alternateView = null;
			if (message.Body != null)
			{
				alternateView = AlternateView.CreateAlternateViewFromString(message.Body, message.BodyEncoding, (!message.IsBodyHtml) ? "text/plain" : "text/html");
				alternateViews.Insert(0, alternateView);
				this.StartSection(boundary, contentType);
			}
			try
			{
				foreach (AlternateView alternateView2 in alternateViews)
				{
					string text2 = null;
					if (alternateView2.LinkedResources.Count > 0)
					{
						text2 = this.GenerateBoundary();
						System.Net.Mime.ContentType contentType2 = new System.Net.Mime.ContentType("multipart/related");
						contentType2.Boundary = text2;
						contentType2.Parameters["type"] = alternateView2.ContentType.ToString();
						this.StartSection(text, contentType2);
						this.StartSection(text2, alternateView2.ContentType, alternateView2.TransferEncoding);
					}
					else
					{
						System.Net.Mime.ContentType contentType2 = new System.Net.Mime.ContentType(alternateView2.ContentType.ToString());
						this.StartSection(text, contentType2, alternateView2.TransferEncoding);
					}
					System.Net.Mime.TransferEncoding transferEncoding = alternateView2.TransferEncoding;
					switch (transferEncoding + 1)
					{
					case System.Net.Mime.TransferEncoding.QuotedPrintable:
					case (System.Net.Mime.TransferEncoding)3:
					{
						byte[] array = new byte[alternateView2.ContentStream.Length];
						alternateView2.ContentStream.Read(array, 0, array.Length);
						this.SendData(Encoding.ASCII.GetString(array));
						break;
					}
					case System.Net.Mime.TransferEncoding.Base64:
					{
						byte[] array2 = new byte[alternateView2.ContentStream.Length];
						alternateView2.ContentStream.Read(array2, 0, array2.Length);
						this.SendData(this.ToQuotedPrintable(array2));
						break;
					}
					case System.Net.Mime.TransferEncoding.SevenBit:
					{
						byte[] array = new byte[alternateView2.ContentStream.Length];
						alternateView2.ContentStream.Read(array, 0, array.Length);
						this.SendData(Convert.ToBase64String(array, Base64FormattingOptions.InsertLineBreaks));
						break;
					}
					}
					if (alternateView2.LinkedResources.Count > 0)
					{
						this.SendLinkedResources(message, alternateView2.LinkedResources, text2);
						this.EndSection(text2);
					}
					if (!attachmentExists)
					{
						this.SendData(string.Empty);
					}
				}
			}
			finally
			{
				if (alternateView != null)
				{
					alternateViews.Remove(alternateView);
				}
			}
			this.EndSection(text);
		}

		private void SendLinkedResources(MailMessage message, LinkedResourceCollection resources, string boundary)
		{
			foreach (LinkedResource linkedResource in resources)
			{
				this.StartSection(boundary, linkedResource.ContentType, linkedResource.TransferEncoding, linkedResource);
				System.Net.Mime.TransferEncoding transferEncoding = linkedResource.TransferEncoding;
				switch (transferEncoding + 1)
				{
				case System.Net.Mime.TransferEncoding.QuotedPrintable:
				case (System.Net.Mime.TransferEncoding)3:
				{
					byte[] array = new byte[linkedResource.ContentStream.Length];
					linkedResource.ContentStream.Read(array, 0, array.Length);
					this.SendData(Encoding.ASCII.GetString(array));
					break;
				}
				case System.Net.Mime.TransferEncoding.Base64:
				{
					byte[] array2 = new byte[linkedResource.ContentStream.Length];
					linkedResource.ContentStream.Read(array2, 0, array2.Length);
					this.SendData(this.ToQuotedPrintable(array2));
					break;
				}
				case System.Net.Mime.TransferEncoding.SevenBit:
				{
					byte[] array = new byte[linkedResource.ContentStream.Length];
					linkedResource.ContentStream.Read(array, 0, array.Length);
					this.SendData(Convert.ToBase64String(array, Base64FormattingOptions.InsertLineBreaks));
					break;
				}
				}
			}
		}

		private void SendAttachments(MailMessage message, Attachment body, string boundary)
		{
			foreach (Attachment attachment in message.Attachments)
			{
				System.Net.Mime.ContentType contentType = new System.Net.Mime.ContentType(attachment.ContentType.ToString());
				if (attachment.Name != null)
				{
					contentType.Name = attachment.Name;
					if (attachment.NameEncoding != null)
					{
						contentType.CharSet = attachment.NameEncoding.HeaderName;
					}
					attachment.ContentDisposition.FileName = attachment.Name;
				}
				this.StartSection(boundary, contentType, attachment.TransferEncoding, (attachment != body) ? attachment.ContentDisposition : null);
				byte[] array = new byte[attachment.ContentStream.Length];
				attachment.ContentStream.Read(array, 0, array.Length);
				System.Net.Mime.TransferEncoding transferEncoding = attachment.TransferEncoding;
				switch (transferEncoding + 1)
				{
				case System.Net.Mime.TransferEncoding.QuotedPrintable:
				case (System.Net.Mime.TransferEncoding)3:
					this.SendData(Encoding.ASCII.GetString(array));
					break;
				case System.Net.Mime.TransferEncoding.Base64:
					this.SendData(this.ToQuotedPrintable(array));
					break;
				case System.Net.Mime.TransferEncoding.SevenBit:
					this.SendData(Convert.ToBase64String(array, Base64FormattingOptions.InsertLineBreaks));
					break;
				}
				this.SendData(string.Empty);
			}
		}

		private SmtpClient.SmtpResponse SendCommand(string command)
		{
			this.writer.Write(command);
			this.writer.Write("\r\n");
			this.writer.Flush();
			return this.Read();
		}

		private void SendHeader(string name, string value)
		{
			this.SendData(string.Format("{0}: {1}", name, value));
		}

		private void StartSection(string section, System.Net.Mime.ContentType sectionContentType)
		{
			this.SendData(string.Format("--{0}", section));
			this.SendHeader("content-type", sectionContentType.ToString());
			this.SendData(string.Empty);
		}

		private void StartSection(string section, System.Net.Mime.ContentType sectionContentType, System.Net.Mime.TransferEncoding transferEncoding)
		{
			this.SendData(string.Format("--{0}", section));
			this.SendHeader("content-type", sectionContentType.ToString());
			this.SendHeader("content-transfer-encoding", SmtpClient.GetTransferEncodingName(transferEncoding));
			this.SendData(string.Empty);
		}

		private void StartSection(string section, System.Net.Mime.ContentType sectionContentType, System.Net.Mime.TransferEncoding transferEncoding, LinkedResource lr)
		{
			this.SendData(string.Format("--{0}", section));
			this.SendHeader("content-type", sectionContentType.ToString());
			this.SendHeader("content-transfer-encoding", SmtpClient.GetTransferEncodingName(transferEncoding));
			if (lr.ContentId != null && lr.ContentId.Length > 0)
			{
				this.SendHeader("content-ID", "<" + lr.ContentId + ">");
			}
			this.SendData(string.Empty);
		}

		private void StartSection(string section, System.Net.Mime.ContentType sectionContentType, System.Net.Mime.TransferEncoding transferEncoding, System.Net.Mime.ContentDisposition contentDisposition)
		{
			this.SendData(string.Format("--{0}", section));
			this.SendHeader("content-type", sectionContentType.ToString());
			this.SendHeader("content-transfer-encoding", SmtpClient.GetTransferEncodingName(transferEncoding));
			if (contentDisposition != null)
			{
				this.SendHeader("content-disposition", contentDisposition.ToString());
			}
			this.SendData(string.Empty);
		}

		private string ToQuotedPrintable(string input, Encoding enc)
		{
			byte[] bytes = enc.GetBytes(input);
			return this.ToQuotedPrintable(bytes);
		}

		private string ToQuotedPrintable(byte[] bytes)
		{
			StringWriter stringWriter = new StringWriter();
			int num = 0;
			StringBuilder stringBuilder = new StringBuilder("=", 3);
			byte b = 61;
			char c = '\0';
			int i = 0;
			while (i < bytes.Length)
			{
				byte b2 = bytes[i];
				int num2;
				if (b2 > 127 || b2 == b)
				{
					stringBuilder.Length = 1;
					stringBuilder.Append(Convert.ToString(b2, 16).ToUpperInvariant());
					num2 = 3;
					goto IL_8E;
				}
				c = Convert.ToChar(b2);
				if (c != '\r' && c != '\n')
				{
					num2 = 1;
					goto IL_8E;
				}
				stringWriter.Write(c);
				num = 0;
				IL_C7:
				i++;
				continue;
				IL_8E:
				num += num2;
				if (num > 75)
				{
					stringWriter.Write("=\r\n");
					num = num2;
				}
				if (num2 == 1)
				{
					stringWriter.Write(c);
					goto IL_C7;
				}
				stringWriter.Write(stringBuilder.ToString());
				goto IL_C7;
			}
			return stringWriter.ToString();
		}

		private static string GetTransferEncodingName(System.Net.Mime.TransferEncoding encoding)
		{
			switch (encoding)
			{
			case System.Net.Mime.TransferEncoding.QuotedPrintable:
				return "quoted-printable";
			case System.Net.Mime.TransferEncoding.Base64:
				return "base64";
			case System.Net.Mime.TransferEncoding.SevenBit:
				return "7bit";
			default:
				return "unknown";
			}
		}

		private void InitiateSecureConnection()
		{
			SmtpClient.SmtpResponse status = this.SendCommand("STARTTLS");
			if (this.IsError(status))
			{
				throw new SmtpException(SmtpStatusCode.GeneralFailure, "Server does not support secure connections.");
			}
			System.Net.Security.SslStream sslStream = new System.Net.Security.SslStream(this.stream, false, this.callback, null);
			this.CheckCancellation();
			sslStream.AuthenticateAsClient(this.Host, this.ClientCertificates, System.Security.Authentication.SslProtocols.Default, false);
			this.stream = sslStream;
		}

		private void Authenticate()
		{
			string userName;
			string password;
			if (this.UseDefaultCredentials)
			{
				userName = CredentialCache.DefaultCredentials.GetCredential(new System.Uri("smtp://" + this.host), "basic").UserName;
				password = CredentialCache.DefaultCredentials.GetCredential(new System.Uri("smtp://" + this.host), "basic").Password;
			}
			else
			{
				if (this.Credentials == null)
				{
					return;
				}
				userName = this.Credentials.GetCredential(this.host, this.port, "smtp").UserName;
				password = this.Credentials.GetCredential(this.host, this.port, "smtp").Password;
			}
			this.Authenticate(userName, password);
		}

		private void Authenticate(string Username, string Password)
		{
			SmtpClient.SmtpResponse status = this.SendCommand("AUTH LOGIN");
			if (status.StatusCode != (SmtpStatusCode)334)
			{
				throw new SmtpException(status.StatusCode, status.Description);
			}
			status = this.SendCommand(Convert.ToBase64String(Encoding.ASCII.GetBytes(Username)));
			if (status.StatusCode != (SmtpStatusCode)334)
			{
				throw new SmtpException(status.StatusCode, status.Description);
			}
			status = this.SendCommand(Convert.ToBase64String(Encoding.ASCII.GetBytes(Password)));
			if (this.IsError(status))
			{
				throw new SmtpException(status.StatusCode, status.Description);
			}
		}

		[Flags]
		private enum AuthMechs
		{
			None = 0,
			CramMD5 = 1,
			DigestMD5 = 2,
			GssAPI = 4,
			Kerberos4 = 8,
			Login = 16,
			Plain = 32
		}

		private class CancellationException : Exception
		{
		}

		private struct HeaderName
		{
			public const string ContentTransferEncoding = "Content-Transfer-Encoding";

			public const string ContentType = "Content-Type";

			public const string Bcc = "Bcc";

			public const string Cc = "Cc";

			public const string From = "From";

			public const string Subject = "Subject";

			public const string To = "To";

			public const string MimeVersion = "MIME-Version";

			public const string MessageId = "Message-ID";

			public const string Priority = "Priority";

			public const string Importance = "Importance";

			public const string XPriority = "X-Priority";

			public const string Date = "Date";
		}

		private struct SmtpResponse
		{
			public SmtpStatusCode StatusCode;

			public string Description;

			public static SmtpClient.SmtpResponse Parse(string line)
			{
				SmtpClient.SmtpResponse result = default(SmtpClient.SmtpResponse);
				if (line.Length < 4)
				{
					throw new SmtpException("Response is to short " + line.Length + ".");
				}
				if (line[3] != ' ' && line[3] != '-')
				{
					throw new SmtpException("Response format is wrong.(" + line + ")");
				}
				result.StatusCode = (SmtpStatusCode)int.Parse(line.Substring(0, 3));
				result.Description = line;
				return result;
			}
		}
	}
}
