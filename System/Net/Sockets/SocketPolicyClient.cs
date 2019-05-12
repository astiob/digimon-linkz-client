using System;
using System.IO;
using System.Text;

namespace System.Net.Sockets
{
	internal static class SocketPolicyClient
	{
		private const string policy_request = "<policy-file-request/>\0";

		private static int session;

		private static void Log(string msg)
		{
			Console.WriteLine(string.Concat(new object[]
			{
				"SocketPolicyClient",
				SocketPolicyClient.session,
				": ",
				msg
			}));
		}

		private static Stream GetPolicyStreamForIP(string ip, int policyport, int timeout)
		{
			SocketPolicyClient.session++;
			SocketPolicyClient.Log("Incoming GetPolicyStreamForIP");
			IPEndPoint ipendPoint = new IPEndPoint(IPAddress.Parse(ip), policyport);
			Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			byte[] array = new byte[5000];
			MemoryStream memoryStream = new MemoryStream();
			try
			{
				SocketPolicyClient.Log("About to BeginConnect to " + ipendPoint);
				IAsyncResult asyncResult = socket.BeginConnect(ipendPoint, null, null, false);
				SocketPolicyClient.Log("About to WaitOne");
				DateTime now = DateTime.Now;
				if (!asyncResult.AsyncWaitHandle.WaitOne(timeout))
				{
					SocketPolicyClient.Log("WaitOne timed out. Duration: " + (DateTime.Now - now).TotalMilliseconds);
					socket.Close();
					throw new Exception("BeginConnect timed out");
				}
				socket.EndConnect(asyncResult);
				SocketPolicyClient.Log("Socket connected");
				byte[] bytes = Encoding.ASCII.GetBytes("<policy-file-request/>\0");
				SocketError socketError;
				socket.Send_nochecks(bytes, 0, bytes.Length, SocketFlags.None, out socketError);
				if (socketError != SocketError.Success)
				{
					SocketPolicyClient.Log("Socket error: " + socketError);
					return memoryStream;
				}
				int count = socket.Receive_nochecks(array, 0, array.Length, SocketFlags.None, out socketError);
				if (socketError != SocketError.Success)
				{
					SocketPolicyClient.Log("Socket error: " + socketError);
					return memoryStream;
				}
				try
				{
					socket.Shutdown(SocketShutdown.Both);
					socket.Close();
				}
				catch (SocketException)
				{
				}
				memoryStream = new MemoryStream(array, 0, count);
			}
			catch (Exception ex)
			{
				SocketPolicyClient.Log("Caught exception: " + ex.Message);
				return memoryStream;
			}
			memoryStream.Seek(0L, SeekOrigin.Begin);
			return memoryStream;
		}
	}
}
