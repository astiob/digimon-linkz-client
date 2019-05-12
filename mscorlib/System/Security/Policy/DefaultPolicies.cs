using System;
using System.Collections.Generic;
using System.Security.Permissions;

namespace System.Security.Policy
{
	internal static class DefaultPolicies
	{
		private const string DnsPermissionClass = "System.Net.DnsPermission, System, Version=2.0.5.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";

		private const string EventLogPermissionClass = "System.Diagnostics.EventLogPermission, System, Version=2.0.5.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";

		private const string PrintingPermissionClass = "System.Drawing.Printing.PrintingPermission, System.Drawing, Version=2.0.5.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";

		private const string SocketPermissionClass = "System.Net.SocketPermission, System, Version=2.0.5.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";

		private const string WebPermissionClass = "System.Net.WebPermission, System, Version=2.0.5.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";

		private const string PerformanceCounterPermissionClass = "System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.5.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";

		private const string DirectoryServicesPermissionClass = "System.DirectoryServices.DirectoryServicesPermission, System.DirectoryServices, Version=2.0.5.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";

		private const string MessageQueuePermissionClass = "System.Messaging.MessageQueuePermission, System.Messaging, Version=2.0.5.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";

		private const string ServiceControllerPermissionClass = "System.ServiceProcess.ServiceControllerPermission, System.ServiceProcess, Version=2.0.5.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";

		private const string OleDbPermissionClass = "System.Data.OleDb.OleDbPermission, System.Data, Version=2.0.5.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";

		private const string SqlClientPermissionClass = "System.Data.SqlClient.SqlClientPermission, System.Data, Version=2.0.5.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";

		private const string DataProtectionPermissionClass = "System.Security.Permissions.DataProtectionPermission, System.Security, Version=2.0.5.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";

		private const string StorePermissionClass = "System.Security.Permissions.StorePermission, System.Security, Version=2.0.5.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";

		private static Version _fxVersion;

		private static byte[] _ecmaKey;

		private static StrongNamePublicKeyBlob _ecma;

		private static byte[] _msFinalKey;

		private static StrongNamePublicKeyBlob _msFinal;

		private static NamedPermissionSet _fullTrust;

		private static NamedPermissionSet _localIntranet;

		private static NamedPermissionSet _internet;

		private static NamedPermissionSet _skipVerification;

		private static NamedPermissionSet _execution;

		private static NamedPermissionSet _nothing;

		private static NamedPermissionSet _everything;

		// Note: this type is marked as 'beforefieldinit'.
		static DefaultPolicies()
		{
			byte[] array = new byte[16];
			array[8] = 4;
			DefaultPolicies._ecmaKey = array;
			DefaultPolicies._msFinalKey = new byte[]
			{
				0,
				36,
				0,
				0,
				4,
				128,
				0,
				0,
				148,
				0,
				0,
				0,
				6,
				2,
				0,
				0,
				0,
				36,
				0,
				0,
				82,
				83,
				65,
				49,
				0,
				4,
				0,
				0,
				1,
				0,
				1,
				0,
				7,
				209,
				250,
				87,
				196,
				174,
				217,
				240,
				163,
				46,
				132,
				170,
				15,
				174,
				253,
				13,
				233,
				232,
				253,
				106,
				236,
				143,
				135,
				251,
				3,
				118,
				108,
				131,
				76,
				153,
				146,
				30,
				178,
				59,
				231,
				154,
				217,
				213,
				220,
				193,
				221,
				154,
				210,
				54,
				19,
				33,
				2,
				144,
				11,
				114,
				60,
				249,
				128,
				149,
				127,
				196,
				225,
				119,
				16,
				143,
				198,
				7,
				119,
				79,
				41,
				232,
				50,
				14,
				146,
				234,
				5,
				236,
				228,
				232,
				33,
				192,
				165,
				239,
				232,
				241,
				100,
				92,
				76,
				12,
				147,
				193,
				171,
				153,
				40,
				93,
				98,
				44,
				170,
				101,
				44,
				29,
				250,
				214,
				61,
				116,
				93,
				111,
				45,
				229,
				241,
				126,
				94,
				175,
				15,
				196,
				150,
				61,
				38,
				28,
				138,
				18,
				67,
				101,
				24,
				32,
				109,
				192,
				147,
				52,
				77,
				90,
				210,
				147
			};
		}

