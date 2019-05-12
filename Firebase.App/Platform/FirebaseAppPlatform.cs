using System;

namespace Firebase.Platform
{
	internal class FirebaseAppPlatform : IFirebaseAppPlatform
	{
		internal FirebaseAppPlatform(FirebaseApp wrappedApp)
		{
			this.app = new WeakReference(wrappedApp, false);
		}

		private WeakReference app { get; set; }

		public object AppObject
		{
			get
			{
				object result;
				try
				{
					result = this.app.Target;
				}
				catch (InvalidOperationException)
				{
					result = null;
				}
				return result;
			}
		}

		internal FirebaseApp App
		{
			get
			{
				return this.AppObject as FirebaseApp;
			}
		}

		public string Name
		{
			get
			{
				FirebaseApp app = this.App;
				if (app != null)
				{
					return app.Name;
				}
				return null;
			}
		}

		public Uri DatabaseUrl
		{
			get
			{
				FirebaseApp app = this.App;
				if (app != null)
				{
					return app.Options.DatabaseUrl;
				}
				return null;
			}
		}
	}
}
