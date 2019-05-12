using System;
using System.Net;
using System.Text.RegularExpressions;

namespace Neptune.Cloud.Core
{
	public class NpCloudSetting
	{
		public NpCloudSetting(NpCloudSocketType type, string url)
		{
			this.Url = url;
			this.init(type, url);
		}

		public IPAddress[] HostAddress { get; set; }

		public string HostName { get; set; }

		public int Port { get; set; }

		public string ProjectId { get; set; }

		public uint UserId { get; set; }

		public string Url { get; set; }

		private void init(NpCloudSocketType type, string url)
		{
			Regex regex;
			if (type != NpCloudSocketType.TCP)
			{
				if (type != NpCloudSocketType.Web)
				{
					throw new NpCloudException(701, "Switchのcaseがdefaultだったよ = {0}");
				}
				regex = new Regex("ws://(?<host>.+):(?<port>[0-9]+)/\\?projectid=(?<projectId>.+)&me=(?<userId>.+)$");
			}
			else
			{
				regex = new Regex("tcp://(?<host>.+):(?<port>[0-9]+)/\\?projectid=(?<projectId>.+)&me=(?<userId>.+)$");
			}
			Match match = regex.Match(url);
			if (!match.Success)
			{
				throw new Exception("接続URLが不正です.");
			}
			this.HostName = match.Groups["host"].Value;
			this.Port = int.Parse(match.Groups["port"].Value);
			this.ProjectId = match.Groups["projectId"].Value;
			this.UserId = uint.Parse(match.Groups["userId"].Value);
			IPHostEntry hostEntry = Dns.GetHostEntry(this.HostName);
			this.HostAddress = hostEntry.AddressList;
			if (this.HostAddress == null || this.HostAddress.Length == 0)
			{
				throw new Exception("IPアドレスが取得できません.");
			}
		}
	}
}