		public static PermissionSet GetSpecialPermissionSet(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			switch (name)
			{
			case "FullTrust":
				return DefaultPolicies.FullTrust;
			case "LocalIntranet":
				return DefaultPolicies.LocalIntranet;
			case "Internet":
				return DefaultPolicies.Internet;
			case "SkipVerification":
				return DefaultPolicies.SkipVerification;
			case "Execution":
				return DefaultPolicies.Execution;
			case "Nothing":
				return DefaultPolicies.Nothing;
			case "Everything":
				return DefaultPolicies.Everything;
			}
			return null;
		}

		public static PermissionSet FullTrust
		{
			get
			{
				if (DefaultPolicies._fullTrust == null)
				{
					DefaultPolicies._fullTrust = DefaultPolicies.BuildFullTrust();
				}
				return DefaultPolicies._fullTrust;
			}
		}

		public static PermissionSet LocalIntranet
		{
			get
			{
				if (DefaultPolicies._localIntranet == null)
				{
					DefaultPolicies._localIntranet = DefaultPolicies.BuildLocalIntranet();
				}
				return DefaultPolicies._localIntranet;
			}
		}

		public static PermissionSet Internet
		{
			get
			{
				if (DefaultPolicies._internet == null)
				{
					DefaultPolicies._internet = DefaultPolicies.BuildInternet();
				}
				return DefaultPolicies._internet;
			}
		}

		public static PermissionSet SkipVerification
		{
			get
			{
				if (DefaultPolicies._skipVerification == null)
				{
					DefaultPolicies._skipVerification = DefaultPolicies.BuildSkipVerification();
				}
				return DefaultPolicies._skipVerification;
			}
		}

		public static PermissionSet Execution
		{
			get
			{
				if (DefaultPolicies._execution == null)
				{
					DefaultPolicies._execution = DefaultPolicies.BuildExecution();
				}
				return DefaultPolicies._execution;
			}
		}

		public static PermissionSet Nothing
		{
			get
			{
				if (DefaultPolicies._nothing == null)
				{
					DefaultPolicies._nothing = DefaultPolicies.BuildNothing();
				}
				return DefaultPolicies._nothing;
			}
		}

		public static PermissionSet Everything
		{
			get
			{
				if (DefaultPolicies._everything == null)
				{
					DefaultPolicies._everything = DefaultPolicies.BuildEverything();
				}
				return DefaultPolicies._everything;
			}
		}

		public static StrongNameMembershipCondition FullTrustMembership(string name, DefaultPolicies.Key key)
		{
			StrongNamePublicKeyBlob blob = null;
			if (key != DefaultPolicies.Key.Ecma)
			{
				if (key == DefaultPolicies.Key.MsFinal)
				{
					if (DefaultPolicies._msFinal == null)
					{
						DefaultPolicies._msFinal = new StrongNamePublicKeyBlob(DefaultPolicies._msFinalKey);
					}
					blob = DefaultPolicies._msFinal;
				}
			}
			else
			{
				if (DefaultPolicies._ecma == null)
				{
					DefaultPolicies._ecma = new StrongNamePublicKeyBlob(DefaultPolicies._ecmaKey);
				}
				blob = DefaultPolicies._ecma;
			}
			if (DefaultPolicies._fxVersion == null)
			{
				DefaultPolicies._fxVersion = new Version("2.0.5.0");
			}
			return new StrongNameMembershipCondition(blob, name, DefaultPolicies._fxVersion);
		}

		private static NamedPermissionSet BuildFullTrust()
		{
			return new NamedPermissionSet("FullTrust", PermissionState.Unrestricted);
		}

