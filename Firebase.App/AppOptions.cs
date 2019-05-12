using System;

namespace Firebase
{
	public sealed class AppOptions : IDisposable
	{
		public AppOptions()
		{
		}

		internal AppOptions(AppOptionsInternal other)
		{
			this.DatabaseUrl = other.DatabaseUrl;
			this.AppId = other.AppId;
			this.ApiKey = other.ApiKey;
			this.MessageSenderId = other.MessageSenderId;
			this.StorageBucket = other.StorageBucket;
			this.ProjectId = other.ProjectId;
			this.PackageName = other.PackageName;
		}

		public void Dispose()
		{
		}

		public Uri DatabaseUrl { get; set; }

		public string AppId { get; set; }

		public string ApiKey { get; set; }

		public string MessageSenderId { get; set; }

		public string StorageBucket { get; set; }

		public string ProjectId { get; set; }

		internal string PackageName { get; set; }

		public static AppOptions LoadFromJsonConfig(string jsonConfigFilename)
		{
			AppOptionsInternal appOptionsInternal = AppUtil.AppOptionsLoadFromJsonConfig(jsonConfigFilename);
			if (appOptionsInternal == null)
			{
				return null;
			}
			return new AppOptions(appOptionsInternal);
		}

		internal AppOptionsInternal ConvertToInternal()
		{
			AppOptionsInternal appOptionsInternal = new AppOptionsInternal();
			appOptionsInternal.DatabaseUrl = this.DatabaseUrl;
			if (!string.IsNullOrEmpty(this.AppId))
			{
				appOptionsInternal.AppId = this.AppId;
			}
			if (!string.IsNullOrEmpty(this.ApiKey))
			{
				appOptionsInternal.ApiKey = this.ApiKey;
			}
			if (!string.IsNullOrEmpty(this.MessageSenderId))
			{
				appOptionsInternal.MessageSenderId = this.MessageSenderId;
			}
			if (!string.IsNullOrEmpty(this.StorageBucket))
			{
				appOptionsInternal.StorageBucket = this.StorageBucket;
			}
			if (!string.IsNullOrEmpty(this.ProjectId))
			{
				appOptionsInternal.ProjectId = this.ProjectId;
			}
			if (!string.IsNullOrEmpty(this.PackageName))
			{
				appOptionsInternal.PackageName = this.PackageName;
			}
			return appOptionsInternal;
		}
	}
}
