using FacebookGames;
using System;
using System.IO;
using System.Threading;

namespace FacebookPlatformServiceClient
{
	public class FacebookNamedPipeClient : IDisposable
	{
		private readonly string pipeName;

		private NamedPipeStream pipeClient;

		public volatile PipePacketResponse PipeResponse;

		public FacebookNamedPipeClient(string pipeName)
		{
			if (pipeName == null)
			{
				throw new ArgumentNullException("pipeName");
			}
			this.pipeName = pipeName;
		}

		private bool Initialized
		{
			get
			{
				return this.pipeName != null && this.pipeName.Length > 0;
			}
		}

		protected bool ValidateCommunicationChannel()
		{
			if (!this.Initialized)
			{
				return false;
			}
			if (this.pipeClient != null)
			{
				return true;
			}
			bool result;
			try
			{
				result = this.AttemptConnectionToCommunicationChannel();
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		private bool AttemptConnectionToCommunicationChannel()
		{
			this.pipeClient = new NamedPipeStream("\\\\.\\pipe\\" + this.pipeName, FileAccess.ReadWrite);
			return true;
		}

		public void SendRequest<T>(PipePacket request) where T : PipePacketResponse
		{
			if (!this.ValidateCommunicationChannel())
			{
				throw new Exception("Failed to establish communication with the server.");
			}
			PipeCommunicationHelper.SendPacket(this.pipeClient, request);
			ThreadPool.QueueUserWorkItem(new WaitCallback(this.ReceiveMessage<T>));
		}

		private void ReceiveMessage<T>(object threadContext) where T : PipePacketResponse
		{
			this.PipeResponse = PipeCommunicationHelper.ReadPacket<T>(this.pipeClient);
		}

		public void Dispose()
		{
			this.pipeClient.Close();
		}
	}
}