		private static NamedPermissionSet BuildLocalIntranet()
		{
			NamedPermissionSet namedPermissionSet = new NamedPermissionSet("LocalIntranet", PermissionState.None);
			namedPermissionSet.AddPermission(new EnvironmentPermission(EnvironmentPermissionAccess.Read, "USERNAME;USER"));
			namedPermissionSet.AddPermission(new FileDialogPermission(PermissionState.Unrestricted));
			namedPermissionSet.AddPermission(new IsolatedStorageFilePermission(PermissionState.None)
			{
				UsageAllowed = IsolatedStorageContainment.AssemblyIsolationByUser,
				UserQuota = long.MaxValue
			});
			namedPermissionSet.AddPermission(new ReflectionPermission(ReflectionPermissionFlag.ReflectionEmit));
			SecurityPermissionFlag flag = SecurityPermissionFlag.Assertion | SecurityPermissionFlag.Execution;
			namedPermissionSet.AddPermission(new SecurityPermission(flag));
			namedPermissionSet.AddPermission(new UIPermission(PermissionState.Unrestricted));
			namedPermissionSet.AddPermission(PermissionBuilder.Create("System.Net.DnsPermission, System, Version=2.0.5.0, Culture=neutral, PublicKeyToken=b77a5c561934e089", PermissionState.Unrestricted));
			namedPermissionSet.AddPermission(PermissionBuilder.Create(DefaultPolicies.PrintingPermission("SafePrinting")));
			return namedPermissionSet;
		}

