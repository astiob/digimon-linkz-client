using Firebase.Platform.Default;
using System;

namespace Firebase.Platform
{
	internal static class Services
	{
		public static IAppConfigExtensions AppConfig { get; internal set; } = AppConfigExtensions.Instance;

		public static IAuthService Auth { get; internal set; } = BaseAuthService.BaseInstance;

		public static ICertificateService RootCerts { get; internal set; } = NoopCertificateService.Instance;

		public static IClockService Clock { get; internal set; } = SystemClock.Instance;

		public static IHttpFactoryService HttpFactory { get; internal set; }

		public static ILoggingService Logging { get; internal set; } = DebugLogger.Instance;
	}
}
