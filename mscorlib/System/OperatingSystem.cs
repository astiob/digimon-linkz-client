using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	/// <summary>Represents information about an operating system, such as the version and platform identifier. This class cannot be inherited.</summary>
	/// <filterpriority>2</filterpriority>
	[ComVisible(true)]
	[Serializable]
	public sealed class OperatingSystem : ICloneable, ISerializable
	{
		private PlatformID _platform;

		private Version _version;

		private string _servicePack = string.Empty;

		/// <summary>Initializes a new instance of the <see cref="T:System.OperatingSystem" /> class, using the specified platform identifier value and version object.</summary>
		/// <param name="platform">One of the <see cref="T:System.PlatformID" /> values that indicates the operating system platform. </param>
		/// <param name="version">A <see cref="T:System.Version" /> object that indicates the version of the operating system. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="version" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="platform" /> is not a <see cref="T:System.PlatformID" /> enumeration value.</exception>
		public OperatingSystem(PlatformID platform, Version version)
		{
			if (version == null)
			{
				throw new ArgumentNullException("version");
			}
			this._platform = platform;
			this._version = version;
		}

		/// <summary>Gets a <see cref="T:System.PlatformID" /> enumeration value that identifies the operating system platform.</summary>
		/// <returns>One of the <see cref="T:System.PlatformID" /> values.</returns>
		/// <filterpriority>2</filterpriority>
		public PlatformID Platform
		{
			get
			{
				return this._platform;
			}
		}

		/// <summary>Gets a <see cref="T:System.Version" /> object that identifies the operating system.</summary>
		/// <returns>A <see cref="T:System.Version" /> object that describes the major version, minor version, build, and revision numbers for the operating system.</returns>
		/// <filterpriority>2</filterpriority>
		public Version Version
		{
			get
			{
				return this._version;
			}
		}

		/// <summary>Gets the service pack version represented by this <see cref="T:System.OperatingSystem" /> object.</summary>
		/// <returns>The service pack version, if service packs are supported and at least one is installed; otherwise, an empty string (""). </returns>
		/// <filterpriority>2</filterpriority>
		public string ServicePack
		{
			get
			{
				return this._servicePack;
			}
		}

		/// <summary>Gets the concatenated string representation of the platform identifier, version, and service pack that are currently installed on the operating system. </summary>
		/// <returns>The string representation of the values returned by the <see cref="P:System.OperatingSystem.Platform" />, <see cref="P:System.OperatingSystem.Version" />, and <see cref="P:System.OperatingSystem.ServicePack" /> properties.</returns>
		/// <filterpriority>2</filterpriority>
		public string VersionString
		{
			get
			{
				return this.ToString();
			}
		}

		/// <summary>Creates an <see cref="T:System.OperatingSystem" /> object that is identical to this instance.</summary>
		/// <returns>An <see cref="T:System.OperatingSystem" /> object that is a copy of this instance.</returns>
		/// <filterpriority>2</filterpriority>
		public object Clone()
		{
			return new OperatingSystem(this._platform, this._version);
		}

		/// <summary>Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object with the data necessary to deserialize this instance.</summary>
		/// <param name="info">The object to populate with serialization information.</param>
		/// <param name="context">The place to store and retrieve serialized data. Reserved for future use.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="info" /> is null. </exception>
		/// <filterpriority>2</filterpriority>
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("_platform", this._platform);
			info.AddValue("_version", this._version);
			info.AddValue("_servicePack", this._servicePack);
		}

		/// <summary>Converts the value of this <see cref="T:System.OperatingSystem" /> object to its equivalent string representation.</summary>
		/// <returns>The string representation of the values returned by the <see cref="P:System.OperatingSystem.Platform" />, <see cref="P:System.OperatingSystem.Version" />, and <see cref="P:System.OperatingSystem.ServicePack" /> properties.</returns>
		/// <filterpriority>2</filterpriority>
		public override string ToString()
		{
			int platform = (int)this._platform;
			string str;
			switch (platform)
			{
			case 0:
				str = "Microsoft Win32S";
				goto IL_96;
			case 1:
				str = "Microsoft Windows 98";
				goto IL_96;
			case 2:
				str = "Microsoft Windows NT";
				goto IL_96;
			case 3:
				str = "Microsoft Windows CE";
				goto IL_96;
			case 4:
				break;
			case 5:
				str = "XBox";
				goto IL_96;
			case 6:
				str = "OSX";
				goto IL_96;
			default:
				if (platform != 128)
				{
					str = Locale.GetText("<unknown>");
					goto IL_96;
				}
				break;
			}
			str = "Unix";
			IL_96:
			return str + " " + this._version.ToString();
		}
	}
}