		private static NamedPermissionSet BuildInternet()
		{
			NamedPermissionSet namedPermissionSet = new NamedPermissionSet("Internet", PermissionState.None);
			namedPermissionSet.AddPermission(new FileDialogPermission(FileDialogPermissionAccess.Open));
			namedPermissionSet.AddPermission(new IsolatedStorageFilePermission(PermissionState.None)
			{
				UsageAllowed = IsolatedStorageContainment.DomainIsolationByUser,
				UserQuota = 512000L
			});
			namedPermissionSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));
			namedPermissionSet.AddPermission(new UIPermission(UIPermissionWindow.SafeTopLevelWindows, UIPermissionClipboard.OwnClipboard));
			namedPermissionSet.AddPermission(PermissionBuilder.Create(DefaultPolicies.PrintingPermission("SafePrinting")));
			return namedPermissionSet;
		}

		private static NamedPermissionSet BuildSkipVerification()
		{
			NamedPermissionSet namedPermissionSet = new NamedPermissionSet("SkipVerification", PermissionState.None);
			namedPermissionSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.SkipVerification));
			return namedPermissionSet;
		}

		private static NamedPermissionSet BuildExecution()
		{
			NamedPermissionSet namedPermissionSet = new NamedPermissionSet("Execution", PermissionState.None);
			namedPermissionSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));
			return namedPermissionSet;
		}

		private static NamedPermissionSet BuildNothing()
		{
			return new NamedPermissionSet("Nothing", PermissionState.None);
		}

		private static NamedPermissionSet BuildEverything()
		{
			NamedPermissionSet namedPermissionSet = new NamedPermissionSet("Everything", PermissionState.None);
			namedPermissionSet.AddPermission(new EnvironmentPermission(PermissionState.Unrestricted));
			namedPermissionSet.AddPermission(new FileDialogPermission(PermissionState.Unrestricted));
			namedPermissionSet.AddPermission(new FileIOPermission(PermissionState.Unrestricted));
			namedPermissionSet.AddPermission(new IsolatedStorageFilePermission(PermissionState.Unrestricted));
			namedPermissionSet.AddPermission(new ReflectionPermission(PermissionState.Unrestricted));
			namedPermissionSet.AddPermission(new RegistryPermission(PermissionState.Unrestricted));
			namedPermissionSet.AddPermission(new KeyContainerPermission(PermissionState.Unrestricted));
			SecurityPermissionFlag securityPermissionFlag = SecurityPermissionFlag.AllFlags;
			securityPermissionFlag &= ~SecurityPermissionFlag.SkipVerification;
			namedPermissionSet.AddPermission(new SecurityPermission(securityPermissionFlag));
			namedPermissionSet.AddPermission(new UIPermission(PermissionState.Unrestricted));
			namedPermissionSet.AddPermission(PermissionBuilder.Create("System.Net.DnsPermission, System, Version=2.0.5.0, Culture=neutral, PublicKeyToken=b77a5c561934e089", PermissionState.Unrestricted));
			namedPermissionSet.AddPermission(PermissionBuilder.Create("System.Drawing.Printing.PrintingPermission, System.Drawing, Version=2.0.5.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", PermissionState.Unrestricted));
			namedPermissionSet.AddPermission(PermissionBuilder.Create("System.Diagnostics.EventLogPermission, System, Version=2.0.5.0, Culture=neutral, PublicKeyToken=b77a5c561934e089", PermissionState.Unrestricted));
			namedPermissionSet.AddPermission(PermissionBuilder.Create("System.Net.SocketPermission, System, Version=2.0.5.0, Culture=neutral, PublicKeyToken=b77a5c561934e089", PermissionState.Unrestricted));
			namedPermissionSet.AddPermission(PermissionBuilder.Create("System.Net.WebPermission, System, Version=2.0.5.0, Culture=neutral, PublicKeyToken=b77a5c561934e089", PermissionState.Unrestricted));
			namedPermissionSet.AddPermission(PermissionBuilder.Create("System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.5.0, Culture=neutral, PublicKeyToken=b77a5c561934e089", PermissionState.Unrestricted));
			namedPermissionSet.AddPermission(PermissionBuilder.Create("System.DirectoryServices.DirectoryServicesPermission, System.DirectoryServices, Version=2.0.5.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", PermissionState.Unrestricted));
			namedPermissionSet.AddPermission(PermissionBuilder.Create("System.Messaging.MessageQueuePermission, System.Messaging, Version=2.0.5.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", PermissionState.Unrestricted));
			namedPermissionSet.AddPermission(PermissionBuilder.Create("System.ServiceProcess.ServiceControllerPermission, System.ServiceProcess, Version=2.0.5.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", PermissionState.Unrestricted));
			namedPermissionSet.AddPermission(PermissionBuilder.Create("System.Data.OleDb.OleDbPermission, System.Data, Version=2.0.5.0, Culture=neutral, PublicKeyToken=b77a5c561934e089", PermissionState.Unrestricted));
			namedPermissionSet.AddPermission(PermissionBuilder.Create("System.Data.SqlClient.SqlClientPermission, System.Data, Version=2.0.5.0, Culture=neutral, PublicKeyToken=b77a5c561934e089", PermissionState.Unrestricted));
			return namedPermissionSet;
		}

		private static SecurityElement PrintingPermission(string level)
		{
			SecurityElement securityElement = new SecurityElement("IPermission");
			securityElement.AddAttribute("class", "System.Drawing.Printing.PrintingPermission, System.Drawing, Version=2.0.5.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
			securityElement.AddAttribute("version", "1");
			securityElement.AddAttribute("Level", level);
			return securityElement;
		}

		public static class ReservedNames
		{
			public const string FullTrust = "FullTrust";

			public const string LocalIntranet = "LocalIntranet";

			public const string Internet = "Internet";

			public const string SkipVerification = "SkipVerification";

			public const string Execution = "Execution";

			public const string Nothing = "Nothing";

			public const string Everything = "Everything";

			public static bool IsReserved(string name)
			{
				if (name != null)
				{
					if (DefaultPolicies.ReservedNames.<>f__switch$map2C == null)
					{
						DefaultPolicies.ReservedNames.<>f__switch$map2C = new Dictionary<string, int>(7)
						{
							{
								"FullTrust",
								0
							},
							{
								"LocalIntranet",
								0
							},
							{
								"Internet",
								0
							},
							{
								"SkipVerification",
								0
							},
							{
								"Execution",
								0
							},
							{
								"Nothing",
								0
							},
							{
								"Everything",
								0
							}
						};
					}
					int num;
					if (DefaultPolicies.ReservedNames.<>f__switch$map2C.TryGetValue(name, out num))
					{
						if (num == 0)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		public enum Key
		{
			Ecma,
			MsFinal
		}
	}
}
